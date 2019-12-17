using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Gumps
{
    public class Openbox : BaseItemTileButtonsGump
    {
        static BaseContainer c;
        public Openbox(BaseContainer box) : base("      Box?wtf!", itemlist(box)) { }

        //public Openbox(BaseContainer box, ArrayList itembutton) : base("      Box?wtf!", itembutton) { }
        //public Openbox(BaseContainer box) : this(box, itemlist(box)) { } //老的写法，不好不好

        public static ArrayList itemlist(BaseContainer box)
        {
            c = box;
            if (c == null) return new ArrayList();
            ArrayList item = new ArrayList(c.Items);
            ArrayList list = new ArrayList();
            for (int i = 0; i < item.Count; i++)
            {
                Item it = (Item)item[i];
                if (it is Gold && ((Gold)it).Amount < 2001)
                    continue;
                if (it is BaseReagent)
                    continue;
                if (it is IGem)
                    continue;
                if (it is SpellScroll)
                    continue;
                if (it is BasePotion)
                    continue;
                if (it is BaseClothing && ((BaseClothing)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseJewel && ((BaseJewel)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseArmor && ((BaseArmor)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseWeapon && ((BaseWeapon)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                ItemTileButtonInfo xx = new ItemTileButtonInfo(it);
                list.Add(xx);
            }
            return list;
        }
        public override void HandleButtonResponse(NetState sender, int adjustedButton, ItemTileButtonInfo buttonInfo)
        {
            PlayerMobile m = sender.Mobile as PlayerMobile;
            Item item = ((ItemTileButtonInfo)buttonInfo).Item;
            if ((m != null) && !item.Deleted && m.Alive)
            {
                if (item.Parent == c)
                {
                    m.AddToBackpack(item);
                    m.SendSound(item.GetDropSound());
                }
            }
            if (Openbox.itemlist(c).Count > 0)
            {
                m.SendGump(new Openbox(c));
            }
            //m.SendGump(new BaseItemTileButtonsGump("ss", itemlist(c)));
        }
    }
    public class Opencorpp : BaseItemTileButtonsGump
    {
        //public static bool youmeiyou = true;
        static Corpse c;
        //public Opencorpp(Corpse  corp):base ("wtf" , itemlist(corp)); //新写法
        public Opencorpp(Corpse corp, ArrayList itembutton) : base(corp.Name, itembutton) { }
        public Opencorpp(Corpse corp) : this(corp, itemlist(corp)) { }
        public static ArrayList itemlist(Corpse corp)
        {
            c = corp;
            if (corp == null) return new ArrayList();
            ArrayList item = new ArrayList(corp.Items);
            ArrayList list = new ArrayList();
            for (int i = 0; i < item.Count; i++)
            {
                Item it = (Item)item[i];
                if (it is Gold && ((Gold)it).Amount < 2001)
                    continue;
                if (it is BaseReagent)
                    continue;
                if (it is IGem)
                    continue;
                if (it is SpellScroll)
                    continue;
                if (it is BasePotion)
                    continue;
                if (it is BaseClothing && ((BaseClothing)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseJewel && ((BaseJewel)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseArmor && ((BaseArmor)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                if (it is BaseWeapon && ((BaseWeapon)it).ItemPower < ItemPower.MajorArtifact)
                    continue;
                ItemTileButtonInfo xx = new ItemTileButtonInfo(it);
                list.Add(xx);
            }
            return list;

        }

        public override void HandleButtonResponse(NetState sender, int adjustedButton, ItemTileButtonInfo buttonInfo)

        {
            PlayerMobile m = sender.Mobile as PlayerMobile;
            Item item = ((ItemTileButtonInfo)buttonInfo).Item;
            if ((m != null) && !item.Deleted && m.Alive)
            {
                if (item.Parent == c && c.CheckLoot(m, item) && c.InLOS(m.Location))
                {
                    //m.Backpack.DropItem(item);
                    m.AddToBackpack(item);
                    m.SendSound(66);
                    //m.SendSound(item.GetDropSound());

                }
                if (Opencorpp.itemlist(c).Count > 0 )
                    {
                        m.SendGump(new Opencorpp(c));
                    if (!item.InRange(m,2)|| !item.InLOS(m.Location))
                    {
                        //item.SendLocalizedMessageTo(m, 501853);
                        m.SendLocalizedMessage(500446); // That is too far away.
                        //m.SendSound(Utility.RandomList(44,555,556,765,737,735,736));
                        m.SendSound(736);
                    }

                    }
            }

            //c.ProcessOpeners(m);
            //m.SendGump(new BaseItemTileButtonsGump("ss", itemlist(c)));


        }
    }
}
