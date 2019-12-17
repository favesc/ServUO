namespace Server.Items
{
    public class ArmoredPlaindress : PlainDress
    {
        [Constructable]
        public ArmoredPlaindress()
        {
            Name = "乱世俱灭";
            Hue = Utility.RandomBrightHue();
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
            list.Add(1049644, "不灭之魂");
        }
        public override int InitMinHits { get { return 200; } }
        public override int InitMaxHits { get { return 200; } }
        public ArmoredPlaindress(Serial serial) : base(serial)
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
    public class ArmoredFemaleElvenRobe : FemaleElvenRobe
    {
        [Constructable]
        public ArmoredFemaleElvenRobe()
        {
            Name = "私人定制(女式)";
            //Hue = Utility.RandomBrightHue();
            Resistances.Physical = 27;
            Resistances.Cold = 3;
            Resistances.Fire = 14;
            Resistances.Poison = 5;
            Resistances.Energy = 6;
            LootType = LootType.Cursed;
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add(1049644, "Cute Lady");
            base.AddNameProperties(list);
        }

        public ArmoredFemaleElvenRobe(Serial serial) : base(serial)
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