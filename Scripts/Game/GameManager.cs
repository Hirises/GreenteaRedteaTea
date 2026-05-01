using Godot;
using RedteaGreenteaTea.Domain;

public partial class GameManager : Node
{
	[Signal]
	public delegate void OrderStartedEventHandler(string orderName, float timeLimitSeconds);

	[Signal]
	public delegate void OrderTimerChangedEventHandler(float remainingSeconds, float timeLimitSeconds);

	[Signal]
	public delegate void OrderEndedEventHandler(int result, string orderName);

	[Export]
	public NodePath CustomerManagerPath { get; set; } = "CustomerManager";

	[Export]
	public NodePath CustomerUiPath { get; set; } = "../CustomerUI";

	private CustomerManager customerManager;
	private CustomerUi customerUi;
	private bool isOnOrder;
	private Customer currentCustomer;
	private float timer;

	public ProductExpression CurrentOrder => currentCustomer?.Order;
	public bool IsOnOrder => isOnOrder;
	public float RemainingOrderSeconds => currentCustomer == null ? 0f : Mathf.Max(currentCustomer.PatienceSeconds - timer, 0f);

	public override void _Ready()
	{
		customerManager = GetNodeOrNull<CustomerManager>(CustomerManagerPath);
		customerUi = GetNodeOrNull<CustomerUi>(CustomerUiPath);

		if (customerManager == null)
		{
			GD.PushError($"GameManager needs a CustomerManager child at path: {CustomerManagerPath}");
		}

		if (customerUi == null)
		{
			GD.PushError($"GameManager needs a CustomerUi node at path: {CustomerUiPath}");
		}

		startOrder();
	}

	public override void _Process(double delta)
	{
		if (!isOnOrder)
		{
			return;
		}

		timer += (float)delta;
		EmitSignal(SignalName.OrderTimerChanged, RemainingOrderSeconds, currentCustomer.PatienceSeconds);

		if (timer >= currentCustomer.PatienceSeconds)
		{
			EndOrder(OrderResult.Timeout);
		}
	}

	public void StartOrder()
	{
		if (isOnOrder)
		{
			return;
		}

		if (customerManager == null)
		{
			GD.PushError("Cannot start order because CustomerManager is missing.");
			return;
		}

		currentCustomer = customerManager.GenerateNextCustomer();
		customerUi?.setTexture(currentCustomer.Name);
		timer = 0f;
		isOnOrder = true;

		GD.Print($"Customer {currentCustomer.Number} entered and ordered: {currentCustomer.Order.DisplayNameWithBrackets}");
		EmitSignal(SignalName.OrderStarted, currentCustomer.Order.DisplayNameWithBrackets, currentCustomer.PatienceSeconds);
		EmitSignal(SignalName.OrderTimerChanged, RemainingOrderSeconds, currentCustomer.PatienceSeconds);
	}

	public void startOrder()
	{
		StartOrder();
	}

	public void Serve(ProductExpression servedMenu)
	{
		if (!isOnOrder || currentCustomer == null)
		{
			return;
		}

		if (TeaRecipeBook.CanServe(servedMenu, currentCustomer.Order))
		{
			EndOrder(OrderResult.Success);
			return;
		}

		EndOrder(OrderResult.WrongMenu);
	}

	public void KickOutCustomer()
	{
		if (!isOnOrder)
		{
			return;
		}

		EndOrder(OrderResult.KickedOut);
	}

	private void EndOrder(OrderResult result)
	{
		if (currentCustomer == null)
		{
			isOnOrder = false;
			timer = 0f;
			return;
		}

		var orderName = currentCustomer.Order.DisplayNameWithBrackets;

		switch (result)
		{
			case OrderResult.Success:
				currentCustomer.Thank();
				break;
			case OrderResult.WrongMenu:
			case OrderResult.Timeout:
			case OrderResult.KickedOut:
				currentCustomer.Complain(result);
				break;
		}

		currentCustomer = null;
		isOnOrder = false;
		timer = 0f;

		EmitSignal(SignalName.OrderEnded, (int)result, orderName);
	}
}

public enum OrderResult
{
	Success,
	WrongMenu,
	Timeout,
	KickedOut,
}
