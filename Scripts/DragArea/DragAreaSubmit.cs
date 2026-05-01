using Godot;
using System;

public partial class DragAreaSubmit : DragArea
{
    [Export] GameManager GameManager;

    public override IDraggable GetDraggable()
    {
        return null;
    }

    public bool TrySubmit(IDraggable draggable)
    {
        if (draggable is DraggableCup)
        {
            var cup = draggable as DraggableCup;
            if (!cup.HasContent)
            {
                GD.Print("Cannot submit empty cup!");
                return false;
            }

            GD.Print($"Submitting cup with content: {cup.LiquidContent.DisplayName}");
            GameManager.Serve(cup.LiquidContent);
            return true;
        }
        if (draggable is DraggablePlate)
        {
            var plate = draggable as DraggablePlate;
            if (!plate.DragArea.HasLeaf())
            {
                GD.Print("Cannot submit empty plate!");
                return false;
            }

            var leaf = plate.DragArea.GetLeaf().GetLeafContent();
            GD.Print($"Submitting plate with leaf: {leaf.DisplayName}");
            GameManager.Serve(leaf);
            return true;
        }
        GD.Print("Unknown draggable type submitted. Rejecting.");
        return false;
    }
}