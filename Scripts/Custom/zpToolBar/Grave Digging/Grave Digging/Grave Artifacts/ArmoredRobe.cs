namespace Server.Items
{
    public class ArmoredRobe : Robe
	{

        [Constructable]
		public ArmoredRobe()
		{
			Name = "�������õ�ħ��";
            Hue = Utility.RandomMetalHue();
            Resistances.Physical = 27;
			Resistances.Cold = 3;
			Resistances.Fire = 2;
			Resistances.Poison = 11;
			Resistances.Energy = 2;
            LootType = LootType.Cursed;

        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "��Ĺ֮��");
        }

        public ArmoredRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
    public class ArmoredMaleElvenRobe : MaleElvenRobe
    {

        [Constructable]
        public ArmoredMaleElvenRobe()
        {
            Name = "˽�˶���(��ʽ)";
            //Hue = Utility.RandomMetalHue();
            Resistances.Physical = 27;
            Resistances.Cold = 3;
            Resistances.Fire = 2;
            Resistances.Poison = 11;
            Resistances.Energy = 2;
            LootType = LootType.Cursed;

        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "����ʱ��");
        }

        public ArmoredMaleElvenRobe(Serial serial) : base(serial)
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