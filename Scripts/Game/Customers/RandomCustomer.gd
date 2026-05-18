class_name RandomCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0

func GetCustomerClassName() -> String:
	return "RandomCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product_by_max_length(mini(Number, 40))
	PatienceSeconds = Order.length * 5 + 15
	isOrderGenerated = true
	return Order
