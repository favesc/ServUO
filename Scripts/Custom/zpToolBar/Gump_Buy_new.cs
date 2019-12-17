using System;
using Server;
using Server.Network;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class BuyGump : Gump
    {
        static int unbind = Config.Get("zp.解除绑定契约", 50000);
        static int viking = Config.Get("zp.viking", 50000);
        static int vstaff = Config.Get("zp.vstaff", 50000);
        static int autores = Config.Get("zp.重生药水", 50000);
        static int vradian = Config.Get("zp.vradian", 50000);
        static int shortbow = Config.Get("zp.shortbow", 50000);
        static int bankcardprice = Config.Get("zp.移动银行", 50000);
        static int belt = Config.Get("zp.忍者腰带", 50000);
        static int trashbagp = Config.Get("zp.trashbagp", 50000);
        static int PetL = Config.Get("zp.宠物皮带", 50000);
        static int Vhalberdprice = Config.Get("zp.vhalberd", 50000);
        static int folloerprice = Config.Get("zp.FollowerOfTheOldLord", 50000);
        static int tessen = Config.Get("zp.tessen", 50000);
        static int travelatlasprice = Config.Get("zp.旅行指南", 5000000);
        static int PowderOfTemperament = Config.Get("zp.PowderOfTemperament", 5000000);
        static int spellchannel = Config.Get("zp.法术通道", 5655);
        static int bonddeed = Config.Get("zp.结盟契约", 3000);
        static int transform = Config.Get("zp.转职契约", 5000);

        const sbyte prev = -1, next = -2, close = -3;
        static byte page;
        static byte i;
        public static void Initialize()
        {
            CommandSystem.Register("buy", AccessLevel.Player, new CommandEventHandler(CommandName_OnCommand));
        }
        [Usage("buy")]
        [Description("Buy Something.")]
        public static void CommandName_OnCommand(CommandEventArgs e)
        {
            PlayerMobile from = e.Mobile as PlayerMobile;

            if (from.HasGump(typeof(BuyGump)))
                from.CloseGump(typeof(BuyGump));
            from.SendGump(new BuyGump(from));
        }


        //zp 2017.10.29 今天刚研究了多维数组和交错数组，练练。下面改成交错数组
        private readonly int[][] price = new int[][] {
            new int [] { autores, PetL, 500,550, transform, spellchannel, bonddeed },
            new int [] { 5000,5001,6000,7000,8888,folloerprice,belt },
            new int [] {1111,2222,3333,4444,5555,bankcardprice,PowderOfTemperament},
            new int [] { trashbagp, travelatlasprice, 7000, 5000 ,3333,3331,2234} ,
            new int [] {300,500,500000,50000},
            new int [] { unbind, viking, vstaff, Vhalberdprice, tessen, vradian, shortbow },
            new int [] { 88880,50000,50000,60000,60000,70000,70000},
            new int [] { 8888,8888,8888,8888,8888,8888,9999}
            };

        private readonly Type[][] onsells = new Type[][] {
            new Type [] {typeof(AutoResPotion), typeof(PetLeash), typeof(RoastPig), typeof(GlassMug), typeof(Transform), typeof(SpellChannelingToken),typeof(Bonddeed)  },
            new Type [] {typeof(ArmoredPlaindress), typeof(ArmoredCloak),typeof(ArmoredRobe),typeof(ArmoredMaleElvenRobe),typeof(ArmoredFemaleElvenRobe),typeof(ArmoredNinjaBelt),typeof(FollowerOfTheOldLord)},
            new Type [] {typeof(GraveRobbersShovel), typeof(GraveDiggersShovel), typeof(HeritageToken),typeof(PersonalAttendantToken),typeof(ElixirOfRebirth),typeof(bankcard),typeof(PowderOfTemperament) },
            new Type [] {typeof(TrashBagP), typeof(TravelAtlas), typeof(MythicCharacterToken), typeof(BookOfMasteries) ,typeof(MysticBook), typeof(SpellweavingBook),typeof(Shovel)},
            new Type [] {typeof(Pitcher),typeof(Engines.Quests.TreasureSeekersLockpick), typeof(SmithHammer),typeof(SewingKit)},
            new Type [] {typeof(UnbindingDeed), typeof(SoulVikingSword), typeof(Vstaff), typeof(vHalberd), typeof(Vtessen), typeof(VRadiantScimitar), typeof(SoulShortBow) },
            new Type [] {typeof(Goldsafe),typeof(AlchemistBook),typeof(GraniteBook), typeof(IngotBook),typeof(LumberBook),typeof(ScaleBook),typeof(TailorBook)},
            new Type []{ typeof(FullMagerySpellbook),typeof(FullNecroSpellbook),typeof(FullChivalrySpellbook),typeof(FullBushidoSpellbook),typeof(FullNinjitsuSpellbook),typeof(FullSpellweavingSpellbook),typeof(FullMysticBook)}
            };

        //例子 交错数组      int[][] scores = new int[2][] { new int[] { 92, 93, 94 }, new int[] { 85, 66, 87, 88 } };
        //int[][] tt =new int [][]  {new int [] { 5, 5, 6, 5 },new int  []{ 1, 2 }, new int[] { 1, 2, 3 } };
        //int[][] ttt = { new int[] { 3 }, new int [] { 3, 5, 6, 7 } };
        //int[] tttt = { 3, 3, 5, 67, 5 };
        //int[,] t = { { 3, 5 }, { 35, 5 }, { 32, 66 } };
        private void writelabel()
        {
            Item it = Activator.CreateInstance(onsells[page][i]) as Item;
            AddLabel(80, i % 7 * 50 + 100, 0, (it.Name == null || it.Name.Length <= 0) ? (TextDefinition)it.GetType().Name : (TextDefinition)it.Name);//name
            AddLabel(80, i % 7 * 50 + 120, 90, "价格： " + price[page][i].ToString());//price
            AddItem(190, i % 7 * 50 + 100, it.ItemID, it.Hue);//image
            //AddItemProperty(it.Serial); //没有效果？
            AddButton(275, i % 7 * 50 + 105, 247, 248, i + 1, GumpButtonType.Reply, 0);//button
            AddItemProperty(it.Serial);
        }
        private void buylabel()
        {
            for (i = 0; i < onsells[page].Length; i++)
            { writelabel(); }
        }


        public BuyGump(PlayerMobile from) : this(from, 0) { }
        public BuyGump(PlayerMobile from, byte p) : base(10, 0x69)
        {
            page = p;
            Closable = true;
            Disposable = false;
            Dragable = true;

            AddPage(0);
            AddBackground(60, 70, 300, 440, 3500);
            AddLabel(80, 460, 50, string.Format("存款: {0}", Banker.GetBalance(from).ToString()));
            AddButton(320, 75, 0xfb2, 0xfb3, close, GumpButtonType.Reply, 0);//button close
            AddPage(1);
            buylabel();//画表了

            if (page > 0)
            {
                AddButton(205, 460, 2443, 2444, prev, GumpButtonType.Reply, 0);
                AddLabel(210, 460, 60, "上一頁");
            }
            if (page < onsells.Length - 1)
            {
                AddButton(275, 460, 2443, 2444, next, GumpButtonType.Reply, 0);
                AddLabel(280, 460, 60, "下一頁");
            }

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile from = sender.Mobile as PlayerMobile;

            for (i = 0; i < onsells[page].Length; i++)
            {
                if (info.ButtonID == i + 1)
                {
                    if (Banker.GetBalance(from) >= price[page][i] && price[page][i] != 0)
                    {
                        var it = Activator.CreateInstance(onsells[page][i]) as Item;


                        Banker.Withdraw(from, price[page][i]);
                        if (it.Name != null)
                            from.SendMessage("购买一个" + it.Name.ToString() + "， 花费了{0}个金币", price[page][i]);
                        else from.SendMessage("购买一个东东" + "， 花费{0}个金币", price[page][i]);
                        if (it is Pitcher)//玻璃水壶
                        {
                            it = new Pitcher(BeverageType.Water);//装的是水
                        }
                        //if (page == 0 && i ==3)
                        else if (it is GlassMug)
                        {
                            it = new Jug(BeverageType.Water);//水壶 10次
                        }
                        else if (it is MysticBook)
                        {
                            ((MysticBook)it).Content = (1ul << ((MysticBook)it).BookCount) - 1;//allspells
                        }
                        else if (it is SpellweavingBook)
                        {
                            ((SpellweavingBook)it).Content = (1ul << ((SpellweavingBook)it).BookCount) - 1;//allspells
                        }
                        else if (it is SewingKit)
                        {
                            it = new RunicSewingKit(CraftResource.BarbedLeather, 15);
                        }
                        else if (it is SmithHammer)
                        {
                            it = new RunicHammer(CraftResource.Valorite, 15);
                        }
                        from.AddToBackpack(it);
                        from.SendSound(it.GetDropSound());
                        from.SendGump(new BuyGump(from, page));
                    }
                    else
                    {
                        from.SendMessage("钱不够了"); break;
                    }
                    break;
                }
            }
            switch (info.ButtonID)
            {
                case prev:
                    {
                        if (page > 0)
                            from.SendGump(new BuyGump(from, --page));
                        break;
                    }
                case next:
                    {
                        if (page < onsells.Length - 1)
                            from.SendGump(new BuyGump(from, ++page));
                        break;
                    }
                default:
                case 0:
                case close:
                    {
                        break;
                    }
            }
        }
    }
}
