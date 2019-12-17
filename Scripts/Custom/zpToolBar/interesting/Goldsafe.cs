/**
 * @author PyrO
 * @version 1.0.0
 * @summary Goldsafe for people to carry around with Weightmanipulation of the Gold
 * @comment have fun with it
 **/

using System;

namespace Server.Items
{
	public class Goldsafe : Container
	{
		public override string DefaultName { get { return "Gold Safe"; } }     /// Default Name for the Bag / Safe
		public override double DefaultWeight { get { return 40; } }            /// Default weight of the Bag / Safe itself
		public override bool DisplayWeight { get { return true; } }            /// Setting to say if it should display the Weight
		public override bool DisplaysContent { get { return false; } }         /// Setting to say if it should display the Item count

		private int maxHeldAmount = -1;                                        /// anything below 0 sets it back to the default, you shouldnt edit this
		public static int defaultHeldMaxAmount = 100000;                       /// the Gold Safe can hold up to 100,000 Gold as the default

		private float goldWeightMultiplier = -1f;                              /// anything below 0 sets it back to the default, you shouldnt edit this
		public static float defaultGoldWeightMultiplier = 0.8f;                /// 80% of the weight remain as default value

		[CommandProperty(AccessLevel.Counselor, true)]                         /// Just a nice way to maybe filter for certain bags, read only
		public int GoldCount { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]                              /// The Property to show and edit the maximum allowed Gold in this Bag / Safe
		public int MaxHeldAmount { get { return maxHeldAmount >= 0 ? maxHeldAmount : defaultHeldMaxAmount; } set { maxHeldAmount = value; } }

		[CommandProperty(AccessLevel.GameMaster)]                              /// The Property to show and edit the Gold weight multiplicator in this Bag / Safe
		public float GoldWeightMultiplier
		{
			get { return goldWeightMultiplier >= 0 ? goldWeightMultiplier : defaultGoldWeightMultiplier; }
			set
			{
				int oldWeight = TotalWeight;
				goldWeightMultiplier = value;
				InvalidateProperties();
				if (Parent is Container)
				{
					((Container)Parent).UpdateTotal(this, TotalType.Weight, TotalWeight - oldWeight);
				}

			}
		}

		[Constructable]
		public Goldsafe() : base(0xE76)
		{
			LootType = LootType.Blessed;
		}

		public Goldsafe(Serial serial) : base(serial)
		{ }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1114057, "Gold: " + GoldCount + " / " + MaxHeldAmount); // ~1_val~
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((byte)0);

			writer.Write(maxHeldAmount);
			writer.Write(goldWeightMultiplier);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadByte();

			maxHeldAmount = reader.ReadInt();
			goldWeightMultiplier = reader.ReadFloat();
		}

		private bool ItemAddIntoCheck(Mobile from, Item item)
		{
			if (item.GetType() != typeof(Gold))
			{
				from.SendMessage("Only Gold may be put into this " + DefaultName);
				return false;
			}
			if (GoldCount >= MaxHeldAmount)
			{
				from.SendMessage("Your " + DefaultName + " is already full!");
				return false;
			}
			if (GoldCount + item.Amount > MaxHeldAmount)
			{
				from.SendMessage("You fill your " + DefaultName + " until it is about to burst!");
				SplitItem<Gold>((Gold)item, MaxHeldAmount - GoldCount);

				UpdateTotals();
			}
			return true;
		}

		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
		{
			return ItemAddIntoCheck(m, item) && base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
		}

		public static T SplitItem<T>(T i, int amount) where T : Item
		{
			T item = Activator.CreateInstance<T>();

			if (item != null)
			{
				BounceInfo bi = i.GetBounce();
				if (bi.m_Parent is Container)
				{
					((Container)bi.m_Parent).AddItem(item);
				}

				item.Location = bi.m_Location;
				item.Map = bi.m_Map;
				item.Amount = i.Amount - amount;
				i.Amount = amount;
			}
			return item;
		}

		public void CountGold()
		{
			GoldCount = GetTotal(TotalType.Gold);
			InvalidateProperties();
		}

		public override int GetTotal(TotalType type)
		{
			int total = base.GetTotal(type);

			if (type == TotalType.Weight)
				total = (int)(total * GoldWeightMultiplier);

			return total;
		}

		public override void UpdateTotal(Item sender, TotalType type, int delta)
		{
			base.UpdateTotal(sender, type, delta);
			CountGold();
		}

		public override void UpdateTotals()
		{
			base.UpdateTotals();
			CountGold();
		}
	}
}