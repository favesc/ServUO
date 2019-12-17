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
    public class GraniteBook : Item
    {
        private int m_Granite;
        private int m_DullCopper;
        private int m_ShadowIron;
        private int m_Copper;
        private int m_Bronze;
        private int m_Gold;
        private int m_Agapite;
        private int m_Verite;
        private int m_Valorite;

        private int m_Sand;

        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Granite { get { return m_Granite; } set { m_Granite = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DullCopper { get { return m_DullCopper; } set { m_DullCopper = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShadowIron { get { return m_ShadowIron; } set { m_ShadowIron = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Copper { get { return m_Copper; } set { m_Copper = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bronze { get { return m_Bronze; } set { m_Bronze = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Gold { get { return m_Gold; } set { m_Gold = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Agapite { get { return m_Agapite; } set { m_Agapite = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Verite { get { return m_Verite; } set { m_Verite = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Valorite { get { return m_Valorite; } set { m_Valorite = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sand { get { return m_Sand; } set { m_Sand = value; InvalidateProperties(); } }

        [Constructable]
        public GraniteBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 2407;
            Name = "magical granite book";
            LootType = LootType.Blessed;
            StorageLimit = 60000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public GraniteBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 2407;
            Name = "magical granite book";
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
                from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
            }
        }

        public void BeginCombine(Mobile from)
        {
            from.Target = new GraniteBookTarget(this);
        }

        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (!(curItem is BaseGranite))
                {
                    if (curItem is Sand)
                    {
                        if (Sand + curItem.Amount > StorageLimit)
                            from.SendMessage("The sand is too full to add more.");

                        else
                        {
                            curItem.Delete();
                            Sand = (Sand + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else
                    {
                        from.SendMessage("That is not sand.");
                    }
                }
                else if (!(curItem is Sand))
                {
                    if (curItem is DullCopperGranite)
                    {
                        if (DullCopper + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (DullCopper + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");

                        else
                        {
                            curItem.Delete();
                            DullCopper = (DullCopper + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is ShadowIronGranite)
                    {

                        if (ShadowIron + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (ShadowIron + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");

                        else
                        {
                            curItem.Delete();
                            ShadowIron = (ShadowIron + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is CopperGranite)
                    {
                        if (Copper + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Bronze + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Bronze = (Copper + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is BronzeGranite)
                    {
                        if (Bronze + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Bronze + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Bronze = (Bronze + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is GoldGranite)
                    {

                        if (Gold + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Gold + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Gold = (Gold + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is AgapiteGranite)
                    {

                        if (Agapite + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Agapite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Agapite = (Agapite + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is VeriteGranite)
                    {

                        if (Verite + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Verite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Verite = (Verite + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is ValoriteGranite)
                    {

                        if (Valorite + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Valorite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Valorite = (Valorite + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is Granite)
                    {

                        if (Granite + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Granite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            curItem.Delete();
                            Granite = (Granite + 1);
                            from.SendGump(new GraniteBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
                }

            }
        }
        public GraniteBook(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_Granite);
            writer.Write((int)m_DullCopper);
            writer.Write((int)m_ShadowIron);
            writer.Write((int)m_Copper);
            writer.Write((int)m_Bronze);
            writer.Write((int)m_Gold);
            writer.Write((int)m_Agapite);
            writer.Write((int)m_Verite);
            writer.Write((int)m_Valorite);
            writer.Write((int)m_Sand);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_Granite = reader.ReadInt();
            m_DullCopper = reader.ReadInt();
            m_ShadowIron = reader.ReadInt();
            m_Copper = reader.ReadInt();
            m_Bronze = reader.ReadInt();
            m_Gold = reader.ReadInt();
            m_Agapite = reader.ReadInt();
            m_Verite = reader.ReadInt();
            m_Valorite = reader.ReadInt();
            m_Sand = reader.ReadInt();
            m_StorageLimit = reader.ReadInt();
            m_WithdrawIncrement = reader.ReadInt();
        }
    }
}


namespace Server.Items
{
    public class GraniteBookGump : Gump
    {
        private PlayerMobile m_From;
        private GraniteBook m_Book;

        public GraniteBookGump(PlayerMobile from, GraniteBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(GraniteBookGump));

            AddPage(0);

            AddBackground(50, 10, 455, 260, 5054);
            AddImageTiled(58, 20, 438, 241, 2624);
            AddAlphaRegion(58, 20, 438, 241);

            AddLabel(225, 25, 88, "Granite Book");

            AddLabel(125, 50, 0x486, "Granite");
            AddLabel(225, 50, 0x480, book.Granite.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 0x486, "Dull Copper");
            AddLabel(225, 75, 0x480, book.DullCopper.ToString());
            AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Shadow Iron");
            AddLabel(225, 100, 0x480, book.ShadowIron.ToString());
            AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Copper");
            AddLabel(225, 125, 0x480, book.Copper.ToString());
            AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "Bronze");
            AddLabel(225, 150, 0x480, book.Bronze.ToString());
            AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 0x486, "Gold");
            AddLabel(225, 175, 0x480, book.Gold.ToString());
            AddButton(75, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);

            AddLabel(125, 200, 0x486, "Agapite");
            AddLabel(225, 200, 0x480, book.Agapite.ToString());
            AddButton(75, 200, 4005, 4007, 7, GumpButtonType.Reply, 0);

            AddLabel(125, 225, 0x486, "Verite");
            AddLabel(225, 225, 0x480, book.Verite.ToString());
            AddButton(75, 225, 4005, 4007, 8, GumpButtonType.Reply, 0);

            AddLabel(325, 50, 0x486, "Valorite");
            AddLabel(425, 50, 0x480, book.Valorite.ToString());
            AddButton(275, 50, 4005, 4007, 9, GumpButtonType.Reply, 0);

            AddLabel(325, 75, 0x486, "Sand");
            AddLabel(425, 75, 0x480, book.Sand.ToString());
            AddButton(275, 75, 4005, 4007, 10, GumpButtonType.Reply, 0);	
			
			AddLabel(325, 200, 88, "Each Max:" );
			AddLabel(425, 200, 0x480, book.StorageLimit.ToString() );	

            AddLabel(325, 225, 88, "Add Granite");
            AddButton(275, 225, 4005, 4007, 11, GumpButtonType.Reply, 0);

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.Granite > 0)
                {
                    m_From.AddToBackpack(new Granite());
                    m_Book.Granite = m_Book.Granite - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.DullCopper > 0)
                {
                    m_From.AddToBackpack(new DullCopperGranite());
                    m_Book.DullCopper = m_Book.DullCopper - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.ShadowIron > 0)
                {
                    m_From.AddToBackpack(new ShadowIronGranite());
                    m_Book.ShadowIron = m_Book.ShadowIron - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Copper > 0)
                {
                    m_From.AddToBackpack(new CopperGranite());
                    m_Book.Copper = m_Book.Copper - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.Bronze > 0)
                {
                    m_From.AddToBackpack(new BronzeGranite());
                    m_Book.Bronze = m_Book.Bronze - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                if (m_Book.Gold > 0)
                {
                    m_From.AddToBackpack(new GoldGranite());
                    m_Book.Gold = m_Book.Gold - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 7)
            {
                if (m_Book.Agapite > 0)
                {
                    m_From.AddToBackpack(new AgapiteGranite());
                    m_Book.Agapite = m_Book.Agapite - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 8)
            {
                if (m_Book.Verite > 0)
                {
                    m_From.AddToBackpack(new VeriteGranite());
                    m_Book.Verite = m_Book.Verite - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 9)
            {
                if (m_Book.Valorite > 0)
                {
                    m_From.AddToBackpack(new ValoriteGranite());
                    m_Book.Valorite = m_Book.Valorite - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Granite!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 10)
            {
                if (m_Book.Sand > 0)
                {
                    m_From.AddToBackpack(new Sand());
                    m_Book.Sand = m_Book.Sand - 1;
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any sand!");
                    m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 11)
            {
                m_From.SendGump(new GraniteBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class GraniteBookTarget : Target
    {
        private GraniteBook m_Book;

        public GraniteBookTarget(GraniteBook book) : base( 18, false, TargetFlags.None )
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