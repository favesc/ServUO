namespace Server.Items
{
    public class ArmoredCloak : Cloak
    {
        [Constructable]
        public ArmoredCloak()
        {
            Name = "基督启示";
            Hue = 1172;
            Resistances.Physical = 27;
            Resistances.Cold = 3;
            Resistances.Fire = 14;
            Resistances.Poison = 5;
            Resistances.Energy = 6;
            LootType = LootType.Cursed;
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "古墓魅影");
        }

        public ArmoredCloak(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}