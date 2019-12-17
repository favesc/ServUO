using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class VengefulSpirit : BaseCreature
	{
		[Constructable]
		public VengefulSpirit(int reagents) : base( AIType.AI_Mage, FightMode.Weakest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Vengeful Spirit";
			Body = 26;
			Hue = 0x4001;
			BaseSoundID = 0x482;

			SetStr( 736, 1100 );
			SetDex( 236, 295 );
			SetInt( 716, 860 );

			SetHits( 26000, 30000 );

			SetDamage( 17, 31 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.EvalInt, 89, 98 );
			SetSkill( SkillName.Magery, 89, 98 );
			SetSkill( SkillName.MagicResist, 55.1, 70.0 );
			SetSkill( SkillName.Tactics, 89.1, 90.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.0 );

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 58;

			PackReg( reagents );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}
		
		public override bool BleedImmune{ get{ return true; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public VengefulSpirit( Serial serial ) : base( serial )
		{
		}
        public override bool OnBeforeDeath()
        {
            if (Utility.Random(10) < 7)
                Engines.CannedEvil.GoldShower.Do(Location, Map, 30, 1000, 5000);

            return base.OnBeforeDeath();

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