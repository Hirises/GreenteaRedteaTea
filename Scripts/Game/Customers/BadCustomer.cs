using RedteaGreenteaTea.Domain;

public class BadCustomer : Customer
{
    private const float DefaultPatienceSeconds = 60f;
    private const int DefaultOrderMaxDepth = 2;

    public BadCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        isBad = true;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateInvalid();
        isOrderGenerated = true;
        return Order;
    }
}
