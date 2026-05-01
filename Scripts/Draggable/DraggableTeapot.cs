using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableTeapot : Node2D, IDraggable
{
    [Export] Sprite2D liquidTop;
    [Export] Sprite2D liquidBottom;
    Vector2 originalPosition;
    bool hasContent = false;
    ProductExpression liquidContent;

    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            Position = Position.Lerp(GetGlobalMousePosition(), 20f * (float)delta);
            ZIndex = DraggableUtil.DragZIndex; // Ensure the dragged item is on top
        }
        else
        {
            Position = Position.Lerp(originalPosition, 10f * (float)delta);
            ZIndex = 0; // Reset ZIndex when not being dragged
        }

        liquidTop.Visible = hasContent;
        liquidBottom.Visible = hasContent;
        if (hasContent)
        {
            liquidTop.Modulate = liquidContent.Color.ToGodotColor();
            liquidBottom.Modulate = liquidContent.Color.ToGodotColor();
        }
    }

    public override void _Ready()
    {
        originalPosition = Position;
    }

    public void OnPick()
    {
        GD.Print("Teapot picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (hasContent && container.TryFill(liquidContent))
            {
                hasContent = false;
                liquidContent = null;
            }
            else
            {
                GD.Print("Failed to pour teapot into container.");
            }
        }
        GD.Print($"Teapot dropped on {dropArea?.Name}!");
    }

    public void OnCancelDrag()
    {
        GD.Print("Teapot drag cancelled.");
    }

    public bool TryFill(ProductExpression liquid)
    {
        if (!liquid.Is(ProductCategory.Liquid))
        {
            GD.Print("Cannot fill teapot with non-liquid product.");
            return false;
        }
        if (hasContent)
        {
            var mix = new MixedLiquidExpression(liquidContent, liquid);
            liquidContent = mix;
            GD.Print($"Teapot already has content. Mixed to {mix.DisplayName}.");
            return true;
        }
        liquidContent = liquid;
        hasContent = true;
        GD.Print($"Teapot filled with {liquid.DisplayName}.");
        return true;
    }
}
