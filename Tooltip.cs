using Godot;
using System;

public partial class Tooltip : Node2D
{
    [Export] Label textNode;
    [Export] float tooltipDelaySeconds = 0.3f;

    bool showing = false;
    DragArea currentHoverDragArea = null;
    float hoverTimer = 0f;

    public override void _Ready()
    {
        base._Ready();

        Scale = new Vector2(0, 0); // Start hidden
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (currentHoverDragArea != null && InputManager.Instance.currentHoverArea == currentHoverDragArea)
        {
            hoverTimer += (float)delta;
            if (!showing && hoverTimer >= tooltipDelaySeconds)
            {
                ShowTooltip(currentHoverDragArea);
            }
        }
        else
        {
            currentHoverDragArea = InputManager.Instance.currentHoverArea;
            hoverTimer = 0f;
            if (showing)
            {
                HideTooltip();
            }
        }

        var mousePos = GetGlobalMousePosition();
        Position = mousePos;

        if (showing)
        {
            Scale = Scale.Lerp(new Vector2(1, 1), 10f * (float)delta); // Smoothly scale up when showing
            textNode.Text = currentHoverDragArea.GetTooltipText();
        }
        else
        {
            Scale = Scale.Lerp(new Vector2(0, 0), 10f * (float)delta); // Smoothly scale down when not showing
        }
    }

    public void ShowTooltip(DragArea dragArea)
    {
        showing = true;
        textNode.Text = dragArea.GetTooltipText();
    }

    public void HideTooltip()
    {
        showing = false;
    }
}
