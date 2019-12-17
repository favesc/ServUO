using Server.Items;
using Server.Network;
using System;

namespace Server.Gumps
{
    public class totGump : Gump
    {
        //Mobile caller;


        //public totGump(Mobile from) : this(from, new somthing())
        //{
        //caller = from;
        //}
        private static int x = Utility.RandomMinMax(100, 600);
        public totGump(Item i) : base(x, 150)

        {
            //caller = from;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;

            //AddBackground(0, 0, 149, 214, 3500);
            AddBackground(0, 0, 300, 214, 302);

            int pic =Utility.RandomMinMax(1, 19); 

            AddImage(17, 17, 0x7725 + pic);
            this.AddTooltip(this.GetTooltip(pic));
            AddLabel(150, 20, 1153, "" + DateTime.Now);
            AddLabel(150, 175, 1153, @"你得到一件宝贝。");
            AddItem(155, 95, i.ItemID, i.Hue);
            AddAlphaRegion(146, 70, 130, 70);
            AddItemProperty(i.Serial);

        }
        private int GetTooltip(int number)
        {
            if (number > 9)
                return 1076015 + number - 10;

            switch (number)
            {
                case 0:
                    return 1076063;
                case 1:
                    return 1076060;
                case 2:
                    return 1076061;
                case 3:
                    return 1076057;
                case 4:
                    return 1076062;
                case 5:
                    return 1076059;
                case 6:
                    return 1076058;
                case 7:
                    return 1076065;
                case 8:
                    return 1076064;
                case 9:
                    return 1076066;
            }

            return 1052009; // I have seen the error of my ways!
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



