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
//This item is a resource storage book.
//Add or remove references to fit your shard.  See IngotBook.cs for comments

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
    public class LumberBook : Item
    {
        private int m_Log;
        private int m_Arrow;
        private int m_Bolt;
        private int m_Feather;
        private int m_Shaft;
        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Log { get { return m_Log; } set { m_Log = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Arrow { get { return m_Arrow; } set { m_Arrow = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bolt { get { return m_Bolt; } set { m_Bolt = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Feather { get { return m_Feather; } set { m_Feather = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Shaft { get { return m_Shaft; } set { m_Shaft = value; InvalidateProperties(); } }

        [Constructable]
        public LumberBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1151;
            Name = "magical lumber book";
            LootType = LootType.Blessed;
            StorageLimit = 50000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public LumberBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1151;
            Name = "magical lumber book";
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
                from.SendGump(new LumberBookGump((PlayerMobile)from, this));
            }
        }
        public void BeginCombine(Mobile from)
        {
            from.Target = new LumberBookTarget(this);
        }
        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (curItem is Log)
                {
                    if (Log + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Log + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Log += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new LumberBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Arrow)
                {
                    if (Arrow + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Arrow + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Arrow += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new LumberBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Bolt)
                {
                    if (Bolt + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Bolt + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Bolt += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new LumberBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Feather)
                {
                    if (Feather + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Feather + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Feather += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new LumberBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Shaft)
                {
                    if (Shaft + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Shaft + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Shaft += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new LumberBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
            }
        }
        public LumberBook(Serial serial) : base( serial )
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_Log);
            writer.Write((int)m_Arrow);
            writer.Write((int)m_Bolt);
            writer.Write((int)m_Feather);
            writer.Write((int)m_Shaft);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_Log = reader.ReadInt();
            m_Arrow = reader.ReadInt();
            m_Bolt = reader.ReadInt();
            m_Feather = reader.ReadInt();
            m_Shaft = reader.ReadInt();
            m_StorageLimit = reader.ReadInt();
            m_WithdrawIncrement = reader.ReadInt();
        }
    }
}

namespace Server.Items
{
    public class LumberBookGump : Gump
    {
        private PlayerMobile m_From;
        private LumberBook m_Book;

        public LumberBookGump(PlayerMobile from, LumberBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(LumberBookGump));

            AddPage(0);

            AddBackground(50, 10, 255, 260, 5054);
            AddImageTiled(58, 20, 238, 241, 2624);
            AddAlphaRegion(58, 20, 238, 241);

            AddLabel(125, 25, 88, "Lumber Book");

            AddLabel(125, 50, 0x486, "Log");
            AddLabel(225, 50, 0x480, book.Log.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 0x486, "Arrow");
            AddLabel(225, 75, 0x480, book.Arrow.ToString());
            AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Bolt");
            AddLabel(225, 100, 0x480, book.Bolt.ToString());
            AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Feather");
            AddLabel(225, 125, 0x480, book.Feather.ToString());
            AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "Shaft");
            AddLabel(225, 150, 0x480, book.Shaft.ToString());
            AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);
			
			AddLabel(125, 200, 88, "Each Max:" );
			AddLabel(225, 200, 0x480, book.StorageLimit.ToString() );	

            AddLabel(125, 225, 88, "Add resource");
            AddButton(75, 225, 4005, 4007, 6, GumpButtonType.Reply, 0);

        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.Log >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Log(m_Book.WithdrawIncrement));
                    m_Book.Log = m_Book.Log - m_Book.WithdrawIncrement;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else if (m_Book.Log > 0)
                {
                    m_From.AddToBackpack(new Log(m_Book.Log));
                    m_Book.Log = 0;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that log!");
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.Arrow >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Arrow(m_Book.WithdrawIncrement));
                    m_Book.Arrow = m_Book.Arrow - m_Book.WithdrawIncrement;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else if (m_Book.Arrow > 0)
                {
                    m_From.AddToBackpack(new Arrow(m_Book.Arrow));
                    m_Book.Arrow = 0;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any arrows stored!");
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.Bolt > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bolt(m_Book.WithdrawIncrement));
                    m_Book.Bolt = m_Book.Bolt - m_Book.WithdrawIncrement;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else if (m_Book.Bolt > 0)
                {
                    m_From.AddToBackpack(new Bolt(m_Book.Bolt));
                    m_Book.Bolt = 0;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any bolts stored!");
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Feather > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Feather(m_Book.WithdrawIncrement));
                    m_Book.Feather = m_Book.Feather - m_Book.WithdrawIncrement;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else if (m_Book.Feather > 0)
                {
                    m_From.AddToBackpack(new Feather(m_Book.Feather));
                    m_Book.Feather = 0;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any feathers stored!");
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.Shaft > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Shaft(m_Book.WithdrawIncrement));
                    m_Book.Shaft = m_Book.Shaft - m_Book.WithdrawIncrement;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else if (m_Book.Shaft > 0)
                {
                    m_From.AddToBackpack(new Shaft(m_Book.Shaft));
                    m_Book.Shaft = 0;
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any shafts stored!");
                    m_From.SendGump(new LumberBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                m_From.SendGump(new LumberBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class LumberBookTarget : Target
    {
        private LumberBook m_Book;

        public LumberBookTarget(LumberBook book) : base( 18, false, TargetFlags.None )
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