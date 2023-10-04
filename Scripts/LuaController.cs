using Godot;
using System;
using MoonSharp.Interpreter;
using Script = MoonSharp.Interpreter.Script;

public partial class LuaController : Node {
	// private LuaAPI LuaCont = new LuaAPI();

	private static Script msLua            = new Script();
	private        Table  msLuaGlobalTable = new Table(msLua);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		
		msLua.DoFile("", msLuaGlobalTable);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}