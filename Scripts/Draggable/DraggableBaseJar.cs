using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableBaseJar : Node2D, IDraggable
{
    [Export] BaseKind baseKind;
    [Export] Vector2 dragOffset;
    public BaseKind BaseKind => baseKind;
    Vector2 originalPosition;

    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            var targetPosition = GetGlobalMousePosition() + dragOffset;
            Position = Position.Lerp(targetPosition, 20f * (float)delta);
            ZIndex = DraggableUtil.DragZIndex; // Ensure the dragged item is on top
        }
        else
        {
            Position = Position.Lerp(originalPosition, 10f * (float)delta);
            ZIndex = 0; // Reset ZIndex when not being dragged
        }
    }

    public override void _Ready()
    {
        originalPosition = Position;
    }

    public void OnPick()
    {
        GD.Print("BaseJar picked up!");
        SoundManager.Play(SFXType.JarPick);
    }

    public void OnDrop(DragArea dropArea)
    {
		SoundManager.Play(SFXType.JarPut);
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (container.TryFill(new BaseExpression(baseKind)))
                SoundManager.Play(SFXType.JarPour);
        }
        else if (dropArea is DragAreaTeapot)
        {
            var teapotArea = dropArea as DragAreaTeapot;
            if (teapotArea.TryFill(new BaseExpression(baseKind)))
                SoundManager.Play(SFXType.JarPour);
        }
        if (dropArea is DragAreaTeapotInside)
        {
            var teapotArea = dropArea as DragAreaTeapotInside;
            if (teapotArea.TryFillTeapot(new BaseExpression(baseKind)))
                SoundManager.Play(SFXType.JarPour);
        }
        GD.Print($"BaseJar dropped on {dropArea?.Name}!");
    }

    public void OnCancelDrag()
    {
        GD.Print("BaseJar drag cancelled.");
    }
}