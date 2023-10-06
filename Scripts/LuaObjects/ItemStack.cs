using System.Text;

namespace ApophisSoftware.LuaObjects {
	public class ItemStack {
		// 
		// Full item identifier ("item name")
		// Optional amount
		// Optional wear value
		// Optional item metadata

		// local stack = ItemStack("default:pick_wood")
		// stack:set_wear(21323)
		// stack:get_meta():set_string("description", "My worn out pick")
		// local itemstring = stack:to_string()

		public  string   ItemName = "";
		public  int      Amount   = 1;
		public  int      Wear     = 0;
		private MetaData metaData = new MetaData();

		public override string ToString() {
			StringBuilder sb = new StringBuilder(ItemName);
			if (Amount != 1) {
				sb.Append(" " + Amount.ToString());
			}

			if (Wear != 0) {
				if (Amount == 1) {
					sb.Append(" " + Amount.ToString());
				}

				sb.Append(" " + Wear.ToString());
			}

			return sb.ToString();
		}

		public MetaData getmeta() {
			return metaData;
		}

		public void setwear(int value) {
			Wear = value;
		}

		public ItemStack(string identifier) {
			ItemName = identifier;
		}

		public ItemStack(string identifier, int amount) {
			ItemName = identifier;
			Amount = amount;
		}
	}
}