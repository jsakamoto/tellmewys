using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TellMeWYS.Models
{
    public class Channel
    {
        public Guid Id { get; set; }
        
        [Required, StringLength(50)]
        public string Name { get; set; }

        public Guid ClientPort { get; set; }

        public virtual ICollection<ChannelMember> ChannelMembers { get; set; }

        public Channel()
        {
            this.Id = Guid.NewGuid();
            this.ClientPort = Guid.NewGuid();
            this.Name = "(no title)";
            this.ChannelMembers = new List<ChannelMember>();
        }
    }
}