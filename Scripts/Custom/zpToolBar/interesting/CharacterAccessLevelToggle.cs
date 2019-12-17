/*
 * Created by SharpDevelop.
 * User:    steamin
 * Date:    20.06.2004
 * Time:    02:04
 * Version: 1.2.0
 * Edited by Liacs and David for RunUO 2.0 compatibility 6/18/06
 * 
 */
/// 
/// Examples:
/// CALT
/// Switch character with a higher level to a player
/// or switch a player access level to account level.
/// </summary>

using System;
using Server;
using Server.Accounting;
using Server.Commands;

namespace Server.Misc
{

    public class CharacterAccessLevelToggle
    {
        public static void Initialize()
        {
            CommandSystem.Register("CharacterAccessLevelToggle", AccessLevel.Player, new CommandEventHandler(CharacterAccessLevelToggle_OnCommand));
            CommandSystem.Register("CALT", AccessLevel.Player, new CommandEventHandler(CharacterAccessLevelToggle_OnCommand));
        }

        [Usage("CALT [<NewLevel>]")]
        [Description("Toggles a Character's Access Level from Player to the level of the characters account. "
                     + "<NewLevel> may be Player, Counselor, Game Master, GM, Seer, Administrator, Developer, dev, Owner, 1,2,3,4,5,6")]
        public static void CharacterAccessLevelToggle_OnCommand(CommandEventArgs e)
        {
            try
            {
                Mobile m_Mob = e.Mobile;
                AccessLevel al_MobLevel = m_Mob.AccessLevel;
                Account a_Account = (Account)m_Mob.Account;
                AccessLevel al_AccLevel = a_Account.AccessLevel;
                if (!m_Mob.Player || al_AccLevel == AccessLevel.Player)
                {
                    e.Mobile.SendMessage("You no not have access to that command.");
                    return;
                }

                AccessLevel al_NewLevel;
                switch (e.Length)
                {
                    case 1:
                        switch (e.GetString(0).Trim().ToLower())
                        {
                            case "player":
                            case "0":
                                al_NewLevel = AccessLevel.Player;
                                break;
                            case "counselor":
                            case "1":
                                al_NewLevel = AccessLevel.Counselor;
                                break;
                            case "game master":
                            case "gm":
                            case "2":
                                al_NewLevel = AccessLevel.GameMaster;
                                break;
                            case "seer":
                            case "3":
                                al_NewLevel = AccessLevel.Seer;
                                break;
                            case "administrator":
                            case "4":
                                al_NewLevel = AccessLevel.Administrator;
                                break;
                            case "developer":
                            case "dev":
                            case "5":
                                al_NewLevel = AccessLevel.Developer;
                                break;
                            case "owner":
                            case "6":
                                al_NewLevel = AccessLevel.Owner;
                                break;
                            default:
                                e.Mobile.SendMessage("Wrong Parameter: " + e.GetString(0));
                                return;
                        }
                        break;
                    case 0:
                        if (al_MobLevel == AccessLevel.Player)
                        {
                            al_NewLevel = al_AccLevel;
                        }
                        else
                        {
                            al_NewLevel = AccessLevel.Player;
                        }
                        break;
                    default:
                        e.Mobile.SendMessage("Usage: CALT [<NewLevel>]");
                        return;
                }

                if (al_NewLevel > al_AccLevel)
                    al_NewLevel = al_AccLevel;

                m_Mob.AccessLevel = al_NewLevel;
            }
            catch (Exception err)
            {
                e.Mobile.SendMessage("Exception: " + err.Message);
            }
        }
    }
}