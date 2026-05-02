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
        Order = orderGenerator.GenerateProductByMaxLength(8);
        PatienceSeconds = Order.Length * 10 + 10;
        isOrderGenerated = true;
        return Order;
    }

}
