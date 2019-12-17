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
using Server.Misc; 
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;


namespace Server.Items
{
    [FlipableAttribute(0xFEF, 0xFF0, 0xFF1, 0xFF2, 0xFF3, 0xFF4, 0xFBD, 0xFBE)]
    public class AlchemistBook : Item
    {
        private int m_BlackPearl;
        private int m_Bloodmoss;
        private int m_Garlic;
        private int m_Ginseng;
        private int m_MandrakeRoot;
        private int m_Nightshade;
        private int m_SulfurousAsh;
        private int m_SpidersSilk;
        private int m_BatWing;
        private int m_GraveDust;
        private int m_PigIron;
        private int m_Bone;
        private int m_NoxCrystal;
        private int m_DaemonBlood;
        private int m_DaemonBone;
        private int m_PotionKeg;
        private int m_BlankScroll;
        private int m_Bottle;
        private int m_Sand;
        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BlackPearl { get { return m_BlackPearl; } set { m_BlackPearl = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bloodmoss { get { return m_Bloodmoss; } set { m_Bloodmoss = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Garlic { get { return m_Garlic; } set { m_Garlic = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Ginseng { get { return m_Ginseng; } set { m_Ginseng = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MandrakeRoot { get { return m_MandrakeRoot; } set { m_MandrakeRoot = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Nightshade { get { return m_Nightshade; } set { m_Nightshade = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SulfurousAsh { get { return m_SulfurousAsh; } set { m_SulfurousAsh = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpidersSilk { get { return m_SpidersSilk; } set { m_SpidersSilk = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BatWing { get { return m_BatWing; } set { m_BatWing = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GraveDust { get { return m_GraveDust; } set { m_GraveDust = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PigIron { get { return m_PigIron; } set { m_PigIron = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bone { get { return m_Bone; } set { m_Bone = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NoxCrystal { get { return m_NoxCrystal; } set { m_NoxCrystal = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DaemonBlood { get { return m_DaemonBlood; } set { m_DaemonBlood = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DaemonBone { get { return m_DaemonBone; } set { m_DaemonBone = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PotionKeg { get { return m_PotionKeg; } set { m_PotionKeg = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BlankScroll { get { return m_BlankScroll; } set { m_BlankScroll = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bottle { get { return m_Bottle; } set { m_Bottle = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sand { get { return m_Sand; } set { m_Sand = value; InvalidateProperties(); } }


        [Constructable]
        public AlchemistBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1153;
            Name = "magical alchemists book";
            LootType = LootType.Blessed;
            StorageLimit = 60000;
            WithdrawIncrement = 100;

        }

        [Constructable]
        public AlchemistBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1153;
            Name = "magical alchemists book";
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
                from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
            }
        }
        public void BeginCombine(Mobile from)
        {
            from.Target = new AlchemistBookTarget(this);
        }
        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (!(curItem is BaseReagent)) //non-stackable items have to be handled differently than stackable. Look at else statements for how.
                {
                    if (curItem is Sand)
                    {
                        if (Sand + curItem.Amount > StorageLimit)
                            from.SendMessage("That resource type cannot hold the amount you're trying to store, in addition to what it currently has.");
                        else
                        {
                            curItem.Delete();
                            Sand = (Sand + 1);
                            from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is PotionKeg)
                    {
                        if (PotionKeg + curItem.Amount > StorageLimit)
                            from.SendMessage("I think you already have enough.");

                        else
                        {
                            curItem.Delete();
                            PotionKeg = (PotionKeg + 1);
                            from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is BlankScroll)
                    {

                        if (BlankScroll + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (BlankScroll + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            BlankScroll += curItem.Amount;
                            curItem.Delete();
                            from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else if (curItem is Bottle)
                    {
                        if (Bottle + curItem.Amount > StorageLimit)
                            from.SendMessage("You are trying to add "+ ( (Bottle + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                        else
                        {
                            Bottle += curItem.Amount;
                            curItem.Delete();
                            from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else
                    {
                        from.SendMessage("That does not belong in here."); //If the item is not a resource, player gets this message
                    }
                }
                else if (curItem is Bloodmoss)
                {
                    if (Bloodmoss + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Bloodmoss + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Bloodmoss += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Garlic)
                {
                    if (Garlic + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Garlic + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Garlic += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Ginseng)
                {
                    if (Ginseng + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Ginseng + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Ginseng += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is MandrakeRoot)
                {

                    if (MandrakeRoot + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (MandrakeRoot + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        MandrakeRoot += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Nightshade)
                {
                    if (Nightshade + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Nightshade + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Nightshade += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SulfurousAsh)
                {
                    if (SulfurousAsh + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (SulfurousAsh + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        SulfurousAsh += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SpidersSilk)
                {

                    if (SpidersSilk + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (SpidersSilk + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        SpidersSilk += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BatWing)
                {
                    if (BatWing + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (BatWing + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        BatWing += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BlackPearl)
                {
                    if (BlackPearl + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (BlackPearl + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        BlackPearl += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is GraveDust)
                {
                    if (GraveDust + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (GraveDust + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        GraveDust += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is PigIron)
                {
                    if (PigIron + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (PigIron + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        PigIron += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
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
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is NoxCrystal)
                {
                    if (NoxCrystal + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (NoxCrystal + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        NoxCrystal += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is DaemonBlood)
                {
                    if (DaemonBlood + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (DaemonBlood + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        DaemonBlood += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is DaemonBone)
                {
                    if (DaemonBone + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (DaemonBone + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        DaemonBone += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new AlchemistBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it. //Targeted items must be in backpack or player gets this messsage
            }
        }
        public AlchemistBook(Serial serial) : base( serial )
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_BlackPearl);
            writer.Write((int)m_Bloodmoss);
            writer.Write((int)m_Garlic);
            writer.Write((int)m_Ginseng);
            writer.Write((int)m_MandrakeRoot);
            writer.Write((int)m_Nightshade);
            writer.Write((int)m_SulfurousAsh);
            writer.Write((int)m_SpidersSilk);
            writer.Write((int)m_BatWing);
            writer.Write((int)m_GraveDust);
            writer.Write((int)m_PigIron);
            writer.Write((int)m_Bone);
            writer.Write((int)m_NoxCrystal);
            writer.Write((int)m_DaemonBlood);
            writer.Write((int)m_DaemonBone);
            writer.Write((int)m_PotionKeg);
            writer.Write((int)m_BlankScroll);
            writer.Write((int)m_Bottle);
            writer.Write((int)m_Sand);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_BlackPearl = reader.ReadInt();
            m_Bloodmoss = reader.ReadInt();
            m_Garlic = reader.ReadInt();
            m_Ginseng = reader.ReadInt();
            m_MandrakeRoot = reader.ReadInt();
            m_Nightshade = reader.ReadInt();
            m_SulfurousAsh = reader.ReadInt();
            m_SpidersSilk = reader.ReadInt();
            m_BatWing = reader.ReadInt();
            m_GraveDust = reader.ReadInt();
            m_PigIron = reader.ReadInt();
            m_Bone = reader.ReadInt();
            m_NoxCrystal = reader.ReadInt();
            m_DaemonBlood = reader.ReadInt();
            m_DaemonBone = reader.ReadInt();
            m_PotionKeg = reader.ReadInt();
            m_BlankScroll = reader.ReadInt();
            m_Bottle = reader.ReadInt();
            m_Sand = reader.ReadInt();
            m_StorageLimit = reader.ReadInt();
            m_WithdrawIncrement = reader.ReadInt();
        }
    }
}


namespace Server.Items
{
    public class AlchemistBookGump : Gump
    {
        private PlayerMobile m_From;
        private AlchemistBook m_Book;

        public AlchemistBookGump(PlayerMobile from, AlchemistBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(AlchemistBookGump));

            AddPage(0);

            AddBackground(50, 10, 655, 260, 5054);
            AddImageTiled(58, 20, 638, 241, 2624);
            AddAlphaRegion(58, 20, 638, 241);
  
            AddLabel(325, 25, 88, "Book of Alchemy");

            AddLabel(125, 50, 0x486, "BlackPearl");
            AddLabel(225, 50, 0x480, book.BlackPearl.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 0x486, "Bloodmoss");
            AddLabel(225, 75, 0x480, book.Bloodmoss.ToString());
            AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Garlic");
            AddLabel(225, 100, 0x480, book.Garlic.ToString());
            AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Ginseng");
            AddLabel(225, 125, 0x480, book.Ginseng.ToString());
            AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "MandrakeRoot");
            AddLabel(225, 150, 0x480, book.MandrakeRoot.ToString());
            AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 0x486, "Nightshade");
            AddLabel(225, 175, 0x480, book.Nightshade.ToString());
            AddButton(75, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);

            AddLabel(125, 200, 0x486, "SulfurousAsh");
            AddLabel(225, 200, 0x480, book.SulfurousAsh.ToString());
            AddButton(75, 200, 4005, 4007, 7, GumpButtonType.Reply, 0);

            AddLabel(125, 225, 0x486, "SpidersSilk");
            AddLabel(225, 225, 0x480, book.SpidersSilk.ToString());
            AddButton(75, 225, 4005, 4007, 8, GumpButtonType.Reply, 0);

            AddLabel(325, 50, 0x486, "BatWing");
            AddLabel(425, 50, 0x480, book.BatWing.ToString());
            AddButton(275, 50, 4005, 4007, 9, GumpButtonType.Reply, 0);

            AddLabel(325, 75, 0x486, "GraveDust");
            AddLabel(425, 75, 0x480, book.GraveDust.ToString());
            AddButton(275, 75, 4005, 4007, 10, GumpButtonType.Reply, 0);

            AddLabel(325, 100, 0x486, "PigIron");
            AddLabel(425, 100, 0x480, book.PigIron.ToString());
            AddButton(275, 100, 4005, 4007, 11, GumpButtonType.Reply, 0);

            AddLabel(325, 125, 0x486, "Bone");
            AddLabel(425, 125, 0x480, book.Bone.ToString());
            AddButton(275, 125, 4005, 4007, 12, GumpButtonType.Reply, 0);

            AddLabel(325, 150, 0x486, "NoxCrystal");
            AddLabel(425, 150, 0x480, book.NoxCrystal.ToString());
            AddButton(275, 150, 4005, 4007, 13, GumpButtonType.Reply, 0);

            AddLabel(325, 175, 0x486, "DaemonBlood");
            AddLabel(425, 175, 0x480, book.DaemonBlood.ToString());
            AddButton(275, 175, 4005, 4007, 14, GumpButtonType.Reply, 0);

            AddLabel(325, 200, 0x486, "DaemonBone");
            AddLabel(425, 200, 0x480, book.DaemonBone.ToString());
            AddButton(275, 200, 4005, 4007, 15, GumpButtonType.Reply, 0);

            AddLabel(325, 225, 0x486, "PotionKeg");
            AddLabel(425, 225, 0x480, book.PotionKeg.ToString());
            AddButton(275, 225, 4005, 4007, 16, GumpButtonType.Reply, 0);

            AddLabel(525, 50, 0x486, "BlankScroll");
            AddLabel(625, 50, 0x480, book.BlankScroll.ToString());
            AddButton(475, 50, 4005, 4007, 17, GumpButtonType.Reply, 0);

            AddLabel(525, 75, 0x486, "Bottle");
            AddLabel(625, 75, 0x480, book.Bottle.ToString());
            AddButton(475, 75, 4005, 4007, 18, GumpButtonType.Reply, 0);

            AddLabel(525, 100, 0x486, "Sand");
            AddLabel(625, 100, 0x480, book.Sand.ToString());
            AddButton(475, 100, 4005, 4007, 19, GumpButtonType.Reply, 0);
			
			AddLabel(  525, 200, 88, "Each Max:" );
			AddLabel(  625, 200, 0x480, book.StorageLimit.ToString() );	

            AddLabel(525, 225, 88, "Add resource or item");
            AddButton(475, 225, 4005, 4007, 20, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.BlackPearl > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BlackPearl(m_Book.WithdrawIncrement));  
                    m_Book.BlackPearl = m_Book.BlackPearl - m_Book.WithdrawIncrement;		
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else if (m_Book.BlackPearl > 0)
                {
                    m_From.AddToBackpack(new BlackPearl(m_Book.BlackPearl));   	 
                    m_Book.BlackPearl = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");	
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));  	
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.Bloodmoss > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bloodmoss(m_Book.WithdrawIncrement));
                    m_Book.Bloodmoss = m_Book.Bloodmoss - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Bloodmoss > 0)
                {
                    m_From.AddToBackpack(new Bloodmoss(m_Book.Bloodmoss));   	 
                    m_Book.Bloodmoss = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.Garlic > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Garlic(m_Book.WithdrawIncrement));
                    m_Book.Garlic = m_Book.Garlic - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Garlic > 0)
                {
                    m_From.AddToBackpack(new Garlic(m_Book.Garlic));   	
                    m_Book.Garlic = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Ginseng > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Ginseng(m_Book.WithdrawIncrement));
                    m_Book.Ginseng = m_Book.Ginseng - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Ginseng > 0)
                {
                    m_From.AddToBackpack(new Ginseng(m_Book.Ginseng));   
                    m_Book.Ginseng = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.MandrakeRoot > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new MandrakeRoot(m_Book.WithdrawIncrement));
                    m_Book.MandrakeRoot = m_Book.MandrakeRoot - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.MandrakeRoot > 0)
                {
                    m_From.AddToBackpack(new MandrakeRoot(m_Book.MandrakeRoot));   	
                    m_Book.MandrakeRoot = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                if (m_Book.Nightshade > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Nightshade(m_Book.WithdrawIncrement));
                    m_Book.Nightshade = m_Book.Nightshade - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Nightshade > 0)
                {
                    m_From.AddToBackpack(new Nightshade(m_Book.Nightshade));   	
                    m_Book.Nightshade = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 7)
            {
                if (m_Book.SulfurousAsh > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new SulfurousAsh(m_Book.WithdrawIncrement));
                    m_Book.SulfurousAsh = m_Book.SulfurousAsh - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.SulfurousAsh > 0)
                {
                    m_From.AddToBackpack(new SulfurousAsh(m_Book.SulfurousAsh));  
                    m_Book.SulfurousAsh = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 8)
            {
                if (m_Book.SpidersSilk > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new SpidersSilk(m_Book.WithdrawIncrement));
                    m_Book.SpidersSilk = m_Book.SpidersSilk - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.SpidersSilk > 0)
                {
                    m_From.AddToBackpack(new SpidersSilk(m_Book.SpidersSilk));  
                    m_Book.SpidersSilk = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 9)
            {
                if (m_Book.BatWing > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BatWing(m_Book.WithdrawIncrement));
                    m_Book.BatWing = m_Book.BatWing - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.BatWing > 0)
                {
                    m_From.AddToBackpack(new BatWing(m_Book.BatWing));   	 
                    m_Book.BatWing = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 10)
            {
                if (m_Book.GraveDust > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new GraveDust(m_Book.WithdrawIncrement));
                    m_Book.GraveDust = m_Book.GraveDust - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.GraveDust > 0)
                {
                    m_From.AddToBackpack(new GraveDust(m_Book.GraveDust));   	 
                    m_Book.GraveDust = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 11)
            {
                if (m_Book.PigIron > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new PigIron(m_Book.WithdrawIncrement));
                    m_Book.PigIron = m_Book.PigIron - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.PigIron > 0)
                {
                    m_From.AddToBackpack(new PigIron(m_Book.PigIron));   
                    m_Book.PigIron = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 12)
            {
                if (m_Book.Bone > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bone(m_Book.WithdrawIncrement));
                    m_Book.Bone = m_Book.Bone - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Bone > 0)
                {
                    m_From.AddToBackpack(new Bone(m_Book.Bone));   	  
                    m_Book.Bone = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 13)
            {
                if (m_Book.NoxCrystal > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new NoxCrystal(m_Book.WithdrawIncrement));
                    m_Book.NoxCrystal = m_Book.NoxCrystal - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.NoxCrystal > 0)
                {
                    m_From.AddToBackpack(new NoxCrystal(m_Book.NoxCrystal));   
                    m_Book.NoxCrystal = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 14)
            {
                if (m_Book.DaemonBlood > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DaemonBlood(m_Book.WithdrawIncrement));
                    m_Book.DaemonBlood = m_Book.DaemonBlood - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.DaemonBlood > 0)
                {
                    m_From.AddToBackpack(new DaemonBlood(m_Book.DaemonBlood)); 
                    m_Book.DaemonBlood = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 15)
            {
                if (m_Book.DaemonBone > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DaemonBone(m_Book.WithdrawIncrement));
                    m_Book.DaemonBone = m_Book.DaemonBone - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.DaemonBone > 0)
                {
                    m_From.AddToBackpack(new DaemonBone(m_Book.DaemonBone));  
                    m_Book.DaemonBone = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 16)
            {
                if (m_Book.PotionKeg > 0)
                {
                    m_From.AddToBackpack(new PotionKeg());			
                    m_Book.PotionKeg = (m_Book.PotionKeg - 1);		
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));	
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 17)
            {
                if (m_Book.BlankScroll > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BlankScroll(m_Book.WithdrawIncrement));
                    m_Book.BlankScroll = m_Book.BlankScroll - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.BlankScroll > 0)
                {
                    m_From.AddToBackpack(new BlankScroll(m_Book.BlankScroll));  
                    m_Book.BlankScroll = 0;						     	  
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 18)
            {
                if (m_Book.Bottle > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bottle(m_Book.WithdrawIncrement));
                    m_Book.Bottle = m_Book.Bottle - m_Book.WithdrawIncrement;
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                }
                else if (m_Book.Bottle > 0)
                {
                    m_From.AddToBackpack(new Bottle(m_Book.Bottle));  
                    m_Book.Bottle = 0;						     	 
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 19)
            {
                if (m_Book.Sand > 0)
                {
                    m_From.AddToBackpack(new Sand());				
                    m_Book.Sand = (m_Book.Sand - 1);				
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));	
                }
                else
                {
                    m_From.SendMessage("You do not have any of that resource!");
                    m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 20)
            {
                m_From.SendGump(new AlchemistBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class AlchemistBookTarget : Target
    {
        private AlchemistBook m_Book;

        public AlchemistBookTarget(AlchemistBook book) : base( 18, false, TargetFlags.None )
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

