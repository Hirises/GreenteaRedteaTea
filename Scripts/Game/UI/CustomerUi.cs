using Godot;
public partial class CustomerUi : Sprite2D
{
	[Export]
	public NodePath TextPath { get; set; } = "Text";
	private const string CustomerSpriteDirectory = "res://Sprites/Customers";
	private const string DefaultCustomerName = "Default";
	private Text textLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ZIndex = 0;
		FindTextLabel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setCustomer(Customer customer)
	{
		setTexture(customer.Name);
	}

	private void setTexture(string name)
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

	public void sayText(string text)
	{
		if (!FindTextLabel())
		{
			GD.PushError($"CustomerUi cannot say text because Text node was not found at path: {TextPath}");
			return;
		}

		textLabel.setText(text);
	}

	private bool FindTextLabel()
	{
		textLabel ??= GetNodeOrNull<Text>(TextPath);
		return textLabel != null;
	}
}
