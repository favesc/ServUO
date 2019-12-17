using System;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dps golem corpse")]
    public sealed class DpsGolem : BaseCreature
    {
        private readonly TimeSpan messageDelay = TimeSpan.FromSeconds(10);
        private long lastDisplayTime;
        private double totalDamage;
        private IMount m;
        [Constructable]
        public DpsGolem()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.Name = "Dps Golem";
            this.Body = 0x191;
            InitStats(125, 100, 256);
            Utility.AssignRandomHair(this);
            this.Hue = Utility.RandomSkinHue();
            this.SetHits(10000);
            this.Female = true;
            this.Frozen = true;
            m = new Horse() { Hue = Utility.Random(1154, 15), Tamable = false, Frozen = true };
            ((Horse)m).Blessed = true;
            ((Horse)m).CantWalk = true;
            m.Rider = this;
        }

        public DpsGolem(Serial serial) : base(serial) { this.Frozen = true; }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRegenHits { get { return false; } }

        private double GetDps
        {
            get
            {
                long elapsedTicks = Core.TickCount - this.lastDisplayTime;
                return Math.Round(this.totalDamage / elapsedTicks * 1000, 1);
            }
        }
        public override bool AllowEquipFrom(Mobile mob)
        {
            return (mob == this || (mob.AccessLevel >= AccessLevel.Player));
        }
        public override bool CheckNonlocalLift(Mobile from, Item item)
        {
            if (from == this || from.AccessLevel >= AccessLevel.Player)
            {
                return true;
            }

            return false;
        }
        public override void OnDelete()
        {
            if (m!=null)
            {
                ((Horse)m).Delete();
            }
            base.OnDelete();
        }
        public override void OnHitsChange(int oldValue)
        {
            if (this.Hits != this.HitsMax)
            {
                this.totalDamage += oldValue - this.Hits;

                if (Core.TickCount - this.lastDisplayTime > this.messageDelay.TotalMilliseconds)
                {
                    double dps = this.GetDps;

                    if (dps > 0)
                    {
                        this.PublicOverheadMessage(MessageType.Regular, 73, true, String.Format("DPS: {0}", dps));
                    }

                    this.lastDisplayTime = Core.TickCount;
                    this.totalDamage = 0;
                }

                this.Hits = this.HitsMax;
            }
            if (!Mounted && m != null)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => { m.Rider = this; });

            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
