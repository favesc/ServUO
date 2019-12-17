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
				return "һ���R��ǰ�����";
			}
		}

		public override object OfferMessage
		{
			get
			{
				return "<U>�@���������ƣ�ڵ������_ʼ�hԒ��.</U><BR><BR>Ҳ�S��������һ���������ϼ��? ����Ҫ�ҵ�һЩˎ�ģ�Ȼ��Ո����ľS���ء��w˹�����u���ҵ�ˎ. ����Ҫ�Լ�ȥ�ҵ��������ˣ����Ҳ��ú܅���. ������܎����ռ�ˎ����ˎ��Ԓ�������o��һ���ǳ��õĹ���, һ�Ѿ�Ĺ�P����ӣ� �]�� �h�^?  �@���K����Ҫ...<BR><BR> ���־�֪���ˣ����������Ǿ�Ĺ��Ҫ֪����ǰ�˵�ĹѨ�п���ʲ�ጚ�ض��еİ������� ���������@���Ƿ���... ֻҪ�]�˿����㲻������...<BR><BR>��΢Ц�������㣺<BR><BR>���ӣ�������������@�������?";
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