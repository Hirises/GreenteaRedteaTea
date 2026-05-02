using System.Runtime.CompilerServices;
using Godot;
public partial class Special : Sprite2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public new void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
}
