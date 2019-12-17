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
            Name = "��δ���������п�";
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
                Name = owner.Name.ToString() + "'�� ���п�";
                o.SendMessage("�����ɹ�.");

            }
            else if (owner != o)
            { o.SendMessage("������Ŀ��������ԭ��.");
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
