class_name PutinCustomer
extends Customer

func _init(number: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0
	isSpecial = true

func GetCustomerClassName() -> String:
	return "PutinCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	var rules := TeaOrderGenerationRules.for_leaf(BasicLeafKind.BLACK)
	Order = orderGenerator.generate_product(2, rules)
	while Order.length <= 3:
		Order = orderGenerator.generate_product(2, rules)
	PatienceSeconds = Order.length * 5 + 10
	isOrderGenerated = true
	return Order
