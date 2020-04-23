using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Application;
using UniOrm.Startup.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.Net;
using HtmlAgilityPack;
using System.Collections;
using System.Text;
using System.Net.Http;
using SqlSugar;

namespace UniNote.WebClient.Controllers
{



    [Area("sd23nj")]
    [Route("sd23nj/[controller]/[action]")]
    [AdminAuthorize]
    public class AFileController : Controller
    {
        ISqlSugarClient dbFactory;
        private readonly string LoggerName = "AFileController";

        public AFileController(ISqlSugarClient _dbFactory)
        {
            dbFactory = _dbFactory;
        }
        public IActionResult GetAllFiles(string dir)
        {
            var basedir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            var fullpath = dir.ToServerFullPath().TrimEnd(Path.DirectorySeparatorChar);
            if (!Directory.Exists(fullpath))
            {
                return new JsonResult(new { isok = false });
            }
            else
            {
                var index = 0;
                var list = new List<FileDTO>();
               
                if (fullpath != basedir  )
                {
                    var fullpadir = fullpath.GetDirFullPath().TrimEnd(Path.DirectorySeparatorChar);
                    var repath = fullpadir.ToRelativePath(basedir);
                    list.Add(new FileDTO { Id = index, Name = "..上一级", RelatedPath = repath, ParentDirName = repath, IsDirectry = true });
                    index++;
                }

                var allfiles = Directory.GetFiles(fullpath);
                var subdirs = Directory.GetDirectories(fullpath);
                //var allobjects = subdirs.Concat(allfiles); 
                foreach (var item in subdirs)
                {
                    var fileio = new DirectoryInfo(item);

                    list.Add(new FileDTO()
                    {
                        Id = index,
                        RelatedPath = fileio.FullName.FindAndSubstring(basedir).Replace("\\", "/"),
                        Name = fileio.Name,
                        FileExtense = fileio.Extension,
                        ParentDirName = dir,
                        IsDirectry = true,
                        IsReaderable = false,
                        IsWritable = true
                    });
                    index++;
                }
                foreach (var item in allfiles)
                {
                    var fileio = new FileInfo(item);

                    list.Add(new FileDTO()
                    {
                        Id = index,
                        RelatedPath = fileio.FullName.FindAndSubstring(basedir).Replace("\\", "/"),
                        Name = fileio.Name,
                        IsTextFile = fileio.FullName.IsTextFile(),
                        FileExtense = fileio.Extension,
                        ParentDirName = dir,
                        IsZipFile = fileio.Extension.ToLower().Contains("zip") || fileio.Extension.ToLower().Contains("raa") || fileio.Extension.ToLower().Contains("7z") || fileio.Extension.ToLower().Contains("tar"),
                        IsDirectry = false,
                        IsReaderable = fileio.IsReadOnly,
                        IsWritable = !fileio.IsReadOnly
                    });
                    index++;
                }
                if (dir != "/")
                {
                    var redir = fullpath.ToRelativePath(basedir);
                    return new JsonResult(new { isok = true, parentdir = redir, list = list });
                }
                else
                {
                    return new JsonResult(new { isok = true, parentdir = "/", list = list });
                }
            }

        }
        public IActionResult GetFileContent(string rePath)
        {
            rePath = rePath.UrlDecode();
            var fullpath = rePath.ToServerFullPath();
            var content = fullpath.ReadAsTextFile();

            return new JsonResult(new { isok = true, content });
        }

        public IActionResult CreateFile(string redir,string filepathname, string context)
        {
            var basedir = AppDomain.CurrentDomain.BaseDirectory.CombineFilePath(redir);

            basedir.EnSureDirectroy();
            var file = basedir.CombineFilePath(filepathname);
            var isCreated = file.FileToCreated();
            if( isCreated)
            {
                System.IO.File.WriteAllText(file, context);
            }
            return new JsonResult(new { isok = isCreated });
        }
        public IActionResult CreateDir(string redir)
        {
            var basedir = AppDomain.CurrentDomain.BaseDirectory.CombineFilePath(redir);

            basedir.EnSureDirectroy();
           
            return new JsonResult(new { isok = true });
        }
        public IActionResult SaveFile(string path, string newfilename, bool isdir, bool istext, string context)
        {
            path = path.ToServerFullPath();
            var newpath = path.FileRename(newfilename);
            if (istext)
            {
                var oldtext = newpath.ReadAsTextFile();
                if (oldtext != context)
                {
                    var sw = newpath.OpenTextFileReadyWrite();
                    sw.Write(context);
                    sw.Close();
                }

            }

            return new JsonResult(new { isok = true });
        }


