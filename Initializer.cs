using Clubby.Discord;
using Clubby.Discord.CommandHandling;
using Clubby.EventManagement;
using Clubby.GeneralUtils;
using Clubby.Plugins;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*TODO:
 * Giving UH/LH Roles via command
 * Renaming participants by the given message shorthand
 */

namespace VyavastaPlugin
{
    [PluginExport, PluginInit]
    public static class Initializer
    {
        enum House
        {
            UpperHouse,
            LowerHouse
        };

        public static void AddSchool(string schoolName, string code)
        {
            globalVyavastaConfig.schoolLookup.Add(schoolName, code);
            globalVyavastaConfig.Save();
        }

        public static EventResult handle(SocketMessage message)
        {
            if (message.Channel.Id == Clubby.Program.config.DiscordChannels.GetValueOrDefault("Registration"))
            {
                //template >> Vishal - The PSBB Millennium School - Upper House
                //template >> Vishal \nThe PSBB Millennium School \nUpper House

                var commArr = message.Content.Split('-', '\n').Select(m => m.Trim()).ToArray();
                Console.WriteLine(commArr.MakeString());

                House houseOut = House.LowerHouse;
                string schoolName = null;
                string ParticipantName = null;

                if (commArr.Length == 3)
                {
                    string house = commArr[2].ToLower();

                    if (house == "lowerhouse" || house == "lh" || house == "lower house" || house == "lower")
                    {
                        houseOut = House.LowerHouse;
                    }
                    else if (house == "upperhouse" || house == "uh" || house == "upper house" || house == "upper")
                    {
                        houseOut = House.UpperHouse;
                    }
                    else
                    {
                        message.Channel.SendError("Invalid House Name! (Upper House/Lower House)").Wait();
                        return EventResult.Stop;
                    }

                    if (globalVyavastaConfig.schoolLookup.ContainsKey(commArr[1]))
                    {
                        ParticipantName = commArr[0];
                        schoolName = commArr[1];

                        (message.Author as SocketGuildUser).ModifyAsync(m => m.Nickname = $"[{globalVyavastaConfig.schoolLookup[schoolName]}] {ParticipantName}").Wait();

                        var role = houseOut == House.UpperHouse ?(message.Channel as SocketGuildChannel).Guild.Roles.FirstOrDefault(r => r.Name == "Participants [UH]") : (message.Channel as SocketGuildChannel).Guild.Roles.FirstOrDefault(r => r.Name == "Participants [LH]");

                        (message.Author as SocketGuildUser).AddRoleAsync(role);

                        return EventResult.Stop;

                    }
                    else message.Channel.SendError("School Name is invalid, NOTE: We couldn't teach the bot about cases (yet)\nEnter the **exact** school name").Wait();
                }
                else message.Channel.SendError("Enter a valid format\nExample: `Rakesh Sharma - The Psbb Millennium School - Upper House`").Wait();
                



                return EventResult.Stop;
            }


            return EventResult.Continue;
        }

        public static VyavastaConfig globalVyavastaConfig;

        public static void Init(DiscordBot bot)
        {
            bot.MessageReceivedHandler.On("MessageReceived", handle, 0);
            globalVyavastaConfig = VyavastaConfig.load();
        }
    }
}
