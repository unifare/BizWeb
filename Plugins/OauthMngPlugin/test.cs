using System;
using System.Collections.Generic;
using System.Text;

namespace OauthMngPlugin
{
    public interface Itest
    {
        string Name { get; set; }
    }
    public class test : Itest
    {
        public string Name { get; set; }
    }
}
