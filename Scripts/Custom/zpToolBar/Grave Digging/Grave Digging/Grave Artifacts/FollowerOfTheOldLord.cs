namespace Server.Items
{
    public class FollowerOfTheOldLord : BodySash
	{
		[Constructable]
		public FollowerOfTheOldLord()
		{
			Name = "˜s×uµÄ×·ëSÕß";
			Hue = 1150;
			Attributes.LowerManaCost = 18;
			Attributes.AttackChance =Utility.RandomMinMax(15, 22);
			Resistances.Fire = 12;
			Resistances.Energy = 9;
            LootType = LootType.Cursed;

        }

        public FollowerOfTheOldLord( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}