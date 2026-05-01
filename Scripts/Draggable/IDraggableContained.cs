using Godot;
using RedteaGreenteaTea.Domain;
using System;

public interface IDraggableContained: IDraggable
{
    public HoverHighlightable GetHoverHighlight();
}