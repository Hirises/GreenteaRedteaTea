using System;
using System.Collections.Generic;
using System.Linq;

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
		return CreateBase(Pick(rules.BaseKinds));
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
			choices.Add(_ => CreateBasicLeaf(kind));
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

	public ProductExpression GenerateProductByMaxLength(int maxLength)
	{
		return GenerateProductByMaxLength(maxLength, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateProductByMaxLength(int maxLength, TeaOrderGenerationRules rules)
	{
		return GenerateProductByLength(PickPossibleLength(maxLength, 1, CanGenerateProductLength), rules);
	}

	public ProductExpression GenerateLeafByMaxLength(int maxLength)
	{
		return GenerateLeafByMaxLength(maxLength, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateLeafByMaxLength(int maxLength, TeaOrderGenerationRules rules)
	{
		return GenerateLeafByLength(PickPossibleLength(maxLength, 1, CanGenerateLeafLength), rules);
	}

	public ProductExpression GenerateTeaByMaxLength(int maxLength)
	{
		return GenerateTeaByMaxLength(maxLength, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateTeaByMaxLength(int maxLength, TeaOrderGenerationRules rules)
	{
		return GenerateTeaByLength(PickPossibleLength(maxLength, 2, CanGenerateTeaLength), rules);
	}

	public ProductExpression GenerateLiquidByMaxLength(int maxLength)
	{
		return GenerateLiquidByMaxLength(maxLength, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateLiquidByMaxLength(int maxLength, TeaOrderGenerationRules rules)
	{
		return GenerateLiquidByLength(PickPossibleLength(maxLength, 1, CanGenerateLiquidLength), rules);
	}

	public ProductExpression GenerateProductByLength(int length)
	{
		return GenerateProductByLength(length, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateProductByLength(int length, TeaOrderGenerationRules rules)
	{
		length = NormalizeLength(length, CanGenerateProductLength, nameof(length));

		var choices = new List<Func<ProductExpression>>();

		if (CanGenerateLeafLength(length))
		{
			choices.Add(() => GenerateLeafByLength(length, rules));
		}

		if (CanGenerateLiquidLength(length))
		{
			choices.Add(() => GenerateLiquidByLength(length, rules));
		}

		if (length == 1)
		{
			choices.Add(() => GenerateBaseByLength(length, rules));
		}

		if (CanGenerateTeaLength(length))
		{
			choices.Add(() => GenerateTeaByLength(length, rules));
		}

		return Pick(choices)();
	}

	public ProductExpression GenerateBaseByLength(int length)
	{
		return GenerateBaseByLength(length, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateBaseByLength(int length, TeaOrderGenerationRules rules)
	{
		length = NormalizeLength(length, CanGenerateBaseLength, nameof(length));
		return CreateBase(Pick(rules.BaseKinds));
	}

	public ProductExpression GenerateLeafByLength(int length)
	{
		return GenerateLeafByLength(length, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateLeafByLength(int length, TeaOrderGenerationRules rules)
	{
		length = NormalizeLength(length, CanGenerateLeafLength, nameof(length));

		if (length == 1)
		{
			return CreateBasicLeaf(Pick(rules.BasicLeafKinds));
		}

		var choices = new List<Func<ProductExpression>>();
		foreach (var split in SplitBrewedLeafChildLengths(length))
		{
			choices.Add(() => TeaRecipeBook.BrewLeaf(
				GenerateLeafByLength(split.Left, rules),
				GenerateLiquidByLength(split.Right, rules)));
		}

		foreach (var split in SplitCombinedLeafChildLengths(length))
		{
			choices.Add(() => TeaRecipeBook.CombineLeaves(
				GenerateLeafByLength(split.Left, rules),
				GenerateLeafByLength(split.Right, rules)));
		}

		return Pick(choices)();
	}

	public ProductExpression GenerateTeaByLength(int length)
	{
		return GenerateTeaByLength(length, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateTeaByLength(int length, TeaOrderGenerationRules rules)
	{
		length = NormalizeLength(length, CanGenerateTeaLength, nameof(length));

		var split = Pick(SplitTeaChildLengths(length));
		return TeaRecipeBook.BrewTea(
			GenerateLeafByLength(split.Left, rules),
			GenerateLiquidByLength(split.Right, rules));
	}

	public ProductExpression GenerateLiquidByLength(int length)
	{
		return GenerateLiquidByLength(length, TeaOrderGenerationRules.Any);
	}

	public ProductExpression GenerateLiquidByLength(int length, TeaOrderGenerationRules rules)
	{
		length = NormalizeLength(length, CanGenerateLiquidLength, nameof(length));

		if (length == 1)
		{
			return GenerateBaseByLength(length, rules);
		}

		var choices = new List<Func<ProductExpression>>
		{
			() => GenerateTeaByLength(length, rules),
		};

		foreach (var split in SplitMixedLiquidChildLengths(length))
		{
			choices.Add(() => TeaRecipeBook.MixLiquids(
				GenerateLiquidByLength(split.Left, rules),
				GenerateLiquidByLength(split.Right, rules)));
		}

		return Pick(choices)();
	}

	public ProductExpression GenerateImpossible(string name)
	{
		return TeaRecipeBook.Impossible(name);
	}

	public ProductExpression GenerateInvalid()
	{
		return GenerateInvalid(2);
	}

	public ProductExpression GenerateInvalid(int maxDepth)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(maxDepth);

		var imp = GenerateImpossible(Pick(ImpossibleStringCatalog.Current.Strings));
		var safeDepth = Math.Max(0, maxDepth);
		var choices = new List<Func<ProductExpression>>
		{
			() => TeaRecipeBook.BrewTea(
				TeaRecipeBook.CombineLeaves(GenerateLeaf(safeDepth), imp),
				GenerateLiquid(safeDepth)),
			() => TeaRecipeBook.BrewTea(
				GenerateLeaf(safeDepth),
				TeaRecipeBook.MixLiquids(imp, GenerateLiquid(safeDepth))),
			() => TeaRecipeBook.BrewTea(
				GenerateLeaf(safeDepth),
				TeaRecipeBook.MixLiquids(GenerateLiquid(safeDepth), imp)),
			() => TeaRecipeBook.BrewLeaf(
				TeaRecipeBook.CombineLeaves(GenerateLeaf(safeDepth), imp),
				GenerateLiquid(safeDepth)),
			() => TeaRecipeBook.BrewLeaf(
				GenerateLeaf(safeDepth),
				TeaRecipeBook.MixLiquids(GenerateLiquid(safeDepth), imp)),
			() => TeaRecipeBook.CombineLeaves(
				TeaRecipeBook.BrewLeaf(GenerateLeaf(safeDepth), imp),
				GenerateLeaf(safeDepth)),
			() => TeaRecipeBook.MixLiquids(
				TeaRecipeBook.BrewTea(GenerateLeaf(safeDepth), imp),
				GenerateLiquid(safeDepth)),
			() => TeaRecipeBook.MixLiquids(
				GenerateLiquid(safeDepth),
				TeaRecipeBook.BrewTea(GenerateLeaf(safeDepth), imp)),
		};

		if (maxDepth > 0)
		{
			choices.Add(() => TeaRecipeBook.BrewTea(
				GenerateLeaf(maxDepth - 1),
				TeaRecipeBook.MixLiquids(imp, GenerateLiquid(maxDepth - 1))));
			choices.Add(() => TeaRecipeBook.BrewLeaf(
				TeaRecipeBook.CombineLeaves(GenerateLeaf(maxDepth - 1), imp),
				GenerateLiquid(maxDepth - 1)));
		}

		return Pick(choices)();
	}

	private BaseExpression CreateBase(BaseKind kind)
	{
		return kind switch
		{
			BaseKind.Tea => TeaRecipeBook.TeaBase(),
			BaseKind.MilkTea => TeaRecipeBook.MilkTeaBase(),
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
		};
	}

	private BasicLeafExpression CreateBasicLeaf(BasicLeafKind kind)
	{
		return kind switch
		{
			BasicLeafKind.Green => TeaRecipeBook.GreenLeaf(),
			BasicLeafKind.Black => TeaRecipeBook.BlackLeaf(),
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
		};
	}

	private int PickPossibleLength(int maxLength, int minLength, Func<int, bool> canGenerate)
	{
		if (maxLength < minLength)
		{
			throw new ArgumentOutOfRangeException(nameof(maxLength), $"Length must be at least {minLength}.");
		}

		var lengths = Enumerable.Range(minLength, maxLength - minLength + 1)
			.Where(canGenerate)
			.ToArray();

		if (lengths.Length == 0)
		{
			throw new ArgumentException("No product can be generated with the requested length range.", nameof(maxLength));
		}

		return Pick(lengths);
	}

	private static int NormalizeLength(int length, Func<int, bool> canGenerate, string parameterName)
	{
		if (length < 1)
		{
			throw new ArgumentOutOfRangeException(parameterName, "Length must be at least 1.");
		}

		if (canGenerate(length))
		{
			return length;
		}

		if (length % 2 == 0 && canGenerate(length - 1))
		{
			return length - 1;
		}

		throw new ArgumentException("No product can be generated with the requested length.", parameterName);
	}

	private static bool CanGenerateProductLength(int length)
	{
		return length >= 1;
	}

	private static bool CanGenerateBaseLength(int length)
	{
		return length == 1;
	}

	private static bool CanGenerateLeafLength(int length)
	{
		return length == 1 || length >= 3;
	}

	private static bool CanGenerateTeaLength(int length)
	{
		return length >= 2;
	}

	private static bool CanGenerateLiquidLength(int length)
	{
		return length >= 1;
	}

	private static IReadOnlyList<(int Left, int Right)> SplitBrewedLeafChildLengths(int length)
	{
		var splits = new List<(int Left, int Right)>();
		var remainingLength = length - 1;

		for (var left = 1; left < remainingLength; left++)
		{
			var right = remainingLength - left;
			if (CanGenerateLeafLength(left) && CanGenerateLiquidLength(right))
			{
				splits.Add((left, right));
			}
		}

		return splits;
	}

	private static IReadOnlyList<(int Left, int Right)> SplitCombinedLeafChildLengths(int length)
	{
		var splits = new List<(int Left, int Right)>();
		var remainingLength = length - 1;

		for (var left = 1; left < remainingLength; left++)
		{
			var right = remainingLength - left;
			if (CanGenerateLeafLength(left) && CanGenerateLeafLength(right))
			{
				splits.Add((left, right));
			}
		}

		return splits;
	}

	private static IReadOnlyList<(int Left, int Right)> SplitTeaChildLengths(int length)
	{
		var splits = new List<(int Left, int Right)>();

		for (var left = 1; left < length; left++)
		{
			var right = length - left;
			if (CanGenerateLeafLength(left) && CanGenerateLiquidLength(right))
			{
				splits.Add((left, right));
			}
		}

		return splits;
	}

	private static IReadOnlyList<(int Left, int Right)> SplitMixedLiquidChildLengths(int length)
	{
		var splits = new List<(int Left, int Right)>();
		var remainingLength = length - 1;

		for (var left = 1; left < remainingLength; left++)
		{
			var right = remainingLength - left;
			if (CanGenerateLiquidLength(left) && CanGenerateLiquidLength(right))
			{
				splits.Add((left, right));
			}
		}

		return splits;
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
