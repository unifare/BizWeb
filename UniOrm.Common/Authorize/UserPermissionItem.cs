using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Authorize
{
    public class UserPermissionItem
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public virtual string ControllerName { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public virtual string ActionName { get; set; }
        public virtual string RoleName { get; set; }
    }
}
