using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.TheGraveDigger
{
	public class FindYeastObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "<U>發酵粉</U> 唯一的綫索就是傑倫城外的農民. 但是他的脾氣聽説很不好，而且從不跟外人交易. 我到底應該怎麽辦？？";
			}
		}

		public FindYeastObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new FindAsianOilObjective() );
		}
	}

	public class FindAsianOilObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "<U>亞洲神油</U> 在紫杉城墓地的附近，可以找到一個忍者, 他的名字叫小泉, 偷偷的攻擊他並從他屍體上找到亞洲神油.";
			}
		}

		public FindAsianOilObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new FindRiceFlavorSticksObjective() );
		}
	}

	public class FindRiceFlavorSticksObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "<U>空心的稻草桿</U> 血巫妖的攜帶品,不知道有何作用，聽説與飲用人血有關，你要做得就是去殺掉它們，得到空心的稻草桿. 它們有一些生活在特林希克城南方的密林中。";
			}
		}

		public FindRiceFlavorSticksObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new FindPureGrainAlcoholObjective() );
		}
	}

	public class FindPureGrainAlcoholObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "<U>純的酒精</U> 你要過了巴克斯這關, 它是酒類製造的神，你要想辦法從它那裏得到純的酒精. 它在美德神殿(犧牲)那裏徘徊.";
			}
		}

		public FindPureGrainAlcoholObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new FindVincentObjective() );
		}
	}

	public class FindVincentObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "<I>檢查你的背包</I><BR>如果以上物品都有的話, 去首都不列顛城找（維肯特·蓋斯）, 他就在城市里的某個地方.";
			}
		}

		public FindVincentObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddConversation( new VincentConversation() );
		}
	}

	public class VincentsLittleGirlObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "去海島巢穴，找到維肯特·蓋斯的女兒琳達.";
			}
		}

		public VincentsLittleGirlObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddObjective( new ReturnToVincentObjective() );
		}
	}

	public class ReturnToVincentObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "你發現了她. 現在回到維肯特·蓋斯處告訴他他女兒的位置.";
			}
		}

		public ReturnToVincentObjective()
		{
		}

		public override void OnComplete()
		{
			System.AddConversation( new VincentSecondConversation() );
		}
	}

	public class ReturnToDrunkObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "回到老男人那裏, 交給他釀好的酒完成你的任務.";
			}
		}

		public ReturnToDrunkObjective()
		{
		}
	}

	public class MakeRoomObjective : QuestObjective
	{
		public override object Message
		{
			get
			{
				return "你的背包中物品太多了，請整理一下再來領取你的獎勵.";
			}
		}

		public MakeRoomObjective()
		{
		}
	}
}