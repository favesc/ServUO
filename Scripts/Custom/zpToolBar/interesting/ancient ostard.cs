using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an ancient ostard corpse")]
    public class AncientOstard : FrenziedOstard
    {
        [Constructable]
        public AncientOstard() : this("an ancient ostard")
        {
        }


        [Constructable]
        public AncientOstard(string name)
        {
            //BaseSoundID = Core.AOS ? 0xA8 : 0x16A;
            //BaseSoundID = 0x275;
            //  (name, 0xDA, 0x3EA4, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
            //Body = 0xda;
            //this.ItemID = 0x3ea4;
            //AI = AIType.AI_Melee;

            SetStr(1100, 1200);
            SetDex(100, 200);
            SetInt(700, 1000);

            SetHits(800, 1200);

            SetDamage(35, 50);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 52.5, 75.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);
            SetWeaponAbility(WeaponAbility.BleedAttack);
            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 50.0;

            //switch ( Utility.Random( 3 ) )
            //{
            //	case 0:
            //	{
            //		BodyValue = 116;
            //		ItemID = 16039;
            //                 Hue = 2971;
            //		break;
            //	}
            //	case 1:
            //	{
            //		BodyValue = 178;
            //		ItemID = 16041;
            //                 Hue = 2971;
            //		break;
            //	}
            //	case 2:
            //	{
            //		BodyValue = 179;
            //		ItemID = 16055;
            //                 Hue = 2971;
            //		break;
            //	}
            //}

            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override int GetAngerSound()
        {
            //if ( !Controlled )
            //	return 0x16A;

            return base.GetAngerSound();
        }

        //public override bool HasBreath { get { return true; } } // fire breath enabled
        public override int Meat { get { return 5; } }

        //public override bool CanHealOwner { get { return true; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }

        public AncientOstard(Serial serial) : base(serial)
        {
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
            //BaseSoundID = 0x275;
            //if ( Core.AOS && BaseSoundID == 0x16A )
            //	BaseSoundID = 0xA8;
            //else if ( !Core.AOS && BaseSoundID == 0xA8 )
            //	BaseSoundID = 0x16A;
        }
    }
}
