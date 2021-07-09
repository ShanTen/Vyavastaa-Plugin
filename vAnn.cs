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

        private int Progress = -1;
        private SocketTextChannel channel = null;

        public async Task Handle(SocketMessage msg, SocketGuild guild, CommandHandler commandHandler)
        {
            var exec = commandHandler.GetExecutingCommand(msg.Author.Id);
            if (exec == this)
            {
                if (Progress == 0)
                {
                    if (msg.Content.StartsWith("<#") && msg.Content.EndsWith(">"))
                    {
                        string id_str = msg.Content.Substring(2, msg.Content.Length - 3);
                        ulong id = ulong.Parse(id_str);

                        channel = guild.GetChannel(id) as SocketTextChannel;

                        if (channel != null)
                        {
                            Progress += 1;
                            await msg.Channel.SendOk("Enter announcement to send");
                        }
                        else
                        {
                            await msg.Channel.SendError("Channel either couldn't be found or was a voice channel!");
                            await msg.Channel.SendOk("Enter channel to send the announcement in");
                        }
                    }
                }
                else if (Progress == 1)
                {
                    if (channel != null)
                    {
                        string content = null;
                        if (msg.Attachments.Count > 0)
                        {
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
            }
            else if (exec == null)
            {
                await msg.Channel.SendOk("Enter channel to send the announcement in");
                this.Progress += 1;
                commandHandler.SetExecutingCommand(msg.Author.Id, this);
            }
        }
    }
}