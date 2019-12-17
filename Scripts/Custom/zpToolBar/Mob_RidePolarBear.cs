using Server.Commands;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Rideable Polar Bear corpse")]
    //[TypeAlias("Server.Mobiles.Polarbear")]
    public class RidePolarBear : BaseMount
    {

        int exp = 0;
        byte level = 1;
        [CommandProperty(AccessLevel.GameMaster)]
        public int valuable { get; set; }
        //Mobile m;
        [CommandProperty(AccessLevel.GameMaster)]
        public int Exp { get { return exp; } set { exp = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public byte Level { get { return level; } set { level = value; } }
        [Constructable]
        public RidePolarBear() : this("a Rideable Polar bear") { }

        public RidePolarBear(string name) : base(name, 213, 0x3EC5, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)

        {

            this.Name = "a Rideable Polar bear";
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Utility.AssignRandomHair(this);
            Female = Utility.RandomBool();

            this.Body = 213;
            this.BaseSoundID = 0xA3;

            this.InitStats(125, 50, 225);
            this.HitsMaxSeed = 125;
            this.SetDamage(7, 10);

            this.SetDamageType(ResistanceType.Cold, 100);
            SetDamageType(ResistanceType.Physical, 0);



            this.SetSkill(SkillName.MagicResist, 75.1, 90.0);
            this.SetSkill(SkillName.Tactics, 77);
            this.SetSkill(SkillName.Wrestling, 120.0);
            this.SetSkill(SkillName.Anatomy, 120.0);
            this.SetSkill(SkillName.Healing, 125.0);
            this.SetSkill(SkillName.Swords, 120.0);
            this.SetSkill(SkillName.Macing, 77);
            this.SetSkill(SkillName.Swords, 77);
            this.SetSkill(SkillName.Fencing, 77);
            //StatCap = 375;
            SkillsCap = 9000;


            this.Fame = 0;
            this.Karma = 0;

            this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 35.1;
        }
        public RidePolarBear(Serial serial)
            : base(serial)
        {
        }
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (this.Controlled && this.ControlMaster == from)
            {
                list.Add(new RPBEntry(from, this));
            }
        }
        private class RPBEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly RidePolarBear m_bc;
            public RPBEntry(Mobile from, RidePolarBear rpb)
                : base(3005031, 8)
            {
                m_From = from;
                m_bc = rpb;
            }

            public override void OnClick()
            {
                if (m_From.HasGump(typeof(Gumps.Polar)))
                    m_From.CloseGump(typeof(Gumps.Polar));
                m_From.SendGump(new Gumps.Polar(m_bc));
            }
        }


        public void ComputeLevel(RidePolarBear rp)
        {

            if (rp.exp >= 500 + 10 * rp.level * rp.level) { rp.exp -= 500 + 10 * rp.level * rp.level; rp.level++; rp.Say("haha"); valuable += Utility.Random(3, 7); }


        }
        public override bool AllowEquipFrom(Mobile from) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalLift(Mobile from, Item item) { return PackAnimal.CheckAccess(this, from); }


        public override void OnDoubleClick(Mobile from)
        {
            if (this.Body.IsHuman) { this.DisplayPaperdollTo(from); return; }
            if (from != ControlMaster || this.level < 10)
            {
                from.SendMessage("此熊尚幼");
                return;
            }
            base.OnDoubleClick(from);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            string color = "#1EFF00";

            base.GetProperties(list);
            if (this.Controlled && this.level < 10)
                list.Add(1060658, "{0}\t{1}", "年幼的", String.Format("<BASEFONT COLOR={0}>{1}", color, this.level + " 级 坦克"));
            else if (this.Controlled)
                list.Add(1060658, "{0}\t{1}", "可骑乘", String.Format("<BASEFONT COLOR={0}>{1}", color, this.level + " 级 坦克"));

        }




        //public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
        //public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }
        //public override bool CanHealOwner { get { return true; } }
        public override WeaponAbility GetWeaponAbility()
        {
            if (Core.AOS)
            {
                //Item item = FindItemOnLayer(Layer.OneHanded);
                //Item twohanditem = FindItemOnLayer(Layer.TwoHanded);
                Item item = FindItemOnLayer(Layer.OneHanded);
                if (item == null)
                    item = FindItemOnLayer(Layer.TwoHanded);

                if (item != null && item is BaseWeapon)
                {
                    BaseWeapon t = (BaseWeapon)item;
                    return Utility.RandomBool() ? t.PrimaryAbility : t.SecondaryAbility;
                }
                else
                    return WeaponAbility.ParalyzingBlow;

            }
            return WeaponAbility.ParalyzingBlow;


        }

        public override bool CanBeHarmful(IDamageable target, bool message)
        {
            if (Controlled)
            {
                if (target is PlayerMobile) return false;
                else if (target is BaseCreature)
                {
                    if (((BaseCreature)target).Controlled) return false;
                    if (((BaseCreature)target).Summoned/* && ((BaseCreature)target).SummonMaster == this.ControlMaster*/) return false;
                    else
                        return base.CanBeHarmful(target, message);

                }
                else
                    return base.CanBeHarmful(target, message);

            }

            else
                return base.CanBeHarmful(target, message);
        }

        public override void OnThink()
        {
            if (this.Controlled && (Combatant is PlayerMobile || (Combatant is BaseCreature && ((BaseCreature)Combatant).Controlled))) { ControlOrder = OrderType.Follow; ControlTarget = this.ControlMaster; this.Say("no!"); Combatant = null; }


            if (this.Combatant != null && this.Body != 213)
            {
                this.Body = 213; this.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
            }
            base.OnThink();
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write(exp);
            writer.Write(level);
            writer.Write(valuable);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            exp = reader.ReadInt();
            level = reader.ReadByte();
            valuable = reader.ReadInt();

        }
    }
}
namespace Server.Gumps
{
    public class Polar : Gump
    {
        RidePolarBear b;
        public Polar(RidePolarBear rpb) : base(600, 150)
        {
            b = rpb;
            byte y = 75;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddBackground(0, 0, 420, 300, 3500);
            AddLabel(30, 20, 0, @"经验:" + rpb.Exp);
            AddLabel(30, 45, 0, @"等级:" + rpb.Level);
            AddLabel(140, 20, 90, @"可用点数:" + rpb.valuable);
            //AddAlphaRegion(289, 15, 111, 272);
            AddLabel(30, y, 0, @"血量:   " + b.HitsMaxSeed);
            if (b.HitsMaxSeed < 3000) AddButton(170, y, 2076, 2075, 1, GumpButtonType.Reply, 0); else AddLabel(170, y, 0, "已达上限");
            y += 25;
            AddLabel(30, y, 0, @"精力:  " + rpb.RawDex);
            if (b.RawDex < 175) AddButton(170, y, 2076, 2075, 2, GumpButtonType.Reply, 0); else AddLabel(170, y, 0, "已达上限");
            y += 25;

            AddLabel(30, y, 0, @"最小伤害值:   " + rpb.DamageMin);
            if (rpb.DamageMin < rpb.DamageMax - 1) AddButton(170, y, 2076, 2075, 3, GumpButtonType.Reply, 0); else AddLabel(170, y, 0, "已达上限");
            y += 25;

            AddLabel(30, y, 0, @"最大伤害值:   " + rpb.DamageMax);
            if (rpb.DamageMax < 20) AddButton(170, y, 2076, 2075, 4, GumpButtonType.Reply, 0); else AddLabel(170, y, 0, "已达上限");
            y += 25;

            AddLabel(30, y, 0, @"伤害类型: " + "寒冷");
            AddButton(170, y, 2076, 2075, 5, GumpButtonType.Reply, 0);
            y += 25;

            AddLabel(30, y, 0, @"这个是坦克");
            AddButton(170, y, 2076, 2075, 5, GumpButtonType.Reply, 0);

            AddLabel(30, 270, 10, @"" + b.Name+(b.Female?"  雌性":"  雄性"));

            AddHtml(293, 19, 105, 232, string.Format(b.GetResistance(ResistanceType.Physical) + "\n" + b.GetResistance(ResistanceType.Fire) + "\n" + b.GetResistance(ResistanceType.Cold) + "\n" + b.GetResistance(ResistanceType.Poison) + "\n" + b.GetResistance(ResistanceType.Energy) + "\n" + " hits" + b.Hits + " stam" + b.Stam + " mana" + b.Mana + " fame" + b.Fame + " karma" + b.Karma), true, true);

            //AddLabel(290, 260, 0, @"变形");
            //AddButton(330, 260, 2076, 2075, 6, GumpButtonType.Reply, 0);
            AddButton(330, 260, 2443, 2444, 6, GumpButtonType.Reply, 0);
            AddItem(300, 255, 8417);
            AddLabel(340, 255, 30, "变 形");


            //AddButton(310, 255, 2076, 2075, 16, GumpButtonType.Reply, 0);

        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                default:
                case 0:
                    {
                        if (from.HasGump(typeof(ControlledMob)))
                            from.CloseGump(typeof(ControlledMob));
                        from.SendGump(new ControlledMob(from, b));
                        break;
                    }
                case 1: { if (b.HitsMaxSeed < 3000 && b.valuable > 0) { --b.valuable; b.HitsMaxSeed += 10; } from.SendGump(new Polar(b)); break; }
                case 2: { if (b.RawDex < 175 && b.valuable > 0) { --b.valuable; ++b.RawDex; } from.SendGump(new Polar(b)); break; }
                case 3: { if (b.DamageMin < b.DamageMax - 1 && b.valuable > 0) { --b.valuable; ++b.DamageMin; } from.SendGump(new Polar(b)); break; }
                case 4: { if (b.DamageMax < 20 && b.valuable > 0) { --b.valuable; ++b.DamageMax; } from.SendGump(new Polar(b)); break; }
                case 5: { from.SendGump(new Polar(b)); break; }



                case 6:
                    {
                        if (!b.Body.IsHuman)
                        {
                            b.Body = (b.Female ? 0x191 : 0x190);
                            b.DisplayPaperdollTo(from);
                            b.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                        }
                        else
                        {
                            b.Body = 213;
                            b.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                        }

                        from.CloseGump(typeof(Polar));
                        from.SendGump(new Polar(b));
                        break;
                    }
                case 16:
                    {
                        if (from.HasGump(typeof(ControlledMob)))
                            from.CloseGump(typeof(ControlledMob));
                        from.SendGump(new ControlledMob(from, b));
                        break;
                    }

            }
        }
    }
}
