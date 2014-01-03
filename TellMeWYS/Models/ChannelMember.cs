using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellMeWYS.Models
{
    public class ChannelMember
    {
        public Guid Id { get; set; }

        public Guid ChannelId { get; set; }

        public Guid AccountId { get; set; }

        public bool IsOwner { get; set; }

        public virtual Account Account { get; set; }

        public ChannelMember()
        {
            this.Id = Guid.NewGuid();
        }
    }
}