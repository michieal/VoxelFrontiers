using Godot;

namespace ApophisSoftware.LuaObjects;

public partial class MCLPP : RefCounted {
	public MCLPP() {
	}

	public void log(string text) {
		Logging.Log(text);

		/*
		minetest.debug(...)
		Equivalent to minetest.log(table.concat({...}, "\t"))
		minetest.log([level,] text)
		level is one of "none", "error", "warning", "action", "info", or "verbose". Default is "none".
		*/
	}

	public void log(string level, string text) {
		if (level.ToLower() == "none") {
			log(text);
			return;
		}

		Logging.Log(level, text);
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