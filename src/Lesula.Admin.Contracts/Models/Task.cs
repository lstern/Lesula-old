﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Task.cs" company="Lesula MapReduce Framework - http://github.com/lstern/lesula">
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
//   Set of MapReduce Jobs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lesula.Admin.Contracts.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Set of MapReduce Jobs
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Job Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Who changed the job
        /// </summary>
        [DisplayName("Changed By")]
        public string ChangedBy { get; set; }

        /// <summary>
        /// Who changed the job
        /// </summary>
        [DisplayName("Changed Date")]
        public DateTime ChangedDate { get; set; }
    }
}