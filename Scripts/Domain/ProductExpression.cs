using System;
using System.Runtime.CompilerServices;

namespace RedteaGreenteaTea.Domain;

public enum BaseKind
{
	Tea,
	MilkTea,
}

public enum BasicLeafKind
{
	Green,
	Black,
}

public abstract record ProductExpression
{
	public abstract ProductCategory Categories { get; }
	public abstract int Depth { get; }
	public abstract int Length {get; }
	public abstract string DisplayName { get; }
	public abstract string DisplayNameWithBrackets { get; }
	public abstract ProductColor Color { get; }
	public abstract bool isWet { get; }

	public bool Is(ProductCategory category)
	{
		return (Categories & category) == category;
	}

	protected static void Require(ProductExpression expression, ProductCategory category, string parameterName)
	{
		if (!expression.Is(category))
		{
			throw new ArgumentException($"{parameterName} must be {category}.", parameterName);
		}
	}
}

public sealed record BaseExpression(BaseKind Kind) : ProductExpression
{
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Base | ProductCategory.Liquid;
	public override int Depth => 0;
    public override int Length => 1;
	public override string DisplayName => ProductVisualCatalog.Current.GetBaseName(Kind);
	public override string DisplayNameWithBrackets => DisplayName;
	public override ProductColor Color => ProductVisualCatalog.Current.GetBaseColor(Kind);
    public override bool isWet => true;

}

public sealed record BasicLeafExpression(BasicLeafKind Kind) : ProductExpression
{
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Leaf;
	public override int Depth => 0;
	public override int Length =>1;
	public override string DisplayName => ProductVisualCatalog.Current.GetBasicLeafName(Kind);
	public override string DisplayNameWithBrackets => DisplayName;
	public override ProductColor Color => ProductVisualCatalog.Current.GetBasicLeafColor(Kind);
	public override bool isWet => false;
}

public sealed record BrewedLeafExpression : ProductExpression
{
	public BrewedLeafExpression(ProductExpression leaf, ProductExpression liquid)
	{
		Require(leaf, ProductCategory.Leaf, nameof(leaf));
		Require(liquid, ProductCategory.Liquid, nameof(liquid));
		Leaf = leaf;
		Liquid = liquid;
	}

	public ProductExpression Leaf { get; }
	public ProductExpression Liquid { get; }
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Leaf;
	public override int Depth => 1 + Math.Max(Leaf.Depth, Liquid.Depth);
	public override int Length => 1 + Leaf.Length + Liquid.Length;
	public override string DisplayName => $"{Leaf.DisplayName}{Liquid.DisplayName}{ProductVisualCatalog.Current.TeaSuffix}{ProductVisualCatalog.Current.BrewedLeafSuffix}";
	public override string DisplayNameWithBrackets => ProductVisualCatalog.Current.WrapDepthIncreasedName(
		$"{Leaf.DisplayNameWithBrackets}{Liquid.DisplayNameWithBrackets}{ProductVisualCatalog.Current.TeaSuffix}{ProductVisualCatalog.Current.BrewedLeafSuffix}");
	public override ProductColor Color => ProductVisualCatalog.Current.CalculateBrewedLeafColor(Leaf.Color, Liquid.Color);
	public override bool isWet => true;
}

public sealed record CombinedLeafExpression : ProductExpression
{
	public CombinedLeafExpression(ProductExpression left, ProductExpression right)
	{
		Require(left, ProductCategory.Leaf, nameof(left));
		Require(right, ProductCategory.Leaf, nameof(right));
		Left = left;
		Right = right;
	}

	public ProductExpression Left { get; }
	public ProductExpression Right { get; }
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Leaf;
	public override int Depth => 1 + Math.Max(Left.Depth, Right.Depth);
	public override int Length => 1 + Left.Length + Right.Length;
	public override string DisplayName => $"{Left.DisplayName}{Right.DisplayName}{ProductVisualCatalog.Current.CombinedLeafSuffix}";
	public override string DisplayNameWithBrackets => ProductVisualCatalog.Current.WrapDepthIncreasedName(
		$"{Left.DisplayNameWithBrackets}{Right.DisplayNameWithBrackets}{ProductVisualCatalog.Current.CombinedLeafSuffix}");
	public override ProductColor Color => ProductVisualCatalog.Current.CalculateCombinedLeafColor(Left.Color, Right.Color);
	public override bool isWet => Left.isWet || Right.isWet;
}

public sealed record TeaExpression : ProductExpression
{
	public TeaExpression(ProductExpression leaf, ProductExpression liquid)
	{
		Require(leaf, ProductCategory.Leaf, nameof(leaf));
		Require(liquid, ProductCategory.Liquid, nameof(liquid));
		Leaf = leaf;
		Liquid = liquid;
	}

	public ProductExpression Leaf { get; }
	public ProductExpression Liquid { get; }
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Tea | ProductCategory.Liquid;
	public override int Depth => 1 + Math.Max(Leaf.Depth, Liquid.Depth);
	public override int Length => Leaf.Length + Liquid.Length;
	public override string DisplayName => $"{Leaf.DisplayName}{Liquid.DisplayName}{ProductVisualCatalog.Current.TeaSuffix}";
	public override string DisplayNameWithBrackets => ProductVisualCatalog.Current.WrapDepthIncreasedName(
		$"{Leaf.DisplayNameWithBrackets}{Liquid.DisplayNameWithBrackets}{ProductVisualCatalog.Current.TeaSuffix}");
	public override ProductColor Color => ProductVisualCatalog.Current.CalculateBrewedTeaColor(Leaf.Color, Liquid.Color);
	public override bool isWet => true;
}

public sealed record MixedLiquidExpression : ProductExpression
{
	public MixedLiquidExpression(ProductExpression left, ProductExpression right)
	{
		Require(left, ProductCategory.Liquid, nameof(left));
		Require(right, ProductCategory.Liquid, nameof(right));
		Left = left;
		Right = right;
	}

	public ProductExpression Left { get; }
	public ProductExpression Right { get; }
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Liquid;
	public override int Depth => 1 + Math.Max(Left.Depth, Right.Depth);
	public override int Length => 1 + Left.Length + Right.Length;
	public override string DisplayName => $"{Left.DisplayName}{Right.DisplayName}{ProductVisualCatalog.Current.MixedLiquidSuffix}";
	public override string DisplayNameWithBrackets => ProductVisualCatalog.Current.WrapDepthIncreasedName(
		$"{Left.DisplayNameWithBrackets}{Right.DisplayNameWithBrackets}{ProductVisualCatalog.Current.MixedLiquidSuffix}");
	public override ProductColor Color => ProductVisualCatalog.Current.CalculateMixedLiquidColor(Left.Color, Right.Color);
	public override bool isWet => true;
}

public sealed record ImposibleExpression : ProductExpression
{
	public ImposibleExpression(string name)
	{
		Name = name;
	}
	private string Name;
	public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Liquid;
	public override int Depth => 1;
	public override int Length => 1;
	public override string DisplayName => $"{Name}";
	public override string DisplayNameWithBrackets => ProductVisualCatalog.Current.WrapDepthIncreasedName(
		$"{Name}");
	public override ProductColor Color => ProductColor.FromRgb255(0, 0, 0, 1);
	public override bool isWet => false;
}