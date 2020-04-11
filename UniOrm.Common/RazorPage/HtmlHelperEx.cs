/*
 * ************************************
 * file:	    HtmlHelperEx.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/7 20:28:59
 * description:	
 * ************************************
 */
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UniOrm
{
    public static class LocalizationHelper
    {
        public static string Include(this HtmlHelper html, string RelativefilePath)
        {
            RelativefilePath = "/Pages/UploadPage/"+ RelativefilePath.UrlDecode();
            var fullpath = RelativefilePath.ToServerFullPath();
            var content = fullpath.ReadAsTextFile(); 
            return content;
        }
    }
}