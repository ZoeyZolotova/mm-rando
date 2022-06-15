using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MMR.DiscordBot.Data.Entities;
using MMR.DiscordBot.Data.Repositories;
using MMR.DiscordBot.Services;
using MMR.Common.Extensions;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using MMR.Common.Utils;

namespace MMR.DiscordBot.Modules
{
    [Group("mmr")]
    public class MMRModule : ModuleBase<SocketCommandContext>
    {
        public UserSeedRepository UserSeedRepository { get; set; }
        public GuildModRepository GuildModRepository { get; set; }
        public MMRService MMRService { get; set; }
        public AsyncGenerationService AsyncGenerationService { get; set; }

        private readonly IReadOnlyCollection<ulong> _tournamentChannels = new List<ulong>
        {
            709731024375906316, // ZoeyZolotova - tournament-admin
            744581433481232425, // MMR - bracket-seeds
        }.AsReadOnly();

        [Command("help")]
        public async Task Help()
        {
            var commands = new Dictionary<string, string>()
            {
                {  "help", "See this help list." },
            };

            if (_tournamentChannels.Contains(Context.Channel.Id))
            {
                commands.Add("seed (<@user>){2,}", "Generate a seed. The patch and hashIcons will be sent in a direct message to the tagged users. The spoiler log will be sent to you.");
            }
            else
            {
                if (Context.IsPrivate)
                {
                    commands.Add("seed", "Generate a seed.");
                }
                else
                {
                    commands.Add("seed (<settingName>)?", "Generate a seed. Optionally provide a setting name.");
                }
                commands.Add("spoiler", "Retrieve the spoiler log for your last generated seed.");
            }

            if (!Context.IsPrivate && Context.User is SocketGuildUser socketGuildUser)
            {
                if (socketGuildUser.GuildPermissions.Has(GuildPermission.Administrator))
                {
                    commands.Add("add-mod-role (<@role>)+", "Allow the tagged role(s) to use the setting commands.");
                    commands.Add("remove-mod-role (<@role>)+", "Disallow the tagged role(s) from using the setting commands.");
                }
                var allowedRoles = await GuildModRepository.ListByGuildId(Context.Guild.Id);
                var allowedRoleIds = allowedRoles.Select(r => r.RoleId);
                if (socketGuildUser.Roles.Any(sr => allowedRoleIds.Contains(sr.Id)))
                {
                    commands.Add("add-settings <settingName>", "Upload a settings file and name it.");
                    commands.Add("remove-settings <settingName>", "Remove a named setting.");
                }
                commands.Add("list-settings", "List the names of available settings.");
            }

            await ReplyAsync("List of commands: (all commands begin with \"!mmr\")\n" + string.Join('\n', commands.Select(kvp => $"`{kvp.Key}` - {kvp.Value}")));
        }

