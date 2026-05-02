using RedteaGreenteaTea.Domain;

public class BorichaCustomer : Customer
{
    private const float DefaultPatienceSeconds = 60f;
    private const int DefaultOrderMaxDepth = 2;

    public BorichaCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        isSpecial = true;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateImpossible("보리차");
        isOrderGenerated = true;
        return Order;
    }
}
