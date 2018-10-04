using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : Node2D
{
	MoveHandler moveHandler;
	Terrain terrain;
	Node units;

	Unit selectedUnit;
	IList<Vector2> selectedUnitPath = new List<Vector2>();

	public override void _Ready()
	{
		moveHandler = (MoveHandler) GetNode("MoveHandler");
		terrain = (Terrain) GetNode("Terrain");
		units = (Node) GetNode("UnitContainer");

		var sprite = (Texture) ResourceLoader.Load("res://units/sprite.png");
		var shaman = (Texture) ResourceLoader.Load("res://units/master-of-curses.png");
		var tree = (Texture) ResourceLoader.Load("res://units/vengeance.png");
		var fighter = (Texture) ResourceLoader.Load("res://units/elvish-fighter.png");
		
		var unit1 = new Unit();
		var unit2 = new Unit();
		var unit3 = new Unit();
		var unit4 = new Unit();

		unit1.SetTexture(sprite);
		unit1.SetPosition(terrain.MapToWorldCentered(new Vector2(5, 3)));
		
		unit2.SetTexture(fighter);
		unit2.SetPosition(terrain.MapToWorldCentered(new Vector2(5, 12)));

		unit3.SetTexture(shaman);
		unit3.SetPosition(terrain.MapToWorldCentered(new Vector2(15, 3)));

		unit4.SetTexture(tree);
		unit4.SetPosition(terrain.MapToWorldCentered(new Vector2(15, 12)));
		
		units.AddChild(unit1);
		units.AddChild(unit2);
		units.AddChild(unit3);
		units.AddChild(unit4);
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("mouse_left"))
		{
			Vector2 mouseCell = terrain.WorldToMap(GetGlobalMousePosition());
			if (IsUnitAtCell(mouseCell))
			{
				selectedUnit = GetUnitAtCell(mouseCell);
			}

			if (!IsUnitAtCell(mouseCell) && selectedUnit != null)
			{
				Vector2 unitCell = terrain.WorldToMap(selectedUnit.GetPosition());
				
				selectedUnitPath = terrain.FindPathByCell(unitCell, mouseCell);
				
				GD.Print(unitCell, " ", mouseCell, "Count ", selectedUnitPath.Count);
				moveHandler.MoveUnit(selectedUnit, selectedUnitPath);

			}
		}

		if (Input.IsActionJustPressed("mouse_right"))
		{
			selectedUnit = null;
		}
	}

	public bool IsUnitAtCell(Vector2 cell)
	{
		foreach (Unit u in units.GetChildren())
		{
			if (u.GetPosition() == terrain.MapToWorldCentered(cell))
			{
				return true;
			}
		}
		return false;
	}

	public Unit GetUnitAtCell(Vector2 cell)
	{
		foreach (Unit u in units.GetChildren())
		{
			if (u.GetPosition() == terrain.MapToWorldCentered(cell))
			{
				return u;
			}
		}
		return null;
	}

	public Unit GetSelectedUnit() 
	{
		return selectedUnit;
	}

	public IList<Vector2> GetSelectedUnitPath()
	{
		return selectedUnitPath;
	}
}