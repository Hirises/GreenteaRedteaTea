using Godot;
using System;

public partial class HoverHighlightableTrash : HoverHighlightable
{
    [Export] public Node2D lid;

    public override void _Process(double delta)
    {
        if (hovering)
        {
            lid.RotationDegrees = Mathf.Lerp(lid.RotationDegrees, 15f, 20f * (float)delta);
        }
        else
        {
            lid.RotationDegrees = Mathf.Lerp(lid.RotationDegrees, 0f, 20f * (float)delta);
        }

        base._Process(delta);
    }
}
