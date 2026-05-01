using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggablePlate : Node2D, IDraggableContained
{
    [Export] DragAreaPlate dragArea;
    [Export] HoverHighlightable hoverHighlight;
    public DragAreaPlate DragArea => dragArea;
    DragAreaContainer returnArea;

    public override void _Process(double delta)
    {
        base._Process(delta);
        DraggableUtil.DefaultDragBehavior(this, this, delta, returnArea);

        dragArea.Visible = dragArea.HasLeaf() && InputManager.Instance.currentDragItem != this;
        dragArea.ZIndex = ZIndex;
    }

    public void OnPick()
    {
        GD.Print("Plate picked up!");
        SoundManager.Play(SFXType.PlatePick);
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Plate dropped on {dropArea?.Name}!");
        SoundManager.Play(SFXType.PlatePut);
        if (dropArea == dragArea)
        {
            GD.Print("Plate dropped back on its own drag area. Returning to original position.");
            ReturnToOriginalPosition();
            return;
        }
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (container.TryDropDraggable(this))
            {
                returnArea = container;
                GD.Print("Plate successfully dropped into container.");
            }
            else
            {
                GD.Print("Failed to drop plate into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        if (dropArea is DragAreaTrash)
        {
            GD.Print("Plate dropped into trash. Destroying plate.");
            (dropArea as DragAreaTrash).OnTrash();
            Destroy();
            return;
        }
        if (dropArea is DragAreaSubmit)
        {
            var submitArea = dropArea as DragAreaSubmit;
            if (submitArea.TrySubmit(this))
            {
                Destroy();
                GD.Print("Plate successfully submitted.");
            }
            else
            {
                GD.Print("Failed to submit plate. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        ReturnToOriginalPosition();
    }

    public void OnCancelDrag()
    {
        GD.Print("Plate drag cancelled. Returning to original position.");
        ReturnToOriginalPosition();
    }

    void Destroy()
    {
        GD.Print("Plate destroyed.");
        QueueFree();
    }

    void ReturnToOriginalPosition()
    {
        if (returnArea == null)
        {
            GD.Print("No return area set. Destroying plate.");
            Destroy();
            return;
        }
        if (returnArea.TryDropDraggable(this))
        {
            GD.Print("Plate returned to original position.");
        }
        else
        {
            GD.Print("Failed to return plate to original position. Destroying plate.");
            Destroy();
        }
    }

    public bool TryPutOnPlate(DraggableLeaf leaf)
    {
        GD.Print("Putting leaf on plate.");
        return dragArea.TryDropDraggable(leaf);
    }

    public bool TryMergeLeaf(DraggableLeaf leaf)
    {
        GD.Print("Trying to merge leaf with existing leaf on plate.");
        return dragArea.TryMergeLeaf(leaf);
    }

    public HoverHighlightable GetHoverHighlight()
    {
        return hoverHighlight;
    }
}
