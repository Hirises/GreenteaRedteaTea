using Godot;
public partial class CustomerUi : Sprite2D
{
	private const string CustomerSpriteDirectory = "res://Sprites/Customers";
	private const string DefaultCustomerName = "Default";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ZIndex = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setTexture(string name)
	{
		var customerName = string.IsNullOrWhiteSpace(name) ? DefaultCustomerName : name;
		var texturePath = $"{CustomerSpriteDirectory}/{customerName}.png";

		if (!ResourceLoader.Exists(texturePath))
		{
			GD.PushWarning($"Customer texture not found: {texturePath}. Loading default customer texture.");
			texturePath = $"{CustomerSpriteDirectory}/{DefaultCustomerName}.png";
		}

		var texture = ResourceLoader.Load<Texture2D>(texturePath);

		if (texture == null)
		{
			GD.PushError($"Failed to load customer texture: {texturePath}");
			return;
		}

		Texture = texture;
	}
}
