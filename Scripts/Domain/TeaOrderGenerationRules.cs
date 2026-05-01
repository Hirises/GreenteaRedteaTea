using System;
using System.Collections.Generic;
using System.Linq;

namespace RedteaGreenteaTea.Domain;

public sealed class TeaOrderGenerationRules
{
    public static TeaOrderGenerationRules Any { get; } = new(
        new[] { BaseKind.Tea, BaseKind.MilkTea },
        new[] { BasicLeafKind.Green, BasicLeafKind.Black });

    public TeaOrderGenerationRules(IEnumerable<BaseKind> baseKinds, IEnumerable<BasicLeafKind> basicLeafKinds)
    {
        BaseKinds = baseKinds.Distinct().ToArray();
        BasicLeafKinds = basicLeafKinds.Distinct().ToArray();

        if (BaseKinds.Count == 0)
        {
            throw new ArgumentException("At least one base kind is required.", nameof(baseKinds));
        }

        if (BasicLeafKinds.Count == 0)
        {
            throw new ArgumentException("At least one leaf kind is required.", nameof(basicLeafKinds));
        }
    }

    public IReadOnlyList<BaseKind> BaseKinds { get; }
    public IReadOnlyList<BasicLeafKind> BasicLeafKinds { get; }

    public static TeaOrderGenerationRules ForBase(BaseKind baseKind)
    {
        return new TeaOrderGenerationRules(
            new[] { baseKind },
            Any.BasicLeafKinds);
    }

    public static TeaOrderGenerationRules ForLeaf(BasicLeafKind leafKind)
    {
        return new TeaOrderGenerationRules(
            Any.BaseKinds,
            new[] { leafKind });
    }

    public static TeaOrderGenerationRules ForBaseAndLeaf(BaseKind baseKind, BasicLeafKind leafKind)
    {
        return new TeaOrderGenerationRules(
            new[] { baseKind },
            new[] { leafKind });
    }
}