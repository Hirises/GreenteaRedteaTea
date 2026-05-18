class_name DefaultCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0

func GetCustomerClassName() -> String:
	return "DefaultCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product(Number)
	isOrderGenerated = true
	return Order
