using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.BulkOrders;
using Server.Items;

namespace Server.Commands
{
    public class xh
    {

        public static void Initialize()
        {
            CommandSystem.Register("p", AccessLevel.Player, new CommandEventHandler(xh_OnCommand));
        }

        [Usage("p")]
        [Description("多功能按钮.")]
        public static void xh_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.Alive)
            {
                e.Mobile.Target = new xTarget();
                e.Mobile.SendMessage("Target a treasureMap or mobile or container.");//or BOD
            }
            else e.Mobile.SendMessage("You are dead.");
        }
        private class xTarget : Target
        {
            //Mobile pm;

            public xTarget() : base(12, false, TargetFlags.None)
            { }
            protected override void OnTarget(Mobile from, object target)
            {
                //pm = from;
                if (target is TreasureMap)
                {
                    TreasureMap tar = (TreasureMap)target;
                    if ((tar.Decoder != null && tar.IsChildOf(from.Backpack)) || from.AccessLevel >= AccessLevel.GameMaster)
                    {
                        if (from.HasGump(typeof(teleporttreasure)))
                            from.CloseGump(typeof(teleporttreasure));
                        from.SendGump(new teleporttreasure(from, tar));
                        return;
                    }
                    else if (!tar.IsChildOf(from.Backpack))
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.

                    else from.SendMessage("It is not decode");
                    return;

                }

                #region BOD
                if (target is SmallBOD || target is LargeBOD)
                {
                    if (from.AccessLevel <= AccessLevel.Player)
                    {
                        from.SendMessage("That is not accessible.");
                        return;
                    }
                    else
                    {
                        if (target is SmallBOD)
                        {
                            SmallBOD x = target as SmallBOD;
                            x.AmountCur = x.AmountMax;
                            x.InvalidateProperties();
                            return;
                        }
                        else if (target is LargeBOD)
                        {
                            LargeBOD y = target as LargeBOD;
                            foreach (LargeBulkEntry e in y.Entries)
                            {
                                e.Amount = y.AmountMax;
                            }
                            y.InvalidateProperties();
                            return;
                        }
                    }
                    return;
                }
                #endregion BOD

                #region Box

                if (target is BaseContainer)
                {
                    BaseContainer tar = (BaseContainer)target;

                    if (from.AccessLevel > AccessLevel.VIP)//if (from.IsStaff())
                    {
                        if (from.HasGump(typeof(EmptyContainner)))
                            from.CloseGump(typeof(EmptyContainner));
                        from.SendGump(new EmptyContainner(tar));
                        if (from.HasGump(typeof(Openbox)))
                            from.CloseGump(typeof(Openbox));
                        if (Gumps.Openbox.itemlist(tar).Count > 0)
                            from.SendGump(new Openbox(tar));
                        return;
                    }
                    if (!tar.CheckContentDisplay(from))
                    {
                        from.SendMessage("ucant");
                    }
                    if (tar.CheckContentDisplay(from) && from.InRange(tar.GetWorldLocation(), 2))
                    {
                        from.SendMessage("can");
                        if (Gumps.Openbox.itemlist(tar).Count > 0)
                            from.SendGump(new Openbox(tar));
                        //string fuck = tar.Items.Count.ToString();
                        //string facee = Openbox.itemlist(tar).Count.ToString(); //zpdebug
                        //from.SendMessage(fuck +"   dd  "+ facee);
                        if (tar.IsChildOf(from.Backpack))
                        {
                            if (tar is LockableContainer && ((LockableContainer)tar).Locked) { from.SendLocalizedMessage(501283); /*it s locked*/ return; }
                            if (from.HasGump(typeof(EmptyContainner)))
                                from.CloseGump(typeof(EmptyContainner));
                            from.SendGump(new EmptyContainner(tar));
                            return;
                        }
                        else if (!tar.IsChildOf(from.Backpack))
                            from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                    return;
                }
                #endregion box

                #region basecreature
                if (target is BaseCreature)
                {
                    BaseCreature bc = target as BaseCreature;
                    if (bc.IsDeadPet) return;
                    if (bc.Controlled && from.AccessLevel > AccessLevel.VIP)
                    {
                        if (from.HasGump(typeof(ControlledMob)))
                            from.CloseGump(typeof(ControlledMob));
                        from.SendGump(new ControlledMob(from, bc));
                        if (from.HasGump(typeof(main)))
                            from.CloseGump(typeof(main));
                        from.SendGump(new main(from, bc));
                        return;
                    }
                    if (bc.Controlled && bc.ControlMaster != from && !bc.IsDeadPet) { from.SendMessage("那是别人的宠物."); return; }

                    if (bc.Controlled && bc.ControlMaster == from && !bc.IsDeadPet)
                    {
                        if (from.HasGump(typeof(ControlledMob)))
                            from.CloseGump(typeof(ControlledMob));
                        from.SendGump(new ControlledMob(from, bc));
                        if (from.HasGump(typeof(main)))
                            from.CloseGump(typeof(main));
                        from.SendGump(new main(from, bc));

                        return;
                    }
                    else if (!bc.IsDeadPet)
                    {
                        if (from.HasGump(typeof(WildMob)))
                            from.CloseGump(typeof(WildMob));
                        from.SendGump(new WildMob(from, bc));
                        if (from.HasGump(typeof(main)))
                            from.CloseGump(typeof(main));
                        from.SendGump(new main(from, bc));
                        return;
                    }
                    return;
                }
                #endregion




                else from.SendMessage("错误的目标");

            }
        }
    }
}
namespace Server.Gumps
{
    #region 驯服的
    public class ControlledMob : Gump
    {
        Mobile from;
        private BaseCreature b;
        public ControlledMob(Mobile pm, BaseCreature bc) : base(1025, 130)
        {
            from = pm;
            b = bc;
            if (!b.InRange(from.Location, 18)) { from.SendMessage("Out of range."); return; }
            if (!b.Controlled || b.IsDeadPet)
            {
                from.SendMessage("yesheng or siwang."); return;
            }
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            const short x = 110;
            short y = 85;
            const short h = 25;
            #region 
            AddBackground(x + 2, 40, 60, 60, 3500);



            if (b is xhorse)
                AddItem(x + 5, y - 35, 8484, b.Hue);//Horse
            else if (b is RidePolarBear)
                AddItem(x + 5, y - 35, 0x20E1);//Polar bear
            else
                AddLabel(x, y - 35, 50, b.Name.ToString());
            //AddAlphaRegion(x + 7, y - 30, 50, 50);





            AddButton(x, y, 0x7eb, 0x7ec, (byte)Buttons.Main, GumpButtonType.Reply, 0);
            //AddLabel(x + 5, y, 90, @"属 性");
            if (pm.AccessLevel > AccessLevel.Player)
                AddButton(x - 15, y + 2, 0x7538, 0x7539, (byte)Buttons.guard, GumpButtonType.Reply, 0);//圆点按钮

            y += h;

            AddButton(x, y, 0x7df, 0x7e0, (byte)Buttons.skills, GumpButtonType.Reply, 0);
            //AddLabel(x + 5, y, 90, @"技 能");
            if (pm.AccessLevel > AccessLevel.Player)
                AddButton(x - 15, y + 2, 0x7538, 0x7539, (byte)Buttons.follow, GumpButtonType.Reply, 0);//圆点按钮

            if (b is xhorse && b.Controlled && b.ControlMaster == from)
            {
                xhorse bx = b as xhorse;
                y += h;
                if (pm.AccessLevel > AccessLevel.Player)
                    AddButton(x - 15, y + 2, 0x7539, 0x7538, (byte)Buttons.AI, GumpButtonType.Reply, 0);//AI 圆点按钮
                AddButton(x, y, 2443, 2444, (byte)Buttons.Polymorph, GumpButtonType.Reply, 0);
                AddTooltip(1075824);//变形术
                if (b.Body.IsHuman)
                    AddLabel(x + 10, y, 50, @"兽 形");
                else
                    AddLabel(x + 10, y, 93, @"人 形");

                y += h;

                AddButton(x, y, 2443, 2444, (byte)Buttons.Duhit, GumpButtonType.Reply, 0);
                AddTooltip(1028845);//毒物攻击
                if (bx.duhit)
                    AddLabel(x + 5, y, 90, @"无毒性");
                else
                    AddLabel(x + 5, y, 63, @"寄生毒");

            }
            if (from.AccessLevel > AccessLevel.VIP)
            {
                y += h;
                AddButton(x, y, 2443, 2444, (byte)Buttons.changeHue, GumpButtonType.Reply, 0);
                AddLabel(x + 10, y, 90, @"GM变色");

            }
            if (b is RidePolarBear)
            {
                RidePolarBear bx = b as RidePolarBear;
                y += h;
                AddButton(x, y, 2443, 2444, (byte)Buttons.polarPolymorph, GumpButtonType.Reply, 0);
                AddTooltip(1075824);//变形术
                if (bx.Body.IsHuman)
                    AddLabel(x + 5, y, 50, @"兽 形");
                else
                    AddLabel(x + 5, y, 93, @"人 形");
            }



            if (from.AccessLevel > AccessLevel.VIP)
            {
                y += h;
                AddButton(x, y, 2443, 2444, (byte)Buttons.Button4, GumpButtonType.Reply, 0);
                AddLabel(x + 5, y, 90, @"未知的");
            }

            y += h;
            AddButton(x, y, 0xf7, 248, (byte)Buttons.Okey, GumpButtonType.Reply, 0);
            AddTooltip(1112448);//更新
            if (pm.AccessLevel > AccessLevel.Player)
                AddImage(x - 22, y, Utility.RandomMinMax(0x2c4d, 0x2c6e));//麻将牌?
            y += h;
            AddButton(x, y, 0xf2, 0xf1, (byte)Buttons.Close, GumpButtonType.Reply, 0);
            AddTooltip(1077860);//关闭选单 cancel


            if (b.Body.IsHuman && b.CanPaperdollBeOpenedBy(from)) //纸娃娃
            {
                y += h;

                AddImageTiledButton(x, y, 0x918, 0x919, (byte)Buttons.PaperDoll, GumpButtonType.Reply, 0, 8454, 0, 5, 5);//button 4 reply paperdoll pet
                AddLabel(x + 15, y + 30, 30, @"纸娃娃");
                //AddButton(x + 10, y, 2443, 2444, (byte)Buttons.PaperDoll, GumpButtonType.Reply, 0);
                //AddLabel(x + 15, y, 30, @"纸娃娃");

                y += 35;
                var Ring = b.FindItemOnLayer(Layer.Ring);
                var Bracelet = b.FindItemOnLayer(Layer.Bracelet);
                var Earrings = b.FindItemOnLayer(Layer.Earrings);
                if (Ring != null)
                {
                    y += 30;
                    AddBackground(x + 2, y, 60, 20, 3000);
                    AddLabel(x + 5, y, 30, "Ring");
                    AddItemProperty(Ring.Serial);
                    //AddItem(x + 10, y+10, Ring.ItemID);
                    AddButton(x - 15, y + 2, 0x7538, 0x7539, (byte)Buttons.ring, GumpButtonType.Reply, 0);
                }
                if (Bracelet != null)
                {
                    y += 30;
                    AddBackground(x + 2, y, 60, 20, 3000);
                    AddLabel(x + 5, y, 30, "Bracelet");
                    AddItemProperty(Bracelet.Serial);
                    //AddItem(x + 15, y+10, Bracelet.ItemID);
                    AddButton(x - 15, y + 2, 0x7538, 0x7539, (byte)Buttons.bracelet, GumpButtonType.Reply, 0);
                }
                if (Earrings != null)
                {
                    y += 30;
                    AddBackground(x + 2, y, 60, 20, 3000);
                    AddLabel(x + 5, y, 30, "Earrings");
                    AddItemProperty(Earrings.Serial);
                    //AddItem(x + 15, y+10, Earrings.ItemID);
                    AddButton(x - 15, y + 2, 0x7538, 0x7539, (byte)Buttons.earrings, GumpButtonType.Reply, 0);
                }
            }

            #endregion

        }

        public enum Buttons { Main, skills, AI, Polymorph, polarPolymorph, PaperDoll, Duhit, Button4, Okey, Close, guard, follow, ring, bracelet, earrings, changeHue }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (b.Deleted) return;
            if (!b.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }
            if (!b.Controlled || b.IsDeadPet)
            {
                from.SendMessage("Live or die."); return;
            }


            if (!b.IsDeadPet)
            {
                switch (info.ButtonID)
                {
                    case (byte)Buttons.Main:
                        {
                            from.SendGump(new ControlledMob(from, b));
                            if (from.HasGump(typeof(main)))
                            { from.CloseGump(typeof(main)); break; }
                            else
                                from.SendGump(new main(from, b));
                            break;
                        }
                    case (byte)Buttons.skills:
                        {
                            from.SendGump(new ControlledMob(from, b));
                            if (from.HasGump(typeof(bcskill)))
                            {
                                from.CloseGump(typeof(bcskill)); break;
                            }
                            else
                                from.SendGump(new bcskill(b));

                            break;
                        }
                    case (byte)Buttons.AI:
                        {
                            from.SendGump(new ControlledMob(from, b));
                            if (b.Controlled && b.ControlMaster == from)
                                if (from.HasGump(typeof(ai)))
                                { from.CloseGump(typeof(ai)); break; }
                                else
                                    from.SendGump(new ai(from, b));

                            break;
                        }
                    case (byte)Buttons.Polymorph:
                        {
                            if (b is xhorse && b.Controlled && b.ControlMaster == from && ((BaseMount)b).Rider == null)
                            {
                                if (!b.Body.IsHuman)
                                {

                                    ((xhorse)b).polymorph(0x191, (xhorse)b);
                                    b.Say("*我变个女人*");
                                }
                                else
                                {
                                    //if (b.Mounted) { from.SendMessage("请先下马");  }
                                    if (b.Mounted) BaseMount.Dismount(b);
                                    ((xhorse)b).polymorph(0xbe, (xhorse)b);
                                    //b.ControlOrder = OrderType.Follow; b.ControlTarget = b.ControlMaster;
                                    b.Say("*火炎神驹*");

                                }
                                from.SendGump(new ControlledMob(from, b));
                            }
                            break;
                        }
                    case (byte)Buttons.polarPolymorph:
                        {
                            if (b is RidePolarBear && b.Controlled && b.ControlMaster == from)
                            {
                                if (!b.Body.IsHuman)
                                {
                                    b.Body = (b.Female ? 0x191 : 0x190);
                                    b.PlaySound(0x56);
                                    if (b.CanPaperdollBeOpenedBy(from))
                                        b.DisplayPaperdollTo(from);
                                    b.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                                }
                                else
                                {
                                    b.Body = 213;
                                    b.PlaySound(0x20C);

                                    b.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                                }

                                from.SendGump(new ControlledMob(from, b));
                            }
                            break;
                        }

                    case (byte)Buttons.PaperDoll:
                        {
                            if (b.Body.IsHuman && b.Controlled && b.ControlMaster == from && b.CanPaperdollBeOpenedBy(from))
                            {
                                b.DisplayPaperdollTo(from);
                            }
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.Duhit:
                        {
                            if (b is xhorse && b.Controlled && b.ControlMaster == from)
                            {
                                xhorse c = (xhorse)b;
                                if (c.duhit == false)
                                {
                                    c.duhit = true;
                                    c.PlaySound(0x56);
                                    IEntity fr = new Entity(Server.Serial.Zero, new Point3D(c.X, c.Y, c.Z), c.Map);
                                    IEntity to = new Entity(Server.Serial.Zero, new Point3D(c.X, c.Y, c.Z + 40), c.Map);
                                    Effects.SendMovingParticles(fr, to, 0xF5F, 3, 0, false, false, 63, 3, 9501, 1, 0, EffectLayer.Head, 0x100);
                                }
                                else
                                {
                                    c.duhit = false; c.PlaySound(0x20C);
                                    c.FixedParticles(0x3779, 1, 30, 9964, 3, 3, EffectLayer.Waist);
                                }
                            }
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.Button4:
                        {
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.Okey:
                        {
                            from.SendGump(new ControlledMob(from, b));
                            break;
                        }
                    case (byte)Buttons.Close:
                        {

                            break;
                        }
                    case (byte)Buttons.guard:
                        {
                            int[] numArray = new int[] { 0x16b };
                            from.DoSpeech("*保护我是你的职责", numArray, 0, 60);
                            from.SendGump(new ControlledMob(from, b));
                            break;
                        }
                    case (byte)Buttons.follow:
                        {
                            int[] numArray = new int[] { 0x16c };
                            from.DoSpeech("*跟随我的步伐", numArray, 0, 63);
                            from.SendGump(new ControlledMob(from, b));
                            break;
                        }
                    case (byte)Buttons.ring:
                        {
                            if (b.InRange(from.Location, 3))
                                if (b.FindItemOnLayer(Layer.Ring) != null)
                                    from.Backpack.DropItem(b.FindItemOnLayer(Layer.Ring));
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.bracelet:
                        {
                            var brac = b.FindItemOnLayer(Layer.Bracelet);

                            if (b.InRange(from.Location, 3))
                                if (brac != null)
                                    from.Backpack.DropItem(brac);
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.earrings:
                        {
                            if (b.InRange(from.Location, 3))
                                if (b.FindItemOnLayer(Layer.Earrings) != null)
                                    from.Backpack.DropItem(b.FindItemOnLayer(Layer.Earrings));
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }
                    case (byte)Buttons.changeHue:
                        {
                            if (from.AccessLevel > AccessLevel.VIP)
                                b.Hue = Utility.RandomList(1461, 1365, 1266, 1257, 1173, 1166, 1168, 1151, 1150);
                            from.SendGump(new ControlledMob(from, b));

                            break;
                        }


                }
            }
        }


    }
    #endregion 驯服的

    #region 野生的 Wild
    public class WildMob : Gump
    {
        private BaseCreature b;
        private Mobile from;
        public WildMob(Mobile pm, BaseCreature bc) : base(1025, 450)
        {
            from = pm;
            b = bc;
            if (b.Deleted) return;
            if (!b.InRange(from.Location, 18)) { from.SendMessage("Out of range."); return; }
            if (b.Controlled || b.IsDeadPet) return;
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            const short x = 110;
            short y = 85;
            const short h = 25;
            AddBackground(x + 2, 40, 60, 60, 3600);

            if (b is xhorse)
                AddItem(x + 5, y - 35, 8484, b.Hue);//Horse
            else if (b is RidePolarBear)
                AddItem(x + 5, y - 35, 0x20E1);//Polar bear

            else if (b.Hue != 0)
                AddItem(x + 5, y - 35, 0x2136, b.Hue);//
            else
                //AddAlphaRegion(x + 7, y - 30, 50, 50);
                //AddItem(x + 5, y - 35, 0x2136);
                AddLabel(x, y - 35, 50, b.Name.ToString());

            AddButton(x, y, 2443, 2444, (byte)Buttons.Main, GumpButtonType.Reply, 0);
            AddLabel(x + 5, y, 50, @"属 性");
            y += h;
            AddButton(x, y, 2443, 2444, (byte)Buttons.skills, GumpButtonType.Reply, 0);
            AddLabel(x + 5, y, 50, @"技 能");
            //y += h;
            //AddButton(x, y, 2443, 2444, (byte)Buttons.AI, GumpButtonType.Reply, 0);
            //AddLabel(x + 5, y, 50, @"职 业");


            if (b.Body.IsHuman && b.CanPaperdollBeOpenedBy(from))
            {
                y += h;
                AddButton(x, y, 2443, 2444, (byte)Buttons.PaperDoll, GumpButtonType.Reply, 0);
                AddLabel(x + 5, y, 30, @"纸娃娃");
            }

            //
            //AddButton(x, y, 2443, 2444, (byte)Buttons.Button4, GumpButtonType.Reply, 0);
            //AddLabel(x + 5, y, 50, @"未知的");
            y += h;
            AddButton(x, y, 0xf7, 248, (byte)Buttons.Okey, GumpButtonType.Reply, 0);
            AddTooltip(1112448);//更新

            y += h;
            AddButton(x, y, 0xf2, 0xf1, (byte)Buttons.Close, GumpButtonType.Reply, 0);
            AddTooltip(1077860);//关闭选单




        }
        public enum Buttons { Main, skills, AI, Button4, PaperDoll, Okey, Close, }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (!b.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }
            if (b.Controlled || b.IsDeadPet) return;

            switch (info.ButtonID)
            {
                case (byte)Buttons.Main:
                    {
                        from.SendGump(new WildMob(from, b));
                        if (from.HasGump(typeof(main)))
                        {
                            from.CloseGump(typeof(main)); break;
                        }
                        else
                            from.SendGump(new main(from, b));

                        break;
                    }
                case (byte)Buttons.skills:
                    {
                        from.SendGump(new WildMob(from, b));
                        if (from.HasGump(typeof(bcskill)))
                        {
                            from.CloseGump(typeof(bcskill)); break;
                        }
                        else
                            from.SendGump(new bcskill(b));

                        break;
                    }
                case (byte)Buttons.AI:
                    {

                        break;
                    }
                case (byte)Buttons.Button4:
                    {
                        from.SendGump(new WildMob(from, b));
                        break;
                    }
                case (byte)Buttons.PaperDoll:
                    {
                        if (b.Body.IsHuman && b.CanPaperdollBeOpenedBy(from))
                        {
                            b.DisplayPaperdollTo(from);
                        }
                        from.SendGump(new WildMob(from, b));

                        break;
                    }
                case (byte)Buttons.Okey:
                    {
                        from.SendGump(new WildMob(from, b));
                        break;
                    }
                case (byte)Buttons.Close:
                    {

                        break;
                    }

            }
        }
    }
    #endregion 

    #region skills
    public class bcskill : Gump
    {
        BaseCreature c;
        private static string FormatZpRight(double val, double val1)
        {
            //if (val <= 0)
            //    return "<div align=right>---</div>";

            return String.Format("<div align=right>{0} / {1}</div>", val, val1);
        }

        public bcskill(BaseCreature bc) : base(525, 130)
        {
            c = bc;
            const short x = 125;
            short y = 15;
            const short bottom = 500;
            //新学二3维数组
            //int[] skname = new int[] { 1044065,1044100,1044101,1044102,1044091,1044077,1044061,1044103,1044090,};
            double[,] sk = new double[22, 3]
            {
                {c.Skills.Parry.Value,c.Skills.Parry.Cap,1044065 },
                {c.Skills.Swords.Value,c.Skills.Swords.Cap,1044100},
                {c.Skills.Macing.Value,c.Skills.Macing.Cap ,1044101},
                {c.Skills.Fencing.Value,c.Skills.Fencing.Cap ,1044102},
                {c.Skills.Archery.Value,c.Skills.Archery.Cap,1044091},
                {c.Skills.Healing.Value,c.Skills.Healing.Cap ,1044077},
                {c.Skills.Anatomy.Value,c.Skills.Anatomy.Cap ,1044061},
                {c.Skills.Wrestling.Value, c.Skills.Wrestling.Cap ,1044103},
                {c.Skills.Poisoning.Value,c.Skills.Poisoning.Cap ,1044090},
                {c.Skills.MagicResist.Value, c.Skills.MagicResist.Cap ,1044086},
                {c.Skills.Hiding.Value,c.Skills.Hiding.Cap,1044081 },
                {c.Skills.Magery.Value,c.Skills.Magery.Cap,1044085 },
                {c.Skills.Necromancy.Value,c.Skills.Necromancy.Cap,1044109 },
                {c.Skills.Ninjitsu.Value,c.Skills.Ninjitsu.Cap ,1044113},
                {c.Skills.Bushido.Value, c.Skills.Bushido.Cap ,1044112},
                {c.Skills.SpiritSpeak.Value,c.Skills.SpiritSpeak.Cap,1044092 },
                {c.Skills.Chivalry.Value,c.Skills.Chivalry.Cap,1044111},
                {c.Skills.Mysticism.Value, c.Skills.Mysticism.Cap,1044115 },
                {c.Skills.Spellweaving.Value,c.Skills.Spellweaving.Cap,1044114 },
                {c.Skills.EvalInt.Value,c.Skills.EvalInt.Cap,1044076 },
                {c.Skills.Meditation.Value, c.Skills.Meditation.Cap,1044106, },
                {c.Skills.Macing.Value, c.Skills.Macing.Cap ,1044101},
            };

            AddBackground(107, 7, 260, bottom, 3500);

            AddLabel(x + 30, y, 90, bc.Name + "___" + bc.AI.ToString());

            for (int i = 0; i < 21; i++)
            {
                y += 20;
                if (i == 6 || i == 12) { AddImage(x, y, 2086); y += 20; }
                this.AddHtmlLocalized(x, y, 100, 20, (int)sk[i, 2], 0, false, false); // name of skills number
                this.AddHtml(x + 130, y, 90, 20, FormatZpRight(sk[i, 0], sk[i, 1]), false, false);

            }
            //y += 25;
            ////cap
            //AddTextEntry(x + 5, y, 50, bottom - 25, 30, 0, string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}\n{12}\n{13}\n\n{14}\n{15}\n{16}\n{17}\n{18}\n{19}\n{20}\n{21}\n{22}",
            //    c.Skills.Parry.Cap, c.Skills.Swords.Cap, c.Skills.Macing.Cap, c.Skills.Fencing.Cap, c.Skills.Archery.Cap, c.Skills.Chivalry.Cap,
            //    c.Skills.Healing.Cap, c.Skills.Anatomy.Cap, c.Skills.Tactics.Cap, c.Skills.Wrestling.Cap, c.Skills.Poisoning.Cap,
            //    c.Skills.MagicResist.Cap, c.Skills.Hiding.Cap, c.Skills.DetectHidden.Cap,
            //    c.Skills.Magery.Cap, c.Skills.EvalInt.Cap, c.Skills.Meditation.Cap, c.Skills.Necromancy.Cap, c.Skills.SpiritSpeak.Cap,
            //    c.Skills.Ninjitsu.Cap, c.Skills.Bushido.Cap, c.Skills.Mysticism.Cap, c.Skills.Spellweaving.Cap));

            ////skills
            //AddTextEntry(x + 40, y, 200, bottom - 25, 0, 0, string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}\n{12}\n{13}\n\n{14}\n{15}\n{16}\n{17}\n{18}\n{19}\n{20}\n{21}\n{22}",
            //    c.Skills.Parry, c.Skills.Swords, c.Skills.Macing, c.Skills.Fencing, c.Skills.Archery, c.Skills.Chivalry,
            //    c.Skills.Healing, c.Skills.Anatomy, c.Skills.Tactics, c.Skills.Wrestling, c.Skills.Poisoning,
            //    c.Skills.MagicResist, c.Skills.Hiding, c.Skills.DetectHidden,
            //    c.Skills.Magery, c.Skills.EvalInt, c.Skills.Meditation, c.Skills.Necromancy, c.Skills.SpiritSpeak,
            //    c.Skills.Ninjitsu, c.Skills.Bushido, c.Skills.Mysticism, c.Skills.Spellweaving));

            ////\n{23}






        }
        public enum Buttons { Main, skills, AI, Button4, Close, }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (!c.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }

            switch (info.ButtonID)
            {
                case (int)Buttons.Main:
                    {

                        break;
                    }
                case (int)Buttons.skills:
                    {

                        break;
                    }
                case (int)Buttons.AI:
                    {

                        break;
                    }
                case (int)Buttons.Button4:
                    {

                        break;
                    }
                case (int)Buttons.Close:
                    {

                        break;
                    }

            }
        }
    }

    #endregion skills

    public class ai : Gump
    {
        BaseCreature c;
        Mobile from;
        public ai(Mobile pm, BaseCreature bc) : base(325, 130)
        {
            c = bc;
            from = pm;
            short x = 125;
            short y = 15;
            const byte h = 30;
            const short bottom = 260;
            AddBackground(107, 7, 160, bottom, 3500);

            AddLabel(x + 10, y, 56, bc.Name);



            if (c.Controlled && c.ControlMaster == from)
            //if (c is xhorse && c.Controlled && c.ControlMaster == from)
            {
                y += h;
                AddButton(x, y, 0x846, 0x845, 1, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 2, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 3, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 4, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 5, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 6, GumpButtonType.Reply, 0);
                y += h;
                AddButton(x, y, 0x846, 0x845, 8, GumpButtonType.Reply, 0);

                x = 145;
                y = 15;
                y += h;
                if (c.AI == AIType.AI_NecroMage)
                    AddLabel(x, y, 50, "死灵");
                else AddLabel(x, y, 90, "死灵");

                y += h;
                if (c.AI == AIType.AI_Mage)
                    AddLabel(x, y, 50, "法师");
                else AddLabel(x, y, 90, "法师");

                y += h;
                if (c.AI == AIType.AI_Ninja)
                    AddLabel(x, y, 50, "忍者");
                else AddLabel(x, y, 90, "忍者");

                y += h;
                if (c.AI == AIType.AI_Samurai)
                    AddLabel(x, y, 50, "武士");
                else AddLabel(x, y, 90, "武士");
                y += h;
                if (c.AI == AIType.AI_Mystic)
                    AddLabel(x, y, 50, "玄术");
                else AddLabel(x, y, 90, "玄术");

                y += h;
                if (c.AI == AIType.AI_Spellweaving)
                    AddLabel(x, y, 50, "集成咒文");
                else AddLabel(x, y, 90, "集成咒文");

                y += h;
                if (c.AI == AIType.AI_Paladin)
                    AddLabel(x, y, 50, "圣殿骑士");
                else AddLabel(x, y, 90, "圣殿骑士");



            }



        }
        public enum Buttons { close, Nec, Mage, Ninja, Samurai, myth, spellweav, warrior, pal }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (!c.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }
            if (!c.Alive || !from.Alive)
            {
                return;
            }
            switch (info.ButtonID)
            {
                case (byte)Buttons.Nec:
                    {
                        if (c.Controlled && c.ControlMaster == from && from.Skills.Necromancy.Value >= 50)
                        {
                            if (c.Skills.Necromancy.Value < 50)
                            {
                                c.SetSkill(SkillName.Necromancy, 50);
                                if (c.Skills.SpiritSpeak.Value < 50)
                                {
                                    c.SetSkill(SkillName.SpiritSpeak, 50);
                                }
                            }
                            c.AI = AIType.AI_NecroMage;
                            from.SendMessage("{0} 改變AI為:死靈!", c.Name);
                            c.Say("我現在是死靈法師!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的死靈技能太低或者這個寵物不屬於你.", c.Name);
                        break;

                    }
                case (byte)Buttons.Mage:
                    {
                        if (c.Controlled && c.ControlMaster == from && from.Skills.Magery.Value >= 50)
                        {
                            if (c.Skills.Magery.Value < 50)
                            {
                                c.SetSkill(SkillName.Magery, 50);
                                if (c.Skills.Meditation.Value < 50)
                                {
                                    c.SetSkill(SkillName.Meditation, 50);
                                }
                            }
                            c.AI = AIType.AI_Mage;
                            from.SendMessage("{0} 改變AI為:法师!", c.Name);
                            c.Say("我現在是魔法師!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的法术技能太低或者這個寵物不屬於你.", c.Name);
                        break;

                    }
                case (byte)Buttons.Ninja:
                    {

                        if (c.Controlled && c.ControlMaster == from && from.Skills.Ninjitsu.Value >= 50)
                        {
                            if (c.Skills.Ninjitsu.Value < 50)
                            {
                                c.SetSkill(SkillName.Ninjitsu, 50);
                                if (c.Skills.Hiding.Value < 50)
                                {
                                    c.SetSkill(SkillName.Hiding, 50);
                                }
                            }
                            c.AI = AIType.AI_Ninja;
                            from.SendMessage("{0} 改變AI為:忍者!", c.Name);
                            c.Say("我現在是忍者!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的忍术技能太低或者這個寵物不屬於你.", c.Name);
                        break;


                    }
                case (byte)Buttons.Samurai:
                    {

                        if (c.Controlled && c.ControlMaster == from && c.ControlMaster.Skills.Bushido.Value >= 50)
                        {
                            if (c.Skills.Bushido.Value < 50)
                            {
                                c.SetSkill(SkillName.Bushido, 50);
                            }
                            c.AI = AIType.AI_Samurai;
                            from.SendMessage("{0} 改變AI為:武士!", c.Name);
                            c.Say("我現在是武士!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的武士道技能太低或者這個寵物不屬於你.", c.Name);
                        break;


                    }
                case (byte)Buttons.myth:
                    {

                        if (c.Controlled && c.ControlMaster == from && c.ControlMaster.Skills.Mysticism.Value >= 50)
                        {
                            if (c.Skills.Mysticism.Value < 50)
                            {
                                c.SetSkill(SkillName.Mysticism, 50);
                            }
                            c.AI = AIType.AI_Mystic;
                            from.SendMessage("{0} 改變AI為:秘术师!", c.Name);
                            c.Say("我現在是秘术师!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的秘术技能太低或者這個寵物不屬於你.", c.Name);
                        break;


                    }
                case (byte)Buttons.spellweav:
                    {

                        if (c.Controlled && c.ControlMaster == from && c.ControlMaster.Skills.Spellweaving.Value >= 50)
                        {
                            if (c.Skills.Spellweaving.Value < 50)
                            {
                                c.SetSkill(SkillName.Spellweaving, 50);
                            }
                            c.AI = AIType.AI_Spellweaving;
                            from.SendMessage("{0} 改變AI為:玄术!", c.Name);
                            c.Say("我現在是玄术士!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的玄术技能太低或者這個寵物不屬於你.", c.Name);
                        break;


                    }
                case (byte)Buttons.pal:
                    {

                        if (c.Controlled && c.ControlMaster == from && c.ControlMaster.Skills.Chivalry.Value >= 50)
                        {
                            if (c.Skills.Chivalry.Value < 50)
                            {
                                c.SetSkill(SkillName.Chivalry, 50);
                            }
                            c.AI = AIType.AI_Paladin;
                            from.SendMessage("{0} 改變AI為:骑士!", c.Name);
                            c.Say("我現在是圣殿骑士!");
                            if (from.HasGump(typeof(ai)))
                                from.CloseGump(typeof(ai));
                            from.SendGump(new ai(from, c));
                        }
                        else from.SendMessage("{0}無法改變,你的骑士精神技能太低或者這個寵物不屬於你.", c.Name);
                        break;


                    }

                default:
                case (byte)Buttons.close:
                    {

                        break;
                    }

            }
        }
    }


    #region main
    public class main : Gump
    {
        private BaseCreature mb;
        Mobile from;
        private static string Fromatzp(int a)
        {
            return String.Format("<div align=right>{0}%</div>", a);
        }

        public main(Mobile pm, BaseCreature bc) : base(830, 130)
        {
            mb = bc;
            from = pm;
            if (!mb.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }
            if (mb.IsDeadPet) return;

            //this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            const short x = 95;
            short y = 15;
            const short h = 20;
            const short bottom = 540;
            if (bc.Controlled && bc.ControlMaster == from)
            {
                //AddLabel(55, 122, 33, string.Format(bc.ControlOrder.ToString()));//宠物命令状态
                AddBackground(15, 110, 75, 225, 3000);
                // "宠物控制");
                byte l = 20;
                AddButton(l, 120, 2443, 2444, (byte)Buttons.petcom, GumpButtonType.Reply, 0);//宠物控制命令菜单
                AddLabel(l, 122, 90, "宠物命令");

                AddButton(l, 150, 2443, 2444, (byte)Buttons.petGuard, GumpButtonType.Reply, 0);
                if (bc.ControlOrder == OrderType.Guard)
                    AddLabel(l + 5, 152, 38, "警 戒");
                else
                    AddLabel(l + 5, 152, 90, "警 戒");
                AddButton(l, 180, 2443, 2444, (byte)Buttons.petFollow, GumpButtonType.Reply, 0);
                if (bc.ControlOrder == OrderType.Follow)
                    AddLabel(l + 5, 182, 38, "跟 随");
                else
                    AddLabel(l + 5, 182, 90, "跟 随");

                AddButton(l, 210, 2443, 2444, (byte)Buttons.petStop, GumpButtonType.Reply, 0);
                if (bc.ControlOrder == OrderType.Stop)
                    AddLabel(l + 5, 212, 38, "停 下");
                else
                    AddLabel(l + 5, 212, 90, "停 下");

                AddButton(l, 240, 2443, 2444, (byte)Buttons.petStay, GumpButtonType.Reply, 0);
                if (bc.ControlOrder == OrderType.Stay)
                    AddLabel(l + 5, 242, 38, "别 动");
                else
                    AddLabel(l + 5, 242, 90, "别 动");

                AddButton(l, 270, 2443, 2444, (byte)Buttons.petAttack, GumpButtonType.Reply, 0);
                if (bc.ControlOrder == OrderType.Attack)
                    AddLabel(l + 5, 272, 38, "攻 击");
                else
                    AddLabel(l + 5, 272, 90, "攻 击");

                AddLabel(l + 5, 290, 33, string.Format(bc.ControlOrder.ToString()));//宠物命令状态


                if (!bc.Body.IsHuman)
                {
                    AddButton(l, 320, 2443, 2444, (byte)Buttons.petStable, GumpButtonType.Reply, 0);
                    AddLabel(l + 5, 322, 90, "寄 存");
                }

                //AddButton(l, 370, 0x7ed, 0x7ec, (byte)Buttons.ok, GumpButtonType.Reply, 0);//status ok


            }

            AddBackground(80, 7, 180, bottom + 25, 3500);

            AddLabel(x + 10, y, 56, bc.Name);

            y += h;
            if (bc.Controlled)
                AddLabel(x, y, 90, string.Format("{0}", (bc.IsBonded ? "已结盟 " : "尚未结盟 ") + bc.AI.ToString()));
            else if (from.AccessLevel > AccessLevel.Player)
                AddLabel(x, y, 63, string.Format("{0}", bc.AI.ToString()));


            y += h;
            AddHtml(x, y, 100, 20, string.Format("物理防御:"), false, false);
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.PhysicalResistance), false, false);
            //AddLabel(x, y, 0, @"物理抗性： " + bc.PhysicalResistance);
            y += h;
            AddHtml(x, y, 100, 20, string.Format("火炎抗性:"), false, false);
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.FireResistance), false, false);
            //AddLabel(x, y, 0, @"火炎抗性： " + bc.FireResistance);
            y += h;
            AddHtml(x, y, 100, 20, string.Format("寒冷抗性:"), false, false);
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.ColdResistance), false, false);
            //AddLabel(x, y, 0, @"寒冷抗性： " + bc.ColdResistance);
            y += h;
            AddHtml(x, y, 100, 20, string.Format("毒物抗性:"), false, false);
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.PoisonResistance), false, false);
            //AddLabel(x, y, 0, @"能量抗性： " + bc.ColdResistance);
            y += h;
            AddHtml(x, y, 100, 20, string.Format("能量抗性:"), false, false);
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.EnergyResistance), false, false);
            //AddLabel(x, y, 0, @"毒物抗性： " + bc.ColdResistance);

            //y += h + 5;
            //AddLabel(x, y, 90, @"伤害能力 ");
            y += h;
            AddLabel(x, y, 0, @"最小伤害：");
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.DamageMin), false, false);
            y += h;
            AddLabel(x, y, 0, @"最大伤害：");
            AddHtml(x + 100, y, 40, 20, string.Format("<div align=right>{0}</div>", bc.DamageMax), false, false);

            int hit = AosAttributes.GetValue(bc, AosAttribute.AttackChance);
            int def = AosAttributes.GetValue(bc, AosAttribute.DefendChance);
            int weapon = AosAttributes.GetValue(bc, AosAttribute.WeaponDamage);
            int spell = AosAttributes.GetValue(bc, AosAttribute.SpellDamage);
            y += h;
            AddHtml(x, y, 130, 20, string.Format("命中几率增加:"), false, false);
            AddHtml(x + 100, y, 40, 20, Fromatzp(hit), false, false);

            y += h;
            AddHtml(x, y, 130, 20, string.Format("防御几率增加:"), false, false);
            AddHtml(x + 100, y, 40, 20, Fromatzp(def), false, false);
            y += h;
            AddHtml(x, y, 130, 20, string.Format("武器伤害提升:"), false, false);
            AddHtml(x + 100, y, 40, 20, Fromatzp(weapon), false, false);
            y += h;
            AddHtml(x, y, 130, 20, string.Format("魔法伤害提升:"), false, false);
            AddHtml(x + 100, y, 40, 20, Fromatzp(spell), false, false);


            y += h + 5;
            this.AddHtmlLocalized(x, y, 130, 28, (!bc.Controlled || bc.Loyalty == 0) ? 1061643 : 1049595 + (bc.Loyalty / 10), 0, false, false);
            int foodPref = 3000340;
            if ((bc.FavoriteFood & FoodType.FruitsAndVegies) != 0)
                foodPref = 1049565; // Fruits and Vegetables
            else if ((bc.FavoriteFood & FoodType.GrainsAndHay) != 0)
                foodPref = 1049566; // Grains and Hay
            else if ((bc.FavoriteFood & FoodType.Fish) != 0)
                foodPref = 1049568; // Fish
            else if ((bc.FavoriteFood & FoodType.Meat) != 0)
                foodPref = 1049564; // Meat
            else if ((bc.FavoriteFood & FoodType.Eggs) != 0)
                foodPref = 1044477; // Eggs
            y += h;
            AddLabel(x, y, 52, string.Format("喜欢的食物："));
            y += h;
            this.AddHtmlLocalized(x, y, 160, 28, foodPref, 0, false, false);
            //y += h;
            //AddLabel(x, y, 33, string.Format("{0}", bc.Loyalty));

            y += h;
            AddLabel(x, y, 0, @"声望：" + bc.Fame);
            y += h;
            AddLabel(x, y, 0, @"因果" + bc.Karma);

            if (bc is RidePolarBear && bc.Controlled)
            {
                RidePolarBear b = bc as RidePolarBear;
                y += h + 5;
                AddLabel(x, y, 90, @"等级： " + b.Level);

                y += h + 5;
                AddLabel(x, y, 90, @"可用点数： " + b.valuable);

            }

            if (bc is xhorse && bc.Controlled)
            {
                xhorse b = bc as xhorse;
                string xx;
                switch (b.AI)
                {
                    case AIType.AI_Ninja: xx = "忍者"; break;
                    case AIType.AI_Mystic: xx = "秘术师"; break;
                    case AIType.AI_Samurai: xx = "武士"; break;
                    case AIType.AI_Spellweaving: xx = "玄术士"; break;
                    case AIType.AI_NecroMage: xx = "巫师"; break;
                    case AIType.AI_Paladin: xx = "影帝"; break;
                    default: xx = "战士"; break;
                }

                y += h + 5;
                AddLabel(x, y, 90, @"第" + b.Generation + "代 " + b.Level + "级 " + xx);

                y += h;
                AddLabel(x, y, 90, @"可用点数： " + b.Valid);

                //if (b.RawStr >= 125 && (b.HitsMaxSeed < 777 && b.Valid > 0))
                if ((b.HitsMaxSeed < 777 && b.Valid > 0))
                { AddButton(x - 35, bottom - 60, 0x4ba, 0x4b9, (byte)Buttons.addhits, GumpButtonType.Reply, 0); }
                //  if (b.RawInt >= 125 && (b.ManaMaxSeed < 255 && b.Valid > 0))
                if ((b.ManaMaxSeed < 255 && b.Valid > 0))
                { AddButton(x - 35, bottom - 40, 0x4ba, 0x4b9, (byte)Buttons.addint, GumpButtonType.Reply, 0); }
                //   if (b.RawDex >= 125 && (b.StamMaxSeed < 255 && b.Valid > 0))
                if ((b.StamMaxSeed < 255 && b.Valid > 0))
                { AddButton(x - 35, bottom - 20, 0x4ba, 0x4b9, (byte)Buttons.adddex, GumpButtonType.Reply, 0); }


            }

            y += h;
            AddLabel(x, y, 20, "Sta  " + bc.RawStr + "/" + bc.RawInt + "/" + bc.RawDex);
            //y += h;
            //AddLabel(x, y, 20, string.Format("{0}/{1}/{2}", bc.StrMaxCap, bc.IntMaxCap, bc.DexMaxCap));

            //AddButton(x + 120, bottom - 60, 0x4b9, 0x4ba, (byte)Buttons.ok, GumpButtonType.Reply, 0);//ok 刷新按钮
            AddImageTiled(x, bottom - 60, 110, 17, 0x80d);//2061 红色血槽
            AddImageTiled(x, bottom - 60, ((bc.Hits) * 110 / bc.HitsMax), 17, 0x80e);//2062 蓝色血槽
            AddLabel(x + 115, bottom - 60, 50, @"" + bc.Hits + "/" + bc.HitsMax);

            AddImageTiled(x, bottom - 40, 110, 17, 0x80d);//2061 红色mana槽
            AddImageTiled(x, bottom - 40, ((bc.Mana) * 110 / bc.ManaMax), 17, 0x80e);//2062 色xi槽
            AddLabel(x + 115, bottom - 40, 93, @"" + bc.Mana + "/" + bc.ManaMax);

            AddImageTiled(x, bottom - 20, 110, 17, 0x80d);//2061 红色dex槽
            AddImageTiled(x, bottom - 20, ((bc.Stam) * 110 / bc.StamMax), 17, 0x80e);//2062 蓝色dex槽
            AddLabel(x + 115, bottom - 20, 50, @"" + bc.Stam + "/" + bc.StamMax);


            //AddImage(x + 10, bottom - 85, 0x756d);
            AddButton(x + 10, bottom - 3, 0x2a44, 0x2a58, (byte)Buttons.ok, GumpButtonType.Reply, 0);//ok 刷新按钮
            AddTooltip(1112448);//更新

            //AddImage(x + 14, bottom - 25, 0x757d);


        }
        public enum Buttons { Main, skills, AI, ok, Button4, Close, petcom, petGuard, petFollow, petStop, petStay, petAttack, petStable, addhits, adddex, addint, }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (!mb.InRange(from.Location, 22)) { from.SendMessage("Out of range."); return; }
            if (mb.IsDeadPet) return;

            switch (info.ButtonID)
            {
                case (byte)Buttons.Main:
                    {

                        break;
                    }
                case (byte)Buttons.skills:
                    {

                        break;
                    }
                case (byte)Buttons.ok:
                    {

                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.AI:
                    {

                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.Button4:
                    {

                        break;
                    }
                case (byte)Buttons.Close:
                    {

                        break;
                    }
                case (byte)Buttons.petcom:
                    {
                        from.CloseGump(typeof(ShowToolpet));
                        from.SendGump(new ShowToolpet((PlayerMobile)from));
                        break;
                    }
                case (byte)Buttons.petGuard:
                    {
                        int[] numArray = new int[] { 0x16b };
                        from.DoSpeech("*保护我", numArray, 0, 60);
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.petFollow:
                    {
                        int[] numArray = new int[] { 0x16c };
                        from.DoSpeech("*跟随我", numArray, 0, 63);
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (int)Buttons.petStop:
                    {
                        int[] numArray = new int[] { 0x167 };
                        from.DoSpeech("*停下", numArray, 0, 68);
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.petStay:
                    {
                        int[] numArray = new int[] { 0x170 };
                        from.DoSpeech("*别动", numArray, 0, 68);
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.petAttack:
                    {
                        int[] numArray = new int[] { 360 };

                        from.DoSpeech("*上吧！..*", numArray, 0, 68);
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.petStable:
                    {
                        int[] numArray = new int[] { 8 };
                        from.DoSpeech("*交给你了.*", numArray, 0, 68);
                        from.Say("stable");
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.addhits:
                    {
                        if (mb.HitsMaxSeed == -1) { mb.HitsMaxSeed = mb.RawStr; }
                        if ((((xhorse)mb).HitsMaxSeed < 777 && ((xhorse)mb).Valid > 0)) { --((xhorse)mb).Valid; ++mb.HitsMaxSeed; };

                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.adddex:
                    {
                        if (mb.StamMaxSeed == -1) { mb.StamMaxSeed = mb.RawDex; }
                        if ((((xhorse)mb).StamMaxSeed < 225 && ((xhorse)mb).Valid > 0)) { --((xhorse)mb).Valid; ++mb.StamMaxSeed; };
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }
                case (byte)Buttons.addint:
                    {
                        if (mb.ManaMaxSeed == -1) { mb.ManaMaxSeed = mb.RawInt; }

                        if ((((xhorse)mb).ManaMaxSeed < 225 && ((xhorse)mb).Valid > 0)) { --((xhorse)mb).Valid; ++mb.ManaMaxSeed; };
                        from.CloseGump(typeof(main));
                        from.SendGump(new main(from, mb));
                        break;
                    }

            }
        }
    }
    #endregion main


    public class teleporttreasure : Gump
    {
        TextRelay P;
        Point3D loc = new Point3D();
        TreasureMap tmap;
        static int Price = Config.Get("zp.Price", 50005); //传送费用　　zp
        static int price { get { return Price; } set { Price = value; } }
        public static void Initialize()
        { }
        public teleporttreasure(Mobile pm, TreasureMap tm) : base(120, 80)
        {
            tmap = tm;
            if (tmap.IsChildOf(pm.Backpack) || pm.AccessLevel >= AccessLevel.GameMaster)
            {

                loc.X = tmap.ChestLocation.X;
                loc.Y = tmap.ChestLocation.Y;
                loc.Z = tmap.Facet.Tiles.GetLandTile(loc.X, loc.Y).Z;


                AddBackground(20, 20, 280, 130, 3500);
                AddLabel(40, 35, 0, string.Format("坐标: {0}", loc.ToString()));
                AddLabel(40, 55, 0, string.Format("这是一张位于{0}的{1}级藏宝图", tmap.Facet.ToString(), tmap.Level));
                AddLabel(45, 75, 0, string.Format("花费{0}金币传送吗？", price));
                AddItem(240, 85, tmap.ItemID);
                AddItemProperty(tmap.Serial);
                AddButton(80, 110, 247, 248, 1, GumpButtonType.Reply, 0);
                AddButton(170, 110, 243, 242, 2, GumpButtonType.Reply, 0);
            }
            if (pm.AccessLevel >= AccessLevel.GameMaster)
            {
                AddLabel(300, 148, 93, "全局变量");
                AddLabel(300, 168, 93, "传送费用" + price + "个金币");
                AddTextEntry(334, 188, 60, 20, 93, 4, @"");
                AddButton(334, 208, 247, 248, 3, GumpButtonType.Reply, 0);
                AddLabel(180, 188, 93, "开门");
                AddButton(180, 208, 247, 248, 4, GumpButtonType.Reply, 0);
            }
            if (!tmap.IsChildOf(pm.Backpack))
            {
                pm.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }


        }
        #region treasureGate
        private class treasureGate : Moongate
        {
            public treasureGate(Point3D target, Map map)
                : base(target, map)
            {
                this.Map = map;

                if (this.ShowFeluccaWarning && map == Map.Felucca)
                    this.ItemID = 0xDDA;

                this.Dispellable = true;

                InternalTimer t = new InternalTimer(this);
                t.Start();
            }

            public treasureGate(Serial serial) : base(serial) { }
            public override bool ShowFeluccaWarning { get { return Core.AOS; } }
            public override void Serialize(GenericWriter writer) { base.Serialize(writer); }
            public override void Deserialize(GenericReader reader) { base.Deserialize(reader); this.Delete(); }

            private class InternalTimer : Timer
            {
                private readonly Item m_Item;
                public InternalTimer(Item item)
                    : base(TimeSpan.FromSeconds(30.0))
                {
                    this.Priority = TimerPriority.OneSecond;
                    this.m_Item = item;
                }

                protected override void OnTick()
                {
                    this.m_Item.Delete();
                }
            }
        }
        #endregion treasureGate

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile pm = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    break;
                case 1:
                    {

                        if (Banker.GetBalance(pm) < price && pm.AccessLevel < AccessLevel.GameMaster)
                        {
                            pm.SendMessage("钱不够");
                            break;
                        }
                        if (tmap.Deleted) break;

                        if (Banker.Withdraw(pm, price) && price != 0)
                        {
                            pm.SendLocalizedMessage(1060398, price.ToString()); // Amount charged
                            pm.SendLocalizedMessage(1060022, Banker.GetBalance(pm).ToString()); // Amount left, from bank
                            pm.PlaySound(0x38e);
                            pm.SendMessage("总共花了{0}个金币", price);

                            BaseCreature.TeleportPets(sender.Mobile, loc, tmap.Facet);
                            pm.MoveToWorld(loc, tmap.Facet);
                            break;
                        }
                        if (pm.AccessLevel > AccessLevel.GameMaster)
                        {
                            BaseCreature.TeleportPets(sender.Mobile, loc, tmap.Facet);
                            pm.MoveToWorld(loc, tmap.Facet);
                            break;
                        }
                        break;
                    }
                case 2:
                    pm.SendMessage("test button 2");
                    break;
                case 3:
                    {
                        if (tmap.Deleted) break;

                        P = info.GetTextEntry(4);
                        {
                            try
                            {
                                Price = Convert.ToInt16(P.Text);
                            }

                            catch
                            {
                                pm.SendMessage("Numbers only, please try again.");
                                pm.CloseGump(typeof(teleporttreasure));
                                pm.SendGump(new teleporttreasure(pm, tmap));

                                break;
                            }
                            pm.CloseGump(typeof(teleporttreasure));
                            pm.SendGump(new teleporttreasure(pm, tmap));

                            break;
                        }

                    }
                case 4:
                    {
                        if (tmap.Deleted) break;

                        if (pm.AccessLevel >= AccessLevel.GameMaster) //权限高于vip 就开传送门    2016.05.31 AccessLevel.vip 改成player了。
                        {
                            pm.SendLocalizedMessage(501024); // You open a magical gate to another location
                            Effects.PlaySound(pm.Location, pm.Map, 0x20E);
                            treasureGate firstGate = new treasureGate(loc, tmap.Facet);
                            firstGate.MoveToWorld(pm.Location, pm.Map);
                            Effects.PlaySound(loc, tmap.Facet, 0x20E);
                            treasureGate secondGate = new treasureGate(pm.Location, pm.Map);
                            secondGate.MoveToWorld(loc, tmap.Facet);
                            if (pm.HasGump(typeof(teleporttreasure)))
                                pm.CloseGump(typeof(teleporttreasure));
                            pm.SendGump(new teleporttreasure(pm, tmap));
                        }

                        break;
                    }
            }

        }
    }
    public class EmptyContainner : Gump
    {
        private BaseContainer tar;
        //      public enum contant        { bag,metalchest,pack}
        public static void Initialize()
        { }
        public EmptyContainner(BaseContainer target) : base(300, 300)
        {
            tar = target;

            AddBackground(20, 20, 300, 80, 3500);
            AddLabel(45, 35, 0, @"要清空这个容器吗？");
            AddItem(255, 35, tar.ItemID, tar.Hue);
            AddItemProperty(tar.Serial);
            AddButton(40, 60, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(180, 60, 243, 242, 2, GumpButtonType.Reply, 0);
            return;
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (tar.Deleted) return;
            if (tar.IsChildOf(from.Backpack) || from.AccessLevel > AccessLevel.GameMaster)
                switch (info.ButtonID)
                {
                    case 0:
                        break;
                    case 1:
                        {
                            //tar.Destroy();
                            List<Item> items = tar.Items;
                            for (int i = items.Count - 1; i >= 0; --i)
                            {
                                if (i >= items.Count)
                                    continue;

                                items[i].Delete();
                            }
                            from.SendMessage("容器被清空了。");

                            break;

                        }
                    case 2:
                        {
                            from.SendMessage("你决定不清空该容器。");
                            break;
                        }

                }
            else from.SendMessage("Not accessible");
        }
    }
}