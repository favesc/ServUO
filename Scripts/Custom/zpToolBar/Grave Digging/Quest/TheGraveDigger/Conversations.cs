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
				return "�ҬF�ں�æ��Ҫ����һ�����ف�?";
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
				return "<I>�@�������˔E���^���˿���</I><BR><BR>���_, ��Ҫ����. �F�ڵ����p�����Y�����������˰�������. ߀�������p���ǂ��r���.";
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
				return "<I>�@�����˿�����ܸ��d��</I><BR><BR>̫����! �@�e��һ���б���߅����������Ҫʲ��Ϳ��������Y�õ�����Ҫ��ӛ�xһ������߅���Ė|����ȫ��ԓȥ���Y��<BR><BR><I>���˽��o�����б�</I><BR><BR><U>�l�ͷ�</U><BR>ֻ�܏Ă܂�������r�����Y�õ�. ��Ƣ��܉Ķ��ҏĲ��u�o���ˣ����²������������܉�õ���. ��ֻ���Լ����k��.<BR><BR><U>��������</U><BR>����ɼ��Ĺ�صĸ����������ҵ�һ������, �������ֽ�СȪ, ͵͵�Ĺ������K�������w���ҵ���������.<BR><BR><U>���ĵĵ��ݗU</U><BR>Ѫ�����Ĕy��Ʒ,��֪���к����ã� �h�c�����Ѫ���P����Ҫ���þ���ȥ�����������õ����ĵĵ��ݗU. ������һЩ����������ϣ�˳��Ϸ��������С�<BR><BR><U>���ľƾ�</U><BR>��Ҫ�^�˰Ϳ�˹�@�P, ���Ǿ���u�������Ҫ���k���������Y�õ����ľƾ�. �����������(����)���Y�ǻ�.<BR><BR>���������Ʒ���е�Ԓ, ȥ�׶�������ң��S���ء��w˹��, �����ڳ������ĳ���ط�.";
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
				return "<I>���p�p�؏ı�������һ�����ļ��</I><BR><BR>��! �����ͻ؁���,���慖�����慖����! ���fʲ��??? ��? ��]���ҵ�. �������̫����!";
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
				return "<I>�㲻����˼�Ľ�����ِķ˹.</I><BR><BR>�gӭ�؁�! �õ��˛]��? �]��? ���ռ�ȫ�˵�Ԓ��ԓȥ�ҾS���ء��w˹��... ��߀�ڵ�ʲ��?";
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
				return "<I>�S����ͣ�����еĹ���������</I><BR><BR>�؁��ˣ��ҵ������᣿�]�У� *�@Ϣ* Ո�^�m������. ����ľƕ��o���.";
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
				return "<I>�S���ء��w˹΢Ц�Ŀ�����.</I><BR><BR>����ҵ����ѣ���������ʲ���܎�����Ć�? ��, �ҿ���, ��������!<BR><BR><I>�S���ذ��������ڵ���Ц�Ĵ����L..</I><BR><BR>��Ҳ��ِķ˹�_��. �Ҳ����t��. ���ǂ�ơ�Ǝ��������u��ơ�Ƶġ���׌���@�������һȦ��������̫�ƻ����������]�в��� ��ʮ�֑ж����đС���]���X߀��Ⱦƣ��Լ��ֲ����ȥ�ռ����ƵĲ���. �y�����]�и��V����Ҫ���M�Ć�? �����������ѵķ��ϣ���Ҫ����ԭ���ϵ�Ԓ������1WԪһƿ�ӹ��M������]��ԭ���ϣ�ÿƿҪ����10WԪ�ء��ޣ�����ԭ���ϣ��Ǿ�����1WԪһƿ... <BR><BR>ʲ�᣿ ��]��1WԪ���� ���@�Ӻ��ˣ���Ů������18�q��Խ��Խ�� �ҵ�Ԓ����Щ�쾹Ȼ��һ���к�ȥ���I�C�cȥ̽�U���@�������߀�]�л؁��ǿ�����Ψһ��Ů��������������܉�����ҵ�����Ԓ��������M�o��һЩ�����ľ�.";
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
				return "<I>�S���ء��w˹�ڌ����ʾ�gӭ��ԃ����������߅�İl�F</I><BR><BR>ʲ�᣿���ҵ��ϵۣ�������Ȼ�����㣿�����Һܸ��d��߀����. �ҵ�Ҫ����������ʲ��r������ؼ�. ���x���ҵ����ѣ����x�����һ���]�õĸ��H, ���������@����Ҫ�þ�. �s����ȥ�������΄հ�. ���������h������.";
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
				return "<I>�@������һ�ѓ��^��ƿ�����������</I><BR><BR>��úȰ����� ʲ��? �f���_����. ��Ҳ�]�k����! �����֪������߀�����ȥ��... ����������ܺȵ�����? ��Ҫ���ˣ����������f���^ȥ�Ŀ����^ȥ����....<BR><BR><I>���ֵ��^���˃ɿ�</I><BR><BR>����? �ޣ��㿴�Ҳ��c���ˣ����������@��������Ī����Ҫ�Ⱦ����㣬��Ĺ��Ҫ��V���ܣ�ͬ�r����Խ�ߣ��@�ӳ�˯�����ߵĎ���Ҳ��Խ�󣬮�������˼҉�Ĺ����Ĳ����������С���ˡ�����<BR><BR><I>�����o��һ���P�Ӻ�һ����</I><BR><BR>�ϵ�ף����, ��F�ڿ����x�_�ˣ��S�r�gӭ��؁�.";
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
				return "<I>�����˿���һ����ı���</I><BR><BR>����Щ��ʲ�ᰡ��ȥ����һ����ı�����Ȼ��o�Ҏ�����Ҫ�Ö|������t�����o�㪄���.";
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