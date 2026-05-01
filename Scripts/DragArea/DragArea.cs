using Godot;
using System;

public partial class DragArea : Area2D
{
    [Export] HoverHighlightable hoverHighlight;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (hoverHighlight == null) return;

        if (InputManager.Instance.currentHoverArea == this && Visible)
        {
            hoverHighlight.SetHover();
        }
    }

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
    
    public void SetHoverHighlight(HoverHighlightable highlight)
    {
        hoverHighlight = highlight;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        InputManager.Instance.OnAreaExited(this);
    }
}
