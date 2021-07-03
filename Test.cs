using Clubby.Discord.CommandHandling;
using Clubby.GeneralUtils;
using Clubby.Plugins;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace VyavastaPlugin
{
    [PluginExport]
    public class Test : IDiscordCommand
    {
        public HelpDetails GetCommandHelp()
        {
            return new HelpDetails()
            {
                CommandName = "Test",
                ShortDescription = "Shan10 Dll Add Test"
            };
        }

        public DiscordCommandPermission GetMinimumPerms()
        {
            return DiscordCommandPermission.Member;
        }

        public async Task Handle(SocketMessage msg, SocketGuild guild, CommandHandler commandHandler)
        {
            await msg.Channel.SendMessageAsync("Test For Clobby");
            await msg.Channel.SendMessageAsync(msg.Arguments().MakeString());
        }
    }
}
