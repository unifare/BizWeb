 
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public class ScriptHelper
    {
        public void Get()
        {
            //dynamic script = CSScript.LoadCode(
            //               @"using System.Windows.Forms;
            //                 public class Script
            //                 {
            //                     public void SayHello(string greeting)
            //                     {
            //                         MessageBox.Show(""Greeting: "" + greeting);
            //                     }
            //                 }")
            //                 .CreateObject("*");
            //script.SayHello("Hello World!");
            ////-----------------
            //var product = CSScript.CreateFunc<int>(@"int Product(int a, int b)
            //                             {
            //                                 return a * b;
            //                             }");
            //int result = product(3, 4);
            ////-----------------
            //var SayHello = CSScript.LoadMethod(
            //                        @"using System.Windows.Forms;
            //              public static void SayHello(string greeting)
            //              {
            //                  MessageBoxSayHello(greeting);
            //                  ConsoleSayHello(greeting);
            //              }
            //              static void MessageBoxSayHello(string greeting)
            //              {
            //                  MessageBox.Show(greeting);
            //              }
            //              static void ConsoleSayHello(string greeting)
            //              {
            //                  Console.WriteLine(greeting);
            //              }")
            //                         .GetStaticMethod("*.SayHello", "");
            //SayHello("Hello again!");
        }
    }
}
