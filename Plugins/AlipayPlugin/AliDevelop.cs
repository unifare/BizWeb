using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Domain;
using Essensoft.AspNetCore.Payment.Alipay.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace AlipayPlugin
{
    public class AliDevelop
    {
        public static string PagePay(dynamic viewModel)
        {
            var model = new AlipayTradePagePayModel
            {
                Body = viewModel.Body,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount,
                OutTradeNo = viewModel.OutTradeNo,
                ProductCode = viewModel.ProductCode
            };
            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);
            req.SetReturnUrl(viewModel.ReturnUrl);
            var _client = AlipayPlugin.AlipayModule.ServiceProvider.GetService<IAlipayClient>();
            var _optionsAccessor = AlipayPlugin.AlipayModule.ServiceProvider.GetService<IOptions<AlipayOptions>>();
            var response = _client.PageExecuteAsync(req, _optionsAccessor.Value);
            return response.Result.ResponseBody;
        }
        

    }
}
