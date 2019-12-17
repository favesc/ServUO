using Server.Gumps;
using Server.Items;


namespace Server.ContextMenus
{
    public class SoulBindWeaponEntry : ContextMenuEntry
    {
        private readonly Mobile m_From;
        private readonly BaseWeapon m_si;
        public SoulBindWeaponEntry(Mobile from, BaseWeapon si)
            : base(1011233, 8)
        {
            this.m_From = from;
            this.m_si = si;
        }

        public override void OnClick()
        {

            m_From.SendGump(new SoulBindGump(m_From, m_si));
 //           m_From.SendMessage(" " + m_si.Name);
        }
    }
}