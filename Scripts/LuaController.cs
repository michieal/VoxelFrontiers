using ApophisSoftware.LuaObjects;
using Godot;

namespace ApophisSoftware {
	public partial class LuaController : Node {
		private static LuaApi lua       = new LuaApi();
		LuaObjectMetatable    MetaTable = new LuaObjectMetatable();
		private Callable      print;

		public LuaController() {
			InitializeThis();
		}

		private void InitializeThis() {
			//lua.ObjectMetatable = MetaTable;

			// define libraries that the lua code has access to. 
			Godot.Collections.Array libraries = new Godot.Collections.Array();
			libraries.Add("base");
			libraries.Add("table");
			libraries.Add("string");

			lua.BindLibraries(libraries);

			//override the built in lua 'print' function to use the logging.
			print = new Callable(this, MethodName.LuaPrint);
			lua.PushVariant("print", print);

			Logging.Log("info", "Initializing Game API.");
			// try to register the api for the game.
			RegisterAPI();
		}

		private void LuaPrint(string message) {
			Logging.Log("[LUA]: " + message);
		}

		internal void Dofile(string filename) {
			lua.DoFile(filename);
		}

		internal void CreateGlobalVar(string VariableName, string VariableValue) {
			// Assign a named Lua Variable and give it a value.
			lua.PushVariant(VariableName, VariableValue);
		}

		internal void CreateGlobalVar(string VariableName, GodotObject VariableValue) {
			// Assign a named Lua Variable and give it a value.
			lua.PushVariant(VariableName, VariableValue);
		}

		internal void RegisterAPI() {
			CreateGlobalVar("ItemStack", new ItemStack());
			CreateGlobalVar("minetest", new MCLPP());
			CreateGlobalVar("mclpp", new MCLPP());
		}

		public override void _Ready() {
			Godot.Collections.Array Params = new Godot.Collections.Array();
			Params.Add("warning");
			Params.Add("Lua System Activated.");
			// We use .CallFunction to actually call the lua function within the Lua State.
			lua.CallFunction("mclpp.log", Params);

			LuaError error = lua.DoString(@"
					mclpp.log (""Test Message"")
				");

			if (error != null && error.Message != "") {
				if (error != null && error.Message != "") {
					Logging.Log("error", "An error occurred calling DoString.");
					Logging.Log("error", "ERROR " + error.Type + ": " + error.Message);
				}
			}
		}

		public override void _Process(double delta) {
			base._Process(delta);
		}
	}
}