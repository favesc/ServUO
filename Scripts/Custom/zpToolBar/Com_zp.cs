using Server.Targeting;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Items;
using System;
using Server.Commands;
using Server.Gumps;
/* 
包括命令 【z 复活     和 Zearring 耳环  和 baodongxi 地上爆炸出战利品。幻化物品【hh
*/
namespace Server.Commands
{
    public class zp
    {
        public static void Initialize()
        {
            Register();
        }

        public static void Register()
        {
            CommandSystem.Register("zp", AccessLevel.GameMaster, new CommandEventHandler(Recover_OnCommand));
            CommandSystem.Register("z", AccessLevel.Player, new CommandEventHandler(Recover_OnCommand));
        }

        private class RecoverTarget : Target
        {
            public RecoverTarget(Mobile m) : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                Mobile m;
                PlayerMobile mo;
                BaseCreature mx;
                if (!BaseCommand.IsAccessible(from, o))
                    from.SendMessage("That is not accessible.");
                else if (o is BaseCreature) { mx = (BaseCreature)o; mx.PlaySound(0x214); mx.FixedEffect(0x376A, 10, 16); mx.ResurrectPet(); mx.Hits = mx.HitsMax; mx.Stam = mx.StamMax; mx.Mana = mx.ManaMax; }
                else if (o is Mobile)
                {
                    m = (Mobile)o;
                    if (m.Alive && m is PlayerMobile && m.Account != null && m.NetState.Address.ToString() == "127.0.0.1")
                    {
                        mo = m as PlayerMobile;
                        if (mo.FindItemOnLayer(Layer.Earrings) == null)
                        {
                            var it = new Zearring();
                            it.BlessedBy = mo;
                            mo.AddItem(it);
                        }
                    }
                    if (!m.Alive)
                    {
                        m.PlaySound(0x214);
                        m.FixedEffect(0x376A, 10, 16);
                        m.Resurrect();
                        from.SendMessage("Resurrect it.");
                    }
                    m.Hits = m.HitsMax;
                    m.Stam = m.StamMax;
                    m.Mana = m.ManaMax;
                    m.Thirst = 20;
                    m.Hunger = 20;
                    from.SendMessage("and Recover it.");
                }
                else
                    from.SendMessage("That is not a mobile.");
            }
        }

        [Usage("zp")]
        [Aliases("z")]
        [Description("Ressurrects and recovers Thirst, Hunger, Hits, Stam and Mana of the targeted at the maximum level.if LocalIPAddres,another fanction will work")]
        private static void Recover_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new RecoverTarget(e.Mobile);
        }
    }
}
namespace Server.Items
{
    public class Zearring : GoldEarrings
    {
        public override bool IsArtifact { get { return true; } }
        public bool PowerUp { get { return powerup; } }
        static bool powerup = false;

