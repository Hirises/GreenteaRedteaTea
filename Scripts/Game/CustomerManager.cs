using Godot;
using RedteaGreenteaTea.Domain;

public partial class CustomerManager : Node
{
	private int customerCount;

	public int CustomerCount => customerCount;

	public Customer GenerateNextCustomer()
	{
		customerCount++;

		var customer = new DefaultCustomer(customerCount);
		customer.GenerateOrder();

		return customer;
	}


	//임시
	private sealed class DefaultCustomer : Customer
	{
		private const float DefaultPatienceSeconds = 30f;
		private const int DefaultOrderMaxDepth = 2;

		public DefaultCustomer(int number) : base(number)
		{
			PatienceSeconds = DefaultPatienceSeconds;
		}

		public override ProductExpression GenerateOrder()
		{
			var orderGenerator = new TeaOrderGenerator();
			Order = orderGenerator.GenerateTea(DefaultOrderMaxDepth);
			return Order;
		}

		public override void Thank()
		{
			GD.Print("Customer thanks you.");
		}

		public override void Complain(OrderResult result)
		{
			switch (result)
			{
				case OrderResult.WrongMenu:
					GD.Print("Customer complains: wrong menu.");
					break;
				case OrderResult.Timeout:
					GD.Print("Customer complains: order timed out.");
					break;
				case OrderResult.KickedOut:
					GD.Print("Customer complains: kicked out.");
					break;
			}
		}
	}
}
