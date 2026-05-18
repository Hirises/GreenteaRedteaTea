class_name BadCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 60.0
	isBad = true

func GetCustomerClassName() -> String:
	return "BadCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_invalid()
	isOrderGenerated = true
	return Order
