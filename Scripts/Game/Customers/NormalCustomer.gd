class_name NormalCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0

func GetCustomerClassName() -> String:
	return "NormalCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product_by_max_length(5)
	PatienceSeconds = Order.length * 5 + 10
	isOrderGenerated = true
	return Order
