﻿using System;
using System.Collections;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class BaseItemTileButtonsGump : Gump
    {
        private readonly ItemTileButtonInfo[] m_Buttons;

        public BaseItemTileButtonsGump(TextDefinition header, ArrayList buttons)
            : this(header, (ItemTileButtonInfo[])buttons.ToArray(typeof(ItemTileButtonInfo)))
        {
        }

        public BaseItemTileButtonsGump(TextDefinition header, ItemTileButtonInfo[] buttons)
            : base(100, 100) //Coords are 0, o on OSI, intentional difference
        {
            m_Buttons = buttons;
            AddPage(0);

            int x = XItems * 180;
            int y = YItems * 64;

            AddBackground(0, 0, x + 20, y + 84, 3600);
            AddImageTiled(10, 10, x, 20, 0xA40);
            //           AddImageTiled(10, 40, x, y + 4, 0xA40);
            AddImageTiled(10, y + 54, x, 20, 0xA40);
            //AddAlphaRegion(10, 10, x, y + 64);

            AddButton(0, 0, 0xFB1, 0xFB2, 999, GumpButtonType.Reply, 0); //zp  all Cancel Button 
            AddButton(10, y + 54, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0); //Cancel Button
            AddHtmlLocalized(45, y + 56, x - 50, 20, 1060051, 0x7FFF, false, false); // CANCEL
            TextDefinition.AddHtmlText(this, 40, 12, x, 20, header, false, false, 0x7FFF, 0xFFFFFF);

            AddPage(1);

            int itemsPerPage = XItems * YItems;

            for (int i = 0; i < buttons.Length; i++)
            {
                int position = i % itemsPerPage;

                int innerX = (position % XItems) * 180 + 14;
                int innerY = (position / XItems) * 64 + 44;

                int pageNum = i / itemsPerPage + 1;

                if (position == 0 && i != 0)
                {
                    AddButton(x - 100, y + 54, 0xFA5, 0xFA7, 0, GumpButtonType.Page, pageNum);
                    AddHtmlLocalized(x - 60, y + 56, 60, 20, 1043353, 0x7FFF, false, false); // Next

                    AddPage(pageNum);

                    AddButton(x - 200, y + 54, 0xFAE, 0xFB0, 0, GumpButtonType.Page, pageNum - 1);
                    AddHtmlLocalized(x - 160, y + 56, 60, 20, 1011393, 0x7FFF, false, false); // Back
                }

                ImageTileButtonInfo b = buttons[i];

                AddImageTiledButton(innerX, innerY, 0x918, 0x919, 100 + i, GumpButtonType.Reply, 0, b.ItemID, b.Hue,
                    15, 10, b.LocalizedTooltip);
                AddItemProperty(buttons[i].Item.Serial);
                TextDefinition.AddHtmlText(this, innerX + 84, innerY, 100, 60, b.Label, false, false, 0x7FFF, 0xFFFFFF);
            }
        }

        protected ItemTileButtonInfo[] Buttons
        {
            get { return m_Buttons; }
        }

        protected virtual int XItems
        {
            get { return 2; }
        }

        protected virtual int YItems
        {
            get { return 5; }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 999) { (sender.Mobile as PlayerMobile).CloseGump(typeof(Opencorpp)); }
            int adjustedID = info.ButtonID - 100;

            if (adjustedID >= 0 && adjustedID < Buttons.Length)
                HandleButtonResponse(sender, adjustedID, Buttons[adjustedID]);
            else
            {
                HandleCancel(sender);
            }
        }

        public virtual void HandleButtonResponse(NetState sender, int adjustedButton, ItemTileButtonInfo buttonInfo)
        {
        }

        public virtual void HandleCancel(NetState sender)
        {
        }
    }
}