using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    // [FlipableAttribute(0x2D1E, 0x2D2A)]
    public class SoulShortBow : ElvenCompositeLongbow
    {
        private int exp = 0;
        private byte level = 1;//最高30
        //public static int ExSet = Config.Get("zp.Ex", 305);
        public Mobile f;
        [CommandProperty(AccessLevel.GameMaster)]
        public int EXP { get { return exp; } set { exp = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public byte Level { get { return level; } set { level = value; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Bond { get { return bond; } set { bond = value; } }
        public int bond = 0;

        //public override int EffectID { get { return 0xF42; } }
        //public override Type AmmoType { get { return typeof(Arrow); } }
        //public override Item Ammo { get { return new Arrow(); } }

        //public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ForceArrow; } }
        //public override WeaponAbility SecondaryAbility { get { return WeaponAbility.SerpentArrow; } }

        //public override int AosStrengthReq { get { return 30; } }
        //public override int AosMinDamage { get { return Core.ML ? 15 : 16; } }
        //public override int AosMaxDamage { get { return Core.ML ? 19 : 18; } }
        //public override int AosSpeed { get { return 40; } }
        //public override float MlSpeed { get { return 3.75f; } }

        //public override int OldStrengthReq { get { return 20; } }
        //public override int OldMinDamage { get { return 9; } }
        //public override int OldMaxDamage { get { return 41; } }
        //public override int OldSpeed { get { return 20; } }

        //public override int DefMaxRange { get { return 10; } }

        //public override WeaponAnimation DefAnimation { get { return WeaponAnimation.ShootBow; } }

        public override int InitMinHits { get { return 200; } }
        public override int InitMaxHits { get { return 200; } }

        [Constructable]
        public SoulShortBow()
        {
            Weight = 17.0;
            Name = "【贪婪】";
            Resource = CraftResource.None;
            switch (Utility.Random(3))
            {
                case 0: EngravedText = "辕门射戟！"; break;
                case 1: EngravedText = "百步穿杨！"; break;
                case 2: EngravedText = "弹无虚发！"; break;
            }


            Layer = Layer.TwoHanded;

            bond = 0;

            Attributes.Luck = Utility.Random(101);
            WeaponAttributes.SelfRepair = Utility.Random(2);

        }

        public override bool OnEquip(Mobile from)
        {
            if (bond == 0) //Check to see if bound to a serial.
            {
                bond = from.Serial; //Bind to a serial on first time equiped.
                this.Name = from.Name.ToString() + "的" + Name;//Change item name and add who it is bound to. "Player's Soul Bow"
                from.Emote("*" + from.Name + " feels a weird energy overwhelming their body*");
                from.PlaySound(0x653);
                from.FixedParticles(0x375A, 1, 17, 0x7DA, 50, 0x3, EffectLayer.Waist);

                base.OnEquip(from);
                return true;//Allow it to bind to the first player to equip it after creation.
                            //Will show in [props as ParentEntity and RootParentEntitty as [m] Serial, "Player Name"
            }
            else if (bond == from.Serial) //Check to see if Bow is bound to who is equiping it.
            {
                from.FixedParticles(0x375A, 1, 17, 0x7DA, 45, 0x3, EffectLayer.Waist);
                base.OnEquip(from);
                return true; //Allow player who had bound to Bow to equip it.
            }
            else
            {
                from.SendMessage("The Bow refuses your soul");
                return false; //Disallow any one else from equiping the Bow.
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
                    list.Add("<BASEFONT COLOR=#FF6347>" + "[已绑定]\n" + "等级 " + level.ToString() + "\n升级还需: " + (UnbindingDeed.ExSet - exp).ToString() + "<BASEFONT COLOR=#FFFFFF>");
                else
                    list.Add("<BASEFONT COLOR=#F0E68C >" + "[已绑定]\n" + "已达最高等级 " + "<BASEFONT COLOR=#FFFFFF>");
            }
        }
        /*When weapon hits this gives a chance to gain Evolution Points*/
        public override void OnHit(Mobile attacker, IDamageable defender, double Damagebonus)
        {
            if (this.level != 30)
            {
                f = attacker;
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
                    f.Say("*lEVEL UP!*");
                    if (f is PlayerMobile)
                    {
                        f.SendGump(new levelup(f, this));
                        f.PlaySound(0x0F5);
                        //                  from.PlaySound(0x1ED);
                        f.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
                        //                   from.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);

                        cs = new levelup.closelevelupgump(f);
                        cs.Start();
                    }

                    exp = 0;
                    this.WeaponAttributes.HitHarm = level;
                    if (level >= 5)
                    {
                        WeaponAttributes.HitLightning = level;
                        Attributes.WeaponDamage = (level - 4) * 2;
                        if (level >= 10)
                        {
                            WeaponAttributes.HitCurse = (level - 9) * 2;
                            Attributes.SpellChanneling = 1;

                            if (level >= 20)
                            {
                                WeaponAttributes.HitLowerDefend = (level + 10);
                                if (level == 30)
                                {
                                    Attributes.CastRecovery = 1;
                                    Attributes.CastSpeed = 1;
                                    WeaponAttributes.HitLeechStam = (level + 15);

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
                InvalidateProperties();

                return;

            }
            else exp = UnbindingDeed.ExSet - 1;

        }

        public override bool CanEquip(Mobile from)
        {
            //if (from.Skills[SkillName.Archery].Base <= 25.0)
            //{
            //    from.SendMessage("You are not skilled enough to equip that.at least 25");
            //    return false;
            //}
            //else
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

        public SoulShortBow(Serial serial) : base(serial)
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
            exp = reader.ReadInt();//Read on startup how many points the Bow has.
            bond = reader.ReadInt();//Read on startup who it is bound to.
            level = reader.ReadByte();
            //ExSet = reader.ReadInt();

        }
    }
}