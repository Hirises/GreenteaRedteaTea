using System;
using System.ComponentModel.DataAnnotations;
using RedteaGreenteaTea.Domain;

public class RandomCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public RandomCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProductByMaxLength(Math.Min(Number, 40));
        PatienceSeconds = Order.Length * 5 + 15;
        isOrderGenerated = true;
        return Order;
    }

}
