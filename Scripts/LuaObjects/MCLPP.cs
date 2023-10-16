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
	#region CTOR

	private static MCLPP instance;

	public static MCLPP Instance {
		get {
			if (instance == null) instance = new MCLPP();

			return instance;
		}
	}

	private MCLPP() {
		// Private constructor to prevent external instantiation
	}

	#endregion

	public int LIGHT_MAX = 15;

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
		//TODO: Search Nodes, Items, Tools for the correct entry, then duplicate said entry with the alias' name.
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
			if (DEBUG) Logging.Log("system", "LBM Added: " + name);
		}
	}

	/*
	 * Item definition

	   Used by minetest.register_node, minetest.register_craftitem, and minetest.register_tool.

	   {
	   description = "",
	   -- Can contain new lines. "\n" has to be used as new line character.
	   -- See also: `get_description` in [`ItemStack`]

	   short_description = "",
	   -- Must not contain new lines.
	   -- Defaults to nil.
	   -- Use an [`ItemStack`] to get the short description, e.g.:
	   --   ItemStack(itemname):get_short_description()

	   groups = {},
	   -- key = name, value = rating; rating = <number>.
	   -- If rating not applicable, use 1.
	   -- e.g. {wool = 1, fluffy = 3}
	   --      {soil = 2, outerspace = 1, crumbly = 1}
	   --      {bendy = 2, snappy = 1},
	   --      {hard = 1, metal = 1, spikes = 1}

	   inventory_image = "",
	   -- Texture shown in the inventory GUI
	   -- Defaults to a 3D rendering of the node if left empty.

	   inventory_overlay = "",
	   -- An overlay texture which is not affected by colorization

	   wield_image = "",
	   -- Texture shown when item is held in hand
	   -- Defaults to a 3D rendering of the node if left empty.

	   wield_overlay = "",
	   -- Like inventory_overlay but only used in the same situation as wield_image

	   wield_scale = {x = 1, y = 1, z = 1},
	   -- Scale for the item when held in hand

	   palette = "",
	   -- An image file containing the palette of a node.
	   -- You can set the currently used color as the "palette_index" field of
	   -- the item stack metadata.
	   -- The palette is always stretched to fit indices between 0 and 255, to
	   -- ensure compatibility with "colorfacedir" (and similar) nodes.

	   color = "#ffffffff",
	   -- Color the item is colorized with. The palette overrides this.

	   stack_max = 99,
	   -- Maximum amount of items that can be in a single stack.
	   -- The default can be changed by the setting `default_stack_max`

	   range = 4.0,
	   -- Range of node and object pointing that is possible with this item held

	   liquids_pointable = false,
	   -- If true, item can point to all liquid nodes (`liquidtype ~= "none"`),
	   -- even those for which `pointable = false`

	   light_source = 0,
	   -- When used for nodes: Defines amount of light emitted by node.
	   -- Otherwise: Defines texture glow when viewed as a dropped item
	   -- To set the maximum (14), use the value 'minetest.LIGHT_MAX'.
	   -- A value outside the range 0 to minetest.LIGHT_MAX causes undefined
	   -- behavior.

	   -- See "Tool Capabilities" section for an example including explanation
	   tool_capabilities = {
	   full_punch_interval = 1.0,
	   max_drop_level = 0,
	   groupcaps = {
	   -- For example:
	   choppy = {times = {2.50, 1.40, 1.00}, uses = 20, maxlevel = 2},
	   },
	   damage_groups = {groupname = damage},
	   -- Damage values must be between -32768 and 32767 (2^15)

	   punch_attack_uses = nil,
	   -- Amount of uses this tool has for attacking players and entities
	   -- by punching them (0 = infinite uses).
	   -- For compatibility, this is automatically set from the first
	   -- suitable groupcap using the formula "uses * 3^(maxlevel - 1)".
	   -- It is recommend to set this explicitly instead of relying on the
	   -- fallback behavior.
	   },

	   node_placement_prediction = nil,
	   -- If nil and item is node, prediction is made automatically.
	   -- If nil and item is not a node, no prediction is made.
	   -- If "" and item is anything, no prediction is made.
	   -- Otherwise should be name of node which the client immediately places
	   -- on ground when the player places the item. Server will always update
	   -- with actual result shortly.

	   node_dig_prediction = "air",
	   -- if "", no prediction is made.
	   -- if "air", node is removed.
	   -- Otherwise should be name of node which the client immediately places
	   -- upon digging. Server will always update with actual result shortly.

	   sound = {
	   -- Definition of item sounds to be played at various events.
	   -- All fields in this table are optional.

	   breaks = <SimpleSoundSpec>,
	   -- When tool breaks due to wear. Ignored for non-tools

	   eat = <SimpleSoundSpec>,
	   -- When item is eaten with `minetest.do_item_eat`

	   punch_use = <SimpleSoundSpec>,
	   -- When item is used with the 'punch/mine' key pointing at a node or entity

	   punch_use_air = <SimpleSoundSpec>,
	   -- When item is used with the 'punch/mine' key pointing at nothing (air)
	   },

	   on_place = function(itemstack, placer, pointed_thing),
	   -- When the 'place' key was pressed with the item in hand
	   -- and a node was pointed at.
	   -- Shall place item and return the leftover itemstack
	   -- or nil to not modify the inventory.
	   -- The placer may be any ObjectRef or nil.
	   -- default: minetest.item_place

	   on_secondary_use = function(itemstack, user, pointed_thing),
	   -- Same as on_place but called when not pointing at a node.
	   -- Function must return either nil if inventory shall not be modified,
	   -- or an itemstack to replace the original itemstack.
	   -- The user may be any ObjectRef or nil.
	   -- default: nil

	   on_drop = function(itemstack, dropper, pos),
	   -- Shall drop item and return the leftover itemstack.
	   -- The dropper may be any ObjectRef or nil.
	   -- default: minetest.item_drop

	   on_pickup = function(itemstack, picker, pointed_thing, time_from_last_punch, ...),
	   -- Called when a dropped item is punched by a player.
	   -- Shall pick-up the item and return the leftover itemstack or nil to not
	   -- modify the dropped item.
	   -- Parameters:
	   -- * `itemstack`: The `ItemStack` to be picked up.
	   -- * `picker`: Any `ObjectRef` or `nil`.
	   -- * `pointed_thing` (optional): The dropped item (a `"__builtin:item"`
	   --   luaentity) as `type="object"` `pointed_thing`.
	   -- * `time_from_last_punch, ...` (optional): Other parameters from
	   --   `luaentity:on_punch`.
	   -- default: `minetest.item_pickup`

	   on_use = function(itemstack, user, pointed_thing),
	   -- default: nil
	   -- When user pressed the 'punch/mine' key with the item in hand.
	   -- Function must return either nil if inventory shall not be modified,
	   -- or an itemstack to replace the original itemstack.
	   -- e.g. itemstack:take_item(); return itemstack
	   -- Otherwise, the function is free to do what it wants.
	   -- The user may be any ObjectRef or nil.
	   -- The default functions handle regular use cases.

	   after_use = function(itemstack, user, node, digparams),
	   -- default: nil
	   -- If defined, should return an itemstack and will be called instead of
	   -- wearing out the item (if tool). If returns nil, does nothing.
	   -- If after_use doesn't exist, it is the same as:
	   --   function(itemstack, user, node, digparams)
	   --     itemstack:add_wear(digparams.wear)
	   --     return itemstack
	   --   end
	   -- The user may be any ObjectRef or nil.

	   _custom_field = whatever,
	   -- Add your own custom fields. By convention, all custom field names
	   -- should start with `_` to avoid naming collisions with future engine
	   -- usage.
	   }

	 */
	public void register_node(Variant table) {
	}


	internal IEnumerator ABMCoRoutine(Array<Variant> _params) {
		Random rng = new Random(DateTime.UtcNow.Millisecond);

		if (DEBUG) Logging.Log(_params.ToString());

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