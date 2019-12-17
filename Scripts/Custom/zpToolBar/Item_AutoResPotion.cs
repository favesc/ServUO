using System;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
    public class AutoResPotion : Item
    {
        private static Dictionary<Mobile, AutoResPotion> m_ResList;

        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        private Timer m_Timer;
        private static TimeSpan m_Delay = TimeSpan.FromSeconds(7.0); /*TimeSpan.Zero*/

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay { get { return m_Delay; } set { m_Delay = value; } }

        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_Death);
        }

        [Constructable]
        public AutoResPotion() : this(1)
        { }

        [Constructable]
        public AutoResPotion(int charges) : base(0x1844) //0xF04 0x1005 0x1844
        {
            m_Charges = charges;

            Name = "Potion Of Resurrect";
            //           LootType = LootType.Blessed;
            /*Stackable = true;*/
            Weight = 1.0;
            /*Amount = amount;*/
        }
        public AutoResPotion(Serial serial)
            : base(serial)
        { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Alive)
                return;
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }
            if (m_ResList != null && m_ResList.ContainsKey(from))
            {
                AutoResPotion arp = m_ResList[from];
                if (arp == null || arp.Deleted)
                {
                    m_ResList.Remove(from);
                }
                else
                {
                    from.SendMessage("你已经喝了一瓶复活水。");//The spirits watch you already.
                    return;
                }
            }


            if (m_ResList == null)
                m_ResList = new Dictionary<Mobile, AutoResPotion>();

            if (!m_ResList.ContainsKey(from))
            {
                if (!m_ResList.ContainsValue(this))
                {
                    m_ResList.Add(from, this);
                    BuffInfo.AddBuff(from, new BuffInfo((BuffIcon)1183, 1070928, 1112762));
                    from.PlaySound(0x53c);
                    from.FixedParticles(0x375A, 1, 17, 0x7DA, 93, 0x3, EffectLayer.Waist);

                    this.Name = "瓶口沾满了口水的重生药水";
                    this.Hue = 91;
                    //this.Consume();
                    from.SendMessage("你感觉灵魂出窍，获得一次自动复活能力。");//You feel the spirits watching you, awaiting to send you back to your body.
                    return;
                }
                else
                    from.SendMessage("明显被别人喝过了");//The spirits of this potion are watching another
                return;
            }
            else
            {
                from.PlaySound(0x41e);
                from.SendMessage("  你已经喝了一瓶复活水。");//The spirits watch you already. 
            }

        }


        private static void EventSink_Death(PlayerDeathEventArgs e)
        {
            PlayerMobile owner = e.Mobile as PlayerMobile;

            if (owner != null && !owner.Deleted)
            {
                if (owner.Alive)
                    return;

                if (m_ResList != null && m_ResList.ContainsKey(owner))
                {
                    AutoResPotion arp = m_ResList[owner];
                    if (arp == null || arp.Deleted)
                    {
                        m_ResList.Remove(owner);
                        return;
                    }
                    arp.m_Timer = Timer.DelayCall(m_Delay, new TimerStateCallback(Resurrect_OnTick), new object[] { owner, arp });
                    m_ResList.Remove(owner);
                }
            }
        }

        private static void Resurrect_OnTick(object state)
        {
            object[] states = (object[])state;
            PlayerMobile owner = (PlayerMobile)states[0];
            AutoResPotion arp = (AutoResPotion)states[1];
            if (owner != null && !owner.Deleted && arp != null && !arp.Deleted)
            {
                if (owner.Alive || arp.m_Charges < 1)
                    return;

                owner.SendMessage("You died under the watch of the spirits, they have offered you another chance at life.");
                //owner.Resurrect();
                owner.SendGump(new RespGump(owner, arp));
                arp.m_Charges--;

                arp.InvalidateProperties();

                if (arp.m_Charges < 1)
                    //arp.Delete();
                    arp.Consume();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(String.Format(("还可以使用{0}次 \n" + "<BASEFONT COLOR=yellow>" + "重生药水\n" + "<BASEFONT COLOR=#FF6377>" + "(似乎xHorse也喜欢这味道)" + "<BASEFONT COLOR=white>"), m_Charges));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((TimeSpan)m_Delay);
            writer.Write((int)m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Delay = reader.ReadTimeSpan();
                        m_Charges = reader.ReadInt();
                        Name = "Potion Of Resurrect";
                        Hue = 0;
                    }
                    break;
            }
        }
    }
}
namespace Server.Gumps
{
    public class RespGump : Gump
    {
        Mobile from;
        Item tar;
        public RespGump(Mobile pm, Item target) : base(210, 180)

        {
            this.Closable = false;

            from = pm;
            tar = target;
            //           if (target is MetalChest)
            //if (tar is MetalChest)
            //           AddBackground(20, 20, 200, 80, 3500);
            //if (tar is Basket)
            AddBackground(20, 20, 300, 80, 3000);
            AddLabel(45, 35, 0, @"立刻复活吗？");
            AddItem(255, 35, tar.ItemID, tar.Hue);
            AddItemProperty(tar.Serial);
            AddButton(70, 60, 247, 248, 1, GumpButtonType.Reply, 0);
            AddButton(180, 60, 243, 242, 2, GumpButtonType.Reply, 0);
            return;
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            switch (info.ButtonID)
            {
                case 0:
                    break;
                case 1:
                    {
                        from.Resurrect();

                        break;

                    }
                case 2:
                    {
                        from.SendMessage("你放弃了这次绝好的机会。");
                        break;
                    }

            }
        }

    }
}
