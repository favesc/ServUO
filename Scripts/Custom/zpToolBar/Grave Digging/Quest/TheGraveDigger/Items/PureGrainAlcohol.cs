using Server.Mobiles;

namespace Server.Engines.Quests.TheGraveDigger
{
    public class PureGrainAlcohol : QuestItem
	{
		[Constructable]
		public PureGrainAlcohol() : base( 0x9C8 )
		{
			LootType = LootType.Blessed;
			Name = "純的酒精 ( 月光 )";
			Weight = 1.0;
		}

		public PureGrainAlcohol( Serial serial ) : base( serial )
		{
		}

		public override bool CanDrop( PlayerMobile player )
		{
			TheGraveDiggerQuest qs = player.Quest as TheGraveDiggerQuest;

			if ( qs == null )
				return true;

			return !( qs.IsObjectiveInProgress( typeof( FindPureGrainAlcoholObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindPureGrainAlcoholObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindPureGrainAlcoholObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindPureGrainAlcoholObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindVincentObjective ) ) );
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