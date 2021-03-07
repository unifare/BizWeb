using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading; 
namespace UniOrm
{
    public class ApplicationManager
    {

        private static ApplicationManager _appManager;
        private IHostBuilder _web;
        private CancellationTokenSource _tokenSource;
        private bool _running;
        private bool _restart;
         
        private int restartTime = 0;
        public ApplicationManager()
        {
            _running = false;
            _restart = false;

        }
        public IHostBuilder Host
        {
            get
            {
                return _web;
            }
            set
            {
                _web = value;
            }
        }

        public static ApplicationManager Load()
        {
            if (_appManager == null)
                _appManager = new ApplicationManager();

            return _appManager;
        }

        public void Start(string[] args,Func<string[] , IHostBuilder> hostBuilderFunc )
        {
            try
            { 
                do
                {
                    _web = hostBuilderFunc(args);
                    DoStart();
                } while (_restart);

            }
            catch (Exception ex)
            {

            }
          

        }
        public void DoStart()
        {
            if(_web==null)
            {
                throw new Exception("no web builder");
            }
            if (_running)
                return;

            if (_tokenSource != null && _tokenSource.IsCancellationRequested)
                return;

            _tokenSource = new CancellationTokenSource();
            _tokenSource.Token.ThrowIfCancellationRequested();
            _running = true;
            Console.WriteLine("Restarting App; " + (restartTime++));
            // Host.RunConsoleAsync(_tokenSource.Token);
            _web.Build().RunAsync(_tokenSource.Token).GetAwaiter().GetResult(); 
        }

        public void Stop()
        {
            if (!_running)
                return;
          
            _tokenSource.Cancel();
            _web = null;
            _restart = false;
            _running = false;
        }

        public void Restart()
        {
            Stop();

            _restart = true;
            _tokenSource = null;
        }
    }
}
