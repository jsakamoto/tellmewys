using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;

namespace TellMeWYS.Models.ViewModel
{
    public class AddMemberViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Provider { get; set; }

        public bool IsOwner { get; set; }

        public IEnumerable<SelectListItem> ProviderList { get; private set; }

        public AddMemberViewModel()
        {
            this.ProviderList = OAuthWebSecurity.RegisteredClientData
                .Select(_ => new SelectListItem { Text = _.DisplayName });
        }
    }
}