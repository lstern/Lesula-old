﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Lesula MapReduce Framework - http://github.com/lstern/lesula">
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//    http://www.apache.org/licenses/LICENSE-2.0
//   
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   Controller para gerenciamento do login.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lesula.Admin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using Lesula.Admin.Contracts;
    using Lesula.Admin.Contracts.Enumerators;
    using Lesula.Admin.Contracts.Models;
    using Lesula.Core;

    /// <summary>
    /// Controller para gerenciamento do login.
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// Tela de login do usuário
        /// </summary>
        /// <returns>
        /// Redireciona para tela principal ou retorna mensagem de erro.
        /// </returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Desloga do sistema
        /// </summary>
        /// <returns>Tela de login</returns>
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Efetua o login do usuário
        /// </summary>
        /// <param name="login">dados do login do usuário</param>
        /// <returns>
        /// Redireciona para tela principal ou retorna mensagem de erro.
        /// </returns>
        [HttpPost]
        public ActionResult Index(Login login)
        {
            UserRole? roles;
            if (login.Password == Context.Config.Get("AdminPassword"))
            {
                roles = UserRole.Admin;
            }
            else
            {
                roles = Context.Container.Resolve<IUserDalc>().Authenticate(login);
            }

            if (roles.HasValue)
            {
                this.Authenticate(login, roles.ToString());

                var returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction("Index", "Home");
            }

            this.@TempData["ErrorMessage"] = "Login failed";
            return this.View();
        }

        /// <summary>
        /// Cria ticket de autenticação do usuário.
        /// </summary>
        /// <param name="login">
        /// Login do usuário (email)
        /// </param>
        /// <param name="roles">
        /// Permissões do usuário.
        /// </param>
        internal void Authenticate(Login login, string roles)
        {
            // Usar com [Authorize(Roles="Admin")] na ação do controller.
            var authTicket = new FormsAuthenticationTicket(
                1,                             // version
                login.Email,                      // user name
                DateTime.Now,                  // created
                DateTime.Now.AddMinutes(20),   // expires
                true,                    // persistent?
                roles); // can be used to store roles

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }
    }
}
