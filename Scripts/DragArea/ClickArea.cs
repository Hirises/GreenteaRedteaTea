using Godot;
using System;

public partial class ClickArea : Area2D
{
    [Export] HoverHighlightable hoverHighlight;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (hoverHighlight == null) return;

        if (InputManager.Instance.CurrentHoverClickArea == this &&
            InputManager.Instance.inputState == InputManager.InputState.None &&
            Visible)
        {
            hoverHighlight.SetHover();
        }
    }

    public void OnArea2DEntered()
    {
        InputManager.Instance?.OnClickAreaEntered(this);
    }

    public void OnArea2DExited()
    {
        InputManager.Instance?.OnClickAreaExited(this);
    }
    
    public void SetHoverHighlight(HoverHighlightable highlight)
    {
        hoverHighlight = highlight;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        InputManager.Instance.OnClickAreaExited(this);
    }

    public virtual void OnClick()
    {
        GD.Print($"Clicked on {Name}!");
    }

    public virtual bool CanClick()
    {
        return true;
    }
}
