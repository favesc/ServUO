using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    //[FlipableAttribute(0x2D25, 0x2D31)]
    public class Vstaff : WildStaff
    {
        private int exp = 0;
        public byte level = 1;//最高30
                              //public int Ex = 50;
                              //public static int ExSet = Config.Get("zp.Ex", 305);
                              //static int ex { get { return ExSet; } set { ExSet = value; } }

        //private static int ex = SoulBindGump.exp;
        public Mobile from;
        [CommandProperty(AccessLevel.GameMaster)]
        public int EXP { get { return exp; } set { exp = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public byte Level { get { return level; } set { level = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Bond { get { return bond; } set { bond = value; } }

        public int bond = 0;

        //public override int AosStrengthReq { get { return 20; } }
        //public override int AosMinDamage { get { return 7; } }
        //public override int AosMaxDamage { get { return 9; } }
        //public override int AosSpeed { get { return 18; } }
        //public override float MlSpeed { get { return 3.5f; } }

        //public override int OldStrengthReq { get { return 35; } }
        //public override int OldMinDamage { get { return 5; } }
        //public override int OldMaxDamage { get { return 33; } }
        //public override int OldSpeed { get { return 35; } }

        //public override int DefHitSound { get { return 0x233; } }
        //public override int DefMissSound { get { return 0x239; } }

        public override int InitMinHits { get { return 200; } }
        public override int InitMaxHits { get { return 200; } }

        //public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Block; } }
        //public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ForceOfNature; } }

        [Constructable]
        public Vstaff()
        {
            Weight = 12.0;
            Attributes.SpellChanneling = 1;

            Name = "【欲望】";
            //Resource = CraftResource.YewWood;//Resource None so the Swords name shows correct once Bound.//zp

            switch (Utility.Random(3))
            {
                case 0: EngravedText = "大法师之杖！"; break;
                case 1: EngravedText = "混乱风暴！"; break;
                case 2: EngravedText = "原始之柱"; break;
            }

            bond = 0;
            // Create item with value at zero. Will show in [props as ParentEntity and RootParentEntitty as null.

            Attributes.Luck = Utility.Random(101);
            WeaponAttributes.SelfRepair = Utility.Random(2);

        }

        public override bool OnEquip(Mobile from)
        {
            if (bond == 0) //Check to see if bound to a serial.
            {
                bond = from.Serial; //Bind to a serial on first time equiped.
                this.Name = from.Name.ToString() + "的" + Name;//Change item name and add who it is bound to. "Player's Soul Sword"
                from.Emote("*" + from.Name + " feels a weird energy overwhelming their body*");
                from.PlaySound(0x653);
                from.FixedParticles(0x375A, 1, 17, 0x7DA, 91, 0x3, EffectLayer.Waist);
                base.OnEquip(from);
                return true;//Allow it to bind to the first player to equip it after creation.
                            //Will show in [props as ParentEntity and RootParentEntitty as [m] Serial, "Player Name"
            }
            else if (bond == from.Serial) //Check to see if sword is bound to who is equiping it.
            {
                from.FixedParticles(0x375A, 1, 17, 0x7DA, 46, 0x3, EffectLayer.Waist);
                base.OnEquip(from);
                return true; //Allow player who had bound to sword to equip it.
            }
            else
            {
                from.SendMessage("The staff refuses your soul");
                return false; //Disallow any one else from equiping the sword.
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            if (bond == 0) //Check to see if bound to a serial.
            {
                list.Add("<BASEFONT COLOR=#FF7F50>" + "[未绑定]" + "<BASEFONT COLOR=#FFFFFF>");
            }
            else if (bond >= 0)//Once the sword is bound it will show the Evolution Points.
            {
                if (level != 30)
                    list.Add("<BASEFONT COLOR=#FF6347>" + "[已绑定]\n" + "等级 " + level.ToString() + "\n升级还需: " + (UnbindingDeed.ExSet - exp).ToString() + "<BASEFONT color=white>");
                else
                    list.Add("<BASEFONT COLOR=#F0E68C >" + "[已绑定]\n" + "已达最高等级 " + "<BASEFONT color=white>");
            }
        }
        /*When weapon hits this gives a chance to gain Evolution Points*/
        public override void OnHit(Mobile attacker, IDamageable defender, double Damagebonus)
        {
            if (level != 30)
            {
                from = attacker;
                if (Utility.Random(3) == 1)
                {
                    ApplyGain();
                }
            }
            base.OnHit(attacker, defender, Damagebonus);
        }
        private Timer cs = null;

        public void ApplyGain()
        {
            if (exp < UnbindingDeed.ExSet)
            {
                exp++;


                if (exp >= UnbindingDeed.ExSet && level < 30)
                {
                    level += 1;
                    from.Say("*Upgrade!*");
                    from.PlaySound(0x1EF);
                    from.PlaySound(0x543);
                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 10, from.Y - 5, from.Z + 25), from.Map), from, 5050, 3, 0, false, true, 90, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X + 1, from.Y - 3, from.Z + 45), from.Map), from, 5050, 7, 0, false, true, 53, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    //                   Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(X - 5, Y - 5, Z + 15), Map), this, 0x36D4, 1, 0, false, true, 132, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                    if (from is PlayerMobile)
                    {
                        from.SendGump(new levelup(from, this));
                        //from.PlaySound(0x0F5);
                        //IEntity fr = new Entity(Server.Serial.Zero, new Point3D(from.X, from.Y, from.Z), from.Map);
                        //IEntity to = new Entity(Server.Serial.Zero, new Point3D(from.X, from.Y, from.Z + 40), from.Map);
                        //Effects.SendMovingParticles(fr, to, 5049, 1, 0, false, false, 91, 3, 9501, 1, 0, EffectLayer.Head, 0x100);

                        //from.PlaySound(0x1ED);
                        //from.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
                        //from.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);

                        cs = new levelup.closelevelupgump(from);
                        cs.Start();
                    }

                    exp = 0;
                    this.WeaponAttributes.HitHarm = level;
                    if (level >= 5)
                    {
                        WeaponAttributes.HitLightning = level;
                        Attributes.SpellDamage = (level - 4) * 8;
                        if (level >= 10)
                        {
                            WeaponAttributes.HitFireball = level;
                            //Attributes.SpellChanneling = 1;
                            Attributes.BonusMana = (level * 2);

                            if (level >= 20)
                            {
                                this.Attributes.BonusInt = (level);
                                Attributes.RegenMana = level / 5 - 1;
                                if (level == 30)
                                {
                                    from.Say("到顶级了.");
                                    Attributes.CastRecovery = 2;
                                    Attributes.CastSpeed = 2;
                                    switch (Utility.Random(1, 6))
                                    {

                                        case 1:
                                            { Slayer = SlayerName.ReptilianDeath; Hue = 1153; break; }
                                        case 2:
                                            { Slayer = SlayerName.DragonSlaying; Hue = 1154; break; }
                                        case 3:
                                            { Slayer = SlayerName.Repond; Hue = 1194; break; }
                                        case 4:
                                            { Slayer = SlayerName.SpidersDeath; Hue = 1195; break; }
                                        case 5:
                                            { Slayer = SlayerName.Ophidian; Hue = 1196; break; }
                                        case 6:
                                            { Slayer = SlayerName.Exorcism; Hue = 1161; break; }
                                        default: break;
                                    }
                                }
                            }
                        }

                    }

                }
                InvalidateProperties(); return;
            }
            else exp = UnbindingDeed.ExSet - 1;
        }

        public override bool CanEquip(Mobile from)
        {
            //if (from.Skills[SkillName.Swords].Base <= 25.0)
            //{
            //    from.SendMessage("You are not skilled enough to equip that.at least 25");
            //    return false;
            //}
            //else
            //{
            //    return base.CanEquip(from);
            //}
            if (bond != from.Serial && bond != 0) return false;
            else return base.CanEquip(from);

        }
        #region Menu
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if ((from.Alive && IsChildOf(from) && bond == from.Serial) || from.AccessLevel > AccessLevel.GameMaster)
                list.Add(new ContextMenus.SoulBindWeaponEntry(from, this));
        }
        #endregion menu
        public Vstaff(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)exp);//Serialize(Save) how many points the Sword has.
            writer.Write((int)bond);//Serialize who it is bound to.
            writer.Write(level);
            //writer.Write(ExSet);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            exp = reader.ReadInt();//Read on startup how many points the Sword has.
            bond = reader.ReadInt();//Read on startup who it is bound to.
            level = reader.ReadByte();
            //ExSet = reader.ReadInt();
        }
    }
}
