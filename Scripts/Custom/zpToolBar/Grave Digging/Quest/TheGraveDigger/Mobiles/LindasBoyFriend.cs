using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Engines.Quests;

namespace Server.Engines.Quests.TheGraveDigger
{
	public class LindasBoyfriend : BaseQuester
	{
		[Constructable]
		public LindasBoyfriend() : base( "the thug" )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83F3;

			Female = false;
			BodyValue = 400;
			Name = "萊米·凱迪";
		}

		public override void InitOutfit()
		{
			AddItem( new FancyShirt( 1150 ) );
			AddItem( new LongPants( 1175 ) );
			AddItem( new Boots( 1175 ) );
			AddItem( new Cutlass() );

			AddItem( new PonyTail( 1175 ) );
			AddItem( new Vandyke( 1175 ) );
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			QuestSystem qs = player.Quest;

			if ( qs is TheGraveDiggerQuest )
			{
				this.Say( "你沒看到我正在和我的心上人説話嗎?" );
			}
		}

		public LindasBoyfriend( Serial serial ) : base( serial )
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