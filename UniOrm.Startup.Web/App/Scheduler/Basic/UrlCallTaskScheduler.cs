﻿/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler.Basic
{
    public class UrlCallTaskScheduler : ScheduleTask, IScheduleTask
    {
        public string Url { get; set; }
        public string Data { get; set; }
        public HttpMethod Method { get; set; }

        public override string Execute()
        {
            return base.Execute();
        }

        public enum HttpMethod
        {
            GET,
            POST
        }
    }
}
