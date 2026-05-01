using Godot;
using System;

public partial class ClickAreaCandle : ClickArea
{
    [Export] float candleMaxTime = 3f;
    [Export] DraggableTeapot teapot;
    float candleTime = 0f;
    bool isLit = false;

    public override void OnClick()
    {
        GD.Print($"Lit candle {Name}!");
        isLit = true;
        candleTime = 0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isLit)
        {
            candleTime += (float)delta;
            if (candleTime >= candleMaxTime)
            {
                isLit = false;
                GD.Print($"Candle {Name} has burned out. Trying brew the tea in teapot...");
                teapot.TryBrew();
                return;
            }
            if (InputManager.Instance.currentDragItem == teapot)
            {
                isLit = false;
                GD.Print($"Teapot is lifted while candle {Name} is lit. Extinguishing candle.");
                return;
            }
        }
    }
}
