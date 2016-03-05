using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpionshopAuthAPI2.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}