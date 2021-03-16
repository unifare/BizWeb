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
        Func<string[], IHostBuilder> hostBuilderFunc;
        private CancellationTokenSource _tokenSource;
        private bool _running;
        private bool _restart;
        public string[] args;
        private int restartTime = 0;
        public ApplicationManager()
        {
            _running = false;
            _restart = false;

        }
       

        public static ApplicationManager Load()
        {
            if (_appManager == null)
                _appManager = new ApplicationManager();

            return _appManager;
        }

        public void StartApp(string[] args, Func<string[], IHostBuilder> hostBuilderFunc)
        {
            Start(args, hostBuilderFunc).GetAwaiter().GetResult();


        }
        public async Task Start(string[] args,Func<string[] , IHostBuilder> hostBuilderFunc )
        {
            try
            { 
                do
                {
                    this.args = args;
                    this.hostBuilderFunc = hostBuilderFunc ;
                    await DoStart() ;
                } while (_restart);

            }
            catch (Exception ex)
            {

            }
          

        }
        public async Task DoStart()
        {
            if( hostBuilderFunc  == null)
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
            await hostBuilderFunc(args).Build().RunAsync(_tokenSource.Token) ; 
        }

        public void Stop()
        {
            if (!_running)
                return;
          
            _tokenSource.Cancel();
            hostBuilderFunc = null;
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
