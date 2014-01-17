using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellMeWYS.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string ProviderName { get; set; }

        public string UniqueIdInProvider { get; set; }

        public string AccountName { get; set; }

        public Account()
        {
            this.Id = Guid.NewGuid();
            this.UniqueIdInProvider = "";
        }
    }
}