using RedteaGreenteaTea.Domain;

public class DefaultCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public DefaultCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        Name = GenerateName(number);
    }

    private string GenerateName(int seed)
    {
        return "Default";
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProduct(Number);
        isOrderGenerated = true;
        return Order;
    }

}
