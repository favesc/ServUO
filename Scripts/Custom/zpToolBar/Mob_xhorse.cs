using System;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
    [CorpseName("a xhorse corpse")]
    public class xhorse : BaseMount
    {
        public override double HealChance { get { return 1.0; } }
        //public override bool AutoRearms { get { return true; } }
        //private DateTime m_NextWeaponChange;
        public bool duhit = false;
        int exp = 0;
        byte level = 1;
        byte generation = 1;
        short valid = 0;
        bool pvp = false;

        static int DaMin = 5;
        static int DaMax = 15;

        [CommandProperty(AccessLevel.GameMaster)]
        public short Valid { get { return valid; } set { valid = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Exp { get { return exp; } set { exp = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public byte Level { get { return level; } set { level = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public byte Generation { get { return generation; } set { generation = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool PvP { get { return pvp; } set { pvp = value; } }


        public void polymorph(int bodyid, xhorse m)
        {
            if (!Controlled || IsDeadPet || IsDeadBondedPet || ControlMaster == null) return;
            if (ControlMaster.HasGump(typeof(xhgump))) ControlMaster.CloseGump(typeof(xhgump));
            m.PlaySound(0x20C);
            if (Mounted)
                Dismount(this);

            if (bodyid == 0x191)
            {
                m.BodyMod = 0x191;
                m.HueMod = 1009;
                if (Race != Race.Elf) Race = Race.Elf;
                //FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                Effects.SendBoltEffect(m, true, 0, true);//Lighting 
                m.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);

                if (m.Controlled && CanPaperdollBeOpenedBy(ControlMaster) && m.Alive)
                {
                    if (m.ControlMaster.Alive) m.ControlMaster.SendGump(new xhgump(ControlMaster, m));
                }
                return;

            }
            if (bodyid == 0xbe)

            {
                m.BodyMod = 0;
                m.HueMod = -1;
                //FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);  
                m.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);//黄色环绕
                m.FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);//刺球

                return;
            }
            else
            {
                m.BodyMod = bodyid;
                m.HueMod = -1;
                //FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
                m.FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);
                m.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);//黄色环绕

                return;
            }
            #region old polymorph
            //if (Mounted)
            //    Dismount(this);

            //if (BodyMod == 0)
            //{
            //    if (bodyid == 0x191)
            //        m.HueMod = 1009;
            //    else
            //        m.HueMod = -1;
            //    if (Mounted)
            //        Dismount(this);

            //    m.PlaySound(0x20C);
            //    m.Body = bodyid;
            //    FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
            //}
            #endregion old
        }
        [Constructable]
        public xhorse() : this("a xhorse")
        {
            //BodyMod=Body = 728;
            //Body = 0xbe;
        }
        [Constructable]
        public xhorse(string name) : base(name, 0xBE, 0x3E9E, AIType.AI_NecroMage, FightMode.Weakest, 10, 1, 0.2, 0.4)
        {

            #region Hue
            if (Utility.RandomDouble() < 0.1)
                Hue = Utility.RandomList(1461, 1365, 1266, 1257, 1173, 1166, 1168, 1151, 1150);
            else Hue = Utility.Random(1002, 57);
            //Hue = Utility.RandomList(0x97A, 0x978, 0x901, 0x8AC, 0x5A7, 0x527);
            #endregion

            InitStats(Utility.Random(385, 25), Utility.Random(360, 50), Utility.Random(370, 50));
            SetHits(980, 1334);
            SetDamage(25, 35);
            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 70, 90);


            SetSkill(SkillName.Archery, 100);
            SetSkill(SkillName.MagicResist, 90.5, 105.5);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Anatomy, 120);
            SetSkill(SkillName.Healing, 120.0);
            SetSkill(SkillName.EvalInt, 100);
            SetSkill(SkillName.MagicResist, 85);
            SetSkill(SkillName.Magery, 100);
            SetSkill(SkillName.Poisoning, 100);
            SetSkill(SkillName.Bushido, 100);
            SetSkill(SkillName.Mysticism, 100);
            SetSkill(SkillName.Ninjitsu, 100);
            SetSkill(SkillName.Necromancy, 100);
            SetSkill(SkillName.Spellweaving, 100);
            SetSkill(SkillName.Hiding, 100);
            SetSkill(SkillName.Stealth, 100);
            SetSkill(SkillName.ArmsLore, 100);
            SetSkill(SkillName.Wrestling, 50);
            SetSkill(SkillName.Chivalry, 90);

            //Body = 1290;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 60.1;
            //LeatherSkirt ls = new LeatherSkirt();
            //ls.Movable = false;
            //ls.Attributes.AttackChance = 40;
            //AddItem(ls);

            Container pack = Backpack;
            if (pack != null)
                pack.Delete();
            pack = new xBackpack();
            pack.Movable = false;
            AddItem(pack);
            int amount = Skills[SkillName.Ninjitsu].Value >= 100 ? 2 : 1;
            for (int i = 0; i < amount; i++)
            {
                Fukiya f = new Fukiya();
                f.UsesRemaining = 10;
                f.Poison = amount == 1 ? Poison.Regular : Poison.Greater;
                f.PoisonCharges = 10;
                f.Movable = true;
                PackItem(f);
            }
            if (Utility.RandomDouble() < 0.2)
                PackItem(new TreasureMap(6, Map.Trammel));
        }



        //public override void GenerateLoot() { AddLoot(LootPack.AosFilthyRich, 2); }
        public override Poison PoisonImmune { get { return Controlled ? Poison.Greater : null; } }
        //public override bool CanHealOwner { get { return true; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return false; } }
        public override bool AutoDispel { get { return true; } }
        public override bool IsSnoop(Mobile from) { if (PackAnimal.CheckAccess(this, from)) return false; return base.IsSnoop(from); }
        public override Poison HitPoison { get { return (duhit == true) ? Poison.Lethal : null; } }
        public void ComputeLevel(xhorse x)
        {
            if (!Controlled) return;
            if (x.exp >= 1000 + 20 * x.level)
            {
                x.exp -= 1000 + 20 * x.level;
                ++x.level;
                x.Emote("*LEVEL UP*");
                valid += (short)Utility.Random(3, 3);
            }
            if (x.level > 100)
            {
                ++x.generation; x.level = 1; x.Emote("*大于一百级，阶段上升*");
            }
            DaMin = generation + 5;
            DaMax = generation + 10;
        }
        public override bool StatLossAfterTame
        {
            get
            {
                //if (this.FindItemOnLayer(Layer.Pants) != null)
                //    this.FindItemOnLayer(Layer.Pants).Delete();
                SetResistance(ResistanceType.Physical, 0);
                SetResistance(ResistanceType.Fire, 0);
                SetResistance(ResistanceType.Cold, 0);
                SetResistance(ResistanceType.Poison, 0);
                SetResistance(ResistanceType.Energy, 0);

                SetSkill(SkillName.EvalInt, 13.4, 16.0);
                SetSkill(SkillName.MagicResist, 20.5, 35.5);
                SetSkill(SkillName.Magery, 55.0);
                SetSkill(SkillName.Poisoning, 100);
                SetSkill(SkillName.Bushido, 70.0);
                SetSkill(SkillName.Mysticism, 70.0);
                SetSkill(SkillName.Ninjitsu, 85.0);
                SetSkill(SkillName.Necromancy, 55.4, 56.0);
                SetSkill(SkillName.Spellweaving, 66.0);
                SetSkill(SkillName.Hiding, 100.0);
                SetSkill(SkillName.Stealth, 100);
                SetSkill(SkillName.ArmsLore, 100);
                SetSkill(SkillName.Wrestling, 25);
                SetSkill(SkillName.Chivalry, 90);
                SetSkill(SkillName.Swords, 77);
                SetSkill(SkillName.Macing, 77);
                SetSkill(SkillName.Fencing, 77);

                InitStats(Utility.Random(85, 25), Utility.Random(60, 50), Utility.Random(70, 50));
                HitsMaxSeed = -1;
                Hits = Str;
                this.AI = AIType.AI_Paladin;
                SpeechHue = EmoteHue = Utility.RandomDyedHue();
                YellHue = Utility.RandomList(36, 26, 36, 46, 56, 66, 76, 86);
                BodyMod = 0x191;
                HueMod = 1009;
                Race = Race.Elf;
                Female = true;
                HairItemID = Utility.RandomList(8252, 8253);
                HairHue = Utility.RandomList(1174, 1175, 1153, 1166, 1172, 1158);
                StrCap = 200;
                StrMaxCap = 255;
                DexCap = 125;
                DexMaxCap = 160;
                IntCap = 200;
                IntMaxCap = 255;
                SkillsCap = 50000;
                StatCap = 525;
                Fame = Utility.Random(2000);
                //AddItem(new FeatheredHat(0x6f));
                AddItem(new ThighBoots());
                AddItem(new PlainDress(Hue));
                //AddItem(new PlainDress(Utility.RandomList(0x97A, 0x978, 0x901, 0x8AC, 0x5A7, 0x527)));
                //if (Utility.RandomDouble() > .7)
                //    AddItem(new MysticalShortbow());
                //else
                //    AddItem(new FrozenLongbow());
                //LeatherNinjaBelt belt = new LeatherNinjaBelt();
                //belt.UsesRemaining = 10;
                //belt.Poison = Poison.Greater;
                //belt.PoisonCharges = 10;
                //belt.Movable = true;
                //belt.Hue = Utility.RandomBrightHue();
                //AddItem(belt);
                //generation = 1;

                //AddItem(new FemaleStuddedChest());
                polymorph(0xbe, this);

                return base.StatLossAfterTame;
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from != ControlMaster && CanPaperdollBeOpenedBy(from))
            {
                this.DisplayPaperdollTo(from);
                //from.SendMessage("无权");
                return;
            }
            if (InRange(from, 2) && Alive)
            {

                if (this.Mounted)
                    Dismount(this);
            }
            //resurect player

            base.OnDoubleClick(from);
            if (from.Mount == this) { polymorph(0xbe, this); }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (Controlled)
            {
                string title = "";
                switch (AI)
                {
                    case AIType.AI_Mage: { title = string.Format("{0}代 " + "法师", generation); break; }
                    case AIType.AI_Ninja: { title = string.Format("{0}代 " + "忍者", generation); break; }
                    case AIType.AI_NecroMage: { title = string.Format("{0}代 " + "巫师", generation); break; }
                    case AIType.AI_Samurai: { title = string.Format("{0}代 " + "武士", generation); break; }
                    case AIType.AI_Mystic: { title = string.Format("{0}代 " + "玄术师", generation); break; }
                    case AIType.AI_Spellweaving: { title = string.Format("{0}代 " + "集成咒文法师", generation); break; }
                    default: { title = string.Format("{0}代 " + "战士", generation); break; }
                }
                string x = "";
                string color = "";
                if (Fame < 5000) { color = "#FFFF00"; x = "斥候"; }
                else if (Fame < 8000) { color = "#33ADFF"; x = "勇气的探寻者"; }
                else if (Fame < 10000) { color = "#00EE00"; x = "勇气的追随者"; }
                else { color = "#00C7EE"; x = "勇气美德骑士"; }
                list.Add(1060658, "{0}\t{1}", title, String.Format("<BASEFONT COLOR={0}>{1}", color, this.level + "级 " + x));//~1_val~：~2_val~
            }
        }
        /*        public override int GetMaxResistance(ResistanceType type) //最大抗性
                {
                    if (this.level < 2)
                        return 75;
                    if (this.level < 3)
                        return 88;
                    else return 100;
                }
        */
        public override WeaponAbility GetWeaponAbility()
        {
            //this.Say("WeapAb");
            if (Controlled)
            {
                if (!Body.IsHuman && Hits > HitsMax / 2) polymorph(0x191, this);
                if (Hits <= HitsMax / 3 && Body != 1290)
                {
                    if (Mounted) Dismount(this);

                    Yell("I am angry!!!");

                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 6, Y - 4, Z + 15), Map), this, 0x36D4, 3, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 4, Y - 6, Z + 15), Map), this, 0x36D4, 5, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    polymorph(1290, this);
                    Mana = ManaMax;
                    Stam = StamMax;
                }
                if (this.Hits <= HitsMax / 4)
                    Yell("{0}! Help me! I was in danger!", ControlMaster.Name);
                if (this.Weapon != null)
                {
                    BaseWeapon weapon = this.Weapon as BaseWeapon;
                    SetDamage(DaMin + weapon.MinDamage * (generation / 2 + 1 + level / 50), DaMin + weapon.MaxDamage * (generation / 2 + 1 + level / 50));
                    return Utility.RandomBool() ? weapon.PrimaryAbility : weapon.SecondaryAbility;

                }
                else SetDamage(DaMin, DaMax);
                //if (Core.AOS)
                //{
                //    if (Combatant != null && Combatant is Mobile && ((Mobile)Combatant).Mounted) { return WeaponAbility.Dismount; }

                //    Item item = FindItemOnLayer(Layer.OneHanded);
                //    if (item == null)
                //        item = FindItemOnLayer(Layer.TwoHanded);

                //    if (item != null && item is BaseWeapon)
                //    {
                //        BaseWeapon t = (BaseWeapon)item;
                //        //SetDamage((level + RawStr) / 15 + t.AosMinDamage + (generation - 1) * 5, (level + RawStr) / 10 + t.AosMaxDamage + (generation - 1) * 5);
                //        //return (6 >= Utility.RandomMinMax(0, 10) ? WeaponAbility.ConcussionBlow : (7 >= Utility.RandomMinMax(0, 10) ? WeaponAbility.DefenseMastery : WeaponAbility.Block));
                //        return Utility.RandomBool() ? t.PrimaryAbility : t.SecondaryAbility;
                //    }
                //    //if (item != null && item is BaseWeapon && !(item is BaseShield) && !(item is BaseEquipableLight))
                //    //else if (twohanditem != null && twohanditem is BaseWeapon && !(twohanditem is BaseShield) && !(twohanditem is BaseEquipableLight))
                //    //zpzpzp 身边敌人多 就用群攻特攻。
                //    /*                  List<Mobile> targets = new List<Mobile>();
                //                      foreach (Mobile m in this.GetMobilesInRange(1))
                //                      {
                //                          if (m == ControlMaster || m == this)
                //                              continue;
                //                          targets.Add(m);
                //                      }
                //                      if (targets.Count > 2)
                //                      {
                //                          return WeaponAbility.WhirlwindAttack;
                //                      }
                //                      if (targets.Count > 1)
                //                      {
                //                          return WeaponAbility.FrenziedWhirlwind;
                //                      }
                //                      else
                //                      {
                //                          return (2 >= Utility.RandomMinMax(0, 10) ? WeaponAbility.RidingSwipe : (7 >= Utility.RandomMinMax(0, 10) ? WeaponAbility.BleedAttack : WeaponAbility.MortalStrike));
                //                      }
                //      */
                //    //else SetDamage(2, 5);
                //    //if (Combatant is Mobile)
                //    //    return ((Combatant != null && ((Mobile)Combatant).Mounted) ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow);
                //    //return WeaponAbility.ParalyzingBlow;
                //}

            }

            return WeaponAbility.ParalyzingBlow;

        }
        #region Heal and Resurrect Player

        private DateTime m_NextResurrect;
        private static TimeSpan ResurrectDelay = TimeSpan.FromSeconds(120.0);
        public virtual bool HealsPlayers { get { return true; } }
        public override void OnDoubleClickDead(Mobile m)
        {
            if (!m.Alive && this.ControlMaster == m)
            {
                if (DateTime.Now >= m_NextResurrect && InRange(m, 4) && InLOS(m))
                {

                    m_NextResurrect = DateTime.Now + ResurrectDelay;

                    if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
                    {
                        m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
                    }
                    else if (CheckResurrect(m))
                    {
                        OfferResurrection(m);
                    }
                }

            }
        }
        public virtual void OfferResurrection(Mobile m)
        {

            this.Direction = GetDirectionTo(m);
            Say(501224); // Thou hast strayed from the path of virtue, but thou still deservest a second chance.

            m.PlaySound(0x214);
            m.FixedEffect(0x376A, 10, 16);

            m.CloseGump(typeof(ResurrectGump));
            m.SendGump(new ResurrectGump(m, ResurrectMessage.Healer));

        }

        ////public virtual void OfferHeal(PlayerMobile m)
        ////{
        ////    Direction = GetDirectionTo(m);
        ////    if (m.Alive)
        ////    {
        ////        Say(501229); // You look like you need some healing my child.

        ////        m.PlaySound(0x1F2);
        ////        m.FixedEffect(0x376A, 9, 32);

        ////        m.Hits = m.HitsMax;
        ////    }

        ////    else
        ////    {
        ////        Say("Thou'rt not a decent and good person. I shall not heal thee.");
        ////    }


        ////}

        public virtual bool CheckResurrect(Mobile m)
        {
            if (m.Criminal)
            {
                Say(501222); // Thou art a criminal.  I shall not resurrect thee.
                return false;
            }
            else if (m.Kills >= 5)
            {
                Say(501223); // Thou'rt not a decent and good person. I shall not resurrect thee.
                return false;
            }

            return true;
        }

        //public virtual bool CheckHeal(Mobile m)
        //{
        //    if (m.Criminal)
        //    {
        //        Say("Thou'rt not a decent and good person. I shall not heal thee."); // Thou art a criminal.  I shall not resurrect thee.
        //        return false;
        //    }
        //    else if (m.Kills >= 5)
        //    {
        //        Say("Thou'rt not a decent and good person. I shall not heal thee."); // Thou'rt not a decent and good person. I shall not resurrect thee.
        //        return false;
        //    }

        //    return true;
        //}

        //public override void OnMovement(Mobile m, Point3D oldLocation)
        //{
        //    if (!m.Alive && this.ControlMaster == m)
        //    {
        //        if (!Controlled && !m.Frozen && DateTime.Now >= m_NextResurrect && InRange(m, 4) && !InRange(oldLocation, 4) && InLOS(m))
        //        {

        //            m_NextResurrect = DateTime.Now + ResurrectDelay;

        //            if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
        //            {
        //                m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
        //            }
        //            else if (CheckResurrect(m))
        //            {
        //                OfferResurrection(m);
        //            }
        //        }
        //        //else if (this.HealsPlayers && m.Hits < m.HitsMax && m is PlayerMobile)
        //        //{
        //        //    OfferHeal((PlayerMobile)m);
        //        //}
        //    }
        //}
        #endregion Heal and resurrect Player
        public xhorse(Serial serial) : base(serial) { }

        #region Pack Animal Methods
        public override bool OnBeforeDeath()
        {
            //if (!this.Controlled && this.FindItemOnLayer(Layer.Pants) != null)
            //    this.FindItemOnLayer(Layer.Pants).Delete();
            if (Mounted)
                Dismount(this);
            //if (this.ControlMaster != null && Backpack != null)//祝福的物品掉主人包里。     
            //移动到   public override DeathMoveResult GetInventoryMoveResultFor(Item item)
            //{
            //    for (int i = Backpack.Items.Count - 1; i >= 0; --i)
            //    {
            //        if (i >= Backpack.Items.Count)
            //            continue;
            //        if ((Backpack.Items[i]).LootType == LootType.Blessed)
            //            this.ControlMaster.Backpack.DropItem(Backpack.Items[i]);
            //    }
            //}
            polymorph(0xbe, this);
            PackAnimal.CombineBackpacks(this);
            if (!Controlled && Utility.Random(10) < 1)
                GoldShower.DoForChamp(Location, Map);

            return base.OnBeforeDeath();
        }
        //public override void OnDeath(Container c) { base.OnDeath(c); }
        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            if (item.LootType == LootType.Blessed || item.Insured)
                return DeathMoveResult.MoveToBackpack;
            else
                return DeathMoveResult.MoveToCorpse;
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (CheckFeed(from, item))
            {
                if (this.ControlMaster == from && BodyMod != 0)
                {
                    if (Mounted) Dismount(this);
                    polymorph(0xbe, this);
                    this.Say("*颗粒颗粒粑粑便*");
                }//zp

                return true;
            }

            if (PackAnimal.CheckAccess(this, from))
            {
                //if (item is AutoResPotion && 230 > this.RawStr && this.RawStr >= 125 && this.ControlMaster == from)
                if (item is AutoResPotion && 230 > this.RawStr  && this.ControlMaster == from)
                {
                    Animate(12, 5, 1, true, false, 0);//attack
                    //Animate(32, 5, 1, true, false, 0);//bow

                    this.RawStr++;
                    //this.HitsMaxSeed += 2;
                    item.Delete();

                    this.Say("yummy!");
                    from.Say("哇哦，慢点吃！");
                    return true;
                }//使用复活药水给xhorse增加1str
                if (item is PowerScroll && this.ControlMaster == from)
                {
                    PowerScroll ps = item as PowerScroll;

                    if (this.Skills[ps.Skill].Cap >= ps.Value)
                    {
                        this.Say(@"为何不肯地老天荒");
                        return false;
                    }

                    else
                    {
                        this.Skills[ps.Skill].Cap = ps.Value;
                        Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                        Effects.PlaySound(this.Location, this.Map, 0x243);

                        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 6, this.Y - 6, this.Z + 15), this.Map), this, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 4, this.Y - 6, this.Z + 15), this.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 6, this.Y - 4, this.Z + 15), this.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                        Effects.SendTargetParticles(this, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

                        this.Say(ps.Skill + "上限是" + this.Skills[ps.Skill].Cap);
                        ps.Delete();
                        return true;
                    }
                    #region 吃卷可以直接提升当前值 以后有空了做些专门喂宠物的威力卷轴
                    /*
                        var s = ps.Skill;
                        byte v = (byte)(ps.Value / 5 - 20);

                        if (this.Skills[s].Base < 125)
                        {
                            SetSkill(s, this.Skills[s].Base += v);
                            this.Say(@"" + s + " 增加了 " + v + "现在是 " + this.Skills[s].Base+"上限是 "+this.Skills[s].Cap);
                            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                            Effects.PlaySound(this.Location, this.Map, 0x243);

                            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 6, this.Y - 6, this.Z + 15), this.Map), this, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 4, this.Y - 6, this.Z + 15), this.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(this.X - 6, this.Y - 4, this.Z + 15), this.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                            Effects.SendTargetParticles(this, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

                            ps.Delete();
                            return true;

                        }
                        else
                        {
                            this.Say( "到顶了"); return false;
                        }

                */
                    #endregion
                }
                AddToBackpack(item);
                return true;
            }
            return base.OnDragDrop(from, item);
        }

        //public override bool OnEquip(Item item)
        //{
        //    //if (item is BaseRanged && Core.AOS)
        //    //{
        //    //    BaseRanged bow = item as BaseRanged;
        //    //    SetDamage(level / 20 + bow.AosMinDamage / 2 + (generation - 1) * 5, level / 10 + bow.AosMaxDamage / 2 + (generation - 1) * 5);
        //    //}

        //    return base.OnEquip(item);
        //}
        public override bool AllowEquipFrom(Mobile from) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalLift(Mobile from, Item item) { return PackAnimal.CheckAccess(this, from); }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {

            base.GetContextMenuEntries(from, list);

            PackAnimal.GetContextMenuEntries(this, from, list);

            if (from.Alive && this.Controlled && this.ControlMaster == from && Alive && !IsDeadPet)
            {
                list.Add(new xhorsePolyEntry(from, this));
                list.Add(new xhorseEntry(from, this));
            }
        }
        #endregion Pack Animal Methods
        private class xhorsePolyEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly BaseCreature m_bc;
            public xhorsePolyEntry(Mobile from, BaseCreature bc)
                : base(1075824, 8)
            {
                this.m_From = from;
                this.m_bc = bc;
            }
            public override void OnClick()
            {
                if (m_From.Alive && m_bc.Controlled && m_bc.ControlMaster == m_From && !m_bc.IsDeadPet)
                {
                    if (m_bc.BodyMod != 0) ((xhorse)m_bc).polymorph(0xbe, (xhorse)m_bc);
                    else ((xhorse)m_bc).polymorph(0x191, (xhorse)m_bc);

                }
            }

        }

        private class xhorseEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly BaseCreature m_bc;
            public xhorseEntry(Mobile from, BaseCreature bc)
                : base(1061176, 8)
            {
                this.m_From = from;
                this.m_bc = bc;
            }
            public override void OnClick()
            {
                if (m_From.Alive && m_bc.Controlled && m_bc.ControlMaster == m_From && m_bc.Alive)
                {
                    if (m_From.HasGump(typeof(ControlledMob)))
                        m_From.CloseGump(typeof(ControlledMob));
                    m_From.SendGump(new ControlledMob(m_From, m_bc));
                    //if (m_From.HasGump(typeof(main)))
                    //    m_From.CloseGump(typeof(main));
                    //m_From.SendGump(new main(m_From, m_bc));  //main 是属性界面   controlledmob 是 按钮栏
                }
            }
        }
        #region 更换武器
        /*       private void ChangeWeapon()
               {
                   if (Backpack == null)
                       return;

                   Item item = FindItemOnLayer(Layer.OneHanded);

                   if (item == null)
                       item = FindItemOnLayer(Layer.TwoHanded);

                   System.Collections.Generic.List<BaseWeapon> weapons = new System.Collections.Generic.List<BaseWeapon>();

                   foreach (Item i in Backpack.Items)
                   {
                       if (i is BaseWeapon && i != item && i.CanEquip(this))//CanEquip只要手中有物品就是false 所以只有空手才能换武器
                           weapons.Add((BaseWeapon)i);
                   }

                   if (weapons.Count > 0)
                   {
                       if (item != null)
                           Backpack.DropItem(item);

                       AddItem(weapons[Utility.Random(weapons.Count)]);

                       m_NextWeaponChange = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 14));
                   }
               }
       */
        #endregion 


        /*    zpzpzpzzpzpzpzpzpzpzpzpzp
                    public override void OnCombatantChange()
            {
                if (this.Combatant != null && this.InRange(this.Combatant, 12) && this.Map == this.Combatant.Map
            public virtual void OnCurrentOrderChanged()
            {
                if (this.ControlOrder == OrderType.Stop) { this.Body = 0xbe; }
                else if (this.ControlOrder == OrderType.Guard) { this.Body = 0x191; }
            }

                  public override bool HarmfulCheck(Mobile target)
                  {
                      if ((this.Controlled) && target is PlayerMobile)
                      {
                          this.Say("no!我不攻击玩家！");
                          this.ControlOrder = OrderType.Stop;
                          Combatant = null;
                          return false;
                      }
                      else if (this.Controlled && (target is BaseCreature && ((BaseCreature)target).Controlled))
                      {
                          this.Say("no!我不攻击玩家的宠物！");
                          this.ControlOrder = OrderType.Stop;
                          Combatant = null;

                          return false;
                      }
                      else if (CanBeHarmful(target))
                      {
                          DoHarmful(target);
                          return true;
                      }
                      else return false;
                  }
          */


        //public override bool CanBeHarmful(IDamageable target, bool message, bool ignoreOurBlessedness)
        //{
        //    if (Controlled)
        //    {
        //        if (target is PlayerMobile)
        //        {
        //            ControlTarget = null;
        //            ControlOrder = OrderType.Stop;
        //            this.ControlMaster.SendLocalizedMessage(1001018); // You can not perform negative acts on your target.
        //            return false;
        //        }
        //        else if (target is BaseCreature)
        //        {
        //            if (((BaseCreature)target).Controlled|| ((BaseCreature)target).Summoned)
        //            {
        //                ControlTarget = null;
        //                ControlOrder = OrderType.Stop;
        //                this.ControlMaster.SendLocalizedMessage(1001018); // You can not perform negative acts on your target.
        //                return false;
        //            }
        //        }
        //    }

        //        if ( /*m_Player &&*/ !Region.AllowHarmful(this, target))
        //        //(target.m_Player || target.Body.IsHuman) && !Region.AllowHarmful( this, target )  )
        //        {
        //            if (message)
        //            {
        //                SendLocalizedMessage(1001018); // You can not perform negative acts on your target.
        //}

        //            return false;
        //        }


        //       else
        //        return base.CanBeHarmful(target, message, ignoreOurBlessedness);
        //}
        /*zpzpzp */
        public override void OnThink()
        {
            //if (this.Controlled && (Combatant is PlayerMobile || (Combatant is BaseCreature && ((BaseCreature)Combatant).Controlled)))//禁止攻击玩家以及已驯服的生物。
            //{ ControlOrder = OrderType.Follow; ControlTarget = this.ControlMaster; this.Say("no!"); Combatant = null; }
            //if (!Controlled) {if (BodyMod!=1290) polymorph(1290, this); }
            if (Controlled)
            {
                if (!pvp && Combatant != null && Combatant is PlayerMobile)
                {
                    Combatant = null; ControlOrder = OrderType.Follow; ControlTarget = this.ControlMaster; Say("no pvp  no "); return;
                }

                EtherealMount ie;
                if (Alive && !Mounted && ControlMaster != null && this.Body.IsHuman && Rider == null)
                {
                    foreach (Item im in this.Backpack.Items)  //如果背包里有影子坐骑则自动使用。
                    {
                        if (im is EtherealMount)
                        {
                            ie = im as EtherealMount;
                            ie.Rider = this;
                            return;
                        }
                    }
                }
            }
            //if ((Mounted && Controlled && ControlOrder != OrderType.Follow && this.Body.IsHuman && Rider == null) || (this.Controlled && ControlMaster != null && Mounted && !ControlMaster.Mounted))
            //{ Dismount(this); }


            //if (Combatant != null && m_NextWeaponChange < DateTime.UtcNow) ChangeWeapon();
            //else if (!Body.IsHuman && Controlled && Combatant != null && Hits > HitsMax / 2 )
            //{
            //    if (Mounted) Dismount(this);
            //    PlaySound(0x20F);
            //    FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
            //    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 6, Y - 4, Z + 15), Map), this, 0x36D4, 5, 0, false, true, 48, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            //    //Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 4, Y - 6, Z + 15), Map), this, 0x63D4, 4, 0, false, true, 68, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            //    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 5, Y - 5, Z + 15), Map), this, 0x36D4, 1, 0, false, true, 132, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            //    //Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 3, Y - 3, Z + 15), Map), this, 0x36D4, 2, 0, false, true, 0x499, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            //    //PlaySound(0x20F);
            //    //FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
            //    Emote("*鲜血与荣耀*");
            //    polymorph(0x191, this);//变形
            //}
            ////			else if (Combatant != null&&Body == 0xbe) {Body = 0x191; FixedParticles(0x37B9, 1, 5, 0x251D, 0x651, 0, EffectLayer.Waist); } 	
            base.OnThink();

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write(exp);
            writer.Write(level);
            writer.Write(valid);
            writer.Write(generation);
            writer.Write(pvp);


        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version < 1 && Name == "a Xhorse")
                Name = "a xhorse";
            if (Body.IsHuman)
                this.HueMod = 1009;

            exp = reader.ReadInt();
            level = reader.ReadByte();
            valid = reader.ReadShort();
            generation = reader.ReadByte();
            pvp = reader.ReadBool();


        }
    }
}
namespace Server.Gumps
{
    public class xhgump : Gump
    {
        //Mobile pm;
        xhorse bc;
        public xhgump(Mobile from, xhorse xh) : base(777, 77)
        {
            //pm = from;
            bc = xh;
            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            AddBackground(5, 5, 80, 35, 3500);

            if (bc.CanPaperdollBeOpenedBy(from)) //纸娃娃
            {
                //    AddImageTiledButton(x, y, 0x918, 0x919, 99, GumpButtonType.Reply, 0, 8454, 0, 5, 5);
                //    AddLabel(x + 15, y + 30, 30, @"纸娃娃");
                AddButton(10, 10, 2443, 2444, 1, GumpButtonType.Reply, 0);
                AddLabel(15, 10, 30, @"纸娃娃");
            }
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (!from.Alive || bc.IsDeadPet || !bc.InRange(from, 11)) return;
            if (from.Alive && info.ButtonID == 1 && !bc.IsDeadPet)
            {
                from.CloseGump(typeof(xhgump));
                bc.DisplayPaperdollTo(from);
                from.SendGump(new xhgump(from, bc));
                if (bc.InRange(from, 3) && !bc.IsDeadPet)
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(delegate { bc.Backpack.OnDoubleClick(from); }));
            }
        }

    }

}
namespace Server.Items
{
    public class xBackpack : StrongBackpack
    {
        //public override int DefaultGumpID { get { return 0x775E; } }  zp x新的背包样式。

        [Constructable]
        public xBackpack()
            : base()
        {
            this.Layer = Layer.Backpack;
            this.Weight = 3.0;
        }


        public xBackpack(Serial serial)
            : base(serial)
        {
        }


        public override int DefaultMaxWeight
        {
            get
            {
                if (Core.ML)
                {
                    Mobile m = this.ParentEntity as Mobile;
                    if (m != null && m.Player && m.Backpack == this)
                    {
                        return 950;
                    }
                    else
                    {
                        return base.DefaultMaxWeight;
                    }
                }
                else
                {
                    return base.DefaultMaxWeight;
                }
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }
}
