using Server.Mobiles;
using Server.Network;
using Server.Commands;


namespace Server.Gumps
{
    public class ShowToolMain : Gump
    {
        private PlayerMobile from;

        public ShowToolMain(PlayerMobile owner) : base(0, 15)
        {
            from = owner;
            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = false;
            AddPage(0);
            //          AddBackground(0x14, 0x0d, 630, 0x1b, 0x2422);
            //         AddImageTiled(0x15, 0x11, 0x273, 0x15, 0x243a);
            //          AddAlphaRegion(0x15, 0x17, 0x273, 0x15);


            int bx = 60;
            if (from.AccessLevel > AccessLevel.VIP)
            {
                AddButton(bx, 0x10, 2443, 2444, 8, GumpButtonType.Reply, 0);//colorpicker res
            }
            else
            {
                AddButton(bx, 0x10, 2443, 2444, 10, GumpButtonType.Reply, 0);//玩家权限，新闻
                AddTooltip(502978); //一些最新新聞！

            }
            bx += 80;
            AddButton(bx, 0x10, 2443, 2444, 12, GumpButtonType.Reply, 0);//
            AddTooltip(1079386);//船隻指令5  呱呱作业12
            bx += 80;
            AddButton(bx, 0x10, 2443, 2444, 4, GumpButtonType.Reply, 0);//房产
            AddTooltip(1042494);//我不會放棄我的財產!
            bx += 80;
            AddButton(bx, 0x10, 2443, 2444, 11, GumpButtonType.Reply, 0);//背包
            AddTooltip(3002139);//背包

            bx += 80;
            AddButton(bx, 0x10, 2443, 2444, 6, GumpButtonType.Reply, 0);//pet

            bx += 80;

            if (!from.Alive)
            {
                AddButton(bx, 0x10, 2443, 2444, 3, GumpButtonType.Reply, 0);//鬼魂回城
            }
            else
            {
                AddButton(bx, 0x10, 2443, 2444, 2, GumpButtonType.Reply, 0);
                AddTooltip(3000135);//角色状态
            }





            bx = 60;
            AddBackground(0, 13, 615, 0x1b, 0x2422);
            AddImageTiled(11, 0x11, 598, 0x15, 0x243a);
            AddAlphaRegion(11, 0x11, 598, 0x15);
            AddButton(5, 0x11, 0x15a4, 0x15a5, 1, GumpButtonType.Reply, 0);//三角按钮？

            //            AddButton(bx, 0x10, 0x7ed, 0x7ec, 2, GumpButtonType.Reply, 0);        //statusbutton
            //           AddButton(bx, 0x10, 0x844, 0x843, 2, GumpButtonType.Reply, 0);            //按钮1 menual
            //            AddLabel(65, 0x10, 0x40, "未知");

            if (from.AccessLevel >= AccessLevel.Player)
            {
                AddButton(35, 0x10, 0x2c38, Utility.Random(0x2c10, 6), 0, GumpButtonType.Reply, 0);// [buy
                AddTooltip(1011037);//<center>購買與販賣</center>
            }

            if (from.AccessLevel > AccessLevel.VIP)
            {
                //AddButton(bx, 0x10, 0x82f, 0x82e, 8, GumpButtonType.Reply, 0);
                AddImage(bx, 0x10, 0x7584);
                AddLabel(bx + 25, 0x10, 0x40, "染色");
            }
            else
            {
                //AddButton(bx, 0x10, 0x82f, 0x82e, 10, GumpButtonType.Reply, 0);
                //AddTooltip(502978); //一些最新新聞！
                AddImage(bx, 0x10, 0x7584);
                AddLabel(bx + 25, 0x10, 90, "更新");
            }
            bx += 80;
            //AddButton(bx, 0x10, 0x82f, 0x82e, 5, GumpButtonType.Reply, 0);
            //AddTooltip(1079386);//船隻指令
            AddImage(bx, 0x10, 0x7584);
            AddLabel(bx + 25, 0x10, 0x40, "作業");

            bx += 80;
            //AddButton(bx, 0x10, 0x82f, 0x82e, 4, GumpButtonType.Reply, 0);
            //AddTooltip(1042494);//我不會放棄我的財產!
            AddImage(bx, 0x10, 0x7584);
            AddLabel(bx + 25, 0x10, 0x40, "房產");

            bx += 80;
            //AddButton(bx, 0x10, 0x82f, 0x82e, 11, GumpButtonType.Reply, 0);
            //AddTooltip(3002139);//背包
            AddImage(bx, 0x10, 0x7584);
            AddLabel(bx + 25, 0x10, 0x40, "背包");
            bx += 80;
            //AddButton(bx, 0x10, 0x82f, 0x82e, 6, GumpButtonType.Reply, 0);
            AddImage(bx, 0x10, 0x7584);
            AddLabel(bx + 25, 0x10, 0x40, "寵物");


            bx += 80;
            if (!from.Alive)
            {
                //AddButton(bx, 0x10, 0x82f, 0x82e, 3, GumpButtonType.Reply, 0);
                AddImage(bx, 0x10, 0x7584);
                AddLabel(bx + 25, 0x10, 28, "鬼魂");
            }
            else
            {
                //AddButton(bx, 0x10, 0x82f, 0x82e, 2, GumpButtonType.Reply, 0);
                //AddTooltip(3000135);//角色状态

                AddImage(bx, 0x10, 0x7584);
                AddLabel(bx + 25, 0x10, 0x40, "屬性");
            }
            bx += 80;

            //AddButton(bx, 0x10, 0x844, 0x843, 7, GumpButtonType.Reply, 0);//menual button
            AddButton(bx, 0x10, 0x841, 0x840, 7, GumpButtonType.Reply, 0);//auto button
            AddTooltip(500394);//cliloc 


            bx += 80;
            if (from.AccessLevel > AccessLevel.VIP)

                AddButton(bx, 10, 0x845, 0x846, 111, GumpButtonType.Reply, 0);//gm test  totGump


        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            //string str = CommandSystem.Prefix;
            //int[] numArray = new int[] { 1 };
            //int[] numArray1 = new int[] { 9 };


            switch (info.ButtonID)
            {
                case 0://buy
                    from.CloseGump(typeof(ShowToolMain));
                    from.SendGump(new ShowToolMain(from));
                    if (from.HasGump(typeof(BuyGump)))
                        from.CloseGump(typeof(BuyGump));
                    else if (from.Alive)
                    {
                        from.SendGump(new BuyGump(from));
                    }

                    break;//zp 结束 switch
                          //reture; 如果是return 就跳出函数
                case 1://mini
                    from.CloseGump(typeof(ShowToolMain));
                    from.SendGump(new ShowTool(from));
                    break;

                case 2://属性
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    if (from.HasGump(typeof(Showtoolstats)))
                        from.CloseGump(typeof(Showtoolstats));
                    else
                        from.SendGump(new Showtoolstats(from));
                    break;

                case 3://回城
                    if (from.Alive) goto case 2;
                    from.SendGump(new ShowToolMain(from));
                    if (from.HasGump(typeof(ShowToolGhost)))
                        from.CloseGump(typeof(ShowToolGhost));
                    else
                        from.SendGump(new ShowToolGhost(from));
                    break;


                case 4://房屋
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    if (from.AccessLevel > AccessLevel.Player) CommandSystem.Handle(from, string.Format("[myhouses"));
                    if (from.HasGump(typeof(ShowToolHouse)))
                        from.CloseGump(typeof(ShowToolHouse));
                    else
                        from.SendGump(new ShowToolHouse(from));
                    break;

                case 5://开船
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    if (from.HasGump(typeof(ShowToolShip)))
                        from.CloseGump(typeof(ShowToolShip));
                    else
                        from.SendGump(new ShowToolShip(from));
                    break;//zp 跳出函数
                case 6://召唤宠物

                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    CommandSystem.Handle(from, string.Format("[getpet"));
                    if (from.HasGump(typeof(ShowToolpet)))
                        from.CloseGump(typeof(ShowToolpet));
                    else
                        from.SendGump(new ShowToolpet(from));
                    break;

                case 7://changeai
                       //                  from.CloseGump(typeof(ShowToolpet));
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    CommandSystem.Handle(from, string.Format("[p"));
                    //               from.SendGump(new ShowToolpet(from));
                    break;

                case 8:
                    CommandSystem.Handle(from, string.Format("[rec"));
                    CommandSystem.Handle(from, string.Format("[ColorPicker"));
                    //       from.CloseGump(typeof(NewsGump));
                    from.SendGump(new ShowToolMain(from));
                    //      from.DoSpeech("[motd", numArray, 0, 20);
                    break;

                case 9://pet
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    if (from.HasGump(typeof(ShowToolpet)))
                        from.CloseGump(typeof(ShowToolpet));
                    else
                        from.SendGump(new ShowToolpet(from));
                    //from.DoSpeech(" ", numArray1, 0, 20);

                    break;

                case 10://news 更新
                    from.SendGump(new ShowToolMain(from));
                    if (from.HasGump(typeof(PnewsGump)))
                        from.CloseGump(typeof(PnewsGump));
                    else
                        from.SendGump(new PnewsGump());
                    //from.DoSpeech(" ", numArray, 0, 20);
                    break;

                case 11://use item in backpack.
                    from.SendGump(new ShowToolMain(from));
                    if (!from.Alive) return;
                    if (from.HasGump(typeof(UseToolItemsGump)))
                        from.CloseGump(typeof(UseToolItemsGump));
                    else
                        from.SendGump(new UseToolItemsGump(UseToolItemsGump.FindToolItems(from)));
                    //from.SendGump(new Opencorpp(Opencorpp.itemlist(Items.Corpse corp));
                    break;
                case 12:
                    //呱呱的作业
                    from.SendGump(new ShowToolMain(from));
                    if (from.HasGump(typeof(guagua)))
                        from.CloseGump(typeof(guagua));
                    else
                        from.SendGump(new guagua());

                    break;
                case 111://test gump


                    //from.SendGump(new totGump(from,new ()));
                    from.SendGump(new goldenskullGump(from));
                    //from.SendGump(new Server.Custom.CG(1,1));
                    from.SendGump(new ShowToolMain(from));

                    if (from.FindItemOnLayer(Layer.FirstValid) != null)
                        from.SendGump(new totGump(from.FindItemOnLayer(Layer.FirstValid)));



                    break;

                default:
                    break;
            }
        }
    }
}

