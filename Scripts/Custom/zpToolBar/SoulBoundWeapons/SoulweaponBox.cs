using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class SoulweaponBox : GiftBoxRectangle
    {
        [Constructable]
        public SoulweaponBox()
        //: base(0x46A2)
        {
            Item i;
            switch (Utility.Random(12))
            {
                case 0: i = new SoulVikingSword(); break;
                case 1: i = new Vstaff(); break;
                case 2: i = new VRadiantScimitar(); break;
                case 3: i = new SoulShortBow(); break;
                case 4: i = new vHalberd(); break;
                case 5: i = new Vtessen(); break;
                default:
                case 6: 
                case 7:
                case 8:
                case 9:
                case 10:
                case 11: i = new UnbindingDeed(); break;

            }
            this.Name = "灵魂武器匣";
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
            this.DropItem(i);
            //this.Movable = false;
        }
        public override int DefaultMaxItems { get { return 1; } }
        public override int DefaultMaxWeight        { get { return 20; } }
        public SoulweaponBox(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11B;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

}
