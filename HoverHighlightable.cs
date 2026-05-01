using Godot;
using System;

public partial class HoverHighlightable : Node2D
{
    protected bool hovering = false;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (hovering)
        {
            Scale = Scale.Lerp(new Vector2(1.1f, 1.1f), 20f * (float)delta);
        }
        else
        {
            Scale = Scale.Lerp(new Vector2(1f, 1f), 20f * (float)delta);
        }

        hovering = false;
    }

    public void SetHover()
    {
        hovering = true;
    }
}
