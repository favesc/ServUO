
// include file :  Scripts\Services\Treasures of Tokuno\TreasuresOfTokuno.cs
//  \Scripts\Mobiles\Monsters\AOS\DemonKnight.cs


using Server.Network;

namespace Server.Gumps
{
    public class goldenskullGump : Gump
    {

        public goldenskullGump(Mobile from) : this()
        {
        }
        public goldenskullGump() : base(250, 150)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            AddBackground(0, 0, 84, 110, 9559);
            AddPage(0);
            AddImage(10, 10, 9013);
            AddLabel(25, 95, 1153, @"Rare!");

        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;



            switch (info.ButtonID)
            {
                case 0:
                    {

                        break;
                    }

            }
        }
    }
}