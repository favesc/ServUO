using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests.TheGraveDigger
{
	public class TheGraveDiggerQuest : QuestSystem
	{
		private static Type[] m_TypeReferenceTable = new Type[]
			{
				typeof( TheGraveDigger.DontOfferConversation ),
				typeof( TheGraveDigger.DeclineConversation ),
				typeof( TheGraveDigger.AcceptConversation ),
				typeof( TheGraveDigger.DuringCollectingConversation ),
				typeof( TheGraveDigger.DuringBrewingConversation ),
				typeof( TheGraveDigger.DuringSearchConversation ),
				typeof( TheGraveDigger.VincentConversation ),
				typeof( TheGraveDigger.VincentSecondConversation ),
				typeof( TheGraveDigger.EndConversation ),
				typeof( TheGraveDigger.FullEndConversation ),
				typeof( TheGraveDigger.FindYeastObjective ),
				typeof( TheGraveDigger.FindAsianOilObjective ),
				typeof( TheGraveDigger.FindRiceFlavorSticksObjective ),
				typeof( TheGraveDigger.FindPureGrainAlcoholObjective ),
				typeof( TheGraveDigger.FindVincentObjective ),
				typeof( TheGraveDigger.VincentsLittleGirlObjective ),
				typeof( TheGraveDigger.ReturnToVincentObjective ),
				typeof( TheGraveDigger.ReturnToDrunkObjective ),
				typeof( TheGraveDigger.MakeRoomObjective )
			};

		public override Type[] TypeReferenceTable{ get{ return m_TypeReferenceTable; } }

		public override object Name
		{
			get
			{
				return "一個臨死前的願望";
			}
		}

		public override object OfferMessage
		{
			get
			{
				return "<U>這個看起來很疲勞的男人開始説話了.</U><BR><BR>也許你願意幫助一個垂死的老家夥? 我需要找到一些藥材，然後請不列顛的維肯特·蓋斯幫我製造我的藥. 我想要自己去找但是我老了，而且病得很厲害. 如果你能幫我收集藥材制藥的話，我願意給你一件非常好的工具, 一把掘墓鏟怎麽樣？ 沒有聽説過?  這個並不重要...<BR><BR>聽名字就知道了，它的作用是掘墓，要知道，前人的墓穴中可是什麽寶藏都有的啊。。。 不不不，這不是犯罪... 只要沒人看到你不就行了...<BR><BR>他微笑著看著你：<BR><BR>孩子，你願意幫我完成這個心願麽?";
			}
		}

		public override TimeSpan RestartDelay{ get{ return TimeSpan.FromMinutes( 1.0 ); } }
		public override bool IsTutorial{ get{ return false; } }

		public override int Picture{ get{ return 0x15A9; } }

		public TheGraveDiggerQuest( PlayerMobile from ) : base( from )
		{
		}

		// Serialization
		public TheGraveDiggerQuest()
		{
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation( new AcceptConversation() );
		}

		public override void Decline()
		{
			base.Decline();

			AddConversation( new DeclineConversation() );
		}
	}
}