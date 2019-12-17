using Server.Network;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Gumps;
using System;

namespace Server.Gumps
{
    public class ppTestGump : Gump
    {
        private Mobile ct;
        private TextRelay EX;
        static int it;/// <summary>
        /// 本cs主要学习了try catch finally 语句。
        /// try 尝试执行会产生异常的条件；
        /// catch 抛出错误；
        /// finally 不管try catch 有没有return ，最终都会执行。
        /// </summary>
        public static void Initialize()
        {

            CommandSystem.Register("pp", AccessLevel.Administrator, new CommandEventHandler(pp_OnCommand));

        }

        [Usage("pp")]
        [Description("Makes a call to your custom gump.")]
        public static void pp_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new cTarget();

        }
        private class cTarget : Target
        {
            public cTarget() : base(12, false, TargetFlags.None)
            {
            }
            protected override void OnTarget(Mobile from, object target)
            {

                if (target is BaseCreature)
                {
                    BaseCreature c = (BaseCreature)target;

                    if (c.Body.IsAnimal || c.Body.IsMonster || c.Body.IsSea || c.Controlled || c.Body.IsHuman)
                    {
                        if (from.HasGump(typeof(ppTestGump))) from.CloseGump(typeof(ppTestGump));
                        from.SendGump(new ppTestGump(c));
                        return;
                    }
                }
                else from.SendMessage("That is not accessible.it is not an animal.");
                return;
            }
        }
        //private int itemid;
        public ppTestGump(Mobile c) : this(c, 3)
        { }
        public ppTestGump(Mobile c, int itemid) : base(0, 0)
        {
            ct = c;
            //it = itemid;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;

            AddPage(1);
            AddBackground(55, 45, 500, 400, 9200);
            AddAlphaRegion(65, 121, 480, 290);

            AddButton(395, 420, 1149, 1148, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddButton(475, 420, 1146, 1145, (int)Buttons.Button2, GumpButtonType.Reply, 0);
            AddImage(397, 157, 990);
            AddImage(30, 160, 991);
            AddImage(65, 426, 52);
            AddLabel(119, 431, 66, string.Format("{0} BAC {1}", c.Name, c.BAC.ToString()));
            AddImage(65, 42, 109);
            AddImage(121, 48, 105);
            AddImageTiled(65, 102, 480, 11, 2091);
            AddButton(522, 43, 5505, 5506, (int)Buttons.Button4, GumpButtonType.Page, 2);
            AddImage(153, 155, 666);
            AddImage(276, 158, 665);
            AddButton(120, 390, 4023, 4024, (int)Buttons.Button7, GumpButtonType.Reply, 0);
            AddButton(240, 390, 4023, 4024, (int)Buttons.Button8, GumpButtonType.Reply, 0);
            AddButton(360, 390, 4023, 4024, (int)Buttons.Button9, GumpButtonType.Reply, 0);
            AddButton(480, 390, 4023, 4024, (int)Buttons.Button10, GumpButtonType.Reply, 0);
            AddLabel(90, 380, 66, @"Mage");
            AddLabel(210, 380, 66, @"Ninja");
            AddLabel(300, 383, 66, @"Necromancer");
            AddLabel(440, 380, 66, @"Samurai");

            AddButton(260, 50, 247, 248, (int)Buttons.ex, GumpButtonType.Reply, 0);//gumpͼƬ
            AddTextEntry(200, 50, 60, 20, 0, (int)Buttons.ex, @"");//输入数字，button转换为数字代表的图片和声音文件。
            AddImage(70, 110, itemid);//ppzppp 
            AddItem(370, 110, itemid);//ppzppp
            AddLabel(330, 50, 23, it.ToString());//如果输入数字为空，则为下一个数字。






            AddPage(2);
            AddButton(522, 43, 5505, 5506, (int)Buttons.Button2, GumpButtonType.Page, 1);
            AddImageTiledButton(510, 110, 0x918, 0x919, 3, GumpButtonType.Page, 2, 5402, 0, 5, 5);


        }

        public enum Buttons
        {
            Button1,
            Button2,
            Button3,
            Button4,
            Button7,
            Button8,
            Button9,
            Button10,
            ex,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;



            switch (info.ButtonID)
            {

                case (int)Buttons.Button1:
                    {
                        from.SendGump(new PropertiesGump(from, ct));
                        break;
                    }
                case (int)Buttons.Button2:
                    {
                        switch (Utility.Random(2, 2))
                        {
                            case 0:
                                { from.SendMessage("0"); break; }
                            case 1:
                                from.SendMessage("1"); break;
                            case 2:
                                from.SendMessage("2"); break;
                            case 3:
                                from.SendMessage("3"); break;

                            case 4:
                                from.SendMessage("4"); break;

                            case 5:
                                from.SendMessage("5"); break;

                            default: break;
                        }
                        from.SendGump(new ppTestGump(ct));
                        break;
                    }
                case (int)Buttons.Button3:
                    {

                        break;
                    }
                case (int)Buttons.Button4:
                    {

                        break;
                    }
                case (int)Buttons.Button7:
                    {

                        break;
                    }
                case (int)Buttons.Button8:
                    {

                        break;
                    }
                case (int)Buttons.Button9:
                    {

                        break;
                    }
                case (int)Buttons.Button10:
                    {

                        break;
                    }
                case (int)Buttons.ex:
                    {
                        EX = info.GetTextEntry(8);
                        int EXs;
                        try
                        {
                                EXs = Convert.ToInt16(EX.Text);
                                it = EXs;

                        }
                        catch
                        {
                            from.SendMessage("Numbers only, please try again.");
                            //break;//没有作用，还会继续往下执行。
                        }
                        finally
                        {
                            //from.CloseGump(typeof(ppTestGump));
                            from.SendSound(it);
                            from.SendGump(new ppTestGump(ct, it));
                            ++it;
                        }
                        break;

                    }

            }
        }
    }
}