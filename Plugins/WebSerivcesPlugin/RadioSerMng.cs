using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WebSerivcesPlugin
{
    public class RadioSerMng
    {
        public string Getcountries()
        {
            var http = new HttpClient();
            var allres = http.GetStringAsync("http://www.radio-browser.info/webservice/json/countries").Result;

            return allres;
        }

    }
}
