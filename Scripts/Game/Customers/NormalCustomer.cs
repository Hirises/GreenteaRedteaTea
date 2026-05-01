using RedteaGreenteaTea.Domain;

public class NormalCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public NormalCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProduct(Number);
        isOrderGenerated = true;
        return Order;
    }

}
