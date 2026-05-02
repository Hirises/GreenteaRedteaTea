using RedteaGreenteaTea.Domain;

public class SatiCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public SatiCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        isSpecial = true;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProductByMaxLength(12);
        while(Order.Length < 8)
        {
            Order = orderGenerator.GenerateProductByMaxLength(12);
        }
        PatienceSeconds = Order.Length * 5 + 15;
        isOrderGenerated = true;
        return Order;
    }

}
