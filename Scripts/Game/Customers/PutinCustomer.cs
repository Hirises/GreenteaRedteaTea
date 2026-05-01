using RedteaGreenteaTea.Domain;

public class PutinCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public PutinCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        var rules = TeaOrderGenerationRules.ForLeaf(BasicLeafKind.Black);
        Order = orderGenerator.GenerateProduct(2, rules);
        while(Order.Length <= 3)
        {
            Order = orderGenerator.GenerateProduct(4, rules);
        }
        PatienceSeconds = Order.Length * 5 + 5;
        isOrderGenerated = true;
        return Order;
    }

}
