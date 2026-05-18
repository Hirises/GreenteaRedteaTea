class_name BorichaCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 60.0
	isSpecial = true
	isBad = true

func GetCustomerClassName() -> String:
	return "BorichaCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_impossible("보리차")
	isOrderGenerated = true
	return Order
