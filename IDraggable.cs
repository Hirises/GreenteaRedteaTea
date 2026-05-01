using Godot;
using System;

public interface IDraggable
{
    public void OnPick();
    public void OnDrop(DragArea dropArea);
    public void OnCancelDrag();
}