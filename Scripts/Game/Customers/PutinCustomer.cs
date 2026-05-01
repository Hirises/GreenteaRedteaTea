using RedteaGreenteaTea.Domain;

public class PutinCustomer : Customer
{
    private const float DefaultPatienceSeconds = 30f;
    private const int DefaultOrderMaxDepth = 2;

    public PutinCustomer(int number) : base(number)
    {
        PatienceSeconds = DefaultPatienceSeconds;
        Name = "Putin";
    }

    public override ProductExpression GenerateOrder()
    {
        var orderGenerator = new TeaOrderGenerator();
        Order = orderGenerator.GenerateProductFromLeaf(3,BasicLeafKind.Black);
        return Order;
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