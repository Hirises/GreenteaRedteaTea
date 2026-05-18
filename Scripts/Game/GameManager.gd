class_name GameManager
extends Node

signal OrderStarted(orderName: String, timeLimitSeconds: float)
signal OrderTimerChanged(remainingSeconds: float, timeLimitSeconds: float)
signal OrderEnded(result: int, orderName: String)

@export var CustomerManagerPath: NodePath = "CustomerManager"
@export var CustomerUiPath: NodePath = "../CustomerUI"

var customerManager: CustomerManager
var customerUi: CustomerUi
var isOnOrder := false
var currentCustomer: Customer
var timer := 0.0
var isHard := false
var rating := 0
var score := 0
var gameover := false

var CurrentOrder: ProductExpression:
	get: return currentCustomer.Order if currentCustomer != null else null
var IsOnOrder: bool:
	get: return isOnOrder
var RemainingOrderSeconds: float:
	get: return maxf(currentCustomer.PatienceSeconds - timer, 0.0) if currentCustomer != null else 0.0

func _ready() -> void:
	customerManager = get_node_or_null(CustomerManagerPath)
	customerUi = get_node_or_null(CustomerUiPath)
	if customerManager == null:
		push_error("GameManager needs a CustomerManager child at path: %s" % CustomerManagerPath)
	if customerUi == null:
		push_error("GameManager needs a CustomerUi node at path: %s" % CustomerUiPath)

func _process(delta: float) -> void:
	if !isOnOrder:
		return
	timer += delta
	emit_signal("OrderTimerChanged", RemainingOrderSeconds, currentCustomer.PatienceSeconds)
	if timer >= currentCustomer.PatienceSeconds:
		_EndOrder(OrderResult.TIMEOUT)

func StartGame() -> void:
	rating = 5
	score = 0
	customerManager.init()
	_StartOrder()
	SoundManager.Instance.PlayIngameBGM()

func _GameOver() -> void:
	gameover = true

func _StartOrder() -> void:
	if isOnOrder:
		return
	if customerManager == null:
		push_error("Cannot start order because CustomerManager is missing.")
		return
	currentCustomer = customerManager.GenerateNextCustomer(score, rating)
	if customerUi != null:
		customerUi.setCustomer(currentCustomer)
	timer = 0.0
	isOnOrder = true
	if customerUi != null:
		customerUi.Appear()
		customerUi.sayText(currentCustomer.SayOrder())
	print("Customer %s entered and ordered: %s" % [currentCustomer.Number, currentCustomer.Order.display_name_with_brackets])
	emit_signal("OrderStarted", currentCustomer.Order.display_name_with_brackets, currentCustomer.PatienceSeconds)
	emit_signal("OrderTimerChanged", RemainingOrderSeconds, currentCustomer.PatienceSeconds)
	SoundManager.Play(SFXType.CUSTOMER_ENTER)
	SoundManager.Play(SFXType.DOOR_OPEN)

func startOrder() -> void:
	_StartOrder()

func Serve(servedMenu: ProductExpression) -> void:
	if !isOnOrder or currentCustomer == null:
		return
	if TeaRecipeBook.can_serve(servedMenu, currentCustomer.Order):
		_EndOrder(OrderResult.SUCCESS)
		return
	_EndOrder(OrderResult.WRONG_MENU)

func CanKick() -> bool:
	return isOnOrder and timer >= 1.2

func KickOutCustomer() -> void:
	if !CanKick():
		return
	_EndOrder(OrderResult.KICKED_OUT)

func _EndOrder(result: int) -> void:
	if currentCustomer == null:
		isOnOrder = false
		timer = 0.0
		return
	var orderName := currentCustomer.Order.display_name_with_brackets
	var text := ""
	match result:
		OrderResult.SUCCESS:
			rating = mini(10, rating + 1)
			score += 1
			text = currentCustomer.Thank()
			SoundManager.Play(SFXType.SUCCESS)
		OrderResult.WRONG_MENU, OrderResult.TIMEOUT:
			rating = maxi(0, rating - 2)
			text = currentCustomer.Complain(result)
			SoundManager.Play(SFXType.FAIL)
		OrderResult.KICKED_OUT:
			if currentCustomer.isBad:
				score += 1
			else:
				rating = maxi(0, rating - 2)
			text = currentCustomer.Complain(result)
			SoundManager.Play(SFXType.FAIL)
	if rating == 0:
		_GameOver()
	currentCustomer = null
	isOnOrder = false
	timer = 0.0
	if customerUi != null:
		customerUi.Disappear()
		customerUi.sayText(text)
	emit_signal("OrderEnded", result, orderName)

func NextOrder() -> void:
	if rating == 0:
		_GameOver()
		return
	startOrder()
