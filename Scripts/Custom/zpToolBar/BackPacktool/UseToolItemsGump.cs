
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class UseToolItemsGump : BaseItemTileButtonsGump
    {

        //public UseToolItemsGump(ItemTileButtonInfo[] buttons)
        //    : base("Which item would you like to use?", buttons)
        //{
        //}

        public UseToolItemsGump(ArrayList buttons)
            : base("Which item would you like to use?", buttons)
        {
        }

        public static ArrayList FindToolItems(Mobile m)
        {
            Container backpack = m.Backpack;
            if (backpack == null)
            {
                return new ArrayList();
            }
            //ArrayList all = new ArrayList(backpack.Items);
            ArrayList tools = new ArrayList(backpack.FindItemsByType(typeof(BaseTool)));
            ArrayList potions = new ArrayList(backpack.FindItemsByType(typeof(BasePotion)));
            List<int> potIDs = new List<int>();
            ArrayList bandages = new ArrayList(backpack.FindItemsByType(typeof(Bandage)));
            ArrayList books = new ArrayList(backpack.FindItemsByType(typeof(Spellbook)));
            ArrayList rbook = new ArrayList(backpack.FindItemsByType(typeof(Runebook)));
            var ta = new ArrayList(backpack.FindItemsByType(typeof(TravelAtlas)));
            //ArrayList tm = new ArrayList(backpack.FindItemsByType(typeof(TMapBook)));
            var bc = new ArrayList(backpack.FindItemsByType(typeof(bankcard)));
            ArrayList list2 = new ArrayList();
            Item item;
            for (int i = 0; i < bc.Count; i++)
            {
                item = (bankcard)bc[i];
                ItemTileButtonInfo t = new ItemTileButtonInfo(item);
                //t.Label = new TextDefinition(string.Format("保险箱:\n{0}", item.Name));
                list2.Add(t);
            }
            //for (int i = 0; i < tm.Count; i++)
            //{
            //    item = (TMapBook)tm[i];
            //    ItemTileButtonInfo t = new ItemTileButtonInfo(item);
            //    t.Label = new TextDefinition(string.Format("藏宝图:{0}", item.Name));
            //    list2.Add(t);
            //}
            for (int i = 0; i < ta.Count; i++)
            {
                item = (TravelAtlas)ta[i];
                ItemTileButtonInfo tam = new ItemTileButtonInfo(item);
                //tam.Label = new TextDefinition(string.Format("旅行指南:\n{0}", item.Name));
                list2.Add(tam);
            }

            for (int i = 0; i < rbook.Count; i++)
            {
                item = (Runebook)rbook[i];
                ItemTileButtonInfo rb = new ItemTileButtonInfo(item);
                //rb.Label = new TextDefinition(string.Format("符石书:\n{1}", item.Name, ((Runebook)item).Description));
                list2.Add(rb);
            }
            for (int i = 0; i < tools.Count; i++)
            {
                item = (Item)tools[i];
                if (((BaseTool)item).UsesRemaining > 0)
                {
                    list2.Add(new ItemTileButtonInfo(item));
                }
            }
            for (int i = 0; i < potions.Count; i++)
            {
                item = (Item)potions[i];
                if (!potIDs.Contains(item.ItemID))
                {
                    list2.Add(new ItemTileButtonInfo(item));
                    potIDs.Add(item.ItemID);
                }
            }

            for (int i = 0; i < bandages.Count; i++)
            {
                item = (Item)bandages[i];
                list2.Add(new ItemTileButtonInfo(item));
            }
            for (int i = 0; i < books.Count; i++)
            {
                item = (Item)books[i];
                ItemTileButtonInfo itbi = new ItemTileButtonInfo(item);
                //itbi.Label = new TextDefinition(string.Format("{0}: {1} Spells", item.GetType().Name, ((Spellbook)item).SpellCount));
                list2.Add(itbi);
            }
            return list2;
        }

        public override void HandleButtonResponse(NetState sender, int adjustedButton, ItemTileButtonInfo buttonInfo)
        {
            PlayerMobile m = sender.Mobile as PlayerMobile;
            Item item = ((ItemTileButtonInfo)buttonInfo).Item;
            if ((m != null) && item.IsChildOf(m.Backpack))
            {
                item.OnDoubleClick(m);
  //              m.SendGump(new UseToolItemsGump(Buttons));
            }
        }
    }
}

