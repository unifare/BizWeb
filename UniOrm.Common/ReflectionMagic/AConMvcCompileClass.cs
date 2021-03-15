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
            var defaultNamespace = @"using UniOrm;
using UniOrm.Application;
using UniOrm.Common;
using UniOrm.Model;
using UniOrm.Startup.Web ;
using System;
using System.Web;
using System.IO;
using System.Text;
using System.Text.Encodings;
using System.Text.RegularExpressions;
using System.Collections.Generic; 
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic ;
using System.Diagnostics ;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using System.Configuration ;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common; 
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Drawing;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;
//using PetaPoco.SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlSugar;
using CSScriptLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics ;
using Peachpie.AspNetCore.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;";
            defaultNamespace += systemHtml.UsingNameSpance;
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
                UsingNameSpance = defaultNamespace
             ,
                VersionNum = systemHtml.VersionNum
            };
        }

        public string AllSourceCode { get; set; }
    }
}
