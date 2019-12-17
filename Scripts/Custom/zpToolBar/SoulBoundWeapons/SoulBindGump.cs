using Server.Network;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Gumps
{
    public class SoulBindGump : Gump
    {
        BaseWeapon s;
        int cash1;
        int cash2;
        int cash3;
        int cash4;
        private TextRelay Exset;
        //[CommandProperty(AccessLevel.GameMaster)]  参考 别删
        //public int EX { get { return ex; } set { ex = value; } }
        public SoulBindGump(Mobile from, BaseWeapon si) : base(200, 100)
        {

            s = si;
            //var k = Utility.RandomList(s.WeaponAttributes.HitColdArea, s.WeaponAttributes.HitEnergyArea, s.WeaponAttributes.HitFireArea, s.WeaponAttributes.HitPoisonArea, s.WeaponAttributes.HitPhysicalArea);
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            AddPage(0);
            AddBackground(40, 32, 380, 250, 3500);
            AddItem(317, 66, s.ItemID);
            AddItemProperty(s.Serial);//显示属性

            AddButton(334, 238, 247, 248, (int)Buttons.Button7, GumpButtonType.Reply, 0);
            AddLabel(70, 40, 93, "SoulWeapon Upgrade system.  ");
            AddLabel(70, 65, 44, si.Name.ToString());
            AddLabel(80, 90, 0, "要命30000系统，乱花钱耳");


            AddCheck(80, 120, 210, 211, false, (int)Buttons.luck);
            AddLabel(110, 120, 0, 30000 + " Luck = " + s.Attributes.Luck);

            AddCheck(80, 150, 210, 211, false, (int)Buttons.LeechHits);
            AddLabel(110, 150, 0, 20000 + " LeechHits = " + s.WeaponAttributes.HitLeechHits);

            AddCheck(80, 180, 210, 211, false, (int)Buttons.LeechMana);
            AddLabel(110, 180, 0, 15000 + " LeechMana = " + s.WeaponAttributes.HitLeechMana);

            AddRadio(70, 210, 208, 209, false, (int)Buttons.RadioButton2);

            AddRadio(95, 210, 208, 209, false, (int)Buttons.RadioButton1);
            AddLabel(125, 210, 0, 20000 + " Mass Hit = " + s.WeaponAttributes.HitFireArea);


            AddRadio(80, 240, 208, 209, true, (int)Buttons.RadioButton3);
            AddLabel(110, 240, 0, "Nothing to do " + "不花钱");

            if (from.AccessLevel > AccessLevel.GameMaster)
            {
                AddButton(334, 208, 247, 248, (int)Buttons.ExButton, GumpButtonType.Reply, 0);
                AddTextEntry(334, 188, 60, 20, 0, (int)Buttons.ExButton, @"");
                AddLabel(300, 148, 93, "全局变量");
            }
            AddLabel(300, 168, 93, " " + UnbindingDeed.ExSet + "点升一级");


            //            AddImageTiled(46, 235, 379, 21, 30089);

            //      AddTextEntry(1, 1, 1, 1, 1, 1, " " + s.Hue);
        }

        public enum Buttons
        {
            close,
            luck,
            LeechHits,
            LeechMana,
            RadioButton1,
            RadioButton2,
            RadioButton3,

            Button7,
            ExButton,
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            from.CloseGump(typeof(SoulBindGump));
            if (s.Deleted) return;
            switch (info.ButtonID)
            {
                case (int)Buttons.ExButton:
                    {
                        Exset = info.GetTextEntry(8);
                        {
                            try
                            {
                                UnbindingDeed.ExSet = Convert.ToInt16(Exset.Text);
                                //SoulVikingSword.ExSet = Convert.ToInt16(Exset.Text);
                                //SoulShortBow.ExSet = Convert.ToInt16(Exset.Text);
                                //VRadiantScimitar.ExSet = Convert.ToInt16(Exset.Text);
                                //Vstaff.ExSet = Convert.ToInt16(Exset.Text);
                                //vHalberd.ExSet = Convert.ToInt16(Exset.Text);
                            }

                            catch
                            {
                                from.SendMessage("Numbers only, please try again.");
                                //from.CloseGump(typeof(SoulBindGump));
                                //from.SendGump(new SoulBindGump(from, s));

                                //break;
                            }
                            from.CloseGump(typeof(SoulBindGump));
                            from.SendGump(new SoulBindGump(from, s));

                            break;
                        }
                    }
                case (int)Buttons.Button7:
                    {

                        if (info.ButtonID == 0) return;

                        if (info.IsSwitched((int)Buttons.luck))  //important!!! if (info.ButtonID == (int)Buttons.Checkbox1)
                        {
                            cash1 = 30000;
                            s.Attributes.Luck = Utility.Random(177);
                        }

                        if (info.IsSwitched((int)Buttons.LeechHits))
                        {
                            cash2 = 20000;
                            s.WeaponAttributes.HitLeechHits = Utility.Random(65);
                        }
                        if (info.IsSwitched((int)Buttons.LeechMana))
                        {
                            cash3 = 15000;
                            s.WeaponAttributes.HitLeechMana = Utility.Random(45);
                        }
                        /*            if (info.IsSwitched((int)Buttons.Checkbox4))
                                    {
                                        s.WeaponAttributes.SelfRepair = Utility.Random(3);
                                        ; cash4 = 6000;
                                    }
                        暂时还煤油第四个按钮*/

                        if (info.IsSwitched((int)Buttons.RadioButton1))
                        {
                            cash4 = 20000;
                            s.WeaponAttributes.HitFireArea = Utility.Random(50);
                        }
                        if (info.IsSwitched((int)Buttons.RadioButton2))
                        {
                            cash4 = 10000;
                            s.WeaponAttributes.HitFireArea = 0;
                        }
                        //           if (info.IsSwitched((int)Buttons.RadioButton2)) { var k = 0; cash5 = 1000; }
                        if (info.IsSwitched((int)Buttons.RadioButton3))
                        {
                            cash4 = 0;
                        }

                        int Price = cash1 + cash2 + cash3 + cash4;

                        if (Banker.GetBalance(from) < Price)
                        {
                            from.SendMessage("not enough gold.   " + "need " + Price);
                            break;
                        }

                        if (Banker.Withdraw(from, Price) && Price != 0)
                        {
                            from.SendLocalizedMessage(1060398, Price.ToString()); // Amount charged
                            from.SendLocalizedMessage(1060022, Banker.GetBalance(from).ToString()); // Amount left, from bank
                            from.PlaySound(0x38e);
                            from.SendMessage("总共花了{0}个金币", Price);
                            from.SendGump(new SoulBindGump(from, s));
                            break;
                        }
                        else
                            break;

                    }
            }
        }
    }
    public class levelup : Gump
    {

        public static void Initialize()
        { }
        private readonly Type[] tp = { typeof(SoulVikingSword), typeof(SoulShortBow), typeof(VRadiantScimitar), typeof(Vstaff), typeof(vHalberd), typeof(Vtessen) };
        public levelup(Mobile from, BaseWeapon s) : base(200, 100)
        {

            if (!(from is PlayerMobile)) return;

            for (int i = 0; i < tp.Length; i++)
            {
                if (s.GetType() == tp[i])
                {
                    s = Activator.CreateInstance(tp[i]) as BaseWeapon ; 
                    break;
                }
            }

            this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
            this.AddLabel(22, 13, 0, string.Format("{0} LEVEL UP！", s.Name));
            AddItem(s.Name.Length * 20 + 85, 5, s.ItemID);
            //AddItemProperty(s.Serial);//mouse over tooltip

            //this.AddLabel(22, 13, 0, string.Format("{0} LEVEL UP！ 等级：{1}", s.Name, s.Level)); return;
        }

        //if (s is SoulVikingSword)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 85, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((SoulVikingSword)s).Name, ((SoulVikingSword)s).Level)); return;
        //}
        //if (s is SoulShortBow)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 65, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((SoulShortBow)s).Name, ((SoulShortBow)s).Level)); return;
        //}
        //if (s is VRadiantScimitar)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 85, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((VRadiantScimitar)s).Name, ((VRadiantScimitar)s).Level)); return;
        //}
        //if (s is Vstaff)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 75, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((Vstaff)s).Name, ((Vstaff)s).Level)); return;
        //}
        //if (s is vHalberd)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 85, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((vHalberd)s).Name, ((vHalberd)s).Level)); return;
        //}
        //if (s is Vtessen)
        //{
        //    this.AddBackground(0, 0, s.Name.Length * 20 + 180, 43, 3500);
        //    AddItem(s.Name.Length * 20 + 100, 5, s.ItemID);
        //    this.AddLabel(22, 13, 0, string.Format("{0} 升级！现在等级：{1}", ((Vtessen)s).Name, ((Vtessen)s).Level)); return;
        //}


        //else return;




        public class closelevelupgump : Timer
        {
            private readonly Mobile f;
            public closelevelupgump(Mobile from) : base(TimeSpan.FromSeconds(5))
            {
                f = from;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                if (f.HasGump(typeof(levelup)))
                {
                    f.CloseGump(typeof(levelup));
                    Stop();
                }
                else Stop();
            }
        }

    }

}