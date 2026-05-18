class_name HardCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0

func GetCustomerClassName() -> String:
	return "HardCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product_by_max_length(8)
	PatienceSeconds = Order.length * 5 + 15
	isOrderGenerated = true
	return Order
