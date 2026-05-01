using RedteaGreenteaTea.Domain;

public abstract class Customer
{
	protected Customer(int number)
	{
		Number = number;
	}

	public int Number { get; }
	public ProductExpression Order { get; protected set; }
	public float PatienceSeconds { get; protected set; }

	public abstract ProductExpression GenerateOrder();
	public abstract void Thank();
	public abstract void Complain(OrderResult result);
}
