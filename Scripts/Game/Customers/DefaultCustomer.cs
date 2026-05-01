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

    public override string SayOrder()
    {
        return GetOrderName() + "please";
    }

    public override string Thank()
    {
        return "thx";
    }

    public override string Complain(OrderResult result)
    {
        string comp = "";
        switch (result)
        {
            case OrderResult.WrongMenu:
                comp = "Customer complains: wrong menu.";
                break;
            case OrderResult.Timeout:
                comp = "Customer complains: order timed out.";
                break;
            case OrderResult.KickedOut:
                comp = "Customer complains: kicked out.";
                break;
        }
        return comp;
    }
}