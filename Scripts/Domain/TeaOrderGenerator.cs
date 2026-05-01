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
		return GenerateProduct(maxDepth, TeaOrderGenerationRules.Any);
    }

    public ProductExpression GenerateProduct(int maxDepth, TeaOrderGenerationRules rules)
    {
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

		var choices = new List<Func<int, ProductExpression>>
		{
			depth => GenerateBase(depth, rules),
            depth => GenerateLeaf(depth, rules),
            depth => GenerateLiquid(depth, rules),
		};

		if (maxDepth > 0)
		{
			choices.Add(depth => GenerateTea(depth, rules));
		}

		return Pick(choices)(maxDepth);
	}

	public ProductExpression GenerateBase(int maxDepth)
	{
		return GenerateBase(maxDepth, TeaOrderGenerationRules.Any);
    }

    public ProductExpression GenerateBase(int maxDepth, TeaOrderGenerationRules rules)
    {
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);
		var kind = Pick(rules.BaseKinds);

        return kind switch
        {
            BaseKind.Tea => TeaRecipeBook.TeaBase(),
            BaseKind.MilkTea => TeaRecipeBook.MilkTeaBase(),
            _ => throw new ArgumentOutOfRangeException(nameof(rules), kind, null),
        };	
	}

	public ProductExpression GenerateLeaf(int maxDepth)
	{
		return GenerateLeaf(maxDepth, TeaOrderGenerationRules.Any);
    }

    public ProductExpression GenerateLeaf(int maxDepth, TeaOrderGenerationRules rules)
    {
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

		var choices = new List<Func<int, ProductExpression>>();

        foreach (var kind in rules.BasicLeafKinds)
		{
			choices.Add(_ => kind switch
            {
                BasicLeafKind.Green => TeaRecipeBook.GreenLeaf(),
                BasicLeafKind.Black => TeaRecipeBook.BlackLeaf(),
                _ => throw new ArgumentOutOfRangeException(nameof(rules), kind, null),
            });
        }

		if (maxDepth > 0)
		{
            choices.Add(depth => TeaRecipeBook.BrewLeaf(GenerateLeaf(depth - 1, rules), GenerateLiquid(depth - 1, rules)));
            choices.Add(depth => TeaRecipeBook.CombineLeaves(GenerateLeaf(depth - 1, rules), GenerateLeaf(depth - 1, rules)));
		}

		return Pick(choices)(maxDepth);
	}

	public ProductExpression GenerateTea(int maxDepth)
	{
		return GenerateTea(maxDepth, TeaOrderGenerationRules.Any);
    }

    public ProductExpression GenerateTea(int maxDepth, TeaOrderGenerationRules rules)
    {
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

		if (maxDepth == 0)
		{
			throw new ArgumentException("Tea requires at least depth 1.", nameof(maxDepth));
		}

        return TeaRecipeBook.BrewTea(GenerateLeaf(maxDepth - 1, rules), GenerateLiquid(maxDepth - 1, rules));
	}

	public ProductExpression GenerateLiquid(int maxDepth)
	{
		return GenerateLiquid(maxDepth, TeaOrderGenerationRules.Any);
    }

    public ProductExpression GenerateLiquid(int maxDepth, TeaOrderGenerationRules rules)
    {
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

		var choices = new List<Func<int, ProductExpression>>
		{
            depth => GenerateBase(depth, rules),
		};

		if (maxDepth > 0)
		{
            choices.Add(depth => GenerateTea(depth, rules));
            choices.Add(depth => TeaRecipeBook.MixLiquids(GenerateLiquid(depth - 1, rules), GenerateLiquid(depth - 1, rules)));
		}

		return Pick(choices)(maxDepth);
	}
	public ProductExpression GenerateProductFromBase(int maxDepth, BaseKind baseKind)
    {
        return GenerateProduct(maxDepth, TeaOrderGenerationRules.ForBase(baseKind));
    }

    public ProductExpression GenerateProductFromLeaf(int maxDepth, BasicLeafKind leafKind)
    {
        return GenerateProduct(maxDepth, TeaOrderGenerationRules.ForLeaf(leafKind));
    }

	private Func<int, ProductExpression> Pick(IReadOnlyList<Func<int, ProductExpression>> choices)
	{
		return choices[_random.Next(choices.Count)];
	}
	
    private T Pick<T>(IReadOnlyList<T> choices)
    {
        return choices[_random.Next(choices.Count)];
    }
}
