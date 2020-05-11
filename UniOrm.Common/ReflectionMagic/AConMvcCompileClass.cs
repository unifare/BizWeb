/*
 * ************************************
 * file:	    AConMvcCompileClass.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/5/11 18:15:48
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;
using UniOrm.Model;

namespace UniOrm.Common.ReflectionMagic
{
    public class AConMvcCompileClass : AConMvcClass
    {
        public AConMvcCompileClass( )
        {
        }

  

        public static AConMvcCompileClass ToCompileClass(AConMvcClass systemHtml )
        {
            return new AConMvcCompileClass()
            {
                Guid = systemHtml.Guid,
                ActionCode = systemHtml.ActionCode
             ,
                AllSourceCode = APPCommon.ToSourceCode(systemHtml)
             ,
                Addtime = systemHtml.Addtime
             ,
                ClassAttrs = systemHtml.ClassAttrs
             ,
                ClassName = systemHtml.ClassName
             ,
                ExReferenceName = systemHtml.ExReferenceName
             ,
                IsEanable = systemHtml.IsEanable
             ,
                Name = systemHtml.Name
             ,
                Id = systemHtml.Id
             ,
                UrlRule = systemHtml.UrlRule
             ,
                UsingNameSpance = systemHtml.UsingNameSpance
             ,
                VersionNum = systemHtml.VersionNum
            };
        }

        public string AllSourceCode { get; set; }
    }
}
