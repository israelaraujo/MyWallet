﻿using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MyWallet.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [Display(Name = "New Photo")]
        public HttpPostedFileBase NewPhoto { get; set; }

        public string PhotoBase64 { get; set; }
    }
}