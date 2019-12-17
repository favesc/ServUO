using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    class Bonddeed : Item
    {
        //public static int bonddeed = Config.Get("zp.结盟契约", 3000);
        [Constructable]
        public Bonddeed() : base(0x14f0)
        {
            Weight = 1.0;
            Hue = 61;
            Name = "结盟契约";
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("你和动物的感情很好");
        }
        public Bonddeed(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            //writer.Write(bonddeed);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            //LootType = LootType.Blessed;
            //bonddeed = reader.ReadInt();
            int version = reader.ReadInt();

        }
        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Alive)
                return;
            else
                from.Target = new xTarget(this);

        }

        private class xTarget : Target
        {
            private Bonddeed bdeed;
            public xTarget(Bonddeed bd) : base(12, true, TargetFlags.None)
            {
                bdeed = bd;
            }
            protected override void OnTarget(Mobile from, object target)
            {
                //pm = from;
                if (target is BaseCreature)
                {
                    var tar = (BaseCreature)target;
                    if (tar.Controlled && tar.ControlMaster == from && !tar.IsBonded || from.AccessLevel >= AccessLevel.GameMaster)
                    {
                        tar.IsBonded = true;
                        from.SendMessage(33, "宠物和你结盟了。");
                        bdeed.Delete();
                        return;
                    }
                }
                from.SendMessage("请😙🙅选择一个属于你的宠物。");
                return;
            }
        }
    }
}

