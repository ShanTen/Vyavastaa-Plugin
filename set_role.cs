using Clubby.Discord.CommandHandling;
using Clubby.Plugins;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VyavastaPlugin
{
    [PluginExport]
    public class set_role : IDiscordCommand
    {
        public HelpDetails GetCommandHelp()
        {
            return new HelpDetails()
            {
                CommandName = "set_role",
                ShortDescription = "Add Upper House and Lower House Roles"
            };
        }

        public DiscordCommandPermission GetMinimumPerms()
        {
            return DiscordCommandPermission.Admin;
        }

        public async Task Handle(SocketMessage msg, SocketGuild guild, CommandHandler commandHandler)
        {
            var args = msg.Arguments();
            var house = args[1];
            var code = ulong.Parse(args[2].Substring(3,args[2].Length-4)); //<@&someulong>

            if (house == "lowerhouse" || house == "lh" || house == "lower house" || house == "lower")
            {
                Initializer.globalVyavastaConfig.LowerHouseRole = code;
                await msg.Channel.SendOk("Role Added.");
            }
            else if (house == "upperhouse" || house == "uh" || house == "upper house" || house == "upper")
            {
                Initializer.globalVyavastaConfig.UpperHouseRole = code;
                await msg.Channel.SendOk("Role Added.");
            }

        }
    }

}