        [Constructable]
        public Zearring()
        {
            this.Attributes.BonusInt = 5;
            this.Attributes.SpellDamage = 10;
            Attributes.LowerRegCost = 100;
            Attributes.LowerManaCost = 80;
            LootType = LootType.Blessed;
            Hue = 1272;
            Name = "Zsa Earring";
            Attributes.AttackChance = 40;
            Attributes.CastRecovery = 4;
            Attributes.CastSpeed = 3;
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (this.Parent != m)
            {
                return;
            }
            if (m is PlayerMobile && m.Account != null && m.NetState.Address.ToString() == "127.0.0.1")
            {
                PlayerMobile mo = m as PlayerMobile;
                if (!mo.AbyssEntry)
                {
                    mo.AbyssEntry = true;
                    mo.BasketWeaving = true;
                    mo.Bedlam = true;
                    mo.GemMining = true;
                    mo.Masonry = true;
                    mo.SandMining = true;
                    mo.StoneMining = true;
                    mo.Spellweaving = true;
                    mo.InitStats(120, 90, 50);
                    mo.SkillsCap = 72000;
                    mo.Glassblowing = true;

                }
                mo.PointSystems.GauntletPoints = (mo.PointSystems.GauntletPoints < 60000 ? 66666 : mo.PointSystems.GauntletPoints);
                mo.AccountSovereigns = (mo.AccountSovereigns < 5000 ? 90000 : mo.AccountSovereigns);
                if (!powerup)
                {
                    mo.Emote("* aha * ");
                    mo.PlaySound(0x653);
                    mo.FixedParticles(0x375A, 1, 17, 0x7DA, 33, 0x3, EffectLayer.Waist);
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => { mo.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist); });
                    Hue = 1172;
                    mo.SendMessage("Powerfull");
                    powerup = true;
                    Name = "War fleze";
                }
                else
                {
                    mo.SendMessage("there's no power.");
                    Hue = 1174;
                    powerup = false;
                    Name = "UnZsa Earring";
                }
            }
        }
        public Zearring(Serial serial)
            : base(serial)
        {
        }
        //public override int LabelNumber
        //{
        //    get
        //    {
        //        return 1094927;
        //    }
        //}// Djinni's Ring [Replica]
        public override int InitMinHits { get { return 150; } }
        public override int InitMaxHits { get { return 150; } }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            Hue = 1272;
            Name = "Zsa Earring";
            powerup = false;
        }
    }
}
namespace Server
{
    public class baodongxi
    {
        #region zp 脚底下爆炸出好东西。
        static BaseCreature bc;
        public static void bao(BaseCreature b)
        {
            bc = b;
            Item g = item;
            if (g == null)
            {
                return;
            }
            else
            {
                Point3D p = Findloc(bc.Map, bc.Location, 2, bc);
                //var g = new ParagonChest(Name, TreasureMapLevel);
                Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                Effects.PlaySound(bc, bc.Map, 0x307);
                //var g = item;
                //c.DropItem(new ParagonChest(Name, TreasureMapLevel));

                g.MoveToWorld(p, bc.Map);
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 30, 5052);
                    Effects.PlaySound(g, g.Map, 0x208);
                });
                Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x375A, 9, 60, 5027));
            }
        }
        static Item item
        {
            get
            {
                if (Paragon.ChestChance > Utility.RandomDouble() / 3 && bc.TreasureMapLevel > 1)
                //if(true)
                {
                    return new ParagonChest(bc.Name, bc.TreasureMapLevel);
                }
                if (Utility.RandomDouble() / 3 < 0.125)
                {
                    switch (Utility.Random(16))
                    {
                        case 0: return (new MyrmidonGloves());
                        case 1: return (new MyrmidonGorget());
                        case 2: return (new MyrmidonLegs());
                        case 3: return (new MyrmidonArms());
                        case 4: return (new PaladinArms());
                        case 5: return (new PaladinGorget());
                        case 6: return (new LeafweaveLegs());
                        case 7: return (new DeathChest());
                        case 8: return (new DeathGloves());
                        case 9: return (new DeathLegs());
                        case 10: return (new GreymistGloves());
                        case 11: return (new GreymistArms());
                        case 12: return (new AssassinChest());
                        case 13: return (new AssassinArms());
                        case 14: return (new HunterGloves());
                        case 15: return (new HunterLegs());
                    }
                }

                return null;
            }
        }
        static Point3D Findloc(Map map, Point3D coploc, int range, BaseCreature o)
        {
            bc = o;
            map = bc.Map;
            coploc = bc.Location;
            int cx = coploc.X;
            int cy = coploc.Y;
            for (int i = 0; i < 16; ++i)
            {

                int x = cx + Utility.Random(range * 2) - range;
                int y = cy + Utility.Random(range * 2) - range;
                if ((cx - x) * (cx - x) + (cy - y) * (cy - y) > range * range)
                    continue;

                //cz = map.GetAverageZ(cx, cy);
                int cz = map.Tiles.GetLandTile(x, y).Z;
                if (map.CanSpawnMobile(x, y, cz))
                {
                    return new Point3D(x, y, cz);
                }
            }

            return coploc;
        }

        #endregion
    }
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
