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

namespace ApophisSoftware.LuaObjects;

public partial class MCLPP : RefCounted {
	public MCLPP() {
	}

	public bool log(string text) {
		Logging.Log(text);
		return true;
		/*
		minetest.debug(...)
		Equivalent to minetest.log(table.concat({...}, "\t"))
		minetest.log([level,] text)
		level is one of "none", "error", "warning", "action", "info", or "verbose". Default is "none".
		*/
	}

	public bool log(string level, string text) {
		if (level.ToLower() == "none") {
			log(text);
			return true;
		}

		Logging.Log(level, text);
		return true;
	}

	public void debbug(string text) {
		Logging.Log(text);
	}

	public string get_modpath(string module_name) {
		if (SCC.Instance.ModPaths.ContainsKey(module_name) == false) {
			return null;
		}

		return SCC.Instance.ModPaths[module_name];
	}

	public string get_current_modname() {
		return "";
	}

	public void register_abm(string label, string nodenames, double interval, int chance, string action) {
		// label = "Bamboo Grow",
		// nodenames = mcl_bamboo.bamboo_index,
		// interval = 10,
		// chance = 20,
		// action = function(pos, _)
		// mcl_bamboo.grow_bamboo(pos, false)
		// end,
	}

	public void register_alias(string item, string alias) {
	}
}