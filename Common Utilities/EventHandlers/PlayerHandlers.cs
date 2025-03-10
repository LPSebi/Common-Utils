using System.Collections.Specialized;

namespace Common_Utilities.EventHandlers;

#pragma warning disable IDE0018
using System;
using System.Collections.Generic;
using System.Linq;
using ConfigObjects;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

using Player = Exiled.API.Features.Player;

public class PlayerHandlers
{
    private Config config => Plugin.Instance.Config;
    
    public void OnPlayerVerified(VerifiedEventArgs ev)
    {
<<<<<<< HEAD
        private readonly Main plugin;

        public PlayerHandlers(Main plugin) => this.plugin = plugin;

        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            string message = FormatJoinMessage(ev.Player);
            if (!string.IsNullOrEmpty(message))
                ev.Player.Broadcast(plugin.Config.JoinMessageDuration, message);
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null)
            {
                Log.Warn($"{nameof(OnChangingRole)}: Triggering player is null.");
                return;
            }

            if (plugin.Config.StartingInventories.ContainsKey(ev.NewRole) && !ev.ShouldPreserveInventory)
            {
                if (ev.Items == null)
                {
                    Log.Warn("items is null");
                    return;
                }

                ev.Items.Clear();
                ev.Items.AddRange(StartItems(ev.NewRole, ev.Player));

                if (plugin.Config.StartingInventories[ev.NewRole].Ammo != null && plugin.Config.StartingInventories[ev.NewRole].Ammo.Count > 0)
                {
                    if (plugin.Config.StartingInventories[ev.NewRole].Ammo.Any(s => string.IsNullOrEmpty(s.Group) || s.Group == "none" || (ServerStatic.PermissionsHandler._groups.TryGetValue(s.Group, out UserGroup userGroup) && userGroup == ev.Player.Group)))
                    {
                        ev.Ammo.Clear();
                        foreach ((ItemType type, ushort amount, string group) in plugin.Config.StartingInventories[ev.NewRole].Ammo)
                        {
                            if (string.IsNullOrEmpty(group) || group == "none" || (ServerStatic.PermissionsHandler._groups.TryGetValue(group, out UserGroup userGroup) && userGroup == ev.Player.Group))
                            {
                                ev.Ammo.Add(type, amount);
                            }
                        }
                    }
                }
            }
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player == null)
            {
                Log.Warn($"{nameof(OnSpawned)}: Triggering player is null.");
                return;
            }

            RoleTypeId newRole = ev.Player.Role.Type;
            if (plugin.Config.HealthValues != null && plugin.Config.HealthValues.TryGetValue(newRole, out int health))
            {
                ev.Player.Health = health;
                ev.Player.MaxHealth = health;
            }

            if (ev.Player.Role is FpcRole && plugin.Config.PlayerHealthInfo)
            {
                ev.Player.CustomInfo = $"({ev.Player.Health}/{ev.Player.MaxHealth}) {(!string.IsNullOrEmpty(ev.Player.CustomInfo) ? ev.Player.CustomInfo.Substring(ev.Player.CustomInfo.LastIndexOf(')') + 1) : string.Empty)}";
            }

            if (plugin.Config.AfkIgnoredRoles.Contains(newRole) && plugin.AfkDict.TryGetValue(ev.Player, out Tuple<int, Vector3> value))
                plugin.AfkDict[ev.Player] = new Tuple<int, Vector3>(newRole is RoleTypeId.Spectator ? value.Item1 : 0, ev.Player.Position);
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null && plugin.Config.HealthOnKill.ContainsKey(ev.Attacker.Role))
            {
                ev.Attacker.Heal(plugin.Config.HealthOnKill[ev.Attacker.Role]);
            }
        }

        public List<ItemType> StartItems(RoleTypeId role, Player player = null)
        {
            List<ItemType> items = new();

            for (int i = 0; i < plugin.Config.StartingInventories[role].UsedSlots; i++)
            {
                IEnumerable<ItemChance> itemChances = plugin.Config.StartingInventories[role][i].Where(x => player == null || string.IsNullOrEmpty(x.Group) || x.Group == "none" || (player.Group != null && x.Group.Contains(player.GroupName)) || (ServerStatic.PermissionsHandler._groups.TryGetValue(x.Group, out var group) && group == player.Group));
                double r;
                if (plugin.Config.AdditiveProbabilities)
                    r = plugin.Rng.NextDouble() * itemChances.Sum(val => val.Chance);
                else
                    r = plugin.Rng.NextDouble() * 100;
                Log.Debug($"[StartItems] ActualChance ({r})/{itemChances.Sum(val => val.Chance)}");
                foreach ((string item, double chance, string groupKey) in itemChances)
                {
                    Log.Debug($"[StartItems] Probability ({r})/{chance}");
                    if (r <= chance)
                    {
                        if (Enum.TryParse(item, true, out ItemType type))
                        {
                            items.Add(type);
                            break;
                        }
                        else if (CustomItem.TryGet(item, out CustomItem customItem))
                        {
                            if (player != null)
                                customItem!.Give(player);
                            else
                                Log.Warn($"{nameof(StartItems)}: Tried to give {customItem!.Name} to a null player.");
                            
                            break;
                        }
                        else
                            Log.Warn($"{nameof(StartItems)}: {item} is not a valid ItemType or it is a CustomItem that is not installed! It is being skipper in inventory decisions.");
                    }

                    r -= chance;
                }
            }

            return items;
        }

        public string FormatJoinMessage(Player player) => 
            string.IsNullOrEmpty(plugin.Config.JoinMessage) ? string.Empty : plugin.Config.JoinMessage.Replace("%player%", player.Nickname).Replace("%server%", Server.Name).Replace("%count%", $"{Player.Dictionary.Count}");
=======
        if (ev.Player is null)
            return;
>>>>>>> 5c9cf829970faac7db0ea545dc34b3d2c226c3a4
        
        string message = FormatJoinMessage(ev.Player);
        
        if (!string.IsNullOrEmpty(message))
            ev.Player.Broadcast(config.JoinMessageDuration, message);
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (ev.Player == null)
        {
            Log.Warn($"{nameof(OnChangingRole)}: Triggering player is null.");
            return;
        }
        
        // no clue why this works while in ChangingRole instead of spawned but if it ain't broke don't fix it
        // answering my previous question, (obviously) it works because we're setting ev.Items which are yet to be given to the player
        if (config.StartingInventories.ContainsKey(ev.NewRole) && !ev.ShouldPreserveInventory)
        {
            if (ev.Items == null)
            {
                Log.Warn("items is null");
                return;
            }
            
            ev.Items.Clear();
            ev.Items.AddRange(GetStartingInventory(ev.NewRole, ev.Player));

            if (config.StartingInventories[ev.NewRole].Ammo == null || config.StartingInventories[ev.NewRole].Ammo.Count <= 0) 
                return;
            
            if (config.StartingInventories[ev.NewRole].Ammo.Any(s => string.IsNullOrEmpty(s.Group) || s.Group == "none" || (ServerStatic.PermissionsHandler._groups.TryGetValue(s.Group, out UserGroup userGroup) && userGroup == ev.Player.Group)))
            {
                ev.Ammo.Clear();
                foreach ((ItemType type, ushort amount, string group) in config.StartingInventories[ev.NewRole].Ammo)
                {
                    if (string.IsNullOrEmpty(group) || group == "none" || (ServerStatic.PermissionsHandler._groups.TryGetValue(group, out UserGroup userGroup) && userGroup == ev.Player.Group))
                    {
                        ev.Ammo.Add(type, amount);
                    }
                }
            }
        }
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player is null)
        {
            Log.Warn($"{nameof(OnSpawned)}: Triggering player is null.");
            return;
        }

        RoleTypeId newRole = ev.Player.Role.Type;
        if (config.HealthValues is not null && config.HealthValues.TryGetValue(newRole, out int health))
        {
            ev.Player.MaxHealth = health;
            ev.Player.Health = health;
        }

        if (ev.Player.Role is FpcRole && config.PlayerHealthInfo)
        {
            ev.Player.CustomInfo = $"({ev.Player.Health}/{ev.Player.MaxHealth}) {(!string.IsNullOrEmpty(ev.Player.CustomInfo) ? ev.Player.CustomInfo.Substring(ev.Player.CustomInfo.LastIndexOf(')') + 1) : string.Empty)}";
        }

        if (config.AfkIgnoredRoles is not null && config.AfkIgnoredRoles.Contains(newRole) && Plugin.AfkDict.TryGetValue(ev.Player, out Tuple<int, Vector3> value))
        {
            Plugin.AfkDict[ev.Player] =
                new Tuple<int, Vector3>(newRole is RoleTypeId.Spectator ? value.Item1 : 0, ev.Player.Position);
        }
    }

    public void OnPlayerDied(DiedEventArgs ev)
    {
        if (ev.Attacker is not null && config.HealthOnKill is not null && config.HealthOnKill.ContainsKey(ev.Attacker.Role))
        {
            ev.Attacker.Heal(config.HealthOnKill[ev.Attacker.Role]);
        }
    }
        
    public void OnPlayerHurting(HurtingEventArgs ev)
    {
        if (config.RoleDamageDealtMultipliers is not null && ev.Attacker is not null && config.RoleDamageDealtMultipliers.TryGetValue(ev.Attacker.Role, out var damageMultiplier))
            ev.Amount *= damageMultiplier;

        if (config.RoleDamageReceivedMultipliers is not null && config.RoleDamageReceivedMultipliers.TryGetValue(ev.Player.Role, out damageMultiplier))
            ev.Amount *= damageMultiplier;
        
        if (config.DamageMultipliers is not null && config.DamageMultipliers.TryGetValue(ev.DamageHandler.Type, out damageMultiplier))
            ev.Amount *= damageMultiplier;

        if (config.PlayerHealthInfo)
            ev.Player.CustomInfo = $"({ev.Player.Health}/{ev.Player.MaxHealth}) {(!string.IsNullOrEmpty(ev.Player.CustomInfo) ? ev.Player.CustomInfo.Substring(ev.Player.CustomInfo.LastIndexOf(')') + 1) : string.Empty)}";

        if (ev.Attacker is not null && Plugin.AfkDict.ContainsKey(ev.Attacker))
        {
            Log.Debug($"Resetting {ev.Attacker.Nickname} AFK timer.");
            Plugin.AfkDict[ev.Attacker] = new Tuple<int, Vector3>(0, ev.Attacker.Position);
        }
    }

    public void OnEscaping(EscapingEventArgs ev)
    {
        if (ev.Player.IsCuffed && config.DisarmedEscapeSwitchRole is not null && config.DisarmedEscapeSwitchRole.TryGetValue(ev.Player.Role, out RoleTypeId newRole))
        {
            ev.NewRole = newRole;
            ev.IsAllowed = newRole != RoleTypeId.None;
        }
    }

    public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
    {
        ev.Drain *= config.RadioBatteryDrainMultiplier;
    }

    public void AntiAfkEventHandler(IPlayerEvent ev)
    {
        if (ev.Player != null && Plugin.AfkDict.ContainsKey(ev.Player))
        {
            Log.Debug($"Resetting {ev.Player.Nickname} AFK timer.");
            Plugin.AfkDict[ev.Player] = new Tuple<int, Vector3>(0, ev.Player.Position);
        }
    }
        
    public List<ItemType> GetStartingInventory(RoleTypeId role, Player player = null)
    {
        List<ItemType> items = new();
        
        Log.Debug($"{nameof(GetStartingInventory)} Iterating through slots...");
        // iterate through slots
        for (int i = 0; i < config.StartingInventories[role].UsedSlots; i++)
        {
            Log.Debug($"\n{nameof(GetStartingInventory)} Iterating slot {i + 1}");
            Log.Debug($"{nameof(GetStartingInventory)} Checking groups...");
            
            // item chances for that slot
            List<ItemChance> itemChances = config.StartingInventories[role][i]
                .Where(x => 
                    player == null 
                    || string.IsNullOrEmpty(x.Group) 
                    || x.Group == "none" 
                    || (ServerStatic.PermissionsHandler._groups.TryGetValue(x.Group, out var group) && group == player.Group))
                .ToList();

            Log.Debug($"{nameof(GetStartingInventory)} Finished checking groups, found {itemChances.Count} valid itemChances.");
            
            double rolledChance = Utils.RollChance(itemChances);
            
            foreach ((string item, double chance) in itemChances)
            {
                Log.Debug($"{nameof(GetStartingInventory)} Trying to assign to slot {i + 1} for {role}; item: {item}; {rolledChance} <= {chance} ({rolledChance <= chance}).");
               
                if (rolledChance <= chance)
                {
                    if (Enum.TryParse(item, true, out ItemType type))
                    {
                        items.Add(type);
                        break;
                    }

                    if (CustomItem.TryGet(item, out CustomItem customItem))
                    {
                        if (player != null)
                            customItem!.Give(player);
                        else
                            Log.Warn($"{nameof(GetStartingInventory)} Tried to give {customItem!.Name} to a null player.");
                            
                        break;
                    }

                    Log.Warn($"{nameof(GetStartingInventory)} Skipping {item} as it is not a valid ItemType or CustomItem!");
                }

                if (config.AdditiveProbabilities) 
                    rolledChance -= chance;
            }
        }
        
        Log.Debug($"{nameof(GetStartingInventory)} Finished iterating slots.");

        return items;
    }
    
    private string FormatJoinMessage(Player player)
    {
        return 
            string.IsNullOrEmpty(config.JoinMessage) 
                ? string.Empty 
                : config.JoinMessage
                    .Replace("%player%", player.Nickname)
                    .Replace("%server%", Server.Name)
                    .Replace("%count%", $"{Player.Dictionary.Count}");
    }
}