﻿using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class GraveRobbersShovel : BaseHarvestTool
	{
		public override HarvestSystem HarvestSystem{ get{ return GraveRobbing.System; } }

		[Constructable]
		public GraveRobbersShovel() : this( 30 )
		{
		}

		[Constructable]
		public GraveRobbersShovel( int uses ) : base( uses, 0xF39 )
		{
            Name = "盗墓用洛阳铲";
            Hue = 1161;
			Weight = 5.0;
		}

		public GraveRobbersShovel( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}