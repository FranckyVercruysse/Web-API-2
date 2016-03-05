using Newtonsoft.Json;
using SpionshopAuthAPI2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpionshopAuthAPI2.Controllers
{
    public class AccountMVCController : Controller
    {
        //public const string BaseAddress = "http://webapi2authenticationexample.azurewebsites.net/";
        public const string BaseAddress = "http://localhost:51190/";


        // GET: AccountMVC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(String.Format("{0}api/Account/Register", BaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                string json = JsonConvert.SerializeObject(model);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                try
                {
                    await request.GetResponseAsync();
                    //return true;
                }
                catch (Exception /* ex */)
                {
                    //return false;
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            var tokenServiceUrl = new Uri(String.Format("{0}Token", BaseAddress));
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Email),
                new KeyValuePair<string, string>("password", model.Password)
            };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                System.Web.HttpContext.Current.Session["token"] = responseString;
            }

            var x = Session["token"];
            var y = Request.IsAuthenticated;

           
            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}