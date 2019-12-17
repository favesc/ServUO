///////////////////////////////////
///Sources:
///
///Ingot Book script by GoldDrac13
///Granite Box script by (unknown)
///////////////////////////////////
///////////////////////////////////////////
///Modified by Ashlar, beloved of Morrigan.  
///Modified by Tylius.
///////////////////////////////////////////
//
//This item is a resource storage book as well as a forge (backpack or ground), an anvil (ground only). 
//Add or remove references to fit your shard's scale types.  See IngotBook.cs for comments

using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;


namespace Server.Items
{
    [FlipableAttribute(0xFEF, 0xFF0, 0xFF1, 0xFF2, 0xFF3, 0xFF4, 0xFBD, 0xFBE)]
    [Server.Engines.Craft.Anvil]
    [Server.Engines.Craft.Forge]
    public class ScaleBook : Item
    {
        private int m_Red;
        private int m_Yellow;
        private int m_Black;
        private int m_Green;
        private int m_White;
        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Red { get { return m_Red; } set { m_Red = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Yellow { get { return m_Yellow; } set { m_Yellow = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Black { get { return m_Black; } set { m_Black = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Green { get { return m_Green; } set { m_Green = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int White { get { return m_White; } set { m_White = value; InvalidateProperties(); } }

        [Constructable]
        public ScaleBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1328;
            Name = "magical scale book";
            LootType = LootType.Blessed;
            StorageLimit = 50000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public ScaleBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1328;
            Name = "magical scale book";
            LootType = LootType.Blessed;
            StorageLimit = storageLimit;
            WithdrawIncrement = withdrawIncrement;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }
            else if (from is PlayerMobile)
            {
                from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
            }
        }

        public void BeginCombine(Mobile from)
        {
            from.Target = new ScaleBookTarget(this);
        }

        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (curItem is RedScales)
                {
                    if (Red + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Red + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Red += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is YellowScales)
                {

                    if (Yellow + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Yellow + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Yellow += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BlackScales)
                {

                    if (Black + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Black + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Black += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is GreenScales)
                {

                    if (Green + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Green + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Green += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is WhiteScales)
                {

                    if (White + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (White + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        White += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new ScaleBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
            }
        }

        public ScaleBook(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_Red);
            writer.Write((int)m_Yellow);
            writer.Write((int)m_Black);
            writer.Write((int)m_Green);
            writer.Write((int)m_White);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_Red = reader.ReadInt();
            m_Yellow = reader.ReadInt();
            m_Black = reader.ReadInt();
            m_Green = reader.ReadInt();
            m_White = reader.ReadInt();
            m_StorageLimit = reader.ReadInt();
            m_WithdrawIncrement = reader.ReadInt();
        }
    }
}

namespace Server.Items
{
    public class ScaleBookGump : Gump
    {
        private PlayerMobile m_From;
        private ScaleBook m_Book;

        public ScaleBookGump(PlayerMobile from, ScaleBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(ScaleBookGump));

            AddPage(0);

            AddBackground(50, 10, 255, 260, 5054);
            AddImageTiled(58, 20, 238, 241, 2624);
            AddAlphaRegion(58, 20, 238, 241);

            AddLabel(125, 25, 88, "Book of Scales");

            AddLabel(125, 75, 0x486, "Red Scales");
            AddLabel(225, 75, 0x480, book.Red.ToString());
            AddButton(75, 75, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Yellow Scales");
            AddLabel(225, 100, 0x480, book.Yellow.ToString());
            AddButton(75, 100, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Black Scales");
            AddLabel(225, 125, 0x480, book.Black.ToString());
            AddButton(75, 125, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "Green Scales");
            AddLabel(225, 150, 0x480, book.Green.ToString());
            AddButton(75, 150, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 0x486, "White Scales");
            AddLabel(225, 175, 0x480, book.White.ToString());
            AddButton(75, 175, 4005, 4007, 5, GumpButtonType.Reply, 0);
			
			AddLabel(125, 200, 88, "Each Max:" );
			AddLabel(225, 200, 0x480, book.StorageLimit.ToString() );	

            AddLabel(125, 225, 88, "Add Scale");
            AddButton(75, 225, 4005, 4007, 6, GumpButtonType.Reply, 0);

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.Red > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new RedScales(m_Book.WithdrawIncrement));
                    m_Book.Red = m_Book.Red - m_Book.WithdrawIncrement;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else if (m_Book.Red > 0)
                {
                    m_From.AddToBackpack(new RedScales(m_Book.Red));
                    m_Book.Red = 0;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that scale type!");
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.Yellow > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new YellowScales(m_Book.WithdrawIncrement));
                    m_Book.Yellow = m_Book.Yellow - m_Book.WithdrawIncrement;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else if (m_Book.Yellow > 0)
                {
                    m_From.AddToBackpack(new YellowScales(m_Book.Yellow));
                    m_Book.Yellow = 0;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that scale type!");
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.Black > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BlackScales(m_Book.WithdrawIncrement));
                    m_Book.Black = m_Book.Black - m_Book.WithdrawIncrement;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else if (m_Book.Black > 0)
                {
                    m_From.AddToBackpack(new BlackScales(m_Book.Black));
                    m_Book.Black = 0;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that scale type!");
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Green > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new GreenScales(m_Book.WithdrawIncrement));
                    m_Book.Green = m_Book.Green - m_Book.WithdrawIncrement;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else if (m_Book.Green > 0)
                {
                    m_From.AddToBackpack(new GreenScales(m_Book.Green));
                    m_Book.Green= 0;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that scale type!");
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.White > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new WhiteScales(m_Book.WithdrawIncrement));
                    m_Book.White = m_Book.White - m_Book.WithdrawIncrement;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else if (m_Book.White > 0)
                {
                    m_From.AddToBackpack(new WhiteScales(m_Book.White));
                    m_Book.White= 0;
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that scale type!");
                    m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                m_From.SendGump(new ScaleBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class ScaleBookTarget : Target
    {
        private ScaleBook m_Book;

        public ScaleBookTarget(ScaleBook book) : base( 18, false, TargetFlags.None )
        {
            m_Book = book;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Book.Deleted)
                return;

            m_Book.EndCombine(from, targeted);
        }
    }
}