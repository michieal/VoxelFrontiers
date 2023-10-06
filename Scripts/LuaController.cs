using Godot;

namespace ApophisSoftware {
	public partial class LuaController : Node {
		private static LuaApi lua       = new LuaApi();
		LuaObjectMetatable    MetaTable = new LuaObjectMetatable();
		private Callable      print;

		public LuaController() {
			lua.ObjectMetatable = MetaTable;
			// var luaPrint = Variant.CreateFrom(LuaPrint());
			// lua.PushVariant("print", luaPrint); // .push_variant("print", _lua_print)

			Godot.Collections.Array libraries = new Godot.Collections.Array();
			libraries.Add("base");
			libraries.Add("table");
			libraries.Add("string");

			lua.BindLibraries(libraries);

			print = new Callable(this, MethodName.LuaPrint);
			lua.PushVariant("print", print); //override the built in lua 'print' function to use the logging.
		}

		private void LuaPrint(string message) {
			Logging.Log("[LUA]: " + message);
		}

		internal void Dofile(string filename) {
			lua.DoFile(filename);
		}

		public override void _Ready() {
		}

		public override void _Process(double delta) {
			base._Process(delta);
		}
	}
}