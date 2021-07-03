using Clubby.Discord;
using Clubby.Discord.CommandHandling;
using Clubby.GeneralUtils;
using Clubby.Plugins;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VyavastaPlugin
{
    [PluginExport]
    public class vAnn : IDiscordCommand
    {
        public HelpDetails GetCommandHelp()
        {
            return Utils.Init((ref HelpDetails h) =>
            {
                h.CommandName = "vAnn";
                h.ShortDescription = "Make a general announcement";
            });
        }

        public DiscordCommandPermission GetMinimumPerms()
        {
            return DiscordCommandPermission.Admin;
        }

        public async Task Handle(SocketMessage msg, SocketGuild guild, CommandHandler commandHandler)
        {
            var exec = commandHandler.GetExecutingCommand(msg.Author.Id);
            if (exec == this)
            {
                SocketTextChannel channel = guild.GetChannel(Clubby.Program.config.DiscordChannels["VyavastaAnnouncements"]) as SocketTextChannel;

                if (channel != null)
                {
                    string content = null;
                    if(msg.Attachments.Count > 0){
                        content = msg.Attachments.First().Url;
                    }

                    var ann = await channel.SendMessageAsync(null, false, new EmbedBuilder()
                        .WithTitle($"Announcement")
                        .WithImageUrl(content)
                        .WithColor(Color.Blue)
                        .WithDescription(msg.Content)
                        .Build());

                    await msg.Channel.SendOk("Announcement sent!");
                }
                else throw new Exception("Annoucement channel is not set!");

                commandHandler.SetExecutingCommand(msg.Author.Id, null);
            }
            else if (exec == null)
            {
                await msg.Channel.SendOk("Enter the announcement to send");
                commandHandler.SetExecutingCommand(msg.Author.Id, this);
            }
        }
    }
}