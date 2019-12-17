using Server.Mobiles;

namespace Server.Engines.Quests.TheGraveDigger
{
    public class AsianOil : QuestItem
	{
		[Constructable]
		public AsianOil() : base( 0xE2A )
		{
			LootType = LootType.Blessed;
			Name = "��������";
			Weight = 1.0;
		}

		public AsianOil( Serial serial ) : base( serial )
		{
		}

		public override bool CanDrop( PlayerMobile player )
		{
			TheGraveDiggerQuest qs = player.Quest as TheGraveDiggerQuest;

			if ( qs == null )
				return true;

			return !( qs.IsObjectiveInProgress( typeof( FindYeastObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindAsianOilObjective ) )
				|| qs.IsObjectiveInProgress( typeof( FindRiceFlavorSticksObjective ) )
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