using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellMeWYS.Models
{
    public class Channel
    {
        public Guid Id { get; set; }
        
        [Required, StringLength(50), AllowHtml]
        public string Name { get; set; }

        public Guid ClientPort { get; set; }

        public DateTime CreateAt { get; set; }

        public virtual ICollection<ChannelMember> ChannelMembers { get; set; }

        public Channel()
        {
            this.Id = Guid.NewGuid();
            this.Name = "";
            this.ClientPort = Guid.NewGuid();
            this.CreateAt = DateTime.UtcNow;
            this.ChannelMembers = new List<ChannelMember>();
        }
    }
}