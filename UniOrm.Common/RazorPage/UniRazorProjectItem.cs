/*
 * ************************************
 * file:	    UniRazorProjectItem.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/23 15:15:59
 * description:	
 * ************************************
 */

using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniOrm.Common.RazorPage
{
   public  class UniRazorProjectItem : RazorLightProjectItem
    {
        private string _content;

        public UniRazorProjectItem(string key, string content)
        {
            Key = key;
            _content = content;
        }

        public override string Key { get; }

        public override bool Exists => _content != null;

        public override Stream Read()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content));
        }
    }
}
