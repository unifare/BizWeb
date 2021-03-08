using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm.Startup.Web
{

    public class CmsInstallController : Controller
    {
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> SetupSuccess()
        {
            string referer = Request.Headers["Referer"].ToString();
            if (referer.EndsWith("/SetupHome/CreateAdmin"))
            {
                await WebSetup.RestartAppAsync();
            }
            return View();
        }

       
        public object SetupConstring(int dbType, string dbserverName, string port, string DBName, string user, string password, string filename = null)
        {
            bool IsInstallDB = true;
            var CmsInstallDir = APP.GetDicstring("CmsInstallDir");
            var CmsInstallLockFileName = APP.GetDicstring("CmsInstallLockFileName");
            var pyDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CmsInstallDir);
            if (!Directory.Exists(pyDir))
            {
                Directory.CreateDirectory(pyDir);

            }
            var lockfilePath = Path.Combine(pyDir, CmsInstallLockFileName);
            FileInfo fileInfo = new FileInfo(lockfilePath);
            if (!fileInfo.Exists)
            {
                IsInstallDB = false;
            }
            var dbtype = dbType;//0 sqlite,1 mssql 2 mysql
            var connectionString = string.Empty;
            if (!IsInstallDB)
            {
               
                switch (dbtype)
                {
                    case 0:
                        connectionString = filename;
                        break; 
                    case 1:
                        connectionString = "Data Source={dbserverName};Initial Catalog={DBName};Port={port};User ID={user};Password={password};Connect Timeout=3000;Max Pool Size =2024;";
                        break;
                    case 2:
                        connectionString =  "Server={dbserverName};Port={port};Database={DBName}; User={user};Password={password};";
                        break;
                }

            }

            return new { isok = false, msg = "Already installed" };


        }
    }
}
