using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Commands;
using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Gumps
{
    public class ShowToolGhost : Gump
    {
        private PlayerMobile from;
        public static void Initialize()
        {
            CommandSystem.Register("ff", AccessLevel.Player, new CommandEventHandler(ff_OnCommand));
        } 
        [Usage("ff")]
        [Description("Ghost can return to town Haven.")]
        public static void ff_OnCommand(CommandEventArgs e)
        {
            PlayerMobile from = (PlayerMobile)e.Mobile;

            e.Mobile.SendGump(new ShowToolGhost(from));
        }


        public ShowToolGhost(PlayerMobile owner) : base(90, 30)
        {
            from = owner;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = true;
            base.AddPage(0);
            base.AddBackground(0x7a, 0x70, 0x156, 0x132, 0x23f0);
            base.AddImageTiled(0x85, 0x7a, 0x13e, 0x11d, 0xa8e);
            base.AddAlphaRegion(0x85, 0x7a, 0x13e, 0x11d);
            base.AddImage(0x1b0, 70, 0x28c9);
            base.AddImage(0x48, 70, 0x28c8);
            base.AddImage(0xca, 0x42, 0x28d4);
            /*
            base.AddLabel(0xd7, 0x80, 0x40, "Welcome to the Dream World!");
            base.AddLabel(150, 0xb9, 0x40, "Even though I am not sure why you are dead,");
            base.AddLabel(150, 0xd7, 0x40, "you placed a 911 call, so God will save you.");
            base.AddLabel(150, 0xf5, 0x40, "Say the word, and I can help.");
            base.AddLabel(150, 0x113, 0x40, "Although you are dead, don't be afraid to return.");
            base.AddButton(0x87, 0x13b, 0xfa5, 0xfa6, 1, GumpButtonType.Reply, 0);
            base.AddLabel(0xa5, 0x13b, 0x40, "Return to town!");
            base.AddButton(0x87, 0x16d, 0xfa5, 0xfa6, 2, GumpButtonType.Reply, 0);
            base.AddLabel(0xa5, 0x16d, 0x40, "Cancel!");
            */
            base.AddLabel(240, 0x80, 23, " 你已然挂了！");
            base.AddLabel(170, 0xb9, 0x40, " 虽然我不知道你是怎么挂的，");
            base.AddLabel(170, 0xd7, 0x40, " 但幸好你发现了这个按钮，");
            base.AddLabel(170, 0xf5, 0x40, " 多做点好事，上帝将拯救你。");
            base.AddLabel(170, 0x113, 0x40, " 那么，先回城再做打算吗？");
            base.AddButton(150, 0x13b, 0xfa5, 0xfa6, 1, GumpButtonType.Reply, 0);
            base.AddLabel(185, 0x13b, 0x40, " Return to town!");
            base.AddButton(150, 0x16d, 0xfa5, 0xfa6, 2, GumpButtonType.Reply, 0);
            base.AddLabel(185, 0x16d, 0x40, " Cancel!");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    break;

                case 1:
                    if (from.Alive)
                    {
                        from.SendMessage("You are already alive!");
                        break;
                    }
                    if (from.Region is Jail)
                    {
                        from.SendMessage("You cannot do this from Jail!");
                        break;

                    }
                    if (from.Kills >= 5)
                    {
                        from.Map = Map.Felucca;
                        from.Location = new Point3D(0x5cb, 0x64c, 20);
                        break;
                    }
                    Point3D loc=new Point3D(3502, 2568, 14);
                    Map map = Map.Trammel;
                    BaseCreature.TeleportPets(from, loc, map, true);
                    from.MoveToWorld(loc,map);

                    break;

                case 2:
                    from.SendMessage("You can't do that yet!");
                    break;

                default:
                    break;
            }
        }
    }
}

