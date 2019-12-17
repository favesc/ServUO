using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class ShowToolHouse : Gump
    {
        private PlayerMobile pm;

        public ShowToolHouse(PlayerMobile owner) : base(10, 110)
        {
            pm = owner;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = true;
            AddPage(0);
            AddPage(1);


            AddBackground(0, 0, 150, 360, 5054);
            //           AddImageTiled(10, 10, 130, 340, 0xa8e);
            AddAlphaRegion(10, 10, 130, 340);

            AddButton(12, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddButton(12, 80, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddButton(12, 110, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddButton(12, 140, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddButton(12, 170, 4005, 4007, 5, GumpButtonType.Reply, 0);
            AddButton(12, 200, 4005, 4007, 6, GumpButtonType.Reply, 0);
            //           AddButton(12, 230, 4026, 4027, 7, GumpButtonType.Reply, 0);
            AddButton(12, 260, 4026, 4027, 8, GumpButtonType.Reply, 0);
            AddButton(12, 290, 4037, 4036, 9, GumpButtonType.Reply, 0);
            AddButton(12, 320, 4037, 4036, 10, GumpButtonType.Reply, 0);


            AddButton(0x60, 0x26, 0x900, 0x901, 0, GumpButtonType.Page, 2);

            AddLabel(15, 15, 50, string.Format("存款: {0}", Banker.GetBalance(pm).ToString()));//存款余额

            AddLabel(45, 50, 0x40, "锁定");
            AddLabel(45, 80, 0x40, "释放");
            AddLabel(45, 110, 0x40, "保险");
            AddLabel(45, 140, 0x40, "释保");
            AddLabel(45, 170, 0x40, "Ban");
            AddLabel(45, 200, 0x40, "垃圾桶");
            //            AddLabel(45, 230, 94, "存款余额");

            AddButton(40, 256, 0x2a44, 0x2a58, 8, GumpButtonType.Reply, 0);
            AddLabel(45, 260, 94, "打开保险箱");
            AddButton(40, 286, 0x2a44, 0x2a58, 9, GumpButtonType.Reply, 0);
            AddLabel(45, 290, 49, "取五千块钱");
            AddButton(40, 316, 0x2a44, 0x2a58, 10, GumpButtonType.Reply, 0);
            AddLabel(45, 320, 49, "取两万块钱");

            AddPage(2);
            AddButton(0x60, 0x26, 0x900, 0x901, 0, GumpButtonType.Page, 1);

        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            int[] numArray = new int[] { 0x23 };
            int[] numArray2 = new int[] { 0x24 };
            int[] numArray3 = new int[] { 0x25 };
            int[] numArray4 = new int[] { 0x26 };
            int[] numArray5 = new int[] { 0x34 };
            int[] numArray6 = new int[] { 40 };
            int[] numArray7 = new int[] { 1 };
            int[] numArray8 = new int[] { 2 };
            int[] numArray9 = new int[] { 0x00 };

            switch (info.ButtonID)
            {
                default:
                case 0: { break; }

                case 1:

                    pm.CloseGump(typeof(ShowToolHouse));
                    pm.SendGump(new ShowToolHouse(pm));
                    pm.DoSpeech("*锁定哪个物件？*", numArray, 0, 20);
                    pm.Say("i wish to lock this down");
                    break;
                case 2:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("*释放哪个？物件*", numArray2, 0, 20);
                        pm.Say("i wish to release this"); break;
                    }
                case 3:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("*保险哪个物件？*", numArray3, 0, 20);
                        pm.Say("i wish to secure this"); break;
                    }
                case 4:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("*不保险了？*", numArray4, 0, 20);
                        pm.Say("i wish to unsecure this"); break;
                    }
                case 5:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("*Select who you want to ban*", numArray5, 0, 20);
                        pm.Say("i ban thee"); break;
                    }
                case 6:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("*把垃圾桶放在脚下。*", numArray6, 0, 20);
                        pm.Say("i wish to place a trash barrel"); break;
                    }
                //case 7:
                //    {
                //        pm.CloseGump(typeof(ShowToolHouse));
                //        pm.SendGump(new ShowToolHouse(pm));
                //        pm.DoSpeech(" ", numArray7, 0, 20);
                //        //              pm.Say("i wish to place a trash barrel");
                //        break;
                //    }
                case 8:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech(" ", numArray8, 0, 20); 
                        pm.Say(" open my bank");break;
                    }
                case 9:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("withdraw 5000", numArray9, 0, 20); break;
                        //               pm.Say("withdraw");
                    }
                case 10:
                    {
                        pm.CloseGump(typeof(ShowToolHouse));
                        pm.SendGump(new ShowToolHouse(pm));
                        pm.DoSpeech("withdraw 20000", numArray9, 0, 20); break;
                        //               pm.Say(" withdraw");
                    }
                case 17:
                    {
                        pm.SendMessage("You close the Housing Command Bar.");
                        break;
                    }
            }
        }
    }
}

