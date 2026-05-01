using System;

namespace RedteaGreenteaTea.Domain;

[Flags]
public enum ProductCategory
{
	None = 0,
	Product = 1 << 0,
	Base = 1 << 1,
	Leaf = 1 << 2,
	Tea = 1 << 3,
	Liquid = 1 << 4,
}
