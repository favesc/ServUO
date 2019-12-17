using Server.Targeting;
using System;

namespace Server.Items
{
    public class UnbindingTarget : Target
    {
        //static int price = Config.Get("zp.½â³ý°ó¶¨ÆõÔ¼", 50000);

        private UnbindingDeed udeed;

        public UnbindingTarget(UnbindingDeed ud) : base(10, false, TargetFlags.None)
        {
            udeed = ud;
        }

        protected override void OnTarget(Mobile from, object target)
        {

            if (!(target is SoulVikingSword) && !(target is Vstaff) && !(target is SoulShortBow) && !(target is VRadiantScimitar)&&!(target is vHalberd)&&!(target is Vtessen))
            { from.SendMessage("You cant do that."); return; }
            else if (target is SoulVikingSword)
            {
                SoulVikingSword sb = (SoulVikingSword)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾¼µ¶Ê¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }
                    udeed.Delete();
                    sb.InvalidateProperties();

                    from.SendMessage("That Soul Weapon is now Unbound!");
                    return;
                }
            }
            else if (target is SoulShortBow)
            {
                SoulShortBow sb = (SoulShortBow)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾Ì°À·¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }

                    udeed.Delete();
                    sb.InvalidateProperties();
                    from.SendMessage("That Soul Weapon is now Unbound!"); return;
                }
            }
            else if (target is VRadiantScimitar)
            {
                VRadiantScimitar sb = (VRadiantScimitar)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾°ÁÂý¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }

                    udeed.Delete();
                    sb.InvalidateProperties();
                    from.SendMessage("That Soul Weapon is now Unbound!"); return;
                }
            }
            else if (target is Vstaff)
            {
                Vstaff sb = (Vstaff)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾ÓûÍû¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }

                    udeed.Delete();
                    sb.InvalidateProperties();
                    from.SendMessage("That Soul Weapon is now Unbound!"); return;
                }
            }
            else if (target is vHalberd)
            {
                vHalberd sb = (vHalberd)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾±©Å­¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }

                    udeed.Delete();
                    sb.InvalidateProperties();
                    from.SendMessage("That Soul Weapon is now Unbound!"); return;
                }
            }
            else if (target is Vtessen)
            {
                Vtessen sb = (Vtessen)target;
                if (sb.bond == 0)
                {
                    from.SendMessage("That is not Soul Bound."); return;
                }
                else if (!sb.IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042001); return; }


                else if (sb.IsChildOf(from.Backpack))
                {
                    sb.Name = "¡¾°ÁÂý¡¿";
                    sb.bond = 0;
                    sb.AddNameProperty(sb.PropertyList);
                    {
                        sb.PropertyList.Add("<BASEFONT COLOR=#FF7F50>" + "[Î´°ó¶¨]" + "<BASEFONT COLOR=#FFFFFF>");
                    }

                    udeed.Delete();
                    sb.InvalidateProperties();
                    from.SendMessage("That Soul Weapon is now Unbound!"); return;
                }
            }


            else return;
        }
    }

    public class UnbindingDeed : Item
    {
        public static int ExSet = Config.Get("zp.Ex", 305);

        [Constructable]
        public UnbindingDeed() : base(0x14F0)
        {
            Weight = 1.0;
            Hue = 11;
            Name = "¡¾÷Ò÷Ñ¡¿";
        }

        public UnbindingDeed(Serial serial) : base(serial)
        {
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Áé»êÎäÆ÷½â³ý°ó¶¨ÆõÔ¼");
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(ExSet);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            //LootType = LootType.Blessed;
            ExSet = reader.ReadInt();
            int version = reader.ReadInt();

        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.SendMessage("Which Soul Weapon would you like to Unbind?");
                from.Target = new UnbindingTarget(this);
            }
        }
    }
}