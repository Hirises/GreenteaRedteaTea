using Godot;
using System;

public partial class Cursor : Node2D
{
    [Export] Sprite2D defaultSpr;
    [Export] Sprite2D draggableSpr;
    [Export] Sprite2D draggingSpr;
    [Export] Sprite2D clickableSpr;
    [Export] Sprite2D kickSpr;

    public override void _Ready()
    {
        base._Ready();

        Input.MouseMode = Input.MouseModeEnum.Hidden;

        SetCursor(defaultSpr);
    }

    void SetCursor(Sprite2D current)
    {
        defaultSpr.Visible = false;
        draggableSpr.Visible = false;
        draggingSpr.Visible = false;
        clickableSpr.Visible = false;
        kickSpr.Visible = false;

        current.Visible = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Position = GetGlobalMousePosition();

        var draggable = InputManager.Instance.currentDragItem;
        if (draggable != null)
        {
            SetCursor(draggingSpr);
            return;
        }

        var clickArea = InputManager.Instance.CurrentHoverClickArea;
        if (clickArea != null && clickArea.CanClick())
        {
            if (clickArea is ClickAreaKick)
                SetCursor(kickSpr);
            else
                SetCursor(clickableSpr);
            return;
        }
        
        var dragArea = InputManager.Instance.currentHoverArea;
        if (dragArea != null && dragArea.CanDrag())
        {
            SetCursor(draggableSpr);
            return;
        }

        SetCursor(defaultSpr);
    }
}