using System.Text;
using Godot;

#region License / Copyright

/*
 * Copyright © 2023, Michieal.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

namespace ApophisSoftware.LuaObjects {
	public partial class ItemStack : RefCounted {
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

		public ItemStack() {
		}

		public ItemStack(string identifier, int amount) {
			ItemName = identifier;
			Amount = amount;
		}
	}
}