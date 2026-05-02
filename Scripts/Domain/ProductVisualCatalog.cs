using System;
using System.IO;
using System.Text.Json;

#nullable enable

namespace RedteaGreenteaTea.Domain;

public sealed class ProductVisualCatalog
{
    private const string DefaultJsonPath = "res://Scripts/Domain/ProductVisualSettings.json";

    private static ProductVisualCatalog? _current;

    private readonly ProductVisualSettings _settings;

    private ProductVisualCatalog(ProductVisualSettings settings)
    {
        _settings = settings;
        _settings.Validate();
    }

    public static ProductVisualCatalog Current => _current ??= LoadFromFile(DefaultJsonPath);

    public string TeaSuffix => _settings.Names.Suffix.Tea;
    public string BrewedLeafSuffix => _settings.Names.Suffix.BrewedLeaf;
    public string CombinedLeafSuffix => _settings.Names.Suffix.CombinedLeaf;
    public string MixedLiquidSuffix => _settings.Names.Suffix.MixedLiquid;

    public static void Configure(ProductVisualCatalog catalog)
    {
        _current = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    public static void ConfigureFromFile(string path)
    {
        _current = LoadFromFile(path);
    }

    public static ProductVisualCatalog LoadFromFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be empty.", nameof(path));
        }

        var json = ReadAllText(path);
        var settings = JsonSerializer.Deserialize<ProductVisualSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        if (settings is null)
        {
            throw new InvalidOperationException($"Failed to read product visual settings from {path}.");
        }

        return new ProductVisualCatalog(settings);
    }

    public string GetBaseName(BaseKind kind)
    {
        return kind switch
        {
            BaseKind.Tea => _settings.Names.Base.Tea,
            BaseKind.MilkTea => _settings.Names.Base.MilkTea,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    public string GetBasicLeafName(BasicLeafKind kind)
    {
        return kind switch
        {
            BasicLeafKind.Green => _settings.Names.Leaf.Green,
            BasicLeafKind.Black => _settings.Names.Leaf.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    public ProductColor GetBaseColor(BaseKind kind)
    {
        return kind switch
        {
            BaseKind.Tea => _settings.Colors.Base.Tea.ToProductColor(),
            BaseKind.MilkTea => _settings.Colors.Base.MilkTea.ToProductColor(),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    public ProductColor GetBasicLeafColor(BasicLeafKind kind)
    {
        return kind switch
        {
            BasicLeafKind.Green => _settings.Colors.Leaf.Green.ToProductColor(),
            BasicLeafKind.Black => _settings.Colors.Leaf.Black.ToProductColor(),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    public string WrapDepthIncreasedName(string name)
    {
        return $"{_settings.Names.Brackets.Open}{name}{_settings.Names.Brackets.Close}";
    }

    public ProductColor CalculateCombinedLeafColor(ProductColor left, ProductColor right)
    {
        var weights = _settings.ColorMixing.CombineLeaves;
        return WeightedRgb(left, weights.Left, right, weights.Right)
            .WithAlpha(_settings.ColorMixing.CombineLeaves.ResultAlpha);
    }

    public ProductColor CalculateBrewedTeaColor(ProductColor leaf, ProductColor liquid)
    {
        var weights = _settings.ColorMixing.BrewTea.Tea;
        var rgb = WeightedRgb(liquid, weights.Liquid, leaf, weights.Leaf);
        return rgb.WithAlpha(Math.Min(1f, liquid.A + weights.AlphaAdd));
    }

    public ProductColor CalculateBrewedLeafColor(ProductColor leaf, ProductColor liquid)
    {
        var tea = CalculateBrewedTeaColor(leaf, liquid);
        var weights = _settings.ColorMixing.BrewTea.SteepedLeaf;
        return WeightedRgb(leaf, weights.OriginalLeaf, tea, weights.BrewedTea)
            .WithAlpha(weights.ResultAlpha);
    }

    public ProductColor CalculateMixedLiquidColor(ProductColor left, ProductColor right)
    {
        float totalAlpha = left.A + right.A;
        float leftWeight = totalAlpha > 0f ? left.A / totalAlpha * 2f : 1f;
        float rightWeight = totalAlpha > 0f ? right.A / totalAlpha * 2f : 1f;
        float alpha = totalAlpha / 2f * _settings.ColorMixing.MixLiquids.AlphaMultiplier;

        return WeightedRgb(left, leftWeight, right, rightWeight).WithAlpha(alpha);
    }

    private static ProductColor WeightedRgb(ProductColor left, float leftWeight, ProductColor right, float rightWeight)
    {
        // float totalWeight = leftWeight + rightWeight;
        // if (totalWeight <= 0f)
        // {
        //     leftWeight = 0.5f;
        //     rightWeight = 0.5f;
        //     totalWeight = 1f;
        // }

        // leftWeight /= totalWeight;
        // rightWeight /= totalWeight;
        
        leftWeight /= 2;
        rightWeight /= 2;

        return new ProductColor(
            left.R * leftWeight + right.R * rightWeight,
            left.G * leftWeight + right.G * rightWeight,
            left.B * leftWeight + right.B * rightWeight,
            left.A * leftWeight + right.A * rightWeight).Clamped();
    }

    private static string ReadAllText(string path)
    {
        if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
        {
            if (!Godot.FileAccess.FileExists(path))
            {
                throw new FileNotFoundException($"Could not find {path}.");
            }

            return Godot.FileAccess.GetFileAsString(path);
        }

        return File.ReadAllText(path);
    }

    public sealed record ProductVisualSettings(
        ProductNames Names,
        ProductColors Colors,
        ColorMixing ColorMixing)
    {
        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(Names);
            ArgumentNullException.ThrowIfNull(Colors);
            ArgumentNullException.ThrowIfNull(ColorMixing);
            Names.Validate();
            Colors.Validate();
            ColorMixing.Validate();
        }
    }

    public sealed record ProductNames(
        BaseNames Base,
        LeafNames Leaf,
        SuffixNames Suffix,
        BracketNames Brackets)
    {
        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(Base);
            ArgumentNullException.ThrowIfNull(Leaf);
            ArgumentNullException.ThrowIfNull(Suffix);
            ArgumentNullException.ThrowIfNull(Brackets);
        }
    }

    public sealed record BaseNames(string Tea, string MilkTea);
    public sealed record LeafNames(string Green, string Black);
    public sealed record SuffixNames(string Tea, string BrewedLeaf, string CombinedLeaf, string MixedLiquid);
    public sealed record BracketNames(string Open, string Close);

    public sealed record ProductColors(BaseColors Base, LeafColors Leaf)
    {
        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(Base);
            ArgumentNullException.ThrowIfNull(Leaf);
        }
    }

    public sealed record BaseColors(ColorRgba255 Tea, ColorRgba255 MilkTea);
    public sealed record LeafColors(ColorRgba255 Green, ColorRgba255 Black);

    public sealed record ColorRgba255(float R, float G, float B, float A)
    {
        public ProductColor ToProductColor()
        {
            return ProductColor.FromRgb255(R, G, B, A);
        }
    }

    public sealed record ColorMixing(
        CombineLeavesMix CombineLeaves,
        BrewTeaMix BrewTea,
        MixLiquidsMix MixLiquids)
    {
        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(CombineLeaves);
            ArgumentNullException.ThrowIfNull(BrewTea);
            ArgumentNullException.ThrowIfNull(MixLiquids);
        }
    }

    public sealed record CombineLeavesMix(float Left, float Right, float ResultAlpha);
    public sealed record BrewTeaMix(BrewTeaColorMix Tea, SteepedLeafColorMix SteepedLeaf);
    public sealed record BrewTeaColorMix(float Liquid, float Leaf, float AlphaAdd);
    public sealed record SteepedLeafColorMix(float OriginalLeaf, float BrewedTea, float ResultAlpha);
    public sealed record MixLiquidsMix(float AlphaMultiplier);
}
