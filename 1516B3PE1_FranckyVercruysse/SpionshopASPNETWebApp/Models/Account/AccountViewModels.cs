using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpionshopASPNETWebApp.Models.Account
{
    public class RegisterViewModel
    {
        public short Klant_id { get; set; }     // nodig ??
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Voornaam { get; set; }
        public string Woonplaats { get; set; }
        public Nullable<System.DateTime> Geboortedatum { get; set; }
        [Required]
        public string Gebruikersnaam { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Pwd { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Gebruikers naam")]
        public string Gebruikersnaam { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Pwd { get; set; }
    }
}