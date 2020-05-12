/*
 * ************************************
 * file:	    UniRazorProject.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/23 15:08:49
 * description:	
 * ************************************
 */

using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniOrm.Model;

namespace UniOrm.Common.RazorPage
{
    public class UniRazorProject: RazorLightProject
    {    
        //
        // 摘要:
        //     Looks up for the ViewImports content for the given template
        //
        // 参数:
        //   templateKey:
        public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult(Enumerable.Empty<RazorLightProjectItem>());
          
        }
        //
        // 摘要:
        //     Looks up for the template source with a given templateKey
        //
        // 参数:
        //   templateKey:
        //     Unique template key
        public override async Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        { 
            var sy = await DB.UniClient.Queryable<SystemHtml>().Where(p => p.Name == templateKey).FirstAsync();
            var projectItem = new UniRazorProjectItem(templateKey, UsingNameSpace+sy?.Value);

            return projectItem; 
        }

        private static string _usingNameSpace;
        private static string UsingNameSpace 
        {
            get
            {
                if(string.IsNullOrEmpty( _usingNameSpace))
                {
                    var stringbuilder = new StringBuilder(); 
                    stringbuilder.AppendLine("\r\n@{ DisableEncoding = true;  ");

                    stringbuilder.AppendLine("\r\n      var Page = new UniOrm.RazorTool();  ");
                    stringbuilder.AppendLine("\r\n      if( UniOrm.RazorTool.IsPropertyExist( Model,\"Item\")){");
                    stringbuilder.AppendLine("\r\n          Page.ResouceInfos=Model.Item as System.Collections.Generic.Dictionary<string, object>; ");
                    stringbuilder.AppendLine("\r\n      }");
                    stringbuilder.AppendLine("\r\n      if( UniOrm.RazorTool.IsPropertyExist( Model,\"Funs\")){");
                    stringbuilder.AppendLine("\r\n          Page.Funs=Model.Funs as System.Collections.Generic.Dictionary<string, MethodDelegate>; ");
                    stringbuilder.AppendLine("\r\n      }");
                    stringbuilder.AppendLine("\r\n }");
                    _usingNameSpace = stringbuilder.ToString();
                }
                return _usingNameSpace;
            }
           
        }
    }
}
