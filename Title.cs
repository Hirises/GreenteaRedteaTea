using Godot;
using System;

public partial class Title : Node2D
{
    [Export] AnimationPlayer shutterLogoAnimation;
    [Export] PackedScene mainScene;
    [Export] SoundPlayer shutterDown;
    [Export] SoundPlayer shutterUp;

    enum State
    {
        Title,
        Game,
        GameOver,
    }

    State state = State.Title;
    MainScene mainSceneInst;
    GameManager gameManager;

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent ||
            !keyEvent.Pressed ||
            keyEvent.Echo ||
            keyEvent.Keycode != Key.F11)
        {
            return;
        }

        var currentMode = DisplayServer.WindowGetMode();
        var nextMode = currentMode == DisplayServer.WindowMode.Fullscreen ||
                       currentMode == DisplayServer.WindowMode.ExclusiveFullscreen
            ? DisplayServer.WindowMode.Windowed
            : DisplayServer.WindowMode.Fullscreen;

        DisplayServer.WindowSetMode(nextMode);
        GetViewport().SetInputAsHandled();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (state != State.Title)
            return;

        if (@event is InputEventMouseButton mouseEvent)
        {
            if (!mouseEvent.Pressed)
            {
                mainSceneInst = mainScene.Instantiate<MainScene>();
                AddChild(mainSceneInst);
                gameManager = mainSceneInst.gameManager;

                shutterLogoAnimation.Play("open");
                state = State.Game;
                shutterUp.Play();
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (state != State.Game)
            return;
        
        if (gameManager.gameover)
            OnGameOver();
    }

    public void OnGameOver()
    {
        shutterLogoAnimation.Play("close");
        state = State.GameOver;
        shutterDown.Play();
    }

    public void OnGameOverAnimationEnd()
    {
        mainSceneInst.QueueFree();

        mainSceneInst = mainScene.Instantiate<MainScene>();
        AddChild(mainSceneInst);
        gameManager = mainSceneInst.gameManager;
        state = State.Game;

        shutterLogoAnimation.Play("reopen");
        shutterUp.Play();
    }
}
