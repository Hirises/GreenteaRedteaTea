using RedteaGreenteaTea.Domain;

public class EasyCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public EasyCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProduct(5);
        while(Order.Depth < 5)
        {
            Order = orderGenerator.GenerateProduct(5);
        }
        // Order = orderGenerator.GenerateProductByMaxLength(2);
        PatienceSeconds = Order.Length * 10 + 10;
        isOrderGenerated = true;
        return Order;
    }

}
