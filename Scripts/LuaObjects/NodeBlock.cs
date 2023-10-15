#region

using Godot;

#endregion

namespace ApophisSoftware.LuaObjects;

public partial class NodeBlock : RefCounted {
	public string   description;
	public string[] tiles;
	public string   drawtype  = "nodebox";
	public string   paramtype = "light";

	public Godot.Collections.Dictionary<string, int>
		groups; // groups = {handy = 1, axey = 1, choppy = 1, dig_by_piston = 1, plant = 1, non_mycelium_plant = 1, flammable = 3},

	public string sounds;

	public string inventory_image = "mcl_bamboo_bamboo_shoot.png";
	public string wield_image     = "mcl_bamboo_bamboo_shoot.png";

	public BlockBox node_box;
	public BlockBox collision_box;
	public BlockBox selection_box;
}