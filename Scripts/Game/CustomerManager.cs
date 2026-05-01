using Godot;
using RedteaGreenteaTea.Domain;

public partial class CustomerManager : Node
{
	private int customerCount;

	public int CustomerCount => customerCount;

	public Customer GenerateNextCustomer()
	{
		customerCount++;

		if(customerCount <= 7)
		{
			var customer = new EasyCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else if(customerCount == 8)
		{
			var customer = new PutinCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else if(customerCount <= 15)
		{
			var customer = new NormalCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else if(customerCount == 16)
		{
			//TODO 중간 보스 추가
			var customer = new HardCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else if(customerCount <= 29)
		{
			var customer = new HardCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else if(customerCount <= 32)
		{
			var customer = new RyuCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
		else
		{
			var customer = new RandomCustomer(customerCount);
			customer.GenerateOrder();
			return customer;
		}
	}
}
