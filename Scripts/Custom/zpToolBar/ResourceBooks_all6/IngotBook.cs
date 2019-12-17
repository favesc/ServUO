///////////////////////////////////
///Sources:
///
///Ingot Book script by GoldDrac13
///Granite Box script by (unknown)
///BankCrystal script by (unknown)
///////////////////////////////////
///////////////////////////////////////////
///Modified by Ashlar, beloved of Morrigan.  
///Modified by Tylius.
///////////////////////////////////////////
//
//This item is a resource storage book as well as a forge (backpack or ground), an anvil (ground only), and a banker (spoken). 
//Add or remove references to fit your shard.  
//Note however, that adding such items that have a number of uses, like shovels, will allow the player to put a almost caput 
//shovel in and pop it back out with 50 uses left.

using System;					//To be honest, I am not sure if all this is needed, but the script works!
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

//Begin Anvil-Forge section (1 of 1)
    [Server.Engines.Craft.Anvil]
    [Server.Engines.Craft.Forge]
//End Anvil-Forge section (1 of 1)

    public class IngotBook : Item
    {
        private int m_Iron;		//Declare all our resources as integer (number) variables.
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

			//This section allows GM's and above to change the amounts of the various properties of the book.
        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Iron { get { return m_Iron; } set { m_Iron = value; InvalidateProperties(); } }

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


        [Constructable]												//This is the default item you get when you [add ingotbook
        public IngotBook() : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1161;
            Name = "magical ingot book";
            LootType = LootType.Blessed;
            StorageLimit = 60000;
            WithdrawIncrement = 100;

        }

        [Constructable]
        public IngotBook(int storageLimit, int withdrawIncrement) : base( 0xFEF + Utility.Random( 4 ) )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1161;
            Name = "magical ingot book";
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
                from.SendGump(new IngotBookGump((PlayerMobile)from, this));
            }
        }
        public void BeginCombine(Mobile from)
        {
            from.Target = new IngotBookTarget(this);
        }
        public void EndCombine(Mobile from, object o)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                Item curItem = o as Item;
                if (!(curItem is BaseIngot)) 			//non-stackable items have to be handled differently than stackable. Look at else statements for how.
                {
                    if (curItem is Sand)
                    {
                        if (Sand >= StorageLimit)		//This book is currently set to hold StorageLimit of each resource.
                            from.SendMessage("The sand is too full to add more.");

                        else
                        {
                            ((Item)o).Delete();
                            Sand = (Sand + 1);
                            from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                            BeginCombine(from);
                        }
                    }
                    else
                    {
                        from.SendMessage("That does not belong in here."); //If the item is not a ingot or sand, player gets this message
                    }
                }
                else if (curItem is IronIngot)
                {
                    if (Iron + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Iron + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Iron += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is DullCopperIngot)
                {
                    if (DullCopper + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (DullCopper + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        DullCopper += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is ShadowIronIngot)
                {
                    if (ShadowIron + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (ShadowIron + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        ShadowIron += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is CopperIngot)
                {
                    if (Copper + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Copper + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Copper += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BronzeIngot)
                {

                    if (Bronze + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Bronze + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Bronze += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is GoldIngot)
                {
                    if (Gold + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Gold + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Gold += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is AgapiteIngot)
                {
                    if (Agapite + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Agapite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Agapite += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is VeriteIngot)
                {

                    if (Verite + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Verite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Verite += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is ValoriteIngot)
                {
                    if (Valorite + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Valorite + curItem.Amount) - m_StorageLimit ) +" too much! This Book can only hold "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Valorite += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new IngotBookGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it. //Targeted items must be in backpack or player gets this messsage
            }
        }
        public IngotBook(Serial serial) : base( serial )
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_Iron);
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

            m_Iron = reader.ReadInt();
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
    public class IngotBookGump : Gump
    {
        private PlayerMobile m_From;
        private IngotBook m_Book;

        public IngotBookGump(PlayerMobile from, IngotBook book) : base( 25, 25 )
        {
            m_From = from;
            m_Book = book;

            m_From.CloseGump(typeof(IngotBookGump));

            AddPage(0);

            AddBackground(50, 10, 455, 260, 5054);
            AddImageTiled(58, 20, 438, 241, 2624);
            AddAlphaRegion(58, 20, 438, 241);
            
            AddLabel(225, 25, 88, "Magical Smithy");

            AddLabel(125, 50, 0x486, "Iron");
            AddLabel(225, 50, 0x480, book.Iron.ToString());
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

            AddLabel(325, 225, 88, "Add resource or item");
            AddButton(275, 225, 4005, 4007, 11, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Book.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Book.Iron > m_Book.WithdrawIncrement)								//if the book currently holds more ot this type than the increment amount
                {
                    m_From.AddToBackpack(new IronIngot(m_Book.WithdrawIncrement));  	//Send the increment amount of this type to players backpack
                    m_Book.Iron = m_Book.Iron - m_Book.WithdrawIncrement;				//removes that many from the books count
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));					//Resets the gump with the new info
                }
                else if (m_Book.Iron > 0)
                {
                    m_From.AddToBackpack(new IronIngot(m_Book.Iron));  					//Sends all stored ingots of whichever type to players backpack
                    m_Book.Iron = 0;						     						//Sets the count in the book back to 0
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));					//Resets the gump with the new info
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");			//Tell the player he is barking up the wrong tree
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));  				//Resets the gump 
                    m_Book.BeginCombine(m_From);										//Send the player a new in-game target in case more resources are to be added
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Book.DullCopper > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DullCopperIngot(m_Book.WithdrawIncrement));
                    m_Book.DullCopper = m_Book.DullCopper - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.DullCopper > 0)
                {
                    m_From.AddToBackpack(new DullCopperIngot(m_Book.DullCopper));  	 
                    m_Book.DullCopper = 0;						     	 
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Book.ShadowIron > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new ShadowIronIngot(m_Book.WithdrawIncrement));
                    m_Book.ShadowIron = m_Book.ShadowIron - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.ShadowIron > 0)
                {
                    m_From.AddToBackpack(new ShadowIronIngot(m_Book.ShadowIron));  	 
                    m_Book.ShadowIron = 0;						     	  
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Book.Copper > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new CopperIngot(m_Book.WithdrawIncrement));
                    m_Book.Copper = m_Book.Copper - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Copper > 0)
                {
                    m_From.AddToBackpack(new CopperIngot(m_Book.Copper));  	  
                    m_Book.Copper = 0;						     	  
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Book.Bronze > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BronzeIngot(m_Book.WithdrawIncrement));
                    m_Book.Bronze = m_Book.Bronze - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Bronze > 0)
                {
                    m_From.AddToBackpack(new BronzeIngot(m_Book.Bronze));  	  
                    m_Book.Bronze = 0;						     	  
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                if (m_Book.Gold > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new GoldIngot(m_Book.WithdrawIncrement));
                    m_Book.Gold = m_Book.Gold - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Gold > 0)
                {
                    m_From.AddToBackpack(new GoldIngot(m_Book.Gold));  	  
                    m_Book.Gold = 0;						     	 
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 7)
            {
                if (m_Book.Agapite > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new AgapiteIngot(m_Book.WithdrawIncrement));
                    m_Book.Agapite = m_Book.Agapite - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Agapite > 0)
                {
                    m_From.AddToBackpack(new AgapiteIngot(m_Book.Agapite));  
                    m_Book.Agapite = 0;						     	  
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 8)
            {
                if (m_Book.Verite > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new VeriteIngot(m_Book.WithdrawIncrement));
                    m_Book.Verite = m_Book.Verite - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Verite > 0)
                {
                    m_From.AddToBackpack(new VeriteIngot(m_Book.Verite));  	
                    m_Book.Verite = 0;						     	 
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 9)
            {
                if (m_Book.Valorite > m_Book.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new ValoriteIngot(m_Book.WithdrawIncrement));
                    m_Book.Valorite = m_Book.Valorite - m_Book.WithdrawIncrement;
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                }
                else if (m_Book.Valorite > 0)
                {
                    m_From.AddToBackpack(new ValoriteIngot(m_Book.Valorite));  	
                    m_Book.Valorite = 0;						     	  
                    m_From.SendGump(new IngotBookGump(m_From, m_Book)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 10)
            {
                if (m_Book.Sand > 0)
                {
                    m_From.AddToBackpack(new Sand());					//Non-stackable item, only give the player one at a time
                    m_Book.Sand = (m_Book.Sand - 1);					//Take one from the available count
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));	//Refresh gump
                }
                else
                {
                    m_From.SendMessage("You do not have any sand available!");	//Players in the wrong tree again
                    m_From.SendGump(new IngotBookGump(m_From, m_Book));			//Refresh gump
                    m_Book.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 11)
            {
                m_From.SendGump(new IngotBookGump(m_From, m_Book));
                m_Book.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class IngotBookTarget : Target
    {
        private IngotBook m_Book;

        public IngotBookTarget(IngotBook book) : base( 18, false, TargetFlags.None )
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
