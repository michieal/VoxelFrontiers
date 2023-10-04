using Godot;
using System;
using MoonSharp.Interpreter;
using Script = MoonSharp.Interpreter.Script;

namespace ApophisSoftware {

	public partial class LuaController : Node {
		// private LuaAPI LuaCont = new LuaAPI();

		private static Script msLua            = new Script();
		private        Table  msLuaGlobalTable = new Table(msLua);

		public LuaController() {
			msLua.Options.DebugPrint = str => { Logging.Log("[LUA]: " + str); };
		}

		internal void Dofile(string filename) {
			msLua.DoFile(filename, msLuaGlobalTable);
		}


	}
}