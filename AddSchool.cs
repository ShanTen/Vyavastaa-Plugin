using Clubby.Discord.CommandHandling;
using Clubby.Plugins;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace VyavastaPlugin
{

    [PluginExport]
    public class AddSchool : IDiscordCommand
    {
        public HelpDetails GetCommandHelp()
        {
            return new HelpDetails()
            {
                CommandName = "addschool",
                ShortDescription = "Add School Name to list of schools."
            };
        }

        public DiscordCommandPermission GetMinimumPerms()
        {
            return DiscordCommandPermission.Admin;
        }

        public async Task Handle(SocketMessage msg, SocketGuild guild, CommandHandler commandHandler)
        {
            var args = msg.Arguments();
            var school = args[1];
            var code = args[2];

            Initializer.AddSchool(school, code);
            await msg.Channel.SendOk("School Added.");
        }
    }
}

