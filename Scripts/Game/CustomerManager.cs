using Godot;
using System;
using RedteaGreenteaTea.Domain;

public partial class CustomerManager : Node
{
	private int customerCount;
	private int easyBorichaScore;
	private int normalBorichaScore;
	private int hardBorichaScore;
	private readonly Random _random = new Random();
	public int CustomerCount => customerCount;
	public void init()
	{
		customerCount = 0;
		easyBorichaScore = _random.Next(4, 8);
		normalBorichaScore = _random.Next(10, 16);
		hardBorichaScore = _random.Next(18, 28);
	}

	public Customer GenerateNextCustomer(int score, int rating)
	{
		customerCount++;
		Customer customer;

		if (score <= 7)
		{
			if (score == easyBorichaScore)
				customer = new BorichaCustomer(customerCount);
			else
				customer = new EasyCustomer(customerCount);
		}
		else if (score == 8)
		{
			customer = new PutinCustomer(customerCount);
		}
		else if (score <= 15)
		{
			if (score == normalBorichaScore)
				customer = new BorichaCustomer(customerCount);
			else
				customer = new NormalCustomer(customerCount);
		}
		else if (score == 16)
		{
			customer = new MiniBossCustomer(customerCount);
		}
		else if (score <= 29)
		{
		if (score == hardBorichaScore)
			customer = new BorichaCustomer(customerCount);
		else
			customer = new HardCustomer(customerCount);
		}
		else if(score <= 32)
		{
			customer = new RyuCustomer(customerCount, score-30);
		}
		else
		{
			if(_random.Next(10) == 0) customer = new BadCustomer(customerCount);
			else customer = new RandomCustomer(customerCount);
		}
		customer.GenerateOrder();
		return customer;
	}
}
