class_name MiniBossCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0
	isSpecial = true

func GetCustomerClassName() -> String:
	return "MiniBossCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product_by_max_length(12)
	while Order.length < 8:
		Order = orderGenerator.generate_product_by_max_length(12)
	PatienceSeconds = Order.length * 5 + 15
	isOrderGenerated = true
	return Order
