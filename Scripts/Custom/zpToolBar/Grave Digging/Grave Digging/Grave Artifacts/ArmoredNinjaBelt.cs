namespace Server.Items
{
    public class ArmoredNinjaBelt : LeatherNinjaBelt
    {
        [Constructable]
        public ArmoredNinjaBelt()
        {
            Name = "F35";
            Hue = Utility.RandomMetalHue();
            Resistances.Physical = 27;
            Resistances.Cold = 3;
            Resistances.Fire = 14;
            Resistances.Poison = 5;
            Resistances.Energy = 6;
            UsesRemaining = 2000;
            Poison = Poison.Greater;
            PoisonCharges = 2000;
            LootType = LootType.Cursed;
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "夜幕刺客");
        }

        public ArmoredNinjaBelt(Serial serial) : base(serial)
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