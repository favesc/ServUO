namespace Server.Items
{
    public class bankcard : Item
    {
        private Mobile owner;
        [Constructable]
        public bankcard():base(4779)
        {
            LootType = LootType.Blessed;
            Weight = 1;
            Name = "尚未开卡的银行卡";
        }

        [Constructable]
        public bankcard(Mobile zp) : base(4779) //7964
        {
            Movable = true;
            Hue = 0x491;
            this.Weight = 1;
            owner = zp;
            LootType = LootType.Blessed;
        }

        public bankcard(Serial serial) : base(serial)
        {
        }


        public override void OnDoubleClick(Mobile o)
        {
            if (!IsChildOf(o))
            {
                o.SendLocalizedMessage(1042001);
                return;
            }
            if (owner == null )
            {
                owner = o;
                Name = owner.Name.ToString() + "'的 银行卡";
                o.SendMessage("开卡成功.");

            }
            else if (owner != o)
            { o.SendMessage("不是你的卡，请物归原主.");
            return; }
            else if (owner==o&& o.BankBox!=null)

                o.BankBox.Open();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 
            writer.Write(owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            owner = reader.ReadMobile();
        }
    }
}
