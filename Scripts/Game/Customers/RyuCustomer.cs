using System;
using System.Data;
using RedteaGreenteaTea.Domain;

public class RyuCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;
    private int[,] arange = 
    { 
        { 15, 18 }, 
        { 22, 25 }, 
        { 30, 33 } 
    };   
    private int c;

    public RyuCustomer(int number, int count) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        isSpecial = true;
        c = count;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProduct(5);
        while(Order.Length < arange[c,0] || arange[c,1] < Order.Length)
        {
            Order = orderGenerator.GenerateProduct(5);
        }
        PatienceSeconds = Order.Length * 15;
        isOrderGenerated = true;
        return Order;
    }

}