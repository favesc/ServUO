using Server.Mobiles;

namespace Server.Engines.Quests.TheGraveDigger
{
    public class VincentsBrew : QuestItem
	{
		[Constructable]
		public VincentsBrew() : base( 0x9A1 )
		{
			LootType = LootType.Blessed;
			Name = "維肯特的精製威士忌酒";
			Weight = 1.0;
		}

		public VincentsBrew( Serial serial ) : base( serial )
		{
		}

		public override bool CanDrop( PlayerMobile player )
		{
			TheGraveDiggerQuest qs = player.Quest as TheGraveDiggerQuest;

			if ( qs == null )
				return true;

			return !( qs.IsObjectiveInProgress( typeof( ReturnToDrunkObjective ) ) );
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