using System;
using System.IO;
using ApophisSoftware.LuaObjects;
using Godot;

namespace ApophisSoftware {
	public partial class LuaController : Node {
		private static LuaApi   lua = new LuaApi();
		private        Callable print;

		public LuaController() {
			InitializeThis();
		}

		private void InitializeThis() {
			//lua.ObjectMetatable = MetaTable;

			// define libraries that the lua code has access to. 
			Godot.Collections.Array libraries = new() {
				"base",  // Base Lua commands
				"table", // Table functionality.
				"string" // String Specific functionality.
			};
			lua.BindLibraries(libraries);

			//override the built in lua 'print' function to use the logging.
			print = new Callable(this, MethodName.LuaPrint);
			lua.PushVariant("print", print);

			Logging.Log("info", "Initializing Game API.");
			// try to register the api for the game.
			RegisterAPI();

			// test out the api.
			Godot.Collections.Array Params = new Godot.Collections.Array();
			Params.Add("system");
			Params.Add("Lua System Activated.");
			// var ApiFunc = lua.PullVariant("mclpp.log");
			// We use .CallFunction to actually call the lua function within the Lua State.
			var x = lua.CallFunction("mclpp.log", Params);
			if (TestForError(x))
				Logging.Log("Error found on API Test Call.");
		}

		internal bool TestForError(Variant x) {
			bool isError = false;

			try {
				if (x.Obj.GetType() == typeof(LuaError)) {
					var z = (LuaError) x;
					isError = true;
					if (z.Message != "") {
						Logging.Log("error", "LUA Runtime Error Catch.");
						Logging.Log("error", "ERROR " + z.Type + ": " + z.Message);
					}
				}
			} catch (Exception e) {
				return false;
			}

			return isError;
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

		internal bool IsInPath(string RelativePath) {
			string basePath = Utils.GetStoragePath();
			string fullPath = Path.Combine(basePath, RelativePath);

			// Get the relative path from the base path to the full path
			string relative = Path.GetRelativePath(basePath, fullPath);

			// Check if the relative path is not ".." or starts with "..\"
			bool isInSubdirectory = !relative.Equals("..") && !relative.StartsWith("..\\");

			return isInSubdirectory;
		}
	}
}