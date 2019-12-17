using System;
using Server.Commands;
using Server;
using Server.Gumps;
using Server.Targeting;
using Server.Items;

namespace Scripts.huanhua
{
    public static class huanhua
    {
        //private static Mobile form;
        private static Item itema;
        private static Item itemb;
        public static void Initialize()
        {
            CommandSystem.Register("hh", AccessLevel.Player, new CommandEventHandler(hh_OnCommand));
        }

        [Usage("hh")]
        [Description("Shows a hh window")]
        public static void hh_OnCommand(CommandEventArgs e)
        {
            //form = e.Mobile; 
            e.Mobile.CloseGump(typeof(hh));
            e.Mobile.SendGump(new hh());
        }

        public class hh : Gump
        {

            public hh() : this(null, null) { }
            public hh(Item ia) : this(itema, null) { }

            public hh(Item ia, Item ib) : base(100, 100)
            {
                itema = ia;
                itemb = ib;

                //AddBackground(0, 0, 400, 247, 0xE10);5100 3000
                AddBackground(0, 0, 400, 247, 3500);
                AddAlphaRegion(15, 15, 370, 35);
                AddAlphaRegion(15, 197, 370, 35);
                AddLabel(35, 15, 168, "幻化物品吗wtf喝晕了，目前只能幻化武器或装备");
                if (ia != null)
                {
                    AddLabel(50, 40, 30, "幻化前：");
                    AddItem(70, 120, ia.ItemID, ia.Hue);
                    AddItemProperty(ia.Serial);
                    AddButton(300, 187, 0x2C89, 0x2C8A, 2, GumpButtonType.Reply, 0); //选择b
                }
                if (ib != null)
                {
                    AddLabel(270, 40, 30, "幻化后：");
                    AddItem(290, 120, ib.ItemID, ib.Hue);
                    AddItemProperty(ib.Serial);
                }

                AddButton(80, 187, 0x2C89, 0x2C8A, 1, GumpButtonType.Reply, 0); //选择a

                if (ia != null & ib != null)
                    AddButton(170, 207, 2124, 2123, 3, GumpButtonType.Reply, 0); //最终合成
            }




            public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
            {
                int a = info.ButtonID;

                if (a == 1)
                {
                    sender.Mobile.Target = new tara();

                    return;
                }
                if (a == 2)
                {
                    sender.Mobile.Target = new tarb();

                    return;
                }

                if (a == 3)  //选择物品a
                {
                    //int x = itemb.ItemID;
                    //string xx = itemb.Name;
                    //Dupe.CopyProperties(itema, itemb);
                    ////itema.Hue = 33;
                    //itemb.ItemID = x;
                    //itemb.Name = xx;
                    //itema.Delete();//zp 
                    if ((itema is BaseWeapon && itemb is BaseWeapon) || (itema is BaseArmor && itemb is BaseArmor) || (itema is BaseHat && itemb is BaseHat))
                    {


                        if (itema.Layer == itemb.Layer)
                        {
                            if (itema is BaseWeapon)
                            {
                                BaseWeapon bb = itema as BaseWeapon;
                                bb.Name = string.Format("<BASEFONT COLOR=#2E9AFE>{0}", bb.GetType().Name + "<BASEFONT COLOR=#FFFFFF>");
                                //bb.EngravedText = (itema.GetType().Name + "幻化为" + itemb.GetType().Name);
                                bb.EngravedText = ("幻化为" + itemb.GetType().Name);
                            }
                            else if (itema is BaseArmor)
                            {
                                ((BaseArmor)itema).Name = string.Format("<BASEFONT COLOR=#2E9AFE>{0}", ((BaseArmor)itema).GetType().Name + "<BASEFONT COLOR=#FFFFFF>");
                                //((BaseArmor)itema).EngravedText = (itema.GetType().Name + "幻化为" + itemb.GetType().Name);
                                ((BaseArmor)itema).EngravedText = ("幻化为" + itemb.GetType().Name);

                            }


                            //ObjectPropertyList list = itema.PropertyList;
                            //list.Add( "已幻化成" + itemb.GetType().Name);
                            //itema.AddNameProperties(list);
                            //itema.AddNameProperty(list);
                            //itema.InvalidateProperties();
                            itema.ItemID = itemb.ItemID;
                            //itema.Name = itemb.Name;
                            itema.Hue = itemb.Hue;
                            itemb.Delete();
                        }
                        else sender.Mobile.SendMessage("那是不同部位的东西。");
                        return;
                    }
                    else if (itema.Layer == Layer.Helm && itemb.Layer == Layer.Helm)
                    {
                        if (itema is BaseArmor && itemb is BaseHat)
                        {
                            ((BaseArmor)itema).Name = string.Format("<BASEFONT COLOR=#2E9AFE>{0}", ((BaseArmor)itema).GetType().Name + "<BASEFONT COLOR=#FFFFFF>");
                            //((BaseArmor)itema).EngravedText = (itema.GetType().Name + "幻化为" + itemb.GetType().Name);
                            ((BaseArmor)itema).EngravedText = ("幻化为" + itemb.GetType().Name);
                            itema.ItemID = itemb.ItemID;
                            itema.Hue = itemb.Hue;
                            itemb.Delete();
                            return;
                        }

                    }
                    else sender.Mobile.SendMessage("不同种类不能进行幻化");

                    return;
                }
            }
        }

        public class tara : Target
        {

            public tara() : base(2, false, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {



                if (!(targeted is BaseWeapon) && !(targeted is BaseArmor) && !(targeted is BaseHat))
                {
                    from.SendMessage("reselect");
                    from.Target = new tara();
                    return;
                }
                else if (targeted is Item && ((Item)targeted).IsChildOf(from.Backpack))
                {
                    itema = (Item)targeted;
                    //itema.IsChildOf(from);

                    from.SendGump(new hh(itema));
                }
                else from.SendMessage("must in your backpack");
            }
        }
        public class tarb : Target
        {

            public tarb() : base(2, false, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is BaseWeapon) && !(targeted is BaseArmor) && !(targeted is BaseHat))
                {
                    from.SendMessage("reselect");
                    from.Target = new tarb();
                    return;
                }
                else if (targeted is Item && ((Item)targeted).IsChildOf(from.Backpack))
                {
                    {
                        itemb = (Item)targeted;
                        //if (itemb.GetType() == itema.GetType())
                        //{

                        from.SendGump(new hh(itema, itemb));
                    }
                }
                //}
                else
                    from.SendMessage("wtf,不一样");
            }
        }

    }
}

