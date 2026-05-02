using System;
using System.Collections.Generic;
using Godot;

public partial class CustomerUi : Node
{
    [Export]
    public NodePath TextPath { get; set; } = "Text";
    [Export]
    public AnimationPlayer SpeechAnimation;
    [Export]
    public AnimationPlayer MoveAnimation;
    [Export]
    public NodePath SpecialPath { get; set; } = "Special";
    [Export]
    public NodePath HeadPath { get; set; } = "Head";
    [Export]
    public NodePath BodyPath { get; set; } = "Body";
    [Export]
    public NodePath ClothPath { get; set; } = "Cloth";
    [Export]
    public NodePath ExpressionPath { get; set; } = "Expression";
    [Export]
    public GameManager gameManager;

    private const string CustomerSpriteDirectory = "res://Sprites/Customers";
    private const string SpecialDir = "Special";
    private const string HeadDir = "\uBA38\uB9AC";
    private const string BodyDir = "\uBAB8";
    private const string ClothDir = "\uC637";
    private const string ExpressionDir = "\uD45C\uC815";
    private const string DefaultCustomerName = "Default";
    private const string TextureExtension = ".png";
    private const string RareGreenBodyTexture = "\uC0AC\uB78C_\uBAB8_\uB179\uC0C9.png";
    private const int DefaultBodyTextureWeight = 20;
    private const int RareGreenBodyTextureWeight = 1;

    private Sprite2D special;
    private Sprite2D head;
    private Sprite2D body;
    private Sprite2D cloth;
    private Sprite2D expression;
    private Text textLabel;

    public override void _Ready()
    {
        special = GetNodeOrNull<Sprite2D>(SpecialPath);
        head = GetNodeOrNull<Sprite2D>(HeadPath);
        body = GetNodeOrNull<Sprite2D>(BodyPath);
        cloth = GetNodeOrNull<Sprite2D>(ClothPath);
        expression = GetNodeOrNull<Sprite2D>(ExpressionPath);

        if (special == null)
        {
            GD.PushError($"CustomerUI needs a Special child at path: {SpecialPath}");
        }

        if (head == null)
        {
            GD.PushError($"CustomerUI needs a Head child at path: {HeadPath}");
        }

        if (body == null)
        {
            GD.PushError($"CustomerUI needs a Body child at path: {BodyPath}");
        }

        if (cloth == null)
        {
            GD.PushError($"CustomerUI needs a Cloth child at path: {ClothPath}");
        }

        if (expression == null)
        {
            GD.PushError($"CustomerUI needs an Expression child at path: {ExpressionPath}");
        }

        FindTextLabel();
    }

    public void setCustomer(Customer customer)
    {
        if (customer == null)
        {
            ClearCustomerTextures();
            return;
        }

        if (customer.isSpecial)
        {
            SetSpecialTexture(customer.Name);
            return;
        }

        GenerateTexture(GetCustomerSeed(customer));
    }

    private void SetSpecialTexture(string name)
    {
        ClearCustomerTextures();
        SetVisible(special, true);

        var customerName = string.IsNullOrWhiteSpace(name) ? DefaultCustomerName : name;
        var texturePath = $"{CustomerSpriteDirectory}/{SpecialDir}/{customerName}{TextureExtension}";

        if (!ResourceLoader.Exists(texturePath))
        {
            var fallbackPath = $"{CustomerSpriteDirectory}/{DefaultCustomerName}{TextureExtension}";
            GD.PushWarning($"Special customer texture not found: {texturePath}. Loading default customer texture.");
            texturePath = fallbackPath;
        }

        SetTexture(special, texturePath);
    }

    private void GenerateTexture(int seed)
    {
        ClearCustomerTextures();
        SetVisible(head, true);
        SetVisible(body, true);
        SetVisible(cloth, true);
        SetVisible(expression, true);

        SetRandomBodyTexture(body, seed);
        SetRandomLayerTexture(cloth, ClothDir, seed + 1);
        SetRandomLayerTexture(head, HeadDir, seed + 2);
        SetRandomLayerTexture(expression, ExpressionDir, seed + 3);
    }

    private void SetRandomBodyTexture(Sprite2D sprite, int seed)
    {
        var textures = GetTexturePaths($"{CustomerSpriteDirectory}/{BodyDir}");
        if (textures.Count == 0)
        {
            GD.PushWarning($"Customer texture directory has no png files: {CustomerSpriteDirectory}/{BodyDir}");
            SetVisible(sprite, false);
            return;
        }

        SetTexture(sprite, SelectWeightedBodyTexture(textures, seed));
    }

    private string SelectWeightedBodyTexture(IReadOnlyList<string> textures, int seed)
    {
        var totalWeight = 0;
        foreach (var texture in textures)
        {
            totalWeight += GetBodyTextureWeight(texture);
        }

        var roll = SelectIndex(seed, totalWeight);
        foreach (var texture in textures)
        {
            roll -= GetBodyTextureWeight(texture);
            if (roll < 0)
            {
                return texture;
            }
        }

        return textures[0];
    }

    private int GetBodyTextureWeight(string texturePath)
    {
        return texturePath.EndsWith($"/{RareGreenBodyTexture}", StringComparison.OrdinalIgnoreCase)
            ? RareGreenBodyTextureWeight
            : DefaultBodyTextureWeight;
    }

    private void SetRandomLayerTexture(Sprite2D sprite, string directoryName, int seed)
    {
        var textures = GetTexturePaths($"{CustomerSpriteDirectory}/{directoryName}");
        if (textures.Count == 0)
        {
            GD.PushWarning($"Customer texture directory has no png files: {CustomerSpriteDirectory}/{directoryName}");
            SetVisible(sprite, false);
            return;
        }

        SetTexture(sprite, textures[SelectIndex(seed, textures.Count)]);
    }

    private int SelectIndex(int seed, int count)
    {
        return count <= 1 ? 0 : (int)((uint)seed % (uint)count);
    }

    private List<string> GetTexturePaths(string directoryPath)
    {
        var paths = new List<string>();
        foreach (var fileName in ResourceLoader.ListDirectory(directoryPath))
        {
            if (!fileName.EndsWith("/") && fileName.EndsWith(TextureExtension, StringComparison.OrdinalIgnoreCase))
            {
                paths.Add($"{directoryPath}/{fileName}");
            }
        }

        paths.Sort(StringComparer.Ordinal);
        return paths;
    }

    private int GetCustomerSeed(Customer customer)
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + customer.Number;
            hash = AddStableHash(hash, customer.Name);
            hash = AddStableHash(hash, customer.GetType().Name);
            return hash & int.MaxValue;
        }
    }

    private int AddStableHash(int hash, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return hash;
        }

        foreach (var character in value)
        {
            hash = hash * 31 + character;
        }

        return hash;
    }

    private void ClearCustomerTextures()
    {
        SetVisible(special, false);
        SetVisible(head, false);
        SetVisible(body, false);
        SetVisible(cloth, false);
        SetVisible(expression, false);
        ClearTexture(special);
        ClearTexture(head);
        ClearTexture(body);
        ClearTexture(cloth);
        ClearTexture(expression);
    }

    private void SetTexture(Sprite2D sprite, string texturePath)
    {
        if (sprite == null || string.IsNullOrEmpty(texturePath))
        {
            return;
        }

        if (!ResourceLoader.Exists(texturePath))
        {
            GD.PushError($"Customer texture not found: {texturePath}");
            return;
        }

        var texture = ResourceLoader.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            GD.PushError($"Failed to load customer texture: {texturePath}");
            return;
        }

        sprite.Texture = texture;
    }

    private void ClearTexture(Sprite2D sprite)
    {
        if (sprite != null)
        {
            sprite.Texture = null;
        }
    }

    private void SetVisible(CanvasItem item, bool visible)
    {
        if (item != null)
        {
            item.Visible = visible;
        }
    }

    public void sayText(string text)
    {
        if (!FindTextLabel())
        {
            GD.PushError($"CustomerUi cannot say text because Text node was not found at path: {TextPath}");
            return;
        }

        textLabel.setText(text);
        SpeechAnimation.Play("speech_open");
    }

    private bool FindTextLabel()
    {
        textLabel ??= GetNodeOrNull<Text>(TextPath);
        return textLabel != null;
    }

    public void Appear()
    {
        MoveAnimation.Play("appear");
    }

    public void Disappear()
    {
        MoveAnimation.Play("disappear");
    }

    public void OnDisappear()
    {
        gameManager.NextOrder();
    }
}
