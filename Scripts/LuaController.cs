using Godot;

namespace ApophisSoftware {
	public partial class LuaController : Node {
		private static LuaApi lua       = new LuaApi();
		LuaObjectMetatable    MetaTable = new LuaObjectMetatable();
		private Callable      print;

		public LuaController() {
			lua.ObjectMetatable = MetaTable;

			// define libraries that the lua code has access to. 
			Godot.Collections.Array libraries = new Godot.Collections.Array();
			libraries.Add("base");
			libraries.Add("table");
			libraries.Add("string");

			lua.BindLibraries(libraries);

			//override the built in lua 'print' function to use the logging.
			print = new Callable(this, MethodName.LuaPrint);
			lua.PushVariant("print", print);
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

		public override void _Ready() {
		}

		public override void _Process(double delta) {
			base._Process(delta);
		}
	}
}