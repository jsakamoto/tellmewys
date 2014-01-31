using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellMeWYS.Models.ViewModel
{
    public class ChannelItemViewModel
    {
        public string name { get; set; }

        public bool isOwner { get; set; }

        public string channelUrl { get; set; }

        public string settingsUrl { get; set; }

        public ChannelItemViewModel(Channel channel, UrlHelper url, Account account)
        {
            name = channel.Name;
            isOwner = channel.ChannelMembers.First(c => c.AccountId == account.Id).IsOwner;
            channelUrl = url.Action("Index", new { id = channel.Id.ToString("N") });
            settingsUrl = url.Action("Settings", new { id = channel.Id.ToString("N") });
        }
    }
}