using System;
using System.Threading.Tasks;
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
	public bool isHard {get; protected set;} = false;
	public int rating {get; protected set;}
	public int score {get; protected set;}

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

	public void StartGame()
	{
		rating = 5;
		score = 29;
		customerManager.init();
		StartOrder();
	}

	private void GameOver()
	{
		
	}

	private void StartOrder()
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

		currentCustomer = customerManager.GenerateNextCustomer(score, rating);
		customerUi?.setCustomer(currentCustomer);
		timer = 0f;
		isOnOrder = true;
		customerUi?.Appear();
		customerUi?.sayText(currentCustomer.SayOrder());

		GD.Print($"Customer {currentCustomer.Number} entered and ordered: {currentCustomer.Order.DisplayNameWithBrackets}");
		EmitSignal(SignalName.OrderStarted, currentCustomer.Order.DisplayNameWithBrackets, currentCustomer.PatienceSeconds);
		EmitSignal(SignalName.OrderTimerChanged, RemainingOrderSeconds, currentCustomer.PatienceSeconds);
		
		SoundManager.Play(SFXType.CustomerEnter);
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

	public bool CanKick()
	{
		return isOnOrder && timer >= 1.2f;
	}

	public void KickOutCustomer()
	{
		if (!CanKick())
			return;

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
		string text = "";

		switch (result)
		{
			case OrderResult.Success:
				rating = Math.Min(10, rating + 1);
				score++;
				text = currentCustomer.Thank();
				SoundManager.Play(SFXType.Success);
				break;
			case OrderResult.WrongMenu:
			case OrderResult.Timeout:
				rating = Math.Max(0, rating - 2);;
				text = currentCustomer.Complain(result);
				SoundManager.Play(SFXType.Fail);
				break;
			case OrderResult.KickedOut:
				if (currentCustomer.isBad) score++;
				else rating = Math.Max(0, rating - 2);;
				text = currentCustomer.Complain(result);
				SoundManager.Play(SFXType.Fail);
				break;
		}

		currentCustomer = null;
		isOnOrder = false;
		timer = 0f;

		customerUi?.Disappear();
		customerUi?.sayText(text);

		EmitSignal(SignalName.OrderEnded, (int)result, orderName);
	}

	public void NextOrder()
	{
		if(rating == 0)
		{
			GameOver();
			return;
		}
		startOrder();
	}
}

public enum OrderResult
{
	Success,
	WrongMenu,
	Timeout,
	KickedOut,
}
