using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SpionshopASPNETWebApp.Models.Account;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using SpionshopASPNETWebApp.Models.dto;

namespace SpionshopASPNETWebApp.Controllers
{
    public class AccountController : Controller
    {
        string uri = "http://spionshopapi2.azurewebsites.net/api/Klants/";

        [HttpPost]
        public JsonResult GetSessionVariable()
        {
            return Json((KlantDTO)System.Web.HttpContext.Current.Session["user"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<RegisterViewModel, KlantDTO>();
                var klantDTO = Mapper.Map<KlantDTO>(model);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.PostAsJsonAsync<KlantDTO>(uri, klantDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        klantDTO = await response.Content.ReadAsAsync<KlantDTO>();
                        System.Web.HttpContext.Current.Session["user"] = klantDTO;
                    }
                    else
                    {
                        throw new Exception("Probleem met wegschrijven klant!");
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            // iets is verkeer gegaan, toon opnieuw de Register.cshtml
            return View(model);
        }


        //
        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.GetAsync(String.Format(uri + "?klantDTO.Gebruikersnaam={0}&klantDTO.Pwd={1}",
                        model.Gebruikersnaam.ToString(),
                        model.Pwd.ToString()))
                        .Result;

                    if (response.IsSuccessStatusCode)
                    {
                        KlantDTO klantDTO = await response.Content.ReadAsAsync<KlantDTO>();
                        System.Web.HttpContext.Current.Session["user"] = klantDTO;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Verkeerde gebruikersnaam of paswoord!");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
