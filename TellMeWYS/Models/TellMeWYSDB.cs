using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TellMeWYS.Models
{
    public class TellMeWYSDB : DbContext
    {
        public DbSet<Channel> Channels { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<ChannelMember> ChannelMembers { get; set; }

        public static TellMeWYSDB Default(HttpContextBase context)
        {
            const string key = "TellMeWYS.Models.TellMeWYSDB.Default";
            return context.Items.GetAsCache(key, () => new TellMeWYSDB());
        }
    }
}