#region

using System.Collections.Generic;
using Godot;

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

public struct GroupCap {
	public string   name;
	public double[] times;
	public int      uses;
	public int      maxlevel;
}

public partial class ToolsCap : RefCounted {
	/*
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
	*/

	public double         full_punch_interval = 1.0d;
	public int            max_drop_level      = 0;
	public List<GroupCap> groupcaps           = new();

	public int? punch_attack_uses {
		get => _punch_attack_uses;
		set => _punch_attack_uses = value;
	}

	private int?                                      _punch_attack_uses = null;
	public  Godot.Collections.Dictionary<string, int> damage_groups { get; set; }
}