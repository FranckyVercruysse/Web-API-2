using SpionshopASPNETWebApp.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpionshopASPNETWebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CopyFiles()
        {
            FilesInfoDTO filesInfo = new FilesInfoDTO();
            //filesInfo.FileName = "champagne.gif";
            filesInfo.FileName = "ferrari.gif";
            filesInfo.Artikel_id = 77;

            try
            {
                HttpClient httpClient = new HttpClient();
                string uri = "http://" + Request.Url.Host + ':' + Request.Url.Port + "/CopyAfbeelding/" + filesInfo.FileName + "/" + filesInfo.Artikel_id;
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();     // Throw on error code.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}