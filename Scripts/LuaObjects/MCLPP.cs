#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Godot;
using Godot.Collections;

#endregion

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
	#region ctor

	public MCLPP() {
	}

	#endregion

	private bool DEBUG = true;

	private System.Collections.Generic.Dictionary<string, Coroutine> ABMs = new();

	private System.Collections.Generic.Dictionary<string, lbm> LBMs = new();


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

	public void debug(string text) {
		Logging.Log(text);
	}

	public string get_modpath(string module_name) {
		if (SCC.Instance.ModPaths.ContainsKey(module_name) == false) return null;

		return SCC.Instance.ModPaths[module_name];
	}

	public string get_current_modname() {
		return "";
	}

	/*
	 *    label = "Lava cooling",
	   -- Descriptive label for profiling purposes (optional).
	   -- Definitions with identical labels will be listed as one.

	   nodenames = {"default:lava_source"},
	   -- Apply `action` function to these nodes.
	   -- `group:groupname` can also be used here.

	   neighbors = {"default:water_source", "default:water_flowing"},
	   -- Only apply `action` to nodes that have one of, or any
	   -- combination of, these neighbors.
	   -- If left out or empty, any neighbor will do.
	   -- `group:groupname` can also be used here.

	   interval = 10.0,
	   -- Operation interval in seconds

	   chance = 50,
	   -- Chance of triggering `action` per-node per-interval is 1.0 / chance

	   min_y = -32768,
	   max_y = 32767,
	   -- min and max height levels where ABM will be processed (inclusive)
	   -- can be used to reduce CPU usage

	   catch_up = true,
	   -- If true, catch-up behavior is enabled: The `chance` value is
	   -- temporarily reduced when returning to an area to simulate time lost
	   -- by the area being unattended. Note that the `chance` value can often
	   -- be reduced to 1.

	   action = function(pos, node, active_object_count, active_object_count_wider),
	   -- Function triggered for each qualifying node.
	   -- `active_object_count` is number of active objects in the node's
	   -- mapblock.
	   -- `active_object_count_wider` is number of active objects in the node's
	   -- mapblock plus all 26 neighboring mapblocks. If any neighboring
	   -- mapblocks are unloaded an estimate is calculated for them based on
	   -- loaded mapblocks.
	 */
	public void register_abm(Godot.Collections.Dictionary<string, Variant> table) {
		string label;
		string[] nodenames;
		double interval = 1.0d;
		int chance = 1;
		LuaFunctionRef action;
		int min_y = -32768;
		int max_y = 32768;
		bool catchup = true;
		string[] neighbors = {"default:water_source", "default:water_flowing"};

		Array<Variant> _params = new();

		try {
			label = (string) table["label"];
			nodenames = (string[]) table["nodenames"];
			interval = (double) table["interval"];
			chance = (int) table["chance"];
			action = (LuaFunctionRef) table["action"];
		} catch (Exception e) {
			Logging.Log("error", e);
			return;
		}

		if (table.ContainsKey("min_y")) min_y = (int) table["min_y"];

		if (table.ContainsKey("max_y")) max_y = (int) table["max_y"];

		if (table.ContainsKey("catchup")) catchup = (bool) table["catchup"];

		if (table.ContainsKey("neighbors")) neighbors = (string[]) table["neighbors"];

		// main parameters needed for the coroutine.
		_params.Add(interval);
		_params.Add(chance);
		_params.Add(action);
		_params.Add(label);

		_params.Add(min_y);
		_params.Add(max_y);
		_params.Add(catchup);
		_params.Add(neighbors);

		if (DEBUG) {
			StringBuilder sbLog = new StringBuilder();
			sbLog.AppendLine("Register ABM Called.");
			sbLog.Append("label: ");
			sbLog.AppendLine(label);
			sbLog.Append("nodename: ");
			foreach (string nodename in nodenames) sbLog.AppendLine(nodename);

			sbLog.Append("interval: ");
			sbLog.AppendLine(interval.ToString());
			sbLog.Append("chance: ");
			sbLog.AppendLine(chance.ToString());
			sbLog.Append("action: ");
			sbLog.AppendLine(action.ToString());

			Logging.Log(sbLog.ToString());
		}

		// ---- Create the coroutine for the abm.
		if (!ABMs.ContainsKey(label)) {
			Coroutine x = CoroutineManager.Instance.StartCoroutine(ABMCoRoutine(_params), CleanUpABM);
			ABMs.Add(label, x);
		} else {
			Logging.Log("warning", "Lua Code tried to create an already defined ABM. Name: " + label);
		}
	}

	internal void CleanUpABM() {
		List<string> removeme = new List<string>();

		// scan and remove completed coroutines, so that they can be GC'd.
		foreach (var abm in ABMs)
			if (abm.Value.IsRunning == false)
				removeme.Add(abm.Key);
		foreach (var remove in removeme) ABMs.Remove(remove);

		GC.Collect(); // finally, take out the trash.
	}

	public void register_alias(string item, string alias) {
	}

	/*
	 * label = "Upgrade legacy doors",
	   -- Descriptive label for profiling purposes (optional).
	   -- Definitions with identical labels will be listed as one.

	   name = "modname:replace_legacy_door",
	   -- Identifier of the LBM, should follow the modname:<whatever> convention

	   nodenames = {"default:lava_source"},
	   -- List of node names to trigger the LBM on.
	   -- Names of non-registered nodes and groups (as group:groupname)
	   -- will work as well.

	   run_at_every_load = false,
	   -- Whether to run the LBM's action every time a block gets activated,
	   -- and not only the first time the block gets activated after the LBM
	   -- was introduced.

	   action = function(pos, node, dtime_s),
	   -- Function triggered for each qualifying node.
	   -- `dtime_s` is the in-game time (in seconds) elapsed since the block
	   -- was last active
	 */
	public void register_lbm(Godot.Collections.Dictionary<string, Variant> table) {
		string label;
		string[] nodenames;
		string name;
		bool RunAtEveryLoad;
		LuaFunctionRef action = new LuaFunctionRef();

		label = (string) table["label"];
		nodenames = (string[]) table["nodenames"];
		name = (string) table["name"];
		RunAtEveryLoad = (bool) table["run_at_every_load"];
		action = (LuaFunctionRef) table["action"];

		lbm LBM = new lbm();
		LBM.RunAtEveryLoad = RunAtEveryLoad;
		LBM.name = name;
		LBM.action = action;
		LBM.label = label;
		LBM.nodenames = nodenames;

		if (!LBMs.TryAdd(name, LBM)) {
			Logging.Log("warning", "Couldn't add in additional LBM: " + name);
		} else {
			if (DEBUG) {
				Logging.Log("system", "LBM Added: " + name);
			}
		}
	}

	internal IEnumerator ABMCoRoutine(Array<Variant> _params) {
		Random rng = new Random(DateTime.UtcNow.Millisecond);

		if (DEBUG) {
			Logging.Log(_params.ToString());
		}

		double Interval = (double) _params[0];
		int chance = (int) _params[1];
		LuaFunctionRef action = (LuaFunctionRef) _params[2];
		string label = (string) _params[3];

		WaitForSeconds delay = new WaitForSeconds((float) Interval);

		// action = function(pos, node, active_object_count, active_object_count_wider)

		Vector3 pos = new Vector3(0, 0, 0);
		NodeBlock node = new NodeBlock();
		int ActiveObjectCount = 0;
		int ActiveObjectCountWider = 0;

		Godot.Collections.Array Params = new Godot.Collections.Array();

		Params.Add(pos);
		Params.Add(node);
		Params.Add(ActiveObjectCount); // number of Entities / Objects.
		Params.Add(ActiveObjectCountWider);

		while (true) {
			int val = rng.Next(1, chance);

			if (val == 1) {
				var error = action.Invoke(Params);
				if (Utils.TestForError(error))
					Logging.Log("Error", "Error executing ABM function callback: " + label);
			}

			yield return delay;
		}
	}
}

internal struct lbm {
	public string         label;
	public string[]       nodenames;
	public string         name;
	public bool           RunAtEveryLoad;
	public LuaFunctionRef action = null;

	public lbm() {
		label = "";
		nodenames = new[] {""};
		name = "";
		RunAtEveryLoad = false;
		action = null;
	}
}