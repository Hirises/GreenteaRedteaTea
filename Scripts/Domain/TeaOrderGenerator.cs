using System;
using System.Collections.Generic;

namespace RedteaGreenteaTea.Domain;

public sealed class TeaOrderGenerator
{
    private readonly Random _random;

    public TeaOrderGenerator(int? seed = null)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public ProductExpression GenerateProduct(int maxDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

        var choices = new List<Func<int, ProductExpression>>
        {
            GenerateBase,
            GenerateLeaf,
            GenerateLiquid,
        };

        if (maxDepth > 0)
        {
            choices.Add(GenerateTea);
        }

        return Pick(choices)(maxDepth);
    }

    public ProductExpression GenerateBase(int maxDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);
        return _random.Next(2) == 0 ? TeaRecipeBook.TeaBase() : TeaRecipeBook.MilkTeaBase();
    }

    public ProductExpression GenerateLeaf(int maxDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

        var choices = new List<Func<int, ProductExpression>>
        {
            _ => TeaRecipeBook.GreenLeaf(),
            _ => TeaRecipeBook.BlackLeaf(),
        };

        if (maxDepth > 0)
        {
            choices.Add(depth => TeaRecipeBook.BrewLeaf(GenerateLeaf(depth - 1), GenerateLiquid(depth - 1)));
            choices.Add(depth => TeaRecipeBook.CombineLeaves(GenerateLeaf(depth - 1), GenerateLeaf(depth - 1)));
        }

        return Pick(choices)(maxDepth);
    }

    public ProductExpression GenerateTea(int maxDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

        if (maxDepth == 0)
        {
            throw new ArgumentException("Tea requires at least depth 1.", nameof(maxDepth));
        }

        return TeaRecipeBook.BrewTea(GenerateLeaf(maxDepth - 1), GenerateLiquid(maxDepth - 1));
    }

    public ProductExpression GenerateLiquid(int maxDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

        var choices = new List<Func<int, ProductExpression>>
        {
            GenerateBase,
        };

        if (maxDepth > 0)
        {
            choices.Add(GenerateTea);
            choices.Add(depth => TeaRecipeBook.MixLiquids(GenerateLiquid(depth - 1), GenerateLiquid(depth - 1)));
        }

        return Pick(choices)(maxDepth);
    }

    private Func<int, ProductExpression> Pick(IReadOnlyList<Func<int, ProductExpression>> choices)
    {
        return choices[_random.Next(choices.Count)];
    }
}
