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
    public class TailorBook : Item
    {
        private int m_Leather;
        private int m_Spined;
        private int m_Horned;
        private int m_Barbed;
        private int m_Cloth;
        private int m_UncutCloth;
        private int m_BoltOfCloth;
        private int m_SpoolOfThread;
        private int m_DarkYarn;
        private int m_LightYarn;
        private int m_LightYarnUnraveled;
        private int m_Bone;
        private int m_Flax;
        private int m_Cotton;
        private int m_Wool;
        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Leather { get { return m_Leather; } set { m_Leather = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Spined { get { return m_Spined; } set { m_Spined = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Horned { get { return m_Horned; } set { m_Horned = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Barbed { get { return m_Barbed; } set { m_Barbed = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Cloth { get { return m_Cloth; } set { m_Cloth = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UncutCloth { get { return m_UncutCloth; } set { m_UncutCloth = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BoltOfCloth { get { return m_BoltOfCloth; } set { m_BoltOfCloth = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpoolOfThread { get { return m_SpoolOfThread; } set { m_SpoolOfThread = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DarkYarn { get { return m_DarkYarn; } set { m_DarkYarn = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LightYarn { get { return m_LightYarn; } set { m_LightYarn = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LightYarnUnraveled { get { return m_LightYarnUnraveled; } set { m_LightYarnUnraveled = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bone { get { return m_Bone; } set { m_Bone = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Flax { get { return m_Flax; } set { m_Flax = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Cotton { get { return m_Cotton; } set { m_Cotton = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Wool { get { return m_Wool; } set { m_Wool = value; InvalidateProperties(); } }


        [Constructable]
        public TailorBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1131;
            Name = "magical tailors book";
            LootType = LootType.Blessed;
            StorageLimit = 50000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public TailorBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1131;
            Name = "magical tailors book";
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
                from.SendGump(new TailorBookGump((PlayerMobile)from, this));
            }
        }

        public void BeginCombine(Mobile from)
        {
            from.Target = new TailorBookTarget(this);
        }

        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (curItem is Leather)
                {
                    if (Leather + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Leather + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Leather += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SpinedLeather)
                {
                    if (Spined + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Spined + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Spined += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is HornedLeather)
                {
                    if (Horned + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Horned + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Horned += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BarbedLeather)
                {
                    if (Barbed + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Barbed + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Barbed += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Cloth)
                {
                    if (Cloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Cloth + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Cloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is UncutCloth)
                {
                    if (UncutCloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (UncutCloth + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        UncutCloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BoltOfCloth)
                {
                    if (BoltOfCloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (BoltOfCloth + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        BoltOfCloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SpoolOfThread)
                {
                    if (SpoolOfThread + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (SpoolOfThread + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        SpoolOfThread += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is DarkYarn)
                {
                    if (DarkYarn + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (DarkYarn + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        DarkYarn += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is LightYarn)
                {
                    if (LightYarn + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (LightYarn + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        LightYarn += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is LightYarnUnraveled)
                {
                    if (LightYarnUnraveled + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (LightYarnUnraveled + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        LightYarnUnraveled += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Bone)
                {
                    if (Bone + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Bone + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Bone += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Flax)
                {
                    if (Flax + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Flax + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Flax += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Cotton)
                {
                    if (Cotton + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Cotton + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Cotton += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Wool)
                {
                    if (Wool + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Wool + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Wool += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
            }
        }

        public TailorBook(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_Leather);
            writer.Write((int)m_Spined);
            writer.Write((int)m_Horned);
            writer.Write((int)m_Barbed);
            writer.Write((int)m_Cloth);
            writer.Write((int)m_UncutCloth);
            writer.Write((int)m_BoltOfCloth);
            writer.Write((int)m_SpoolOfThread);
            writer.Write((int)m_DarkYarn);
            writer.Write((int)m_LightYarn);
            writer.Write((int)m_LightYarnUnraveled);
            writer.Write((int)m_Bone);
            writer.Write((int)m_Flax);
            writer.Write((int)m_Cotton);
            writer.Write((int)m_Wool);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_Leather = reader.ReadInt();
            m_Spined = reader.ReadInt();
            m_Horned = reader.ReadInt();
            m_Barbed = reader.ReadInt();
            m_Cloth = reader.ReadInt();
            m_UncutCloth = reader.ReadInt();
            m_BoltOfCloth = reader.ReadInt();
            m_SpoolOfThread = reader.ReadInt();
            m_DarkYarn = reader.ReadInt();
            m_LightYarn = reader.ReadInt();
            m_LightYarnUnraveled = reader.ReadInt();
            m_Bone = reader.ReadInt();
            m_Flax = reader.ReadInt();
            m_Cotton = reader.ReadInt();
            m_Wool = reader.ReadInt();
            m_StorageLimit = reader.ReadInt();
            m_WithdrawIncrement = reader.ReadInt();
        }
    }
}

namespace Server.Items
{
    public class TailorBookGump : Gump
    {
        private PlayerMobile m_From;
        private TailorBook m_Book;

        public TailorBookGump(PlayerMobile from, TailorBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(TailorBookGump));

            AddPage(0);

            AddBackground(50, 10, 655, 260, 5054);
            AddImageTiled(58, 20, 638, 241, 2624);
            AddAlphaRegion(58, 20, 638, 241);

            AddLabel(325, 25, 88, "Tailor Supplies");

            AddLabel(125, 75, 0x486, "Leather");
            AddLabel(225, 75, 0x480, book.Leather.ToString());
            AddButton(75, 75, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Spined Leather");
            AddLabel(225, 100, 0x480, book.Spined.ToString());
            AddButton(75, 100, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Horned Leather");
            AddLabel(225, 125, 0x480, book.Horned.ToString());
            AddButton(75, 125, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "Barbed Leather");
            AddLabel(225, 150, 0x480, book.Barbed.ToString());
            AddButton(75, 150, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 0x486, "Cloth");
            AddLabel(225, 175, 0x480, book.Cloth.ToString());
            AddButton(75, 175, 4005, 4007, 5, GumpButtonType.Reply, 0);

            AddLabel(125, 200, 0x486, "Uncut Cloth");
            AddLabel(225, 200, 0x480, book.UncutCloth.ToString());
            AddButton(75, 200, 4005, 4007, 6, GumpButtonType.Reply, 0);

            AddLabel(125, 225, 0x486, "BoltOfCloth");
            AddLabel(225, 225, 0x480, book.BoltOfCloth.ToString());
            AddButton(75, 225, 4005, 4007, 7, GumpButtonType.Reply, 0);

            AddLabel(325, 75, 0x486, "SpoolOfThread");
            AddLabel(425, 75, 0x480, book.SpoolOfThread.ToString());
            AddButton(275, 75, 4005, 4007, 8, GumpButtonType.Reply, 0);

            AddLabel(325, 100, 0x486, "LightYarn");
            AddLabel(425, 100, 0x480, book.LightYarn.ToString());
            AddButton(275, 100, 4005, 4007, 9, GumpButtonType.Reply, 0);

            AddLabel(325, 125, 0x486, "DarkYarn");
            AddLabel(425, 125, 0x480, book.DarkYarn.ToString());
            AddButton(275, 125, 4005, 4007, 10, GumpButtonType.Reply, 0);

            AddLabel(325, 150, 0x486, "Bone");
            AddLabel(425, 150, 0x480, book.Bone.ToString());
            AddButton(275, 150, 4005, 4007, 11, GumpButtonType.Reply, 0);

            AddLabel(325, 175, 0x486, "Flax");
            AddLabel(425, 175, 0x480, book.Flax.ToString());
            AddButton(275, 175, 4005, 4007, 12, GumpButtonType.Reply, 0);

            AddLabel(325, 200, 0x486, "Cotton");
            AddLabel(425, 200, 0x480, book.Cotton.ToString());
            AddButton(275, 200, 4005, 4007, 13, GumpButtonType.Reply, 0);

            AddLabel(325, 225, 0x486, "Wool");
            AddLabel(425, 225, 0x480, book.Wool.ToString());
            AddButton(275, 225, 4005, 4007, 14, GumpButtonType.Reply, 0);

            AddLabel(525, 75, 0x486, "LightYarn Unraveled");
            AddLabel(650, 75, 0x480, book.LightYarnUnraveled.ToString());
            AddButton(475, 75, 4005, 4007, 15, GumpButtonType.Reply, 0);
			
			AddLabel(525, 200, 88, "Each Max:" );
			AddLabel(625, 200, 0x480, book.StorageLimit.ToString() );	

            AddLabel(525, 225, 88, "Add resource");
            AddButton(475, 225, 4005, 4007, 16, GumpButtonType.Reply, 0);

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.Leather >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Leather(m_Book.WithdrawIncrement));
                    m_Book.Leather = m_Book.Leather - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Leather > 0)
                {
                    m_From.AddToBackpack(new Leather(m_Book.Leather));
                    m_Book.Leather = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.Spined >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new SpinedLeather(m_Book.WithdrawIncrement));
                    m_Book.Spined = m_Book.Spined - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Spined > 0)
                {
                    m_From.AddToBackpack(new SpinedLeather(m_Book.Spined));
                    m_Book.Spined = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.Horned >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new HornedLeather(m_Book.WithdrawIncrement));
                    m_Book.Horned = m_Book.Horned - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Horned > 0)
                {
                    m_From.AddToBackpack(new HornedLeather(m_Book.Horned));
                    m_Book.Horned = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Barbed >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BarbedLeather(m_Book.WithdrawIncrement));
                    m_Book.Barbed = m_Book.Barbed - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Barbed > 0)
                {
                    m_From.AddToBackpack(new BarbedLeather(m_Book.Barbed));
                    m_Book.Barbed = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.Cloth >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Cloth(m_Book.WithdrawIncrement));
                    m_Book.Cloth = m_Book.Cloth - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Cloth > 0)
                {
                    m_From.AddToBackpack(new Cloth(m_Book.Cloth));
                    m_Book.Cloth = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                if (m_Book.UncutCloth >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new UncutCloth(m_Book.WithdrawIncrement));
                    m_Book.UncutCloth = m_Book.UncutCloth - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.UncutCloth > 0)
                {
                    m_From.AddToBackpack(new UncutCloth(m_Book.UncutCloth));
                    m_Book.UncutCloth = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 7)
            {
                if (m_Book.BoltOfCloth >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BoltOfCloth(m_Book.WithdrawIncrement));
                    m_Book.BoltOfCloth = m_Book.BoltOfCloth - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.BoltOfCloth > 0)
                {
                    m_From.AddToBackpack(new BoltOfCloth(m_Book.BoltOfCloth));
                    m_Book.BoltOfCloth = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 8)
            {
                if (m_Book.SpoolOfThread >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new SpoolOfThread(m_Book.WithdrawIncrement));
                    m_Book.SpoolOfThread = m_Book.SpoolOfThread - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.SpoolOfThread > 0)
                {
                    m_From.AddToBackpack(new SpoolOfThread(m_Book.SpoolOfThread));
                    m_Book.SpoolOfThread = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 9)
            {
                if (m_Book.LightYarn >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new LightYarn(m_Book.WithdrawIncrement));
                    m_Book.LightYarn = m_Book.LightYarn - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.LightYarn > 0)
                {
                    m_From.AddToBackpack(new LightYarn(m_Book.LightYarn));
                    m_Book.LightYarn = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 10)
            {
                if (m_Book.DarkYarn >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DarkYarn(m_Book.WithdrawIncrement));
                    m_Book.DarkYarn = m_Book.DarkYarn - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.DarkYarn > 0)
                {
                    m_From.AddToBackpack(new DarkYarn(m_Book.DarkYarn));
                    m_Book.DarkYarn = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 11)
            {
                if (m_Book.Bone >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bone(m_Book.WithdrawIncrement));
                    m_Book.Bone = m_Book.Bone - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Bone > 0)
                {
                    m_From.AddToBackpack(new Bone(m_Book.Bone));
                    m_Book.Bone = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 12)
            {
                if (m_Book.Flax >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Flax(m_Book.WithdrawIncrement));
                    m_Book.Flax = m_Book.Flax - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Flax > 0)
                {
                    m_From.AddToBackpack(new Flax(m_Book.Flax));
                    m_Book.Flax = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 13)
            {
                if (m_Book.Cotton >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Cotton(m_Book.WithdrawIncrement));
                    m_Book.Cotton = m_Book.Cotton - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Cotton > 0)
                {
                    m_From.AddToBackpack(new Cotton(m_Book.Cotton));
                    m_Book.Cotton = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 14)
            {
                if (m_Book.Wool >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Wool(m_Book.WithdrawIncrement));
                    m_Book.Wool = m_Book.Wool - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.Wool > 0)
                {
                    m_From.AddToBackpack(new Wool(m_Book.Wool));
                    m_Book.Wool = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 15)
            {
                if (m_Book.LightYarnUnraveled >= m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new LightYarnUnraveled(m_Book.WithdrawIncrement));
                    m_Book.LightYarnUnraveled = m_Book.LightYarnUnraveled - m_Book.WithdrawIncrement;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else if (m_Book.LightYarnUnraveled > 0)
                {
                    m_From.AddToBackpack(new LightYarnUnraveled(m_Book.LightYarnUnraveled));
                    m_Book.LightYarnUnraveled = 0;
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource type!");
                    m_From.SendGump(new TailorBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 16)
            {
                m_From.SendGump(new TailorBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class TailorBookTarget : Target
    {
        private TailorBook m_Book;

        public TailorBookTarget(TailorBook book) : base( 18, false, TargetFlags.None )
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