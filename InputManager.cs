using Godot;
using System;

public partial class InputManager : Node
{
    public static InputManager Instance { get; private set; }

    public enum InputState
    {
        None,
        MouseDown,
        Dragging,
    }

    InputState inputState = InputState.None;
    DragArea lastClickArea;
    DragArea currentHoverArea;

    Vector2 lastClickPosition;
    [Export] float dragThreshold = 10f;

    public IDraggable currentDragItem { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr("Multiple instances of InputManager detected!");
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void OnAreaEntered(DragArea area)
    {
        currentHoverArea = area;
    }

    public void OnAreaExited(DragArea area)
    {
        if (currentHoverArea == area)
        {
            currentHoverArea = null;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                lastClickPosition = mouseEvent.Position;
                lastClickArea = currentHoverArea;
                inputState = InputState.MouseDown;
            }
            else
            {
                if (inputState == InputState.Dragging)
                {
                    OnDragEnd(currentHoverArea);
                }
                else if (inputState == InputState.MouseDown)
                {
                    OnClick(lastClickArea);
                }
                inputState = InputState.None;
            }
        }
        else if (@event is InputEventMouseMotion motionEvent)
        {
            if (inputState == InputState.MouseDown)
            {
                if (motionEvent.Position.DistanceTo(lastClickPosition) > dragThreshold)
                {
                    inputState = InputState.Dragging;
                    OnDragStart(lastClickArea);
                }
            }
        }
    }

    void OnClick(DragArea area)
    {
        if (currentDragItem != null)
        {
            currentDragItem.OnDrop(area);
            currentDragItem = null;
            return;
        }

        var draggable = area?.GetDraggable();
        if (draggable != null)
        {
            currentDragItem = draggable;
            currentDragItem.OnPick();
        }
    }

    void OnDragStart(DragArea area)
    {
        if (area == null)
        {
            return;
        }

        if (currentDragItem != null)
        {
            currentDragItem.OnCancelDrag();
            currentDragItem = null;
        }

        var draggable = area?.GetDraggable();
        if (draggable != null)
        {
            currentDragItem = draggable;
            currentDragItem.OnPick();
        }
    }

    void OnDragEnd(DragArea area)
    {
        if (currentDragItem != null)
        {
            currentDragItem.OnDrop(area);
            currentDragItem = null;
        }
    }
}
