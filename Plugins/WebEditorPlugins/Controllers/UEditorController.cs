using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using UEditor.Core;
using UniOrm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace WebEditorPlugins
{

    public class UEditorController : Controller
    {
        private readonly UEditorService _ueditorService;
        public UEditorController(IHttpContextAccessor httpContextAccessor, UEditorService uEditorService)
        { 
            this._ueditorService = uEditorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, HttpPost]
        public ContentResult Upload()
        {
            var response = _ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }

    }
}