        public IActionResult RenameDir(string path, string newfilename)
        {
            path = path.ToServerFullPath();
            var isok = path.DirRename(newfilename);
            return new JsonResult(new { isok = true });
        }
        public IActionResult DelFile(string path)
        {
            path = path.ToServerFullPath();
            return new JsonResult(new { isok = path.FileDelete() });
        }
        public IActionResult DelFileOrDir(string path, bool isdir)
        {
            path = path.ToServerFullPath();
            var isok = false;
            if (!isdir)
            {
                isok = path.FileDelete();
            }
            else
            {
                isok = path.DirDelete();
            }
            return new JsonResult(new { isok });
        }
        public IActionResult UploadFile(string dirName)
        {
            var remsg = string.Empty;
            dirName = dirName.UrlDecode();
            if (Request.Form.Files.Count == 0)
            {
                remsg = ("未检测到文件");
                return new JsonResult(new { isok = false, msg = remsg });
            }
            dirName = dirName.ToServerFullPathEnEnsure();

            foreach (var file in Request.Form.Files)
            {
                file.UploadSaveSingleFile(dirName);
            }
            return new JsonResult(new { isok = true, msg = remsg });
        }


        public IActionResult GetUrl(string url)
        {
            //var remsg = string.Empty;

            //return new JsonResult(new { isok = true, msg = remsg });
            return View();
        }
        public IActionResult GetUrlToLocal(string url, string static_dirname, string indexname)
        {
            var remsg = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return new JsonResult(new { isok = false, msg = remsg });
            }
            try
            {
                var urlTest = new Uri(url);
                HttpClient client = new System.Net.Http.HttpClient();
                var response = client.GetAsync(urlTest).Result;
                var allresult = response.Content.ReadAsStringAsync().Result;
                var savedPageName = "";
                if (string.IsNullOrEmpty(indexname))
                {
                    var indexfileName = url.Substring(url.LastIndexOf('/') + 1);
                    if (string.IsNullOrEmpty(indexfileName))
                    {
                        savedPageName = Guid.NewGuid().ToString("N").Replace("-", "");
                    }
                    else
                    {
                        savedPageName = indexfileName;
                    }
                }
                else
                {
                    savedPageName = indexname;
                }

                //System.IO.File.WriteAllText(Path.Combine(APPCommon.UserUploadBaseDir, savedPageName), allresult);

                Encoding encoder = Encoding.GetEncoding("utf-8");
                HtmlWeb webClient = new HtmlWeb();
                HtmlDocument htmlDoc = webClient.Load(url);
                HtmlNodeCollection hrefList = htmlDoc.DocumentNode.SelectNodes(".//a[@href]");
                HtmlNodeCollection scriptList = htmlDoc.DocumentNode.SelectNodes(".//script[@src]");
                HtmlNodeCollection cssList = htmlDoc.DocumentNode.SelectNodes(".//link[@href]");
                HtmlNodeCollection imgList = htmlDoc.DocumentNode.SelectNodes(".//img[@src]");

                foreach (var hr in scriptList)
                {
                    var href = hr.GetAttributeValue("src", "");

                    DownAndSaveFile(static_dirname, urlTest, href);

                    ReplaceUrl("src", static_dirname, urlTest, hr, href);

                }
                foreach (var hr in cssList)
                {
                    var href = hr.GetAttributeValue("href", "");
                    DownAndSaveFile(static_dirname, urlTest, href);
                    ReplaceUrl("href", static_dirname, urlTest, hr, href);
                }

                foreach (var hr in imgList)
                {
                    var href = hr.GetAttributeValue("src", "");
                    DownAndSaveImgFile(static_dirname, urlTest, href);
                    ReplaceUrl("src", static_dirname, urlTest, hr, href);
                }
                htmlDoc.Save(Path.Combine(APPCommon.UserUploadBaseDir, savedPageName));
                //System.IO.File.WriteAllText(Path.Combine(APPCommon.UserUploadBaseDir, savedPageName), allresult);

            }
            catch (Exception exception)
            {
                Logger.LogDebug(LoggerName, LoggerHelper.GetExceptionString(exception));
            }
            return new JsonResult(new
            {
                isok = true,
                msg = remsg
            });
            //return View();
        }

        private static void ReplaceUrl(string attr,string static_dirname, Uri urlTest, HtmlNode hr, string href)
        {
            var newehref = "";
            if (!href.StartsWith("http://") && !href.StartsWith("https://"))
            {
                newehref = "/w/" + static_dirname + (href.StartsWith('/') ? "" : "/") + href;
                hr.SetAttributeValue(attr, newehref);
            }
            else
            {
                var urisection = new Uri(href);
                if (urisection.Authority == urlTest.Authority)
                {
                    newehref = href.Replace(urlTest.Scheme + "://" + urlTest.Authority, "/w/" + static_dirname);
                    hr.SetAttributeValue(attr, newehref);
                }
            }

        }

