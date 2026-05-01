using Godot;
using RedteaGreenteaTea.Domain;

public partial class CustomerManager : Node
{
	private int customerCount;

	public int CustomerCount => customerCount;

	public Customer GenerateNextCustomer()
	{
		customerCount++;

		var customer = new PutinCustomer(customerCount);
		customer.GenerateOrder();

		return customer;
	}
}
