﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Login.cs" company="Lesula MapReduce Framework - http://github.com/lstern/lesula">
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
//   Objeto usado para controle de login
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lesula.Admin.Contracts.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Objeto usado para controle de login
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Nome do usuário.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