        private static void DownAndSaveImgFile(string static_dirname, Uri urlTest, string href)
        {
           
            if (!href.StartsWith("http://") && !href.StartsWith("https://"))
            {
                href = urlTest.Scheme + "://" + urlTest.Authority + (href.StartsWith('/') ? "" : "/") + href;
            }
            
            var urisection = new Uri(href);
            if (urisection.Authority != urlTest.Authority)
            {
                return;
            }
            if (!string.IsNullOrEmpty(href))
            {
                var wholeurl = href;
                //if (!urisection.IsAbsoluteUri)
                //{
                //    wholeurl = urlTest.Scheme + "://" + urlTest.Authority + urisection.PathAndQuery;
                //}
                HttpClient client = new System.Net.Http.HttpClient();

                var response = client.GetAsync(wholeurl).Result;
                var allresult = response.Content.ReadAsStreamAsync().Result;

                var allselements = urisection.LocalPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var filename = "";
                if (allselements.Length > 1)
                {
                    filename = allselements[allselements.Length - 1];
                }
                if (string.IsNullOrEmpty(filename))
                {
                    filename = Guid.NewGuid().ToString("N").Replace("-", "");
                }
                var userwwwroot = Path.Combine(APPCommon.UserUploadBaseDir, "wwwroot");
                var newdirName = Path.Combine(userwwwroot, static_dirname);
                if (!Directory.Exists(newdirName))
                {
                    Directory.CreateDirectory(newdirName);
                }
                var subdir = newdirName;
                for (var i = 0; i < allselements.Length - 1; i++)
                {
                    var seg = allselements[i];

                    subdir = Path.Combine(subdir, seg);
                    if (!Directory.Exists(subdir))
                    {
                        Directory.CreateDirectory(subdir);
                    }
                }

                var physicfilePath = Path.Combine(subdir, filename);
                System.IO.File.WriteAllBytes(physicfilePath, StreamToBytes(allresult));
            }
        }


        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        private static void DownAndSaveFile(string static_dirname, Uri urlTest, string href)
        {
            if (!href.StartsWith("http://") && !href.StartsWith("https://"))
            {
                href = urlTest.Scheme + "://" + urlTest.Authority + (href.StartsWith('/') ? "" : "/") + href;
            }
            var urisection = new Uri(href);
            if (!string.IsNullOrEmpty(href))
            {
                var wholeurl = href;
                if (!urisection.IsAbsoluteUri)
                {
                    wholeurl = urlTest.Scheme + "://" + urlTest.Authority + urisection.PathAndQuery;
                }
                HttpClient client = new System.Net.Http.HttpClient();

                var response = client.GetAsync(wholeurl).Result;
                var allresult = response.Content.ReadAsStringAsync().Result;

                var allselements = urisection.LocalPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var filename = "";
                if (allselements.Length > 1)
                {
                    filename = allselements[allselements.Length - 1];
                }
                if (string.IsNullOrEmpty(filename))
                {
                    filename = Guid.NewGuid().ToString("N").Replace("-", "");
                }
                var userwwwroot = Path.Combine(APPCommon.UserUploadBaseDir, "wwwroot");
                var newdirName = Path.Combine(userwwwroot, static_dirname);
                if (!Directory.Exists(newdirName))
                {
                    Directory.CreateDirectory(newdirName);
                }
                var subdir = newdirName;
                for (var i = 0; i < allselements.Length - 1; i++)
                {
                    var seg = allselements[i];

                    subdir = Path.Combine(subdir, seg);
                    if (!Directory.Exists(subdir))
                    {
                        Directory.CreateDirectory(subdir);
                    }
                }

                var physicfilePath = Path.Combine(subdir, filename);
                System.IO.File.WriteAllText(physicfilePath, allresult);
            }
        }

        // 提取HTML代码中的网址
        public static ArrayList GetHyperLinks(string htmlCode)
        {
            ArrayList al = new ArrayList();

            string strRegex = @"[http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

            Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(htmlCode);

            for (int i = 0; i <= m.Count - 1; i++)
            {
                bool rep = false;
                string strNew = m[i].ToString();

                // 过滤重复的URL
                foreach (string str in al)
                {
                    if (strNew == str)
                    {
                        rep = true;
                        break;
                    }
                }

                if (!rep) al.Add(strNew);
            }

            al.Sort();

            return al;
        }


        public IActionResult UnzipFile(string refilepath, string distDir)
        {
            var fullpath = refilepath.ToServerFullPath();
            var fullDistpath = distDir.ToServerFullPath();

            //将指定 zip 存档中的所有文件都解压缩到文件系统的一个目录下
            ZipFile.ExtractToDirectory(fullpath, fullDistpath, true);
            return new JsonResult(new { isok = true });
        }
        public IActionResult FileMng()
        {
            return View();
        }

    }  
    
    
    [Area("sd23nj")]

    [AdminAuthorize]
    public class FileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RelatedPath { get; set; }
        public string ParentDirName { get; set; }

        public string FileExtense { get; set; }
        public bool IsDirectry { get; set; }
        public bool IsReaderable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsTextFile { get; set; }
        public bool IsZipFile { get; set; }
        public string LinuxRights { get; set; }

    }
}
