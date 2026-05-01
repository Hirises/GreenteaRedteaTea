using RedteaGreenteaTea.Domain;

public abstract class Customer
{
	protected Customer(int number)
	{
		Number = number;
	}

	protected bool isOrderGenerated = false;
	public int Number { get; }
	public string Name { get; protected set; } = "Default";
	public ProductExpression Order { get; protected set; }
	public float PatienceSeconds { get; protected set; }

	public ProductExpression GenerateOrder()
	{
		if(isOrderGenerated) return Order;
		return _GenerateOrder();
	}

	protected abstract ProductExpression _GenerateOrder();
	public abstract string Thank();
	public abstract string Complain(OrderResult result);
}
