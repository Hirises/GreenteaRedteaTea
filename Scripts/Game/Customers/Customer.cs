using RedteaGreenteaTea.Domain;

public abstract class Customer
{
	protected Customer(int number)
	{
		Number = number;
		Name = CustomerNameBook.GetName(GetType().Name);
	}

	protected bool isOrderGenerated = false;

	public int Number { get; }
	public string Name { get; protected set; } = "Default";
	public bool isSpecial { get; protected set; } = false;
	public ProductExpression Order { get; protected set; }
	public float PatienceSeconds { get; protected set; }

	public ProductExpression GenerateOrder()
	{
		if (isOrderGenerated)
		{
			return Order;
		}

		return _GenerateOrder();
	}

	protected abstract ProductExpression _GenerateOrder();

	public virtual string SayOrder()
	{
		return CustomerDialogueBook.GetOrder(GetType().Name, GetOrderName());
	}

	public virtual string Thank()
	{
		return CustomerDialogueBook.GetThank(GetType().Name);
	}

	public virtual string Complain(OrderResult result)
	{
		return CustomerDialogueBook.GetComplaint(GetType().Name, result);
	}

	protected string GetOrderName()
	{
		return "\"" + Order.DisplayName + "\"";
	}
}
