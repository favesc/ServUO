using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    public class GraveDiggersShovel : Item
    {
        private int m_Uses;
        private bool m_IsDigging;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get { return m_Uses; }
            set { m_Uses = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsDigging
        {
            get { return m_IsDigging; }
            set { m_IsDigging = value; }
        }

        [Constructable]
        public GraveDiggersShovel() : base(0xF39)
        {
            Weight = 0.0;
            Hue = 93;
            Name = "掘墓鏟";
            m_Uses = Utility.RandomList(5, 5, 5, 5, 10, 10, 10, 15, 15, 20);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1060662, "Uses\t{0}", m_Uses);
        }

        public GraveDiggersShovel(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("掘墓鏟必須在你的背包内.");
            }
            if (IsDigging != true)
            {
                from.Target = new GraveTarget(this, from);
                from.SendMessage("選擇你想要挖掘的墳墓.");
            }
            else
            {
                from.SendMessage("你必須等待上一次挖掘結束才能再次挖掘.");
            }
        }

        private static void GetRandomAOSStats(out int attributeCount, out int min, out int max)
        {
            int rnd = Utility.Random(15);

            if (rnd < 2)
            {
                attributeCount = Utility.RandomMinMax(5, 9);
                min = 50; max = 100;
            }
            else if (rnd < 4)
            {
                attributeCount = Utility.RandomMinMax(4, 8);
                min = 40; max = 80;
            }
            else if (rnd < 6)
            {
                attributeCount = Utility.RandomMinMax(3, 6);
                min = 30; max = 60;
            }
            else if (rnd < 8)
            {
                attributeCount = Utility.RandomMinMax(2, 5);
                min = 20; max = 50;
            }
            else if (rnd < 10)
            {
                attributeCount = Utility.RandomMinMax(1, 4);
                min = 10; max = 40;
            }
            else
            {
                attributeCount = 1;
                min = 10; max = 30;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version 

            writer.Write(m_Uses);
            writer.Write(m_IsDigging);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Uses = reader.ReadInt();
            m_IsDigging = reader.ReadBool();
        }

        private class DigTimer : Timer
        {
            private Mobile m_From;
            private GraveDiggersShovel m_Item;

            public DigTimer(Mobile from, GraveDiggersShovel shovel, TimeSpan duration) : base(duration)
            {
                Priority = TimerPriority.OneSecond;
                m_From = from;
                m_Item = shovel;
            }

            protected override void OnTick()
            {
                Item gem = Loot.RandomGem();
                Item reg = Loot.RandomPossibleReagent();

                Item equip;
                equip = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

                if (m_Item != null)
                    m_Item.IsDigging = false;

                if (equip is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)equip;

                    int attributeCount;
                    int min, max;

                    GetRandomAOSStats(out attributeCount, out min, out max);

                    BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);
                }
                else if (equip is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)equip;

                    int attributeCount;
                    int min, max;

                    GetRandomAOSStats(out attributeCount, out min, out max);

                    BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);
                }
                else if (equip is BaseJewel)
                {
                    int attributeCount;
                    int min, max;

                    GetRandomAOSStats(out attributeCount, out min, out max);

                    BaseRunicTool.ApplyAttributesTo((BaseJewel)equip, attributeCount, min, max);
                }

                if (Utility.Random(100) < 45)
                {
                    switch (Utility.Random(16))
                    {
                        case 0:
                            Skeleton skel = new Skeleton();
                            skel.Location = m_From.Location;
                            skel.Map = m_From.Map;
                            skel.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                skel.IsParagon = true;

                            World.AddMobile(skel);
                            break;

                        case 1:
                            Ghoul ghoul = new Ghoul();
                            ghoul.Location = m_From.Location;
                            ghoul.Map = m_From.Map;
                            ghoul.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                ghoul.IsParagon = true;

                            World.AddMobile(ghoul);
                            break;

                        case 2:
                            Wraith wraith = new Wraith();
                            wraith.Location = m_From.Location;
                            wraith.Map = m_From.Map;
                            wraith.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                wraith.IsParagon = true;

                            World.AddMobile(wraith);
                            break;

                        case 3:
                            Lich lich = new Lich();
                            lich.Location = m_From.Location;
                            lich.Map = m_From.Map;
                            lich.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                lich.IsParagon = true;

                            World.AddMobile(lich);
                            break;

                        case 4:
                            LichLord lichl = new LichLord();
                            lichl.Location = m_From.Location;
                            lichl.Map = m_From.Map;
                            lichl.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                lichl.IsParagon = true;

                            World.AddMobile(lichl);
                            break;

                        case 5:
                            AncientLich alich = new AncientLich();
                            alich.Location = m_From.Location;
                            alich.Map = m_From.Map;
                            alich.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                alich.IsParagon = true;

                            World.AddMobile(alich);
                            break;

                        case 6:
                            Mummy mum = new Mummy();
                            mum.Location = m_From.Location;
                            mum.Map = m_From.Map;
                            mum.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                mum.IsParagon = true;

                            World.AddMobile(mum);
                            break;

                        case 7:
                            Zombie zom = new Zombie();
                            zom.Location = m_From.Location;
                            zom.Map = m_From.Map;
                            zom.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                zom.IsParagon = true;

                            World.AddMobile(zom);
                            break;

                        case 8:
                            SkeletalKnight sk = new SkeletalKnight();
                            sk.Location = m_From.Location;
                            sk.Map = m_From.Map;
                            sk.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                sk.IsParagon = true;

                            World.AddMobile(sk);
                            break;

                        case 9:
                            SkeletalMage sm = new SkeletalMage();
                            sm.Location = m_From.Location;
                            sm.Map = m_From.Map;
                            sm.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                sm.IsParagon = true;

                            World.AddMobile(sm);
                            break;

                        case 10:
                            BoneKnight bk = new BoneKnight();
                            bk.Location = m_From.Location;
                            bk.Map = m_From.Map;
                            bk.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                bk.IsParagon = true;

                            World.AddMobile(bk);
                            break;

                        case 11:
                            BoneMagi bm = new BoneMagi();
                            bm.Location = m_From.Location;
                            bm.Map = m_From.Map;
                            bm.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                bm.IsParagon = true;

                            World.AddMobile(bm);
                            break;

                        case 12:
                            Spectre spec = new Spectre();
                            spec.Location = m_From.Location;
                            spec.Map = m_From.Map;
                            spec.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                spec.IsParagon = true;

                            World.AddMobile(spec);
                            break;

                        case 13:
                            Shade shade = new Shade();
                            shade.Location = m_From.Location;
                            shade.Map = m_From.Map;
                            shade.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                shade.IsParagon = true;

                            World.AddMobile(shade);
                            break;

                        case 14:
                            Bogle bog = new Bogle();
                            bog.Location = m_From.Location;
                            bog.Map = m_From.Map;
                            bog.Combatant = m_From;

                            if (Utility.Random(100) < 50)
                                bog.IsParagon = true;

                            World.AddMobile(bog);
                            break;
                        case 15:
                            FrostDragon fDragon = new FrostDragon();
                            fDragon.PackGold(50000);
                            fDragon.Location = m_From.Location;
                            fDragon.Map = m_From.Map;
                            fDragon.Combatant = m_From;
                            if (Utility.Random(100) < 20)
                            {
                                fDragon.IsParagon = true;
                                fDragon.PackItem(new SoulweaponBox());
                            }
                            break;
                    }
                    m_From.SendMessage(6, "你驚醒了沉睡中的亡靈.");
                }
                else if (m_From.Skills[SkillName.Mining].Base < 15.0)
                {
                    if (Utility.Random(100) < 55)
                    {
                        m_From.SendMessage("你什麽都沒有挖掘到.");
                    }
                    else
                    {
                        switch (Utility.Random(3))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;
                        }
                    }
                }
                else if (m_From.Skills[SkillName.Mining].Base < 35.0)
                {
                    if (Utility.Random(100) < 45)
                    {
                        m_From.SendMessage("你什麽都沒有挖掘到.");
                    }
                    else
                    {
                        gem.Amount = Utility.RandomMinMax(2, 4);
                        reg.Amount = Utility.RandomMinMax(2, 4);

                        switch (Utility.Random(5))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了一些寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了一些藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;

                            case 3:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 20, SpellbookType.Regular));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 4:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 5, SpellbookType.Necromancer));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;
                        }
                    }
                }
                else if (m_From.Skills[SkillName.Mining].Base < 50.0)
                {
                    if (Utility.Random(100) < 35)
                    {
                        m_From.SendMessage("你什麽都沒有挖掘到.");
                    }
                    else
                    {
                        gem.Amount = Utility.RandomMinMax(2, 10);
                        reg.Amount = Utility.RandomMinMax(2, 10);

                        switch (Utility.Random(6))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了一些寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了一些藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;

                            case 3:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 40, SpellbookType.Regular));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 4:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 10, SpellbookType.Necromancer));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 5:
                                m_From.AddToBackpack(new Bones());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;
                        }
                    }
                }
                else if (m_From.Skills[SkillName.Mining].Base < 75.0)
                {
                    if (Utility.Random(100) < 25)
                    {
                        m_From.SendMessage("你什麽都沒有挖掘到.");
                    }
                    else
                    {
                        gem.Amount = Utility.RandomMinMax(2, 20);
                        reg.Amount = Utility.RandomMinMax(2, 20);

                        switch (Utility.Random(7))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了一些寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了一些藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;

                            case 3:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 50, SpellbookType.Regular));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 4:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 13, SpellbookType.Necromancer));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 5:
                                m_From.AddToBackpack(new Bones());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;

                            case 6:
                                m_From.AddToBackpack(new BonePile());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;
                        }
                    }
                }
                else if (m_From.Skills[SkillName.Mining].Base < 90.0)
                {
                    if (Utility.Random(100) < 15)
                    {
                        m_From.SendMessage("你什麽都沒有挖掘到.");
                    }
                    else
                    {
                        gem.Amount = Utility.RandomMinMax(2, 30);
                        reg.Amount = Utility.RandomMinMax(2, 30);

                        switch (Utility.Random(8))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了一些寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了一些藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;

                            case 3:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 63, SpellbookType.Regular));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 4:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 15, SpellbookType.Necromancer));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 5:
                                m_From.AddToBackpack(new Bones());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;

                            case 6:
                                m_From.AddToBackpack(new BonePile());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;

                            case 7:
                                m_From.AddToBackpack(new GraveItem());
                                m_From.SendMessage("你挖到了一些前人留下的寶藏.");
                                break;
                        }
                    }
                }
                else if (m_From.Skills[SkillName.Mining].Base < 150.0)
                {
                    if (Utility.Random(10) < 1)//概率 zp
                    {

                        Item i;
                        switch (Utility.Random(13))
                        {
                            case 0: { i = new ArmoredNinjaBelt(); break; }

                            case 1: { i = new ButchersResolve(); break; }

                            case 2: { i = new FollowerOfTheOldLord(); break; }
                            case 3: { i = new SkirtOfTheAmazon(); break; }

                            case 4: { i = new HolyHammerOfExorcism(); break; }

                            case 5: { i = new ArmoredCloak(); break; }
                            case 6: { i = new SoulweaponBox(); break; }
                            default: 
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12: { i = new AutoResPotion(); break; }
                        }
                        m_From.SendMessage(25, "你挖到了一些很有價值的東西.");
                        m_From.PlaySound(0x41e);
                        m_From.AddToBackpack(i);
                        m_From.SendGump(new totGump( i));

                    }
                    else
                    {
                        gem.Amount = Utility.RandomMinMax(2, 40);
                        reg.Amount = Utility.RandomMinMax(2, 40);

                        switch (Utility.Random(8))
                        {
                            case 0:
                                m_From.AddToBackpack(gem);
                                m_From.SendMessage("你挖到了一些寶石.");
                                break;

                            case 1:
                                m_From.AddToBackpack(reg);
                                m_From.SendMessage("你挖到了一些藥材.");
                                break;

                            case 2:
                                m_From.AddToBackpack(equip);
                                m_From.SendMessage("你挖到了一些裝備.");
                                break;

                            case 3:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 63, SpellbookType.Regular));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 4:
                                m_From.AddToBackpack(Loot.RandomScroll(0, 15, SpellbookType.Necromancer));
                                m_From.SendMessage("你挖到了一些卷軸.");
                                break;

                            case 5:
                                m_From.AddToBackpack(new Bones());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;

                            case 6:
                                m_From.AddToBackpack(new BonePile());
                                m_From.SendMessage("你挖到了一些骨頭.");
                                break;

                            case 7:
                                m_From.AddToBackpack(new GraveItem());
                                m_From.SendMessage("你挖到了一些前人留下的寶藏.");
                                break;
                        }
                    }
                }
                else
                {
                    m_From.SendMessage("你什麽都沒有挖掘到.");
                }

                Stop();
            }
        }
        private class GraveTarget : Target
        {

            //Grave ItemIDs
            public static int[] m_Grave = new int[]
            {
                3795,
                3807,
                3808,
                3809,
                3810,
                3816

            };

            private GraveDiggersShovel m_Item;
            private Mobile m_From;

            public GraveTarget(GraveDiggersShovel item, Mobile from) : base(12, false, TargetFlags.None)
            {
                m_Item = item;
                m_From = from;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item i = (Item)targeted;

                    bool isGrave = false;

                    foreach (int check in m_Grave)
                    {
                        if (check == i.ItemID)
                            isGrave = true;
                    }

                    if (isGrave == true)
                    {
                        m_Item.Uses -= 1;
                        if (m_Item.Uses == 0)
                        {
                            m_Item.Delete();
                            if (m_From != null)
                                m_From.SendMessage(30, "你的鏟子坏掉了.");
                        }

                        if (m_From != null)
                            m_From.SendMessage(66, "你開始掘墓.");

                        DigTimer dt = new DigTimer(m_From, m_Item, TimeSpan.FromSeconds(10.0));
                        dt.Start();
                        m_From.PlaySound(Utility.RandomList(0x125, 0x126));
                        if (Core.SA)
                        {
                            from.Animate(AnimationType.Attack, 3);
                        }
                        else
                        {
                            from.Animate(11, 5, 1, true, false, 0);
                        }
                        m_Item.IsDigging = true;
                    }
                    else
                    {
                        if (m_From != null)
                            m_From.SendMessage("那不是墳墓.");
                    }

                }
                else if (targeted is StaticTarget)
                {
                    StaticTarget i = (StaticTarget)targeted;

                    bool isGrave = false;

                    foreach (int check in m_Grave)
                    {
                        if (check == i.ItemID)
                            isGrave = true;
                    }

                    if (isGrave == true)
                    {
                        m_Item.Uses -= 1;
                        if (m_Item.Uses <= 0)
                        {
                            m_Item.Delete();
                            if (m_From != null)
                                m_From.SendMessage(60, "你的鏟子坏掉了.");
                        }

                        if (m_From != null)
                            m_From.SendMessage(66, "你開始掘墓.");

                        DigTimer dt = new DigTimer(m_From, m_Item, TimeSpan.FromSeconds(10.0));
                        dt.Start();
                        m_From.PlaySound(Utility.RandomList(0x125, 0x126));
                        m_From.Animate(11, 1, 1, true, false, 0);
                        m_Item.IsDigging = true;
                    }
                }
                else
                {
                    m_From.SendMessage("那不是墳墓.");
                }
            }
        }
    }
}
