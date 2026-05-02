using Godot;
using System;

public partial class DragAreaTrash : DragArea
{
    [Export] AnimationPlayer animationPlayer;

    public override IDraggable GetDraggable()
    {
        return null;
    }

    public void OnTrash()
    {
        animationPlayer.Play("trash");
        SoundManager.Play(SFXType.Trashbin);
    }

    public override bool CanDrag()
    {
        return false;
    }
}
