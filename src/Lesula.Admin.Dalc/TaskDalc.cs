﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskDalc.cs" company="Lesula MapReduce Framework - http://github.com/lstern/lesula">
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
//   Defines the TaskDalc type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lesula.Admin.Dalc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Apache.Cassandra;

    using Lesula.Admin.Contracts;
    using Lesula.Admin.Contracts.Models;
    using Lesula.Cassandra;
    using Lesula.Cassandra.FrontEnd;
    using Lesula.Core.Cassandra;

    /// <summary>
    /// The task dalc.
    /// </summary>
    public class TaskDalc : ITaskDalc
    {
        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns>
        /// List of all tasks.
        /// </returns>
        public List<Task> GetAll()
        {
            var selector = DataBase.CreateSelector();
            var rows = selector.GetColumnsFromRows("Task", Selector.KeyRangeAll, Selector.NewColumnsPredicate("DAT"), ConsistencyLevel.ONE);
            var tasks = rows.Select(row => this.MapFromColumn(row.Value[0])).ToList();
            return tasks;
        }

        /// <summary>
        /// Get task info
        /// </summary>
        /// <param name="taskId">task id</param>
        /// <returns>requested task info</returns>
        public Task Get(Guid taskId)
        {
            var selector = DataBase.CreateSelector();
            var columns = selector.GetColumnFromRow("Task", taskId, "DAT", ConsistencyLevel.ONE);
            var task = this.MapFromColumn(columns);
            return task;
        }

        /// <summary>
        /// Save task
        /// </summary>
        /// <param name="task">task data</param>
        public void Save(Task task)
        {
            var serialized = ServiceStack.Text.TypeSerializer.SerializeToString(task);
            var compressed = LZ4Sharp.LZ4.Compress(serialized.ToBytes());

            var mutator = DataBase.CreateMutator();
            var column = mutator.NewColumn("DAT", compressed);
            mutator.InsertColumn("Task", task.Id.ToByteArray(), column, ConsistencyLevel.ONE);
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="taskId">task id</param>
        public void Delete(Guid taskId)
        {
            var deletor = DataBase.CreateRowDeletor();
            deletor.DeleteRow("Task", taskId, ConsistencyLevel.ONE);
        }

        /// <summary>
        /// Map data column to Task
        /// </summary>
        /// <param name="column">
        /// Column
        /// </param>
        /// <returns>
        /// Resulting task
        /// </returns>
        private Task MapFromColumn(Column column)
        {
            var decompressed = LZ4Sharp.LZ4.Decompress(column.Value).ToUtf8String();
            var task = ServiceStack.Text.TypeSerializer.DeserializeFromString<Task>(decompressed);
            return task;
        }
    }
}
