using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TellMeWYS.Models;

namespace TellMeWYS.Hubs
{
    public class ChannelHub : Hub
    {
        public void BeginListen(string channelId)
        {
            var channelGuid = default(Guid);
            if (Guid.TryParse(channelId, out channelGuid) == false) return;

            var db = new TellMeWYSDB();
            var channel = db.Channels.Find(channelGuid);
            if (channel == null) return;

            var account = this.Context.User.ToAccount(db);
            if (channel.ChannelMembers.Any(_ => _.AccountId == account.Id) == false) return;

            this.Groups.Add(this.Context.ConnectionId, channelGuid.ToString("N"));
        }
    }
}