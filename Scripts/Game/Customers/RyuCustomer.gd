class_name RyuCustomer
extends Customer

var arange := [[15, 18], [22, 25], [30, 33]]
var c := 0

func _init(number: int = 0, count: int = 0) -> void:
	super(number)
	PatienceSeconds = 30.0
	isSpecial = true
	c = count

func GetCustomerClassName() -> String:
	return "RyuCustomer"

func _GenerateOrder() -> ProductExpression:
	var orderGenerator := TeaOrderGenerator.new()
	Order = orderGenerator.generate_product(5)
	while Order.length < arange[c][0] or arange[c][1] < Order.length:
		Order = orderGenerator.generate_product(5)
	PatienceSeconds = Order.length * 15
	isOrderGenerated = true
	return Order
