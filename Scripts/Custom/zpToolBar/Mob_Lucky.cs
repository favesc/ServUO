using System;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Spells.Mysticism;
using Server.Spells.Spellweaving;
using System.Collections;
/*
if (LastKiller is xhorse && ((xhorse)LastKiller).Controlled && ((xhorse)LastKiller).Level < 255)//zp
{
xhorse x = LastKiller as xhorse;
Titles.AwardFame(x, this.Fame / 100, false);
x.Exp += this.Fame / 100;
x.ControlMaster.SendMessage(90,"{0}获得经验：{1}", x.Name, this.Fame / 100);
x.ComputeLevel(x);
}
if (LastKiller is RidePolarBear && ((RidePolarBear)LastKiller).Controlled && ((RidePolarBear)LastKiller).Level < 255)
{
RidePolarBear l = (RidePolarBear)LastKiller;
Titles.AwardFame(l, this.Fame / 100, false);
l.Exp += this.Fame / 100;
l.ControlMaster.SendMessage(56,"{0}获得经验：{1}", l.Name, this.Fame / 100);
l.ComputeLevel(l);
}

if (LastKiller is Lucky && ((Lucky)LastKiller).Controlled && ((Lucky)LastKiller).Level < 255)
{
Lucky l = (Lucky)LastKiller;
Titles.AwardFame(l, this.Fame / 100, false);
Titles.AwardKarma(l, Karma / 100, false);
l.Exp += this.Fame / 100;
l.ControlMaster.SendMessage(26,"{0}获得经验：{1}", l.Name, this.Fame / 100);
l.ComputeLevel(l);
}
Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback( delegate
{       from.Frozen = false;
from.PrivateOverheadMessage(MessageType.Regular, 65, 1112941, from.NetState); // You manage to free your hand!
}));

if (player.Alive)
player.SendGump(new Gumps.Opencorpp(Gumps.Opencorpp.itemlist(this)));

if (player.Alive)
if (player.HasGump(typeof(Opencorpp)))
player.CloseGump(typeof(Opencorpp));
player.SendGump(new Opencorpp(Opencorpp.itemlist(this)));
base.OnDoubleClick(from);


*/
namespace Server.Mobiles
{
    [CorpseName("a Lucky corpse")]
    public class Lucky : BaseMount
    {
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
        #region edit
        [Constructable]
        public Lucky() : this("a lucky")
        {
        }
        [Constructable]
        public Lucky(string name) : base(name, 719, 16076, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            //SpeechHue = Utility.RandomDyedHue();
            YellHue = 15;
            //Race = Race.Elf;
            Female = true;
            HairItemID = Utility.RandomList(8252, 8253);
            HairHue = Utility.RandomList(1174, 1175, 1153, 1166, 1172, 1158);
            Hue = GetHue();
            Fame = 13500;

            InitStats(Utility.Random(125, 50), Utility.Random(200, 50), Utility.Random(60, 25));
            SetHits(980, 1334);
            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Energy, 30);
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

            //BaseSoundID = 0xA8;//horse
            SkillsCap = 50000;
            StatCap = 1450;
            AddItem(new ThighBoots());
            AddItem(new PlainDress(Hue));

            AddItem(new FemaleStuddedChest());

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 10.1;

            Container pack = Backpack;
            if (pack != null)
                pack.Delete();
            pack = new xBackpack();
            pack.Movable = false;
            AddItem(pack);
            if (Utility.RandomDouble() < 0.2)
                PackItem(new TreasureMap(6, this.Map));
        }
        #endregion edit
        private static int GetHue()
        {
            int rand = Utility.Random(1075);
            if (rand <= 0)
                return 0x855C;
            else if (rand <= 1)
                return 0x8490;
            else if (rand <= 3)
                return 0x8030;
            else if (rand <= 5)
                return 0x8037;
            else if (rand <= 8)
                return 0x8295;
            else if (rand <= 11)
                return 0x8123;
            else if (rand <= 16)
                return 0x8482;
            else if (rand <= 24)
                return 0x8487;
            else if (rand <= 34)
                return 0x8032;
            else if (rand <= 44)
                return 0x8899;
            else if (rand <= 54)
                return 0x8495;
            else if (rand <= 64)
                return 0x848D;
            else if (rand <= 74)
                return 0x847F;

            return 0;
        }

        public override int GetIdleSound() { return 0x577; }
        public override int GetAttackSound() { return Body.IsHuman?Utility.Random(0x136,9): 0x576; }
        public override int GetAngerSound() { return 0x578; }
        public override int GetHurtSound() { return    this.Body.IsHuman?   Utility.Random(0x14c,5) :0x576; }
        public override int GetDeathSound() { return this.Body.IsHuman ? Utility.Random(0x151, 2) : 0x579; }

        public override double HealChance { get { return 1.0; } }
        //public override bool AutoRearms { get { return true; } }
        //public override void GenerateLoot() { AddLoot(LootPack.AosFilthyRich, 2); }
        public override Poison PoisonImmune { get { return Controlled ? Poison.Greater : null; } }
        //public override bool CanHealOwner { get { return true; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool AutoDispel { get { return true; } }
        public override bool IsSnoop(Mobile from) { if (PackAnimal.CheckAccess(this, from)) return false; return base.IsSnoop(from); }
        public override Poison HitPoison { get { return (duhit == true) ? Poison.Lethal : null; } }
        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            if (item.LootType == LootType.Blessed || item.Insured)
                return DeathMoveResult.MoveToBackpack;
            else
                return DeathMoveResult.MoveToCorpse;
        }
        public void ComputeLevel(Lucky x)
        {
            if (!Controlled) return;
            if (x.exp >= 1000 + 20 * x.level)
            {
                x.exp -= 1000 + 20 * x.level;
                ++x.level;
                x.Emote("*等级提升*");
                valid += (short)Utility.Random(3, 3);
            }
            if (x.level > 100)
            {
                ++x.generation; x.level = 1; x.Emote("*大于一百级，阶段上升*");
            }
            //SetDamage(level / 10 + 12 + generation * 5, level / 5 + 13 + generation * 5);
            DaMin = generation + 5;
            DaMax = generation + 10;

        }
        public override bool StatLossAfterTame
        {
            get
            {
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
                SetSkill(SkillName.Chivalry, 85);
                this.SkillsCap = 50000;
                StrCap = 220;
                DexCap = 350;
                IntCap = 150;
                StatCap = 720;
                this.HitsMaxSeed = this.HitsMax;
                this.ChangeAIType(AIType.AI_Paladin);
                return base.StatLossAfterTame;
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            //if (this.Body.IsHuman) { this.DisplayPaperdollTo(from); return; }
            if (from != ControlMaster && CanPaperdollBeOpenedBy(from))
            {
                this.DisplayPaperdollTo(from);

                //from.SendMessage("无权");
                return;
            }
            //if (this.Mounted)
            //    Dismount(this);
            base.OnDoubleClick(from);
            if (from.Mount == this) { polymorph(719, this); }

        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (Controlled)
            {
                string title = "";
                switch (AI)
                {
                    case AIType.AI_Mage: { title = string.Format("{0}代 " + "魔法师", generation); break; }
                    case AIType.AI_Ninja: { title = string.Format("{0}代 " + "忍者", generation); break; }
                    case AIType.AI_NecroMage: { title = string.Format("{0}代 " + "巫师", generation); break; }
                    case AIType.AI_Samurai: { title = string.Format("{0}代 " + "武士", generation); break; }
                    case AIType.AI_Mystic: { title = string.Format("{0}代 " + "玄术师", generation); break; }
                    case AIType.AI_Spellweaving: { title = string.Format("{0}代 " + "秘术师", generation); break; }
                    case AIType.AI_Paladin: { title = string.Format("{0}代 " + "圣骑士", generation); break; }

                    default: { title = string.Format("{0}代 " + "战士", generation); break; }
                }
                string x = "";
                string color = "";
                if (Fame < 5000) { color = "#FFFF00"; x = "新兵"; }
                else if (Fame < 8000) { color = "#33ADFF"; x = "勇气的探寻者"; }
                else if (Fame < 10000) { color = "#00EE00"; x = "勇气的追随者"; }
                else { color = "#00C7EE"; x = "勇气美德骑士"; }

                list.Add(1060658, "{0}\t{1}", title, String.Format("<BASEFONT COLOR={0}>{1}", color, this.level + "级 " + x));//~1_val~：~2_val~

            }
        }
        public override WeaponAbility GetWeaponAbility()
        {
            if (Controlled)
            {
                //if (BodyMod != 0x191 && Hits > HitsMax / 2) polymorph(0x191, this);
                //if (Hits <= HitsMax / 3 && BodyMod != 1290)
                //{
                //    if (Mounted) Dismount(this);

                //    Yell("I am angry!!!");

                //    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 6, Y - 4, Z + 15), Map), this, 0x36D4, 3, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                //    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 4, Y - 6, Z + 15), Map), this, 0x36D4, 5, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                //    polymorph(1290, this);
                //    Mana = ManaMax;
                //    Stam = StamMax;
                //}
                if (this.Hits <= HitsMax / 4)
                    Yell("{0}! Help me! I was in danger!", ControlMaster.Name);

                //if (Core.AOS)
                //{
                //    if (Combatant != null && Combatant is Mobile && ((Mobile)Combatant).Mounted) { return WeaponAbility.Dismount; }

                //    Item item = FindItemOnLayer(Layer.OneHanded);
                //    if (item == null)
                //        item = FindItemOnLayer(Layer.TwoHanded);

                //    if (item != null && item is BaseWeapon)
                //    {
                //        BaseWeapon tt = (BaseWeapon)item;
                //        //SetDamage((level ) / 15 + tt.AosMinDamage + (generation - 1) * 5, (level ) / 10 + tt.AosMaxDamage + (generation - 1) * 5);
                //        return Utility.RandomBool() ? tt.PrimaryAbility : tt.SecondaryAbility;
                //    }
                //}

                if (this.Weapon != null)
                {
                    BaseWeapon weapon = this.Weapon as BaseWeapon;
                    SetDamage(DaMin + weapon.MinDamage * (generation / 2 + 1 + level / 50), DaMin + weapon.MaxDamage * (generation / 2 + 1 + level / 50));
                    return Utility.RandomBool() ? weapon.PrimaryAbility : weapon.SecondaryAbility;

                }
                else SetDamage(DaMin, DaMax);
                //else SetDamage(12, 15);
                //if (Combatant is Mobile)
                //    return ((Combatant != null && ((Mobile)Combatant).Mounted) ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow);
                //return WeaponAbility.ParalyzingBlow;
            }
            return WeaponAbility.ParalyzingBlow;
            //return (Utility.RandomBool()) ? WeaponAbility.ParalyzingBlow : WeaponAbility.BleedAttack;
        }





        public Lucky(Serial serial) : base(serial) { }

        #region Pack Animal Methods
        public override bool OnBeforeDeath()
        {
            //if (Mounted)
            //    Dismount(this);
            polymorph(719, this);
            if (ControlMaster != null && this.ControlMaster.HasGump(typeof(Luckgump))) ControlMaster.CloseGump(typeof(Luckgump));
            PackAnimal.CombineBackpacks(this);
            //if  (Utility.Random(10)<1)
            //GoldShower.DoForChamp(Location, Map);
            if (!Controlled)
            {
                baodongxi.bao(this as BaseCreature);
            }

            return base.OnBeforeDeath();


        }
        public override void OnDeath(Container c) {  base.OnDeath(c); }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (CheckFeed(from, item))
            {
                if (BodyMod != 0 && this.ControlMaster == from)
                {
                    polymorph(719, this);
                    this.Say("*动物形态*");
                }//zp
                return true;
            }
            if (PackAnimal.CheckAccess(this, from))
            {
                if (item is PowerScroll && this.ControlMaster == from)
                {
                    PowerScroll ps = item as PowerScroll;

                    if (this.Skills[ps.Skill].Cap >= ps.Value)
                    {
                        this.Say(@"魏丽娟周等级有点低了");
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
                        ps.Consume();
                        return true;
                    }
                    #region 吃卷可以直接提升当前值 以后有空了做些专门喂宠物的威力卷轴
                    #endregion
                }
                AddToBackpack(item);
                return true;
            }
            return base.OnDragDrop(from, item);
        }

        ////public override bool OnEquip(Item item)
        ////{
        ////    //if (item is BaseRanged && Core.AOS)
        ////    //{
        ////    //    BaseRanged bow = item as BaseRanged;
        ////    //    //SetDamage(level / 20 + bow.AosMinDamage / 2 + (generation - 1) * 5, level / 10 + bow.AosMaxDamage / 2 + (generation - 1) * 5);
        ////    //}

        ////    return base.OnEquip(item);
        ////}
        public override bool AllowEquipFrom(Mobile from) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target) { return PackAnimal.CheckAccess(this, from); }
        public override bool CheckNonlocalLift(Mobile from, Item item) { return PackAnimal.CheckAccess(this, from); }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {

            base.GetContextMenuEntries(from, list);
            PackAnimal.GetContextMenuEntries(this, from, list);
            //if (IsBodyMod && !Body.IsHuman) list.Remove(new PaperdollEntry(this));
            if (from.Alive && this.Controlled && this.ControlMaster == from && !this.IsDeadPet)
            {
                list.Add(new PolyEntry(from, this));
            }
        }
        #endregion Pack Animal Methods
        //public void polymorph(Mobile m)
        //{
        //    //if (m.Body.IsHuman) { BodyMod = 243; }
        //    //else { BodyMod = 0; }
        //}
        public void polymorph(Body bd, Lucky m)
        {
            //if (Mounted)
            //    Dismount(this);
            if (!Controlled || IsDeadPet || IsDeadBondedPet || ControlMaster == null) return;

            m.PlaySound(0x20C);
            if (bd == 0x191)
            {
                m.BodyMod = 0x191;
                m.HueMod = 1009;
                if (Race != Race.Elf)
                    Race = Race.Elf;
                FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);

            }
            else if (bd == 719)

            {
                m.BodyMod = 0;
                m.HueMod = -1;
                FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
            }
            else
            {
                m.BodyMod = bd;
                m.HueMod = -1;
                FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
            }
            if (m.Controlled && m.ControlMaster != null && m.ControlMaster.Alive)
            {
                if (m.ControlMaster.HasGump(typeof(Luckgump)))
                    m.ControlMaster.CloseGump(typeof(Luckgump));
                m.ControlMaster.SendGump(new Luckgump(m.ControlMaster, m));
            }

        }

        private class PolyEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly Lucky m_bc;
            public PolyEntry(Mobile from, Lucky bc)
                : base(1075824, 8)
            {
                this.m_From = from;
                this.m_bc = bc;
            }
            public override void OnClick()
            {
                if (m_From.Alive && m_bc.Controlled && m_bc.ControlMaster == m_From && !m_bc.IsDeadPet)
                {
                    if (m_bc.BodyMod != 0)
                        ((Lucky)m_bc).polymorph(719, m_bc);
                    else
                    {
                        ((Lucky)m_bc).polymorph(0x191, m_bc);
                        //m_bc.DisplayPaperdollTo(m_From);
                    }
                    //if (m_From.HasGump(typeof(Luckgump)))
                    //    m_From.CloseGump(typeof(Luckgump));
                    //m_From.SendGump(new Luckgump(m_From, m_bc));

                }
            }

        }
        public override bool CanBeHarmful(IDamageable damageable, bool message, bool ignoreOurBlessedness)
        {
            if (!pvp && damageable is PlayerMobile)
            {
                return false;
            }
            else
                return base.CanBeHarmful(damageable, message, ignoreOurBlessedness);
        }

        public override void OnThink()
        {
            //if (!pvp && Combatant != null && Combatant is PlayerMobile)
            //{
            //    Combatant = null; ControlOrder = OrderType.Follow; ControlTarget = this.ControlMaster; DebugSay("hahah pvp  no "); return;
            //}

            //else
            if (Combatant != null && Combatant is Mobile && this.InRange(Combatant.Location, 8))
            {
                #region old no use
                ////Spells.Spell spell = Spells.SpellRegistry.NewSpell(690, this, null);
                //var t = Combatant as Targeting.Target;
                //Spells.Spell spell = new Spells.Mysticism.HailStormSpell(this, null);
                //Spells.Spell ss = new Spells.Necromancy.WitherSpell(this, null);
                //if (Combatant != null && this.InRange(Combatant.Location, 8))
                //{
                //    spell.Cast();
                //    ss.Cast();
                //    //t.Invoke(this, Combatant);
                //}
                //Timer.DelayCall(TimeSpan.FromSeconds(5.0) + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 10)), () =>
                //{
                //    this.Say("haha");
                //});********************************************
                //List<Mobile> targets = new List<Mobile>();
                //foreach (Mobile m in (this.Combatant as Mobile).GetMobilesInRange(3))
                //{
                //    if (m == ControlMaster || m == this)
                //        continue;
                //    targets.Add(m);
                //}

                //if (targets.Count >= 2)//敌人数量>=2 使用冰风暴
                //{
                //    Spell sp;
                //    if (Utility.Random(2) < 1)
                //        sp = new HailStormSpell(this, null);
                //    else
                //        sp = new WildfireSpell(this, null);
                //    ProcessTarget();
                //    sp.Cast();
                //}
                //targets.Clear();
                //targets = null;
                #endregion no use

                ArrayList targets = new ArrayList();
                IPooledEnumerable eable = ((Mobile)Combatant).GetMobilesInRange(3);
                foreach (Mobile m in eable)
                {
                    if (m == this || !this.CanBeHarmful(m))
                        continue;
                    targets.Add(m);
                }
                if (targets.Count >= 2 )//敌人数量>=2 使用冰风暴
                {
                    ProcessTarget();
                    Spell sp;
                    if (Utility.Random(3) < 1)
                        sp = new HailStormSpell(this, null);
                    else
                        sp = new WildfireSpell(this, null);
                    
                    if (sp.Cast())
                    {
                        Emote(sp.Name.ToString());
                    }
                }
                //Timer.DelayCall(TimeSpan.FromSeconds(6), () => Say("oh"));
                eable.Free();
                targets.Clear();
                targets = null;

            }
            base.OnThink();
        }
        protected bool ProcessTarget()
        {
            var t = this.Target;
            if (t == null)
            {
                //Say("null");
                return false;
            }
            //Yell("youle");
            var harmful = (t.Flags & TargetFlags.Harmful) != 0 || t is HailStormSpell.InternalTarget || t is WildfireSpell.InternalTarget;
            if (harmful && Combatant != null)
            {
                if (t.Range == -1 || this.InRange(Combatant, t.Range) && CanSee(Combatant) && InLOS(Combatant))
                {
                    t.Invoke(this, Combatant);
                }
                else
                    t.Invoke(this, this);

                return true;
            }
            return false;
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
            //if (version < 1 && Name == "a 拉奇")
            //    Name = "a 露西";
            //if (Body.IsHuman)
            //    this.HueMod = 1009;

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
    public class Luckgump : Gump
    {
        private static Mobile from;
        private static Lucky bc;
        //public Luckgump(Lucky b):this( ){ bc = b; }
        //public Luckgump(Lucky b) : this(from, b) { }

        public Luckgump(Mobile m, Lucky b) : base(240, 400)
        {
            bc = b;
            from = m;
            AddBackground(10, 10, 50, 50, 3500);
            if (bc.Alive && bc.BodyMod.IsHuman && bc.CanPaperdollBeOpenedBy(from))
            { AddButton(40, 40, 0x4ba, 0x4b9, 2, GumpButtonType.Reply, 0); }//DisplayPaperdollTo
            AddButton(15, 40, 0x4ba, 0x4b9, 1, GumpButtonType.Reply, 0);//0x191
            AddButton(15, 15, 0x4ba, 0x4b9, 3, GumpButtonType.Reply, 0);
            AddButton(40, 15, 0x4ba, 0x4b9, 4, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            from = sender.Mobile;
            switch (info.ButtonID)
            {
                case 0: break;
                case 1: { if (from.Alive) { bc.polymorph(48, bc); } break; }//蠍子
                case 2: { if (from.Alive) { bc.DisplayPaperdollTo(from); from.SendGump(new Luckgump(from, bc)); } break; }
                case 3: { if (from.Alive) { bc.polymorph(51, bc); } break; }//史莱姆
                case 4: { if (from.Alive) { bc.polymorph(728, bc); } break; }
                default: break;

            }
        }
    }
}
