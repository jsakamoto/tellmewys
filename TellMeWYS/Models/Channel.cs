using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellMeWYS.Models
{
    public class Channel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ClientPort { get; set; }

        public virtual ICollection<ChannelMember> ChannelMembers { get; set; }

        public Channel()
        {
            this.Id = Guid.NewGuid();
            this.ClientPort = Guid.NewGuid();
        }
    }
}