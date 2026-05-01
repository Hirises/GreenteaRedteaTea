namespace RedteaGreenteaTea.Domain;

public static class TeaRecipeBook
{
	public static BaseExpression TeaBase()
	{
		return new BaseExpression(BaseKind.Tea);
	}

	public static BaseExpression MilkTeaBase()
	{
		return new BaseExpression(BaseKind.MilkTea);
	}

	public static BasicLeafExpression GreenLeaf()
	{
		return new BasicLeafExpression(BasicLeafKind.Green);
	}

	public static BasicLeafExpression BlackLeaf()
	{
		return new BasicLeafExpression(BasicLeafKind.Black);
	}

	public static TeaExpression BrewTea(ProductExpression leaf, ProductExpression liquid)
	{
		return new TeaExpression(leaf, liquid);
	}

	public static BrewedLeafExpression BrewLeaf(ProductExpression leaf, ProductExpression liquid)
	{
		return new BrewedLeafExpression(leaf, liquid);
	}

	public static CombinedLeafExpression CombineLeaves(ProductExpression left, ProductExpression right)
	{
		return new CombinedLeafExpression(left, right);
	}

	public static MixedLiquidExpression MixLiquids(ProductExpression left, ProductExpression right)
	{
		return new MixedLiquidExpression(left, right);
	}

	public static bool CanServe(ProductExpression made, ProductExpression order)
	{
		return made == order;
	}
}
