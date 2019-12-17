using Server.Targeting;
using Server.Mobiles;
using System;

namespace Server.Items
{
    class Transform : Item
    {
        //public static int transform = Config.Get("zp.转职契约", 3000);
        [Constructable]
        public Transform() : base(0x14f0)
        {
            //Stackable = true;
            Weight = 1.0;
            Hue = 1173;
            Name = "转职契约";
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("你需要有对应的技能才能更改宠物的AI类型并且提高总技能的上限还有其他zpcheat效果");

        }
        public Transform(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.Alive)
                from.Target = new tTarget(this);
            else
                return;
        }

        private class tTarget : Target
        {
            private Transform transformdeed;
            public tTarget(Transform tfdeed) : base(12, true, TargetFlags.None)
            {
                transformdeed = tfdeed;
            }

            private void transf(Mobile a, BaseCreature b)
            {
                a.SendSound(0x3d);
                ((BaseCreature)b).HueMod = 33770;
                //((BaseCreature)b).BodyMod = 0x191;
                //if (b.FindItemOnLayer(Layer.InnerTorso) == null)
                //    b.AddItem(new FemaleLeatherChest());
                //if (b.FindItemOnLayer(Layer.Shoes) == null)
                //    b.AddItem(new Sandals());
                //b.Female = true;
                //b.HairItemID = Utility.RandomList(8252, 8253);
                //b.HairHue = Utility.RandomList(1174, 1175, 1153, 1166, 1172, 1158);//以上 人型

                ((BaseCreature)b).BodyMod = 0x579;//蜘蛛
            }
            protected override void OnTarget(Mobile from, object target)
            {
                if (target is BaseCreature)
                {
                    var tar = (BaseCreature)target;

                    int io = tar.Body;
                    int ioo = tar.Hue;
                    if (tar.Controlled && tar.ControlMaster == from && tar.IsBonded || from.AccessLevel >= AccessLevel.GameMaster)//zp cheat
                    {
                        if (from.HasGump(typeof(Gumps.ai)))
                            from.CloseGump(typeof(Gumps.ai));
                        from.SendGump(new Gumps.ai(from, tar));
                        tar.SkillsCap = 50000;
                        if (tar is xhorse)
                        {
                            if (((xhorse)tar).TrainingProfile.TrainingProgress < 99) { ((xhorse)tar).TrainingProfile.TrainingProgress = 100; }  
                            ((xhorse)tar).ControlSlots = 1;
                            if (((xhorse)tar).RawStatTotal < 1200)
                            {
                                ((xhorse)tar).RawStr += 555;
                                ((xhorse)tar).RawDex += 92;
                                ((xhorse)tar).RawInt += 92;
                                ((xhorse)tar).HitsMaxSeed += 555;

                            }
                        }
                        if (tar is FrenziedOstard && !tar.TrainingProfile.HasBegunTraining)
                        {
                           tar. SetStr(1100, 1200);
                            tar.SetDex(100, 200);
                            tar.SetInt(700, 1000);

                            tar.SetHits(800, 1200);

                            tar.SetDamage(35, 50);

                            tar.SetDamageType(ResistanceType.Physical, 75);
                            tar.SetDamageType(ResistanceType.Fire, 25);

                            tar.SetResistance(ResistanceType.Physical, 65, 75);
                            tar.SetResistance(ResistanceType.Fire, 80, 90);
                            tar.SetResistance(ResistanceType.Cold, 70, 80);
                            tar.SetResistance(ResistanceType.Poison, 60, 70);
                            tar.SetResistance(ResistanceType.Energy, 60, 70);

                            tar.SetSkill(SkillName.EvalInt, 80.1, 100.0);
                            tar.SetSkill(SkillName.Magery, 80.1, 100.0);
                            tar.SetSkill(SkillName.Meditation, 52.5, 75.0);
                            tar.SetSkill(SkillName.MagicResist, 100.5, 150.0);
                            tar.SetSkill(SkillName.Tactics, 97.6, 100.0);
                            tar.SetSkill(SkillName.Wrestling, 97.6, 100.0);
                            tar.SetWeaponAbility(WeaponAbility.BleedAttack);
                            
                            
                        }
                        if (tar is Nightmare && ((Nightmare)tar).RawStatTotal < 2000 && ((Nightmare)tar).TrainingProfile.HasBegunTraining == true)
                        {
                            if (((Nightmare)tar).TrainingProfile.TrainingPoints < 700) { ((Nightmare)tar).TrainingProfile.TrainingPoints += 700; }
                            if (((Nightmare)tar).TrainingProfile.TrainingProgress < 99) { ((Nightmare)tar).TrainingProfile.TrainingProgress = 99.5; }
                            else if (Spells.TransformationSpellHelper.UnderTransformation(tar))
                            {
                                from.SendMessage("Test if TransformationSpellHelper.UnderTransformation(tar)。");
                            }
                                if (tar.BodyMod != 0)
                            {
                                from.SendMessage("wtdfaseluanm。");
                            }
                            else
                            {
                                transf(from, tar);

                                Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
                             {
                                 ((BaseCreature)tar).HueMod = -1; ((BaseCreature)tar).BodyMod = 0;
                                 if (tar.FindItemOnLayer(Layer.InnerTorso) != null)
                                     tar.FindItemOnLayer(Layer.InnerTorso).Delete();
                                 if (tar.FindItemOnLayer(Layer.Shoes) != null)
                                     tar.FindItemOnLayer(Layer.Shoes).Delete();
                                 from.SendSound(0x3e);
                                 from.CloseGump(typeof(Gumps.ai));

                                 return;
                             });
                            }
                        }
                        //transform.Delete();                

                        transformdeed.Consume();

                    }//zp

                }
                else if (from.Name == "ffff" && target is Item && ((Item)target).RootParent == from)//zp cheat
                {
                    if (target is BaseArmor)
                    {
                        from.PlaySound(0x41e);
                        ((BaseArmor)target).Attributes.Luck = ((BaseArmor)target).Attributes.Luck < 155 ? 155 : ((BaseArmor)target).Attributes.Luck;
                    }
                    if (target is BaseTalisman)
                    {
                        from.PlaySound(0x41e);
                        ((BaseTalisman)target).Attributes.Luck = ((BaseTalisman)target).Attributes.Luck < 185 ? 185 : ((BaseTalisman)target).Attributes.Luck;
                    }
                    if (target is BaseJewel)
                    {
                        from.PlaySound(0x41e);
                        ((BaseJewel)target).Attributes.Luck = ((BaseJewel)target).Attributes.Luck < 185 ? 185 : ((BaseJewel)target).Attributes.Luck;
                    }

                    else from.SendMessage("请🙅选择正确的目标。😙 目前只能选择 防具 护身符 饰品");
                }
                else if (from is PlayerMobile && ((PlayerMobile)from).PointSystems.GauntletPoints < 650000)
                {
                    from.PlaySound(0x41e);
                    ((PlayerMobile)from).PointSystems.GauntletPoints += 650000;
                }//zp cheat
                else
                    from.SendMessage("请😙🙅选择一个属于你的宠物。");
                return;
            }
        }
    }
}


