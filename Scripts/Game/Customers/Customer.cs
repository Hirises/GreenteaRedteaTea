using RedteaGreenteaTea.Domain;

public abstract class Customer
{
	protected Customer(int number)
	{
		Number = number;
	}

	public int Number { get; }
	public string Name { get; protected set; } = "Default";
	public ProductExpression Order { get; protected set; }
	public float PatienceSeconds { get; protected set; }

	public abstract ProductExpression GenerateOrder();
	public abstract string Thank();
	public abstract string Complain(OrderResult result);
}
