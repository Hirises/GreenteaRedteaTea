using RedteaGreenteaTea.Domain;

public class RandomCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public RandomCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
    }

    protected override ProductExpression _GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProductByMaxLength(Number);
        PatienceSeconds = Order.Length * 5 + 15;
        isOrderGenerated = true;
        return Order;
    }

}
