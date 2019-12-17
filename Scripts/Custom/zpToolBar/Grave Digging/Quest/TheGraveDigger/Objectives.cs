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
				return "<U>�l�ͷ�</U> Ψһ�ľQ�����ǂ܂�������r��. ��������Ƣ�� �h�ܲ��ã����ҏĲ������˽���. �ҵ��ב�ԓ�����k����";
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
				return "<U>��������</U> ����ɼ��Ĺ�صĸ����������ҵ�һ������, �������ֽ�СȪ, ͵͵�Ĺ������K�������w���ҵ���������.";
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
				return "<U>���ĵĵ��ݗU</U> Ѫ�����Ĕy��Ʒ,��֪���к����ã� �h�c�����Ѫ���P����Ҫ���þ���ȥ�����������õ����ĵĵ��ݗU. ������һЩ����������ϣ�˳��Ϸ��������С�";
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
				return "<U>���ľƾ�</U> ��Ҫ�^�˰Ϳ�˹�@�P, ���Ǿ���u�������Ҫ���k���������Y�õ����ľƾ�. �����������(����)���Y�ǻ�.";
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
				return "<I>�z����ı���</I><BR>���������Ʒ���е�Ԓ, ȥ�׶�������ң��S���ء��w˹��, �����ڳ������ĳ���ط�.";
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
				return "ȥ���u��Ѩ���ҵ��S���ء��w˹��Ů�����_.";
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
				return "��l�F����. �F�ڻص��S���ء��w˹̎���V����Ů����λ��.";
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
				return "�ص����������Y, ���o��ᄺõľ��������΄�.";
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
				return "��ı�������Ʒ̫���ˣ�Ո����һ���ف��Iȡ��Ī���.";
			}
		}

		public MakeRoomObjective()
		{
		}
	}
}