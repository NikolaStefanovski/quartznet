#region License

/*
 * All content copyright Marko Lahma, unless otherwise indicated. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy
 * of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 */

#endregion

namespace Quartz.Examples.Example10
{
    /// <summary>
    /// This is just a simple job.
    /// </summary>
    /// <author>Bill Kratzer</author>
    /// <author>Marko Lahma (.NET)</author>
    public class SimpleJob : IJob
    {
        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual Task Execute(IJobExecutionContext context)
        {
            // This job simply prints out its job name and the
            // date and time that it is running
            JobKey jobKey = context.JobDetail.Key;
            Console.WriteLine("Executing job: {0} executing at {1:r}", jobKey, DateTime.Now);

            if (context.MergedJobDataMap.Count > 0)
            {
                ICollection<string> keys = context.MergedJobDataMap.Keys;
                foreach (string key in keys)
                {
                    var val = context.MergedJobDataMap.GetString(key);
                    Console.WriteLine(" - jobDataMap entry: {0} = {1}", key, val);
                }
            }

            context.Result = "hello";
            return Task.CompletedTask;
        }
    }
}