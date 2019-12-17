using Server.Commands;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Gumps
{
    public class Showtoolstats : Gump
    {
        public Showtoolstats(PlayerMobile owner)
            : base(60, 0x69)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 200;
            double num4 = 5.0;
            int lr = (AosAttributes.GetValue(owner, AosAttribute.LowerRegCost) > num3) ? num3 : AosAttributes.GetValue(owner, AosAttribute.LowerRegCost);
            int lm = (AosAttributes.GetValue(owner, AosAttribute.LowerManaCost) > num3) ? num3 : AosAttributes.GetValue(owner, AosAttribute.LowerManaCost);
            if ((5.0 + (0.5 * (((double)(120 - owner.Dex)) / 10.0))) >= num4)
            {
                double num1 = ((double)(120 - owner.Dex)) / 10.0;
            }
            Closable = true;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddPage(1);

            AddBackground(num, num2, 300, 520, 3500);
            num = num2 = 15;
            //AddHtml(40, num2, 90, 20, "<basefont color=blue><center>角色属性</center></font>", false, false);
            AddLabel(40, num2, 20, ("角色属性"));
            AddButton(215, 350, 5520, 5521, 1, GumpButtonType.Page, 2);//ok

            if (owner.AccessLevel > AccessLevel.VIP)
            {
                //AddImageTiled(150, 110, 138, 237, 990);
                AddImage(150, 110, 990);
                AddButton(215, 95, 5523, 5524, 2, GumpButtonType.Reply, 0);
            }
            else
            {
                //AddImageTiled(150, 110, 138, 237, 991);
                AddImage(150, 110, 991);

            }
            //AddButton(130, 10, 2443, 2444, 3, GumpButtonType.Reply, 0);//news
            //AddLabel(135, 12, 68, "新 闻");
            //AddButton(200, 10, 2443, 2444, 4, GumpButtonType.Reply, 0);//buygump
            //AddLabel(205, 12, 68, "买东西");


            num2 += 20;
            AddLabel(num, num2, 0x40, string.Format("力: {0} + {1}", owner.RawStr, owner.Str - owner.RawStr));
            num = 178;
            AddLabel(num, num2, 0x40, string.Format("敏: {0} + {1}", owner.RawDex, owner.Dex - owner.RawDex));
            num2 += 20;
            AddLabel(15, num2, 0x40, string.Format("智: {0} + {1}", owner.RawInt, owner.Int - owner.RawInt));
            num2 += 20;
            AddLabel(15, num2, 20, string.Format("Karma: {0}", owner.Karma));
            num = 0xb2;
            AddLabel(num, num2, 20, string.Format("Fame: {0}", owner.Fame));
            num2 += 20;
            AddLabel(15, num2, 0x40, string.Format("省药/省魔: {0}%  {1}%", lr, lm));

            AddLabel(15, 0x73, 0x4eb, "饥饿度");
            AddImageTiled(15, 0x87, 150, 14, 0x2340);
            AddImage(15, 0x87, 0x233f);
            AddImage(0xa5, 0x87, 0x2341);
            double num6 = ((double)owner.Hunger) / 20.0;
            int num7 = (int)(148.0 * num6);
            AddImage(15 + num7, 0x87, 0x233e, 0x4eb);
            AddLabel(15, 0x9b, 0x4f0, "口渴度");
            AddImageTiled(15, 0xaf, 150, 14, 0x2340);
            AddImage(15, 0xaf, 0x233f);
            AddImage(0xa5, 0xaf, 0x2341);
            double num8 = ((double)owner.Thirst) / 20.0;
            int num9 = (int)(148.0 * num8);
            AddImage(15 + num9, 0xaf, 0x233e, 0x4f0);
            num = 15;
            num2 = 0xeb;
            //AddLabel(num, num2, 0x40, string.Format("铁匠订单剩余时间: {0}:{1}:{2}", owner.NextSmithBulkOrder.Hours, owner.NextSmithBulkOrder.Minutes, owner.NextSmithBulkOrder.Seconds));
            //num2 += 20;
            //AddLabel(num, num2, 0x40, string.Format("裁缝订单剩余时间: {0}:{1}:{2}", owner.NextTailorBulkOrder.Hours, owner.NextTailorBulkOrder.Minutes, owner.NextTailorBulkOrder.Seconds));
            //num2 += 20;
            //     AddLabel(num, num2, 0x40, string.Format("铁匠订单剩余 {0} 张。", owner.BODData.Smithy.CachedDeeds));
            num2 += 20;
            //     AddLabel(num, num2, 0x40, string.Format("裁缝订单剩余 {0} 张。", owner.BODData.Tailor.CachedDeeds));
            num2 += 20;

            //AddLabel(num, num2, 0x40, string.Format("悬赏订单剩余: {0}:{1}:{2}", owner.NextKillingAnimalBulkOrder.Hours, owner.NextKillingAnimalBulkOrder.Minutes, owner.NextKillingAnimalBulkOrder.Seconds));
            //AddLabel(num, num2, 0x40, string.Format("驯兽订单剩余: {0}:{1}:{2}", owner.NextTamingBulkOrder.Hours, owner.NextTamingBulkOrder.Minutes, owner.NextTamingBulkOrder.Seconds));
            num2 += 20;
            AddLabel(num, num2, 0x40, string.Format("已谋害 {0}人", owner.Kills));
            num2 += 20;
            AddLabel(num, num2, 50, "您所在的位置是:");
            num2 += 20;
            AddLabel(num, num2, 50, string.Format("地图名: {0}", owner.Map));
            num2 += 20;
            AddLabel(num, num2, 50, string.Format("x: {0} y: {1} z: {2}", owner.X, owner.Y, owner.Z));
            num2 += 20;
            AddLabel(num, num2, 0x110, "该角色在线时间:");
            num2 += 20;
            AddLabel(num, num2, 0x110, string.Format("{0} 天, {1} 小时, {2} 分, {3} 秒.", new object[] { owner.GameTime.Days, owner.GameTime.Hours, owner.GameTime.Minutes, owner.GameTime.Seconds }));//学习 zp
            num2 += 20;
            AddLabel(num, num2, 35, string.Format("德野岛杀怪积分  {0}", owner.PointSystems.TOTPoints / 100));
            num2 += 20;
            AddLabel(num, num2, 35, string.Format("高级怪是啥积分  {0}", owner.PointSystems.VASPoints / 100));

            num2 += 20;
            AddButton(210, num2 - 7, 2443, 2444, 3, GumpButtonType.Reply, 0);//news
            AddLabel(215, num2 - 5, 90, "看更新");

            //Luck

            //AddLabel(num, num2, 30, string.Format("{0} /5 + 1500|750", (int)(Math.Pow(owner.Luck, 1 / 1.8) * 100)));
            AddLabel(num, num2, 90, string.Format("{0} Luck ", owner.Luck));


            num2 += 20;
            AddLabel(num, num2, 30, string.Format("GauntletPoints:{0}", owner.PointSystems.GauntletPoints));

            if (owner.AccessLevel > AccessLevel.Player)
            {
                AddButton(210, num2, 2443, 2444, 4, GumpButtonType.Reply, 0);//buygump
                AddLabel(215, num2 + 2, 90, "买东西");
            }

            num2 += 50;//获得ml神器之类的条件
            double Min = 1 / (Math.Max(10, 100 * (0.83 - Math.Round(Math.Log(Math.Round(0.0 / 6000, 3) + 0.001, 10), 3))) * (100 - Math.Sqrt(owner.Luck)) / 100.0);
            double Max = 1 / (Math.Max(10, 100 * (0.83 - Math.Round(Math.Log(Math.Round(32000.0 / 6000, 3) + 0.001, 10), 3))) * (100 - Math.Sqrt(owner.Luck)) / 100.0);

            AddLabel(num, num2, 0x39, string.Format("{0}", Min));
            num2 += 20;
            AddLabel(num, num2, 0x40, string.Format("{0}", Max));
            num2 += 20;
            AddLabel(num, num2, 0x41, owner.NetState.Address.ToString());
            num2 += 20;
            if (owner.NetState.Address.ToString() == "127.0.0.1") { AddLabel(num, num2, 0x42, "true"); }
            else AddLabel(num, num2, 0x42, "false");


            AddPage(2);
            AddButton(215, 350, 5520, 5521, 1, GumpButtonType.Reply, 0);//ok



        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile from = (PlayerMobile)sender.Mobile;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        from.SendGump(new Showtoolstats(from));
                        from.SendMessage("頁面已刷新");
                        break;
                    }
                case 2:
                    {
                        CommandSystem.Handle(from, string.Format("[sstat"));
                        break;
                    }
                case 3:
                    {
                        from.CloseGump(typeof(PnewsGump));
                        from.SendGump(new PnewsGump());
                        break;
                    }
                case 4:
                    {
                        from.CloseGump(typeof(BuyGump));
                        from.SendGump(new BuyGump(from)); break;
                    }
                default: break;
            }
        }
    }
}

