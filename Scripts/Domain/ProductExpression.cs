using System;

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
    public abstract string DisplayName { get; }

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

    public override string DisplayName => Kind switch
    {
        BaseKind.Tea => "차",
        BaseKind.MilkTea => "밀크티",
        _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };
}

public sealed record BasicLeafExpression(BasicLeafKind Kind) : ProductExpression
{
    public override ProductCategory Categories => ProductCategory.Product | ProductCategory.Leaf;
    public override int Depth => 0;

    public override string DisplayName => Kind switch
    {
        BasicLeafKind.Green => "녹찻잎",
        BasicLeafKind.Black => "홍찻잎",
        _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };
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
    public override string DisplayName => $"{Leaf.DisplayName}{Liquid.DisplayName}우린찻잎";
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
    public override string DisplayName => $"{Left.DisplayName}{Right.DisplayName}찻잎";
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
    public override string DisplayName => $"{Leaf.DisplayName}{Liquid.DisplayName}";
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
    public override string DisplayName => $"{Left.DisplayName}{Right.DisplayName}믹스";
}
