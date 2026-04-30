using Godot;
using System;

public partial class DragArea : Area2D
{
    public void OnArea2DEntered()
    {
        InputManager.Instance?.OnAreaEntered(this);
    }

    public void OnArea2DExited()
    {
        InputManager.Instance?.OnAreaExited(this);
    }

    public virtual IDraggable GetDraggable()
    {
        return null;
    }
}
