using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a purple chicken corpse" )]
	public class PurpleChicken : BaseCreature
	{
		[Constructable]
		public PurpleChicken () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a purple chicken";
			Body = 0xD0;
			BaseSoundID = 0x6E;
            Hue = 1373;

			SetStr( 1100, 1200 );
			SetDex( 100, 200 );
			SetInt( 700, 1000 );

			SetHits( 800, 1200 );

			SetDamage( 35, 50 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 80.1, 100.0 );
			SetSkill( SkillName.Magery, 80.1, 100.0 );
			SetSkill( SkillName.Meditation, 52.5, 75.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 30000;
			Karma = -30000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 110.0;
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.Gems, 5 );
		}
        public override bool OnBeforeDeath()
        {
            if (!Controlled)
            {
                baodongxi.bao(this as BaseCreature);
            }
            return base.OnBeforeDeath();
        }
        public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		//public override bool HasBreath{ get{ return true; } } // fire breath enabled  changed to SetSpecialAbility(SpecialAbility.DragonBreath);
        public override bool AutoDispel{ get{ return !Controlled; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 19; } }
		public override int Scales{ get{ return 12; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Utility.RandomBool() ? Poison.Lesser : Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanFly { get { return true; } }

		public PurpleChicken( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
