using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    // 一个自定义特性 BugFix 被赋给类及其成员
    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Field |
    AttributeTargets.Method |
    AttributeTargets.Property,
    AllowMultiple = true)]

    public class ExportClassAttribute: Attribute
    {

    }
}