        [Command("seed")]
        public async Task Seed([Remainder] string settingName = null)
        {
            if (_tournamentChannels.Contains(Context.Channel.Id))
            {
                // Tournament seed
                var mentionedUsers = Context.Message.MentionedUsers.DistinctBy(u => u.Id);
                if (mentionedUsers.Any(u => u.Id == Context.User.Id))
                {
                    await ReplyAsync("Cannot generate a seed for yourself.");
                    return;
                }
                if (mentionedUsers.Count() < 2)
                {
                    await ReplyAsync("Must mention at least two users.");
                    return;
                }
                var tournamentSeedReply = await ReplyAsync("Generating seed...");
                new Thread(async () =>
                {
                    try
                    {
                        var seed = (await MMRService.GetSeed()).Single();
                        var filenameWithoutExtension = FileUtils.MakeFilenameValid(DateTime.UtcNow.ToString("o"));
                        (var patchPath, var hashIconPath, var spoilerLogPath) = await MMRService.GenerateSeed(filenameWithoutExtension, null, seed);
                        foreach (var user in mentionedUsers)
                        {
                            await user.SendFileAsync(patchPath, "Here is your tournament match seed! Please be sure your Hash matches and let an organizer know if you have any issues before you begin.");
                            await user.SendFileAsync(hashIconPath);
                        }
                        await Context.User.SendFileAsync(spoilerLogPath);
                        await Context.User.SendFileAsync(hashIconPath);
                        File.Delete(spoilerLogPath);
                        File.Delete(patchPath);
                        File.Delete(hashIconPath);
                        await tournamentSeedReply.ModifyAsync(mp => mp.Content = "Success.");
                    }
                    catch (Exception e)
                    {
                        // TODO log exception.
                        await tournamentSeedReply.ModifyAsync(mp => mp.Content = "An error occured.");
                    }
                }).Start();
                return;
            }
            var lastSeedRequest = (await UserSeedRepository.GetById(Context.User.Id))?.LastSeedRequest;
            if (lastSeedRequest.HasValue && (DateTime.UtcNow - lastSeedRequest.Value).TotalHours < 6)
            {
                await ReplyAsync("You may only request a seed once every 6 hours.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(settingName))
            {
                settingName = MMRService.GetSettingsPath(Context.Guild.Id, settingName);
                if (!File.Exists(settingName))
                {
                    await ReplyAsync("Setting not found.");
                    return;
                }
            }
            var now = DateTime.UtcNow;
            await UserSeedRepository.Save(new UserSeedEntity
            {
                UserId = Context.User.Id,
                LastSeedRequest = now
            });
            var messageResult = await ReplyAsync("Generating seed...");
            new Thread(async () =>
            {
                try
                {
                    var seed = (await MMRService.GetSeed()).Single();
                    var filenameWithoutExtension = FileUtils.MakeFilenameValid(now.ToString("o"));
                    (var patchPath, var hashIconPath, var spoilerLogPath) = await MMRService.GenerateSeed(filenameWithoutExtension, settingName, seed);
                    await Context.Channel.SendFileAsync(patchPath);
                    await Context.Channel.SendFileAsync(hashIconPath);
                    File.Delete(patchPath);
                    File.Delete(hashIconPath);
                    await messageResult.DeleteAsync();
                }
                catch
                {
                    await UserSeedRepository.DeleteById(Context.User.Id);
                    await messageResult.ModifyAsync(mp => mp.Content = "An error occured.");
                }
            }).Start();
        }

        [Command("spoiler")]
        public async Task Spoiler()
        {
            if (_tournamentChannels.Contains(Context.Channel.Id))
            {
                // Silently ignore.
                return;
            }
            
            var lastSeedRequest = (await UserSeedRepository.GetById(Context.User.Id))?.LastSeedRequest;
            if (!lastSeedRequest.HasValue)
            {
                await ReplyAsync("You haven't generated any seeds recently.");
                return;
            }
            var filenameWithoutExtension = FileUtils.MakeFilenameValid(lastSeedRequest.Value.ToString("o"));
            var spoilerLogFilename = MMRService.GetSpoilerLogPath(filenameWithoutExtension);
            if (File.Exists(spoilerLogFilename))
            {
                var result = await Context.Channel.SendFileAsync(spoilerLogFilename);
                File.Delete(spoilerLogFilename);
            }
            else
            {
                await ReplyAsync("Spoiler log not found.");
            }
        }

        [Command("add-mod-role")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddModRole(IRole role)
        {
            var guildMod = await GuildModRepository.Single(gm => gm.RoleId == role.Id && gm.GuildId == Context.Guild.Id);
            if (guildMod != null)
            {
                await ReplyAsync("Role is already a mod role.");
                return;
            }
            await GuildModRepository.Save(new GuildModEntity
            {
                GuildId = Context.Guild.Id,
                RoleId = role.Id,
            });
            await ReplyAsync("Added");
        }

        [Command("remove-mod-role")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveModRole(IRole role)
        {
            var guildMod = await GuildModRepository.Single(gm => gm.RoleId == role.Id && gm.GuildId == Context.Guild.Id);
            if (guildMod == null)
            {
                await ReplyAsync("Role is already not a mod role.");
                return;
            }
            await GuildModRepository.DeleteById(guildMod.Id);
            await ReplyAsync("Removed");
        }

        [Command("add-settings")]
        [RequireContext(ContextType.Guild)]
        public async Task AddSettings([Remainder] string settingName)
        {
            var allowedRoles = await GuildModRepository.ListByGuildId(Context.Guild.Id);
            var allowedRoleIds = allowedRoles.Select(r => r.RoleId);
            var socketGuildUser = Context.User as SocketGuildUser;
            if (!socketGuildUser.Roles.Any(sr => allowedRoleIds.Contains(sr.Id)))
            {
                await ReplyAsync("You don't have permission.");
                return;
            }
            //await ReplyAsync("Allowed");

            if (Context.Message.Attachments.Count != 1)
            {
                await ReplyAsync("Must attach one settings json file.");
                return;
            }

            var settingsFile = Context.Message.Attachments.Single();
            if (settingsFile.Size > 10000) // kinda arbitrary
            {
                await ReplyAsync("File is too large.");
                return;
            }

            if (Path.GetExtension(settingsFile.Filename) != ".json")
            {
                await ReplyAsync("File must be a json file.");
                return;
            }

            var replacing = false;
            var settingsPath = MMRService.GetSettingsPath(Context.Guild.Id, settingName);
            if (File.Exists(settingsPath))
            {
                replacing = true;
            }

            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(settingsFile.Url), settingsPath);
            }

            await ReplyAsync($"{(replacing ? "Replaced" : "Added")} settings.");
        }

        [Command("remove-settings")]
        [RequireContext(ContextType.Guild)]
        public async Task RemoveSettings([Remainder] string settingName)
        {
            var allowedRoles = await GuildModRepository.ListByGuildId(Context.Guild.Id);
            var allowedRoleIds = allowedRoles.Select(r => r.RoleId);
            var socketGuildUser = Context.User as SocketGuildUser;
            if (!socketGuildUser.Roles.Any(sr => allowedRoleIds.Contains(sr.Id)))
            {
                await ReplyAsync("You don't have permission.");
                return;
            }
            //await ReplyAsync("Allowed");

            var settingsPath = MMRService.GetSettingsPath(Context.Guild.Id, settingName);
            if (!File.Exists(settingsPath))
            {
                await ReplyAsync("Setting does not exist.");
                return;
            }

            File.Delete(settingsPath);

            await ReplyAsync("Deleted settings.");
        }


        [Command("list-settings")]
        [RequireContext(ContextType.Guild)]
        public async Task ListSettings()
        {
            //var allowedRoles = await GuildModRepository.ListByGuildId(Context.Guild.Id);
            //var allowedRoleIds = allowedRoles.Select(r => r.RoleId);
            //var socketGuildUser = Context.User as SocketGuildUser;
            //if (!socketGuildUser.Roles.Any(sr => allowedRoleIds.Contains(sr.Id)))
            //{
            //    // Silently disallow.
            //    //await ReplyAsync("Not Allowed");
            //    return;
            //}
            //await ReplyAsync("Allowed");

            var settingsPaths = MMRService.GetSettingsPaths(Context.Guild.Id);

            if (!settingsPaths.Any())
            {
                await ReplyAsync("No settings found.");
                return;
            }

            await ReplyAsync("List of settings:\n" + string.Join('\n', settingsPaths.Select(p => Path.GetFileNameWithoutExtension(p))));
        }

        [Command("async-generate")]
        [RequireOwner]
        public async Task AsyncGenerate()
        {
            var message = await ReplyAsync("Generating Async Seeds");
            var (sheetLink, oldSheetDate) = await AsyncGenerationService.GenerateAsyncSheet((progressMessage) =>
            {
                return message.ModifyAsync(mp => mp.Content = progressMessage);
            });

            await message.DeleteAsync();

            // post link to new sheet on discord
            var postMessage =
@$"{MentionUtils.MentionRole(630531640480890930)}
**{DateTime.UtcNow.ToString("MMMM yyyy")} Asyncs**
{sheetLink}";
            if (oldSheetDate.HasValue)
            {
                postMessage += $"\n\nSpoiler Logs for {oldSheetDate.Value.ToString("MMMM yyyy")} Asyncs are now available.";
            }

            await ReplyAsync(postMessage);
        }
    }
}
