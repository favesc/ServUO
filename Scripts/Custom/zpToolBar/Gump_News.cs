using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class PnewsGump : Gump
    {
        static short buffindex = 1003; TextRelay num;
        public PnewsGump() : base(210, 180)
        {
            string x = Config.Get("zp.News", "No news");
            this.Closable = true;
            this.Dragable = true;
            this.AddPage(0);
            this.AddBackground(17, 24, 364, 236, 3500);
            //this.AddImage(385, 30, Utility.RandomMinMax(0x4287, 0x42a8));
            this.AddHtml(40, 67, 322, 171, x, true, true);
            this.AddLabel(70, 41, 0, @"更新内容 与 新闻");
            bool test = true;
            if (!test)//测试bufficon按钮，没大用了
            {
                AddButton(10, 25, 247, 248, 11, GumpButtonType.Reply, 0);// 11
                AddButton(100, 25, 247, 248, 12, GumpButtonType.Reply, 0);
                AddTextEntry(75, 25, 60, 20, 0, 12, @"");
                AddLabel(145, 25, 60, buffindex.ToString());
            }
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 11)
            {//1003 1175
             //short buffindex=1003;
                BuffInfo.RemoveBuff(sender.Mobile, (BuffIcon)buffindex - 1);
                BuffInfo.AddBuff(sender.Mobile, new BuffInfo((BuffIcon)buffindex, 1070928, string.Format("{0}", buffindex)));
                buffindex++;
                sender.Mobile.SendGump(new PnewsGump());
            }
            if (info.ButtonID == 12)
            {
                num = info.GetTextEntry(12);
                try
                {
                    buffindex = Convert.ToInt16(num.Text);
                }
                catch
                {
                    if (buffindex <= 1003 || buffindex >= 1775)
                    {
                        sender.Mobile.SendMessage("1003 -  1175 Numbers only, please try again.");
                        sender.Mobile.CloseGump(typeof(PnewsGump));
                        sender.Mobile.SendGump(new PnewsGump());
                    }
                }
                sender.Mobile.CloseGump(typeof(PnewsGump));
                sender.Mobile.SendGump(new PnewsGump());
            }
        }
    }
    public class guagua : Gump
    {
        static DateTime start;
        static TimeSpan timer;
        static TimeSpan done = TimeSpan.Zero;
        private TextRelay text_input;
        static int Right;
        static int Wrong;
        int a, b, c, i, ly;
        static byte right = 0, wrong = 0;
        public static void Initialize()
        {
            CommandSystem.Register("gaga", AccessLevel.VIP, new CommandEventHandler(gaga_OnCommand));
        }

        [Usage("gaga")]
        [Description("guagua mei tian lian xi jia jian fa.")]
        public static void gaga_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new guagua());
        }
        public guagua() : this(0) { }
        public guagua(int p) : base(300, 60)
        {
            //ArrayList xx = new ArrayList  {String.Format( "gaga compute /n") };
            this.Closable = false;
            this.Dragable = true;
            if (p != 9 && p != 11)
            {
                ly = 65;
                this.AddPage(0);
                this.AddBackground(17, 24, 364, 530, 3500);
                AddImage(376, 20, Utility.Random(39904, 10));
                this.AddLabel(70, 41, 0, String.Format("呱呱的练习。今日已学习{0}:{1} _{2}/{3}", done.Minutes.ToString(), done.Seconds.ToString(), Right, Wrong));
                AddButton(290, 450, 247, 248, 2, GumpButtonType.Reply, 0);// 2 加法
                AddButton(290, 480, 247, 248, 1, GumpButtonType.Reply, 0);//okey 1 剑法
                AddButton(290, 510, 242, 241, 0, GumpButtonType.Reply, 0);//cancel
                AddButton(170, 510, 40014, 40015, 3, GumpButtonType.Reply, 1);//lian xi page 1
                AddLabel(195, 510, 0, String.Format("加减练习"));
                AddButton(170, 480, 40014, 40015, 4, GumpButtonType.Reply, 1);//lian xi page 1 cheng fa
                AddLabel(195, 480, 0, String.Format("综合练习"));
            }
            #region 剑法 p=0
            if (p == 0)
            {
                for (i = 0; i < 20; i++)
                {
                    a = Utility.Random(2, 28);
                    b = Utility.Random(3, 28);
                    c = a < b ? b - a : a - b;
                    if (a == b) { i--; continue; }
                    string x = a > b ? String.Format("{0} - {1} = ", a, b) : String.Format("{0} - {1} = ", b, a);
                    AddTextEntry(40, ly, 130, 20, 90, 0, x);
                    ly += 20;
                }
                ly = 65;
                for (i = 0; i < 20; i++)
                {
                    a = Utility.Random(2, 28);
                    b = Utility.Random(3, 28);
                    if (a == b) { i--; continue; }
                    c = a > b ? a - b : b - a;
                    string x = a > b ? String.Format("{0} - {1} = ", a, b) : String.Format("{0} - {1} = ", b, a);
                    AddTextEntry(182, ly, 130, 20, 90, 0, x);
                    ly += 20;

                }
            }
            #endregion 剑法
            #region 加法 p=1
            if (p == 1)
            {
                for (i = 0; i < 20; i++)
                {
                    a = Utility.Random(2, 28);
                    b = Utility.Random(3, 28);
                    string x = String.Format("{0} + {1} = ", a, b);
                    AddTextEntry(40, ly, 130, 20, 90, 0, x);
                    ly += 20;
                }
                ly = 65;
                for (i = 0; i < 20; i++)
                {
                    a = Utility.Random(2, 28);
                    b = Utility.Random(3, 28);
                    string x = String.Format("{0} + {1} = ", a, b);
                    AddTextEntry(182, ly, 130, 20, 90, 0, x);
                    ly += 20;

                }
            }
            #endregion 加法
            #region 加减法练习 p=9  
            if (p == 9)
            {
                AddPage(1);
                timer = DateTime.Now - start;

                this.AddBackground(17, 24, 364, 330, 3500);
                this.AddLabel(70, 41, 0, String.Format("呱呱的家剑法练习，今日用时{0}:{1}", done.Minutes.ToString(), done.Seconds.ToString()));
                AddLabel(70, 70, 0, DateTime.Now.ToString());
                AddImage(280, 70, 0x7777);
                AddImage(380, 70, Utility.Random(30568, 15));

                AddLabel(70, 101, 0, String.Format("正确{0}道，错误{1}道,用时{2}分{3}秒", right, wrong, timer.Minutes.ToString(), timer.Seconds.ToString()));
                AddLabel(70, 132, 0, String.Format("可用游戏时间{0}分钟", right / 3 - wrong * 5));

                ly = 200;
                string x;
                a = Utility.Random(2, 48);
                b = Utility.Random(3, 48);
                string xa = String.Format("{0} + {1} = ", a, b);
                string xd = a > b ? String.Format("{0} - {1} =", a, b) : String.Format("{0} - {1} =", b, a);
                int cx = a > b ? a - b : b - a;
                if (a == b)
                { x = xa; c = a + b; }
                else
                    x = Utility.RandomBool() ? xa : xd;
                c = (x == xa) ? a + b : cx;
                AddLabel(70, ly, 90, x);//question
                AddTextEntry(160, ly, 60, 20, 80, 9, @"");// input
                AddButton(150, ly + 20, 248, 247, 9, GumpButtonType.Reply, 1);//9 lianxi Okey
                AddButton(290, ly + 80, 248, 247, 10, GumpButtonType.Reply, 1);//reset right wrong
                AddButton(290, ly + 110, 242, 241, 19, GumpButtonType.Reply, 0);//cancel

            }
            #endregion
            #region 惩罚联系   p =11  现在是综合练习
            if (p == 11)
            {
                ly = 65;
                AddPage(1);
                timer = DateTime.Now - start;
                this.AddBackground(17, 24, 364, 330, 3500);
                this.AddLabel(70, 41, 0, String.Format("呱呱的上乘剑法练习。今日用时{0}:{1}", done.Minutes.ToString(), done.Seconds.ToString()));
                AddLabel(70, 70, 0, DateTime.Now.ToString());
                AddImage(280, 70, 0x7777);
                AddImage(380, 70, Utility.Random(30568, 15));

                AddLabel(70, 101, 0, String.Format("正确{0}道，错误{1}道,用时{2}分{3}秒", right, wrong, timer.Minutes.ToString(), timer.Seconds.ToString()));
                AddLabel(70, 132, 0, String.Format("可用游戏时间{0}分钟", right / 3 - wrong * 5));

                ly = 200;
                string x;
                if (Utility.RandomDouble() > 0.25)
                {
                    a = Utility.Random(2, 49);
                    b = Utility.Random(3, 49);
                    string xa = String.Format("{0} + {1} = ", a, b);
                    string xd = a > b ? String.Format("{0} - {1} =", a, b) : String.Format("{0} - {1} =", b, a);
                    int cx = a > b ? a - b : b - a;
                    if (a == b)
                    { x = xa; c = a + b; }
                    else
                        x = Utility.RandomBool() ? xa : xd;
                    c = (x == xa) ? a + b : cx;
                }
                else
                {
                    a = Utility.Random(2, 8);
                    b = Utility.Random(2, 8);
                    x = String.Format("{0} X {1} = ", a, b);
                    c = a * b;

                }
                AddLabel(70, ly, 90, x);//question
                AddTextEntry(160, ly, 60, 20, 80, 11, @"");// input
                AddButton(150, ly + 20, 248, 247, 11, GumpButtonType.Reply, 1);//11 lianxi Okey cheng fa
                AddButton(290, ly + 80, 248, 247, 10, GumpButtonType.Reply, 1);//reset right wrong
                AddButton(290, ly + 110, 242, 241, 19, GumpButtonType.Reply, 0);//cancel

            }
            #endregion cheng fa lian xi

        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        sender.Mobile.SendGump(new guagua());//jianfa
                        break;
                    }
                case 2:
                    {
                        sender.Mobile.SendGump(new guagua(1));//jiafa
                        break;
                    }
                case 3:
                    {
                        start = DateTime.Now;
                        sender.Mobile.SendGump(new guagua(9));//lianxi
                        break;
                    }
                case 4:
                    {
                        start = DateTime.Now;
                        sender.Mobile.SendGump(new guagua(11));//lianxi cheng fa 
                        break;
                    }


                case 9://lianxi
                    {
                        text_input = info.GetTextEntry(9);//输入数字答案。
                        try
                        {
                            if (c == Convert.ToInt16(text_input.Text))
                            {
                                ++right;
                                sender.Mobile.SendMessage(50, "你答对了！呱呱，游戏时间增加了！正确{0}道题", right, right / 3);
                            }
                            else
                            {
                                ++wrong;
                                sender.Mobile.SendMessage(40, String.Format("回答错误！游戏时间减少{1}分钟，错误{0}道题。", wrong, wrong * 5));
                            }

                        }
                        catch
                        {
                            ++wrong;
                            sender.Mobile.SendMessage(40, String.Format("回答错误！游戏时间减少了，错误{0}道题。", wrong, wrong * 5));
                        }
                        sender.Mobile.SendGump(new guagua(9));
                        break;
                    }
                case 10://okey reset
                    { Wrong += wrong; Right += right; done += timer; right = wrong = 0; sender.Mobile.SendGump(new guagua()); break; }
                case 11:
                    {
                        text_input = info.GetTextEntry(11);//输入数字答案。
                        try
                        {
                            if (c == Convert.ToInt16(text_input.Text))
                            {
                                ++right;
                                sender.Mobile.SendMessage(50, "你答对了！呱呱，游戏时间增加{1}分钟！正确{0}道题", right, right / 3);
                            }
                            else
                            {
                                ++wrong;
                                sender.Mobile.SendMessage(40, String.Format("回答错误！游戏时间减少{1}分钟，错误{0}道题。", wrong, wrong * 5));
                            }
                        }
                        catch
                        {
                            ++wrong;
                            sender.Mobile.SendMessage(40, String.Format("回答错误！游戏时间减少{1}分钟，错误{0}道题。", wrong, wrong * 5));
                        }
                        sender.Mobile.SendGump(new guagua(11));
                        break;

                    }
                case 19://cancel
                    {
                        done += timer;
                        Wrong += wrong;
                        Right += right;
                        wrong = right = 0;
                        break;
                    }
                default: break;

            }
        }
    }
}
/*try

{

//执行的代码，其中可能有异常。一旦发现异常，则立即跳到catch执行。否则不会执行catch里面的内容

} 

catch

{

//除非try里面执行代码发生了异常，否则这里的代码不会执行

} 

finally

{

//不管什么情况都会执行，包括try catch 里面用了return ,可以理解为只要执行了try或者catch，就一定会执行 finally  

}*/
