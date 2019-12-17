using System;
using Server;

namespace Server.Engines.Quests.TheGraveDigger
{
	public class DontOfferConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "我現在很忙，要不你一會兒再來?";
			}
		}

		public override bool Logged{ get{ return false; } }

		public DontOfferConversation()
		{
		}
	}

	public class DeclineConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>這個老男人擡起頭來看了看你</I><BR><BR>走開, 不要管我. 現在的年輕人哪裏懂得尊重老人啊。。唉. 還是我年輕的那個時候好.";
			}
		}

		public override bool Logged{ get{ return false; } }

		public DeclineConversation()
		{
		}
	}

	public class AcceptConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>這個老人看起來很高興：</I><BR><BR>太好了! 這裡是一張列表，上邊寫明了你需要什麽和可以在哪裏得到，不要忘記讀一下最後邊寫的東西找全後該去哪裏。<BR><BR><I>老人交給了你列表。</I><BR><BR><U>發酵粉</U><BR>只能從傑倫城外的農民那裏得到. 他脾氣很壞而且從不賣給外人，恐怕不是那麽容易能夠得到的. 你只有自己想辦法.<BR><BR><U>亞洲神油</U><BR>在紫杉城墓地的附近，可以找到一個忍者, 他的名字叫小泉, 偷偷的攻擊他並從他屍體上找到亞洲神油.<BR><BR><U>空心的稻草桿</U><BR>血巫妖的攜帶品,不知道有何作用，聽説與飲用人血有關，你要做得就是去殺掉它們，得到空心的稻草桿. 它們有一些生活在特林希克城南方的密林中。<BR><BR><U>純的酒精</U><BR>你要過了巴克斯這關, 它是酒類製造的神，你要想辦法從它那裏得到純的酒精. 它在美德神殿(犧牲)那裏徘徊.<BR><BR>如果以上物品都有的話, 去首都不列顛城找（維肯特·蓋斯）, 他就在城市里的某個地方.";
			}
		}

		public AcceptConversation()
		{
		}

		public override void OnRead()
		{
			System.AddObjective( new FindYeastObjective() );
		}
	}

	public class DuringCollectingConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>我輕輕地從背後拍了一下他的肩膀</I><BR><BR>嗨! 那麽快就回來了,你真厲害，真厲害！! 你說什麽??? 哼? 你沒有找到. 你真的是太爛了!";
			}
		}

		public override bool Logged{ get{ return false; } }

		public DuringCollectingConversation()
		{
		}
	}

	public class DuringBrewingConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>你不好意思的叫了下賽姆斯.</I><BR><BR>歡迎回來! 得到了沒有? 沒有? 你收集全了的話應該去找維肯特·蓋斯啊... 你還在等什麽?";
			}
		}

		public override bool Logged{ get{ return false; } }

		public DuringBrewingConversation()
		{
		}
	}

	public class DuringSearchConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>維肯特停下手中的工作看著你</I><BR><BR>回來了，找到她了麽？沒有？ *嘆息* 請繼續幫我找. 答應你的酒會給你的.";
			}
		}

		public override bool Logged{ get{ return false; } }

		public DuringSearchConversation()
		{
		}
	}

	public class VincentConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>維肯特·蓋斯微笑的看著你.</I><BR><BR>你好我的朋友，今天我有什麽能幫助你的嗎? 噢, 我看看, 哈哈哈哈!<BR><BR><I>維肯特按著肚子在地上笑的打著滾..</I><BR><BR>你也被賽姆斯騙了. 我不是醫生. 我是個啤酒師，我是製作啤酒的。他讓你繞了那麽大一圈，是因爲他太狡猾，他根本沒有病。 他十分懶惰而又膽小，沒有錢還想喝酒，自己又不願意去收集做酒的材料. 難道他沒有告訴你我要收費的嗎? 看在是我朋友的份上，你要是有原材料的話，算你1W元一瓶加工費，如果沒有原材料，每瓶要收你10W元呢。噢，你有原材料，那就算你1W元一瓶... <BR><BR>什麽？ 你沒有1W元？？ 那這樣好了，我女兒今年18歲，越來越不聽我的話，早些天竟然和一個男孩去海盜窩點去探險，這麽多天了還沒有回來。那可是我唯一的女兒啊。。如果你能夠幫我找到她的話，我願免費給你一些我釀造的酒.";
			}
		}

		public VincentConversation()
		{
		}

		public override void OnRead()
		{
			System.AddObjective( new VincentsLittleGirlObjective() );
		}
	}

	public class VincentSecondConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>維肯特·蓋斯在對你表示歡迎後詢問了你在外邊的發現</I><BR><BR>什麽？！我的上帝，她們竟然攻擊你？！！我很高興她還活著. 我倒要看看她到底什麽時候才願意回家. 感謝你我的朋友，感謝你幫助一個沒用的父親, 來，拿著，這是你要得酒. 趕快拿去完成你的任務吧. 你是我永遠的朋友.";
			}
		}

		public VincentSecondConversation()
		{
		}

		public override void OnRead()
		{
			System.AddObjective( new ReturnToDrunkObjective() );
		}
	}

	public class EndConversation : QuestConversation
	{
		public override object Message
		{
			get
			{
				return "<I>這個老人一把搶過酒瓶，狂飲了起來</I><BR><BR>真好喝啊。。 什麽? 說我騙了你. 我也沒辦法啊! 如果你知道事實你還會願意去麽... 那我怎麽才能喝到它呢? 不要想了，不管怎麽說，過去的總是過去了嘛....<BR><BR><I>他又低頭喝了兩口</I><BR><BR>獎勵? 噢，你看我差點忘了，來，拿著，這個就是你的獎勵。我要先警告你，掘墓需要採礦技能，同時技能越高，驚動沉睡中死者的幾率也就越大，畢竟你掘人家墳墓是你的不對嘛，你完事小心了。。。<BR><BR><I>他交給你一個鏟子和一個包</I><BR><BR>上帝祝福你, 你現在可以離開了，隨時歡迎你回來.";
			}
		}

		public EndConversation()
		{
		}

		public override void OnRead()
		{
			System.Complete();
		}
	}

	public class FullEndConversation : QuestConversation
	{
		private bool m_Logged;

		public override object Message
		{
			get
			{
				return "<I>老男人看了一下你的背包</I><BR><BR>你那些是什麽啊。去清理一下你的背包，然後給我帶來我要得東西，否則不會給你獎勵的.";
			}
		}

		public override bool Logged{ get{ return m_Logged; } }

		public FullEndConversation( bool logged )
		{
			m_Logged = logged;
		}

		public FullEndConversation()
		{
			m_Logged = true;
		}

		public override void OnRead()
		{
			if ( m_Logged )
				System.AddObjective( new MakeRoomObjective() );
		}
	}
}