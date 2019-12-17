namespace Server.Items
{
    public class ButchersResolve : Cleaver
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

        [Constructable]
        public ButchersResolve()
        {
            Name = "ÍÀ·ò²Ã›Q";
            Hue = 1157;
            SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
            PoisonCharges = 100;
            Poison = Poison.Lethal;
            Attributes.WeaponSpeed = 35;
            Attributes.WeaponDamage = 75;
            Attributes.SpellChanneling = 1;
            if (Utility.Random(100) < 50)
            {
                Slayer = SlayerName.Repond;
                WeaponAttributes.HitEnergyArea = 30;

                EngravedText = "Ãð¼£";
            }

            else
            { Slayer = SlayerName.Silver;
            EngravedText = "¿Ö¾åÖ®ÐÄ"; }

            Identified = true;
		}

		public ButchersResolve( Serial serial ) : base( serial )
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