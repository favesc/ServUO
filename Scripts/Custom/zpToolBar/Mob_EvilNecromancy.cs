using Server.Items;
using Server.Mobiles;

namespace Server.mobiles
{

    [CorpseName("a Evil Necromancy corpse")]

    public class EvilNecromancy : BaseCreature
    {
        IMount m;
        static Map randommap()
        {
            {
                switch (Utility.Random(8))
                {
                    default:
                    case 0: return Map.Trammel;
                    case 1: return Map.Felucca;
                    case 2:
                    case 3: return Map.Ilshenar;
                    case 4:
                    case 5: return Map.Malas;
                    case 6:
                    case 7: return Map.Tokuno;
                }
            }
        }
        static Map map = randommap();
        [Constructable]

        public EvilNecromancy() : base(AIType.AI_NecroMage, FightMode.Closest, 16, 1, 0.1, 0.2)
        {
            Body = ((this.Female = Utility.RandomBool()) ? Body = 0x191 : Body = 0x190);
            Name = this.Female ? NameList.RandomName("female") : NameList.RandomName("male");
            //SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Utility.AssignRandomHair(this);
            SetStr(150, 180);
            SetDex(180, 200);
            SetInt(345, 456);
            SetDamage(25, 28);

            SetHits(1000, 1599);
            SetMana(1500, 1600);
            Title = "惡靈法師";
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 50, 58);
            SetResistance(ResistanceType.Fire, 50, 58);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 55);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Archery, 90.0, 120.0);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Necromancy, 120.5, 130.0);
            SetSkill(SkillName.SpiritSpeak, 110.0, 120.0);



            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 115.0);
            SetSkill(SkillName.Wrestling, 103.0);
            SetSkill(SkillName.DetectHidden, 120.0);
            SetSkill(SkillName.Wrestling, 121.9, 130.6);
            SetSkill(SkillName.Tactics, 114.4, 117.4);
            SetSkill(SkillName.MagicResist, 147.7, 153.0);
            SetSkill(SkillName.Poisoning, 122.8, 124.0);
            SetSkill(SkillName.Magery, 121.8, 127.8);
            SetSkill(SkillName.EvalInt, 103.6, 117.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 55;

            if (this.Female == true)
            {
                AddItem(new PlainDress(Utility.RandomRedHue()));
                AddItem(new FeatheredHat(Utility.RandomRedHue()));
            }

            else
            {
                AddItem(new Robe(Utility.RandomBlueHue()));
                AddItem(new MagicWizardsHat(Utility.RandomBlueHue()));
            }
            m = new Windrunner("御风者");
            //var b = new Bow(); b.Attributes.SpellChanneling = 1;
            var nc = new NecromancerSpellbook();
            AddItem(nc);
            //PackItem(new Arrow(55));
            BaseRing r = new GoldRing();
            r.Attributes.AttackChance = 40;
            r.Movable = false;
            r.Attributes.SpellDamage = 50;
            AddItem(r);

            AddItem(new ThighBoots());
            //AddItem(new NecromancerSpellbook());
            //new Nightmare().Rider = this;
            m.Rider = this;
            //PackReg(200);
            PackGold(1800, 2000);
            if (Utility.RandomDouble() < 0.5)
                PackItem(new TreasureMap(Utility.RandomBool() == true ? 6 : 7, map));
        }

        public override bool OnBeforeDeath()
        {


            if (this.FindItemOnLayer(Layer.Ring) != null)
                this.FindItemOnLayer(Layer.Ring).Delete();

            if (m != null)
            {
                m.Rider = null;

                if (m is Mobile)
                    ((Mobile)m).Delete();
            }

            return base.OnBeforeDeath();
        }
        public override void OnDeath(Container c)
        {

            Mobile m = FindMostRecentDamager(false);

            if (m != null && m.Player)
            {
                bool gainedPath = false;

                int theirTotal = m.SkillsTotal;
                int ourTotal = this.SkillsTotal;

                int pointsToGain = 61 + ((theirTotal - ourTotal) / 50);

                if (pointsToGain < 1)
                    pointsToGain = 1;
                else if (pointsToGain > 94)
                    pointsToGain = 94;

                if (Services.Virtues.VirtueHelper.Award(m, Services.Virtues.VirtueName.Justice, pointsToGain, ref gainedPath))
                {
                    if (gainedPath)
                        m.SendLocalizedMessage(1049367); // You have gained a path in Justice!
                    else
                        m.SendLocalizedMessage(1049363); // You have gained in Justice.

                    m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                    m.PlaySound(0x1F7);
                }
            }
            baodongxi.bao(this as BaseCreature);
            base.OnDeath(c);
        }

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.FilthyRich);
        //    //AddLoot(LootPack.Rich);
        //    //AddLoot(LootPack.Gems, 2);
        //}
        public override bool AlwaysMurderer { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        //public override double WeaponAbilityChance
        //{
        //    get
        //    {
        //        if (Combatant != null && Combatant.Mounted)
        //            return 0.8;

        //        return base.WeaponAbilityChance;
        //    }
        //}

        //public override WeaponAbility GetWeaponAbility()
        //{
        //    this.DebugSay("Now!");
        //    if (Combatant is Mobile)
        //    {
        //        return (((Mobile)Combatant).Mounted ? WeaponAbility.RidingSwipe : WeaponAbility.MortalStrike);
        //    }
        //    else return WeaponAbility.MortalStrike;
        //}

        public EvilNecromancy(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {

            if (m is PlayerMobile)
            {
                PlayerMobile pm = m as PlayerMobile;

                if (pm != null)
                {
                    if (pm.AccessLevel == AccessLevel.Player && pm.Hidden)
                    {
                        this.UseSkill(SkillName.DetectHidden);
                        pm.RevealingAction();
                        pm.FixedParticles(0x375A, 9, 20, 5049, EffectLayer.Head);
                        pm.PlaySound(0x1FD);
                        pm.SendLocalizedMessage(500814, Title, 33); // You have been revealed!

                        base.OnMovement(pm, oldLocation);
                    }
                }
            }
        }

        public override bool AutoDispel { get { return true; } }
        public override bool GivesMLMinorArtifact
        {
            get { return true; }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}