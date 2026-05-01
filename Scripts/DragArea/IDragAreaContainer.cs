using Godot;
using RedteaGreenteaTea.Domain;
using System;

public interface IDragAreaContainer
{
    public bool TryDropDraggable(IDraggable draggable);
    public Node2D GetNode();
}