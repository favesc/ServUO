using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using System;

namespace Server.Gumps
{
    public class ShowTool : Gump
    {
        private PlayerMobile from;

        public static void Initialize()
        {
            CommandSystem.Register("ShowTool", AccessLevel.Player, OnShowTool);
            EventSink.Login += OnLogin;
            EventSink.PlayerDeath += OnPlayerDeath;
        }

        [Usage("ShowTool")]
        [Description("Display the Player Command ToolBar.")]
        public static void OnShowTool(CommandEventArgs args)
        {

            Mobile mobile = args.Mobile;
            if (mobile.AccessLevel >= AccessLevel.Player)
               
            {
                PlayerMobile from = mobile as PlayerMobile;
                from.CloseGump(typeof(ShowToolMain));
                from.SendGump(new ShowToolMain(from));
            }
        }


        public ShowTool(PlayerMobile pm) : base(0, 15)
        {
            from = pm;
            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = true;
            AddPage(0);
            AddBackground(0, 13, 0x1f, 0x1b, 0x2422);
            AddImageTiled(1, 0x11, 0x19, 0x15, 0x243a);
            AddAlphaRegion(1, 0x11, 0x19, 0x15);

            AddButton(5, 0x11, 0x15a5, 0x15a2, 1, GumpButtonType.Reply, 0);
        }


        private static void OnLogin(LoginEventArgs e)
        {
            if (e.Mobile.AccessLevel >= AccessLevel.Player )
            {
                if (e.Mobile.HasGump(typeof(ShowToolMain)))
                e.Mobile.CloseGump(typeof(ShowToolMain));
                e.Mobile.SendGump(new ShowToolMain((PlayerMobile)e.Mobile));
            }
        }

        private static void OnPlayerDeath(PlayerDeathEventArgs e)
        {

            if (e.Mobile.AccessLevel >= AccessLevel.Player)

            {
                if (e.Mobile.HasGump(typeof(ShowToolMain)))
                    e.Mobile.CloseGump(typeof(ShowToolMain));
                    e.Mobile.SendGump(new ShowToolMain((PlayerMobile)e.Mobile));
            }
        }


        public override void OnResponse(NetState state, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    break;

                case 1:
                    if (from.HasGump(typeof(ShowTool)))
                    from.CloseGump(typeof (ShowTool));
                    from.SendGump(new ShowToolMain(from));
                    break;

                default:
                    break;
            }
        }
    }
}

