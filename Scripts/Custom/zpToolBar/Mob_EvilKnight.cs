using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Engines.CannedEvil;

namespace Server.mobiles
{
    [CorpseName("a Evil Knight corpse")]

    public class EvilKnight : BaseCreature
    {
        static IMount m;



        [Constructable]
        public EvilKnight() : base(AIType.AI_Paladin, FightMode.Closest, 16, 1, 0.1, 0.3)
        {
            var h = cr();


            //this.Female = Utility.RandomBool();
            Body = ((this.Female = Utility.RandomBool()) ? Body = 0x191 : Body = 0x190);
            Name = this.Female ? NameList.RandomName("female") : NameList.RandomName("male");
            //SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Utility.AssignRandomHair(this);

            SetStr(176, 205);
            SetDex(178, 187);
            SetInt(335, 445);

            SetHits(1500, 2500);

            SetDamage(24, 32);
            Title = "惡靈騎士";
            SetDamageType(ResistanceType.Physical, 90);
            SetDamageType(ResistanceType.Poison, 10);

            SetResistance(ResistanceType.Physical, 22, 33);
            SetResistance(ResistanceType.Fire, 22, 33);
            SetResistance(ResistanceType.Cold, 22, 33);
            SetResistance(ResistanceType.Poison, 22, 33);
            SetResistance(ResistanceType.Energy, 22, 33);

            SetSkill(SkillName.MagicResist, 42.6, 57.5);
            SetSkill(SkillName.Tactics, 115.1, 130.0);
            SetSkill(SkillName.Wrestling, 92.6, 107.5);
            SetSkill(SkillName.Anatomy, 110.1, 125.0);

            SetSkill(SkillName.Fencing, 92.6, 107.5);
            SetSkill(SkillName.Macing, 92.6, 107.5);
            SetSkill(SkillName.Swords, 92.6, 107.5);

            SetSkill(SkillName.Bushido, 95.0, 120.0);


            Fame = 16000;
            Karma = -16000;

            m = new Nightmare();
            //PlateHelm ch = new PlateHelm() { Resource = h };
            PlateChest pc = new PlateChest() { Resource = h };
            PlateGorget lg = new PlateGorget() { Resource = h };
            PlateArms pa = new PlateArms() { Resource = h };
            PlateLegs pl = new PlateLegs() { Resource = h };
            PlateGloves pg = new PlateGloves() { Resource = h };
            //AddItem(ch);
            AddItem(pc);
            AddItem(pl);
            AddItem(pa);
            AddItem(lg);
            AddItem(pg);
            BaseRing r = new GoldRing();
            r.Attributes.AttackChance = 40;
            r.Movable = false;
            AddItem(r);

            if (Utility.RandomBool())
            {
                AddItem(new Scythe() { Resource = h });
            }
            else
            {
                switch (Utility.Random(3))
                {
                    case 0: AddItem(new Longsword()); break;
                    case 1: AddItem(new Broadsword()); break;
                    case 2: AddItem(new VikingSword()); break;
                }
                AddItem(new ChaosShield() { Resource = h });
            }
            m.Rider = this;
            PackGold(2000, 3000);
            if (Utility.RandomDouble() < 0.1)
                PackItem(new TreasureMap(6, this.Map));
        }
        static CraftResource cr()
        {
            switch (Utility.Random(6))
            {

                default:
                case 0: return CraftResource.ShadowIron;
                case 1: return CraftResource.Bronze;
                case 2: return CraftResource.Gold;
                case 3: return CraftResource.Valorite;
                case 4: return CraftResource.Verite;
                case 5: return CraftResource.Agapite;
            }

        }

        public override bool OnBeforeDeath()
        {
            //IMount mount = m;
            if (this.FindItemOnLayer(Layer.Ring) != null)
                this.FindItemOnLayer(Layer.Ring).Delete();
            if (m != null)
            {
                m.Rider = null;

                if (m is Mobile)
                    ((Mobile)m).Delete();
            }
            if (Utility.Random(10) < 1)
                GoldShower.Do(Location, Map, 30, 5000, 8000);
            baodongxi.bao(this as BaseCreature);
            return base.OnBeforeDeath();
        }

        public override void OnDeath(Container c)
        {

            Mobile dm = FindMostRecentDamager(false);

            if (dm != null && dm.Player)
            {
                bool gainedPath = false;

                int theirTotal = dm.SkillsTotal;
                int ourTotal = this.SkillsTotal;

                int pointsToGain = 51 + ((theirTotal - ourTotal) / 50);//zp

                if (pointsToGain < 1)
                    pointsToGain = 1;
                else if (pointsToGain > 84)
                    pointsToGain = 84;

                if (Services.Virtues.VirtueHelper.Award(dm, Services.Virtues.VirtueName.Justice, pointsToGain, ref gainedPath))
                {
                    if (gainedPath)
                        dm.SendLocalizedMessage(1049367); // You have gained a path in Justice!
                    else
                        dm.SendLocalizedMessage(1049363); // You have gained in Justice.

                    dm.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                    dm.PlaySound(0x1F7);
                }
            }
            //bao();

            //if (Paragon.ChestChance > Utility.RandomDouble())
            //if (item != null)
            //{
            //    Point3D p = Findloc(this.Map, this.Location, 5);
            //    //var g = new ParagonChest(Name, TreasureMapLevel);

            //    var g = item;
            //    g.MoveToWorld(p, this.Map);
            //    //c.DropItem(new ParagonChest(Name, TreasureMapLevel));
            //    switch (Utility.Random(3))
            //    {
            //        case 0: // Fire column
            //            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
            //            Effects.PlaySound(g, g.Map, 0x208);
            //            break;
            //        case 1: // Explosion
            //            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
            //            Effects.PlaySound(g, g.Map, 0x307);
            //            break;
            //        case 2: // Ball of fire
            //            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);
            //            Effects.PlaySound(g, g.Map, 0x307);
            //            break;
            //    }



            //}

            //if (Utility.RandomDouble() < 0.125)
            //{
            //    switch (Utility.Random(16))
            //    {
            //        case 0: c.DropItem(new MyrmidonGloves()); break;
            //        case 1: c.DropItem(new MyrmidonGorget()); break;
            //        case 2: c.DropItem(new MyrmidonLegs()); break;
            //        case 3: c.DropItem(new MyrmidonArms()); break;
            //        case 4: c.DropItem(new PaladinArms()); break;
            //        case 5: c.DropItem(new PaladinGorget()); break;
            //        case 6: c.DropItem(new LeafweaveLegs()); break;
            //        case 7: c.DropItem(new DeathChest()); break;
            //        case 8: c.DropItem(new DeathGloves()); break;
            //        case 9: c.DropItem(new DeathLegs()); break;
            //        case 10: c.DropItem(new GreymistGloves()); break;
            //        case 11: c.DropItem(new GreymistArms()); break;
            //        case 12: c.DropItem(new AssassinChest()); break;
            //        case 13: c.DropItem(new AssassinArms()); break;
            //        case 14: c.DropItem(new HunterGloves()); break;
            //        case 15: c.DropItem(new HunterLegs()); break;
            //    }
            //}

            base.OnDeath(c);
        }
        //#region zp 脚底下爆炸出好东西。
        //private void bao()
        //{
        //    if (item != null)
        //    {
        //        Point3D p = Findloc(this.Map, this.Location, 2);
        //        //var g = new ParagonChest(Name, TreasureMapLevel);

        //        var g = item;
        //        g.MoveToWorld(p, this.Map);
        //        //c.DropItem(new ParagonChest(Name, TreasureMapLevel));
        //        switch (Utility.Random(3))
        //        {
        //            case 0: // Fire column
        //                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
        //                Effects.PlaySound(g, g.Map, 0x208);
        //                break;
        //            case 1: // Explosion
        //                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
        //                Effects.PlaySound(g, g.Map, 0x307);
        //                break;
        //            case 2: // Ball of fire
        //                Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);
        //                Effects.PlaySound(g, g.Map, 0x307);
        //                break;
        //        }
        //    }

        //}

        //private Item item
        //{
        //    get
        //    {
        //        if (Paragon.ChestChance > Utility.RandomDouble()/3)
        //        {
        //            return new ParagonChest(Name, TreasureMapLevel);
        //        }
        //        if (Utility.RandomDouble()/3 < 0.125)
        //        {
        //            switch (Utility.Random(16))
        //            {
        //                case 0: return (new MyrmidonGloves());
        //                case 1: return (new MyrmidonGorget());
        //                case 2: return (new MyrmidonLegs());
        //                case 3: return (new MyrmidonArms());
        //                case 4: return (new PaladinArms());
        //                case 5: return (new PaladinGorget());
        //                case 6: return (new LeafweaveLegs());
        //                case 7: return (new DeathChest());
        //                case 8: return (new DeathGloves());
        //                case 9: return (new DeathLegs());
        //                case 10: return (new GreymistGloves());
        //                case 11: return (new GreymistArms());
        //                case 12: return (new AssassinChest());
        //                case 13: return (new AssassinArms());
        //                case 14: return (new HunterGloves());
        //                case 15: return (new HunterLegs());
        //            }
        //        }
        //        return null;
        //    }


        //}
        //Point3D Findloc(Map map, Point3D coploc, byte range)
        //{
        //    map = this.Map;
        //    coploc = this.Location;
        //    int cx = coploc.X;
        //    int cy = coploc.Y;
        //    for (int i = 0; i < range; ++i)
        //    {
        //        ++cy; ;
        //        for (int ii = 0; ii < range; ++ii)
        //        {
        //            ++cx;
        //            //cz = map.GetAverageZ(cx, cy);
        //            int cz = map.Tiles.GetLandTile(cx, cy).Z;
        //            if (map.CanSpawnMobile(cx, cy, cz))
        //            {
        //                return new Point3D(cx, cy, cz);
        //            }
        //        }
        //        cx = coploc.X;
        //    }

        //    return coploc;
        //}

        //#endregion
        //public override void GenerateLoot()
        //{
        //    //AddLoot(LootPack.FilthyRich);
        //    //AddLoot(LootPack.Rich);
        //    //AddLoot(LootPack.Gems, 2);
        //    AddLoot(LootPack.UltraRich, 1);
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

        public override WeaponAbility GetWeaponAbility()
        {
            //this.DebugSay("die Now!");
            if (Combatant is Mobile)
            {
                return (((Mobile)Combatant).Mounted ? WeaponAbility.RidingSwipe : Utility.RandomBool() ? WeaponAbility.ArmorIgnore : WeaponAbility.MortalStrike);
            }
            else return WeaponAbility.ArmorIgnore;
        }

        public EvilKnight(Serial serial) : base(serial)
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
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 5;
            }
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