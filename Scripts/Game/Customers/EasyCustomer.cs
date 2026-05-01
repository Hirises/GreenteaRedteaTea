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
        Order = orderGenerator.GenerateProductByMaxLength(2);
        isOrderGenerated = true;
        return Order;
    }

}
