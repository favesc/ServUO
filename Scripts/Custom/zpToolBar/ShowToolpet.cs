using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class ShowToolpet : Gump
    {
        private PlayerMobile pm;

        public ShowToolpet(PlayerMobile owner) : base(100, 100)
        {
            pm = owner;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = true;
            AddPage(0);
            AddPage(1);

            AddBackground(0, 0, 130, 380, 3500);//5054
            //            AddImageTiled(10, 10, 130, 340, 0xa8e);
            AddAlphaRegion(10, 10, 110, 360);
            //            AddItem(0x60, 0x26, 0x21f1);
            AddButton(20, 320, 0x759d, 0x7597, 00, GumpButtonType.Page, 2);
            AddButton(70, 325, 0x758d, 0x758e, 0x17, GumpButtonType.Reply, 0);

            AddButton(85, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddButton(85, 80, 4005, 4007, 2, GumpButtonType.Reply, 0);
            //           AddButton(15, 110, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddButton(85, 140, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddButton(85, 170, 4005, 4007, 5, GumpButtonType.Reply, 0);
            AddButton(85, 200, 4005, 4007, 6, GumpButtonType.Reply, 0);
            AddButton(85, 230, 4005, 4007, 7, GumpButtonType.Reply, 0);
            AddButton(85, 260, 4005, 4007, 8, GumpButtonType.Reply, 0);
            AddButton(85, 290, 4005, 4007, 9, GumpButtonType.Reply, 0);

            AddLabel(23, 15, 0x4f0, "宠物控制");

            AddLabel(15, 50, 0x40, "警戒一一");
            AddLabel(15, 80, 0x40, "跟随一一");
            //           AddLabel(50, 110, 0x40, "丟棄");
            AddLabel(15, 140, 0x40, "攻击一一");
            AddLabel(15, 170, 0x40, "停止一一");
            AddLabel(15, 200, 0x40, "别动一一");
            AddLabel(15, 230, 0x40, "过来一一");
            AddLabel(15, 260, 94, "寄存宠物");
            AddLabel(15, 290, 94, "领取宠物");

            AddPage(2);
            AddButton(20, 320, 0x759d, 0x7597, 0x16, GumpButtonType.Page, 1);

        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            int[] numArray1 = new int[] { 0x16b };
            int[] numArray2 = new int[] { 0x16c };
            int[] numArray3 = new int[] { 0x156 };
            int[] numArray4 = new int[] { 360 };
            int[] numArray5 = new int[] { 0x167 };
            int[] numArray6 = new int[] { 0x170 };
            int[] numArray7 = new int[] { 0x164 };
            int[] numArray8 = new int[] { 8 };
            int[] numArray9 = new int[] { 9 };
            //int[] numarray = new int[] { 0x16b, 0x16c, 0x156, 360, 0x167, 0x170, 0x164, 8, 9, };
            switch (info.ButtonID)
            {
                default:
                case 0: { break; }
                case 1:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*保护我，宝贝..*", numArray1, 0, 20); break;
                        //pm.DoSpeech("*保护我，宝贝..*", new int [numarray[1]], 0, 20);
                        //pm.Say("all guard me");
                    }
                case 2:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*跟我走，宝贝..*", numArray2, 0, 20); break;
                        //pm.Say("all follow me");
                    }
                case 3:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*卸载..*", numArray3, 0, 20);
                        pm.Say("all drop"); break;
                    }
                case 4:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*上吧！..*", numArray4, 0, 20); break;
                        //pm.Say("all kill");
                    }
                case 5:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*停止!!*", numArray5, 0, 20); break;
                        //pm.Say("all stop");
                    }
                case 6:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*别动!*", numArray6, 0, 20); break;
                        //pm.Say("all stay");
                    }
                case 7:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*过来，宝贝!*", numArray7, 0, 20); break;
                        //pm.Say("all come");
                    }
                case 8:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*麻烦你 帮我好好照顾它.*", numArray8, 0, 20);
                        pm.Say("stable"); break;
                    }
                case 9:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        pm.DoSpeech("*取回我的宝贝!* ", numArray9, 0, 20);
                        pm.Say("claim pet");
                        break;
                    }
                case 0x16:
                    {
                        pm.CloseGump(typeof(ShowToolpet));
                        pm.SendGump(new ShowToolpet(pm));
                        break;
                        //pm.SendMessage("*关闭宠物控制界面.*"); 
                    }

                case 0x17:
                    {


                        pm.SendMessage("*关闭宠物控制界面.*");
                        break;
                    }
            }
        }
    }
}
