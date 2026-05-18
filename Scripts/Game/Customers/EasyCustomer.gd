class_name EasyCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0

func GetCustomerClassName() -> String:
	return "EasyCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product_by_max_length(2)
	PatienceSeconds = Order.length * 10 + 10
	isOrderGenerated = true
	return Order
