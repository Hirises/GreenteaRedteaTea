using RedteaGreenteaTea.Domain;

public class RyuCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public RyuCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProduct(6);
        while(Order.Depth <= 4)
        {
            Order = orderGenerator.GenerateProduct(6);
        }
        PatienceSeconds = Order.Length * 15;
        isOrderGenerated = true;
        return Order;
    }

}
