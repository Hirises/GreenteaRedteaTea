class_name Customer
extends RefCounted

var isOrderGenerated := false
var Number: int
var Name: String = "Default"
var isSpecial := false
var isBad := false
var Order: ProductExpression
var PatienceSeconds: float

func _init(number: int = 0) -> void:
	Number = number
	Name = CustomerNameBook.GetName(GetCustomerClassName())

func GetCustomerClassName() -> String:
	return "Customer"

func GenerateOrder() -> ProductExpression:
	if isOrderGenerated:
		return Order
	return _GenerateOrder()

func _GenerateOrder() -> ProductExpression:
	return null

func SayOrder() -> String:
	return CustomerDialogueBook.GetOrder(GetCustomerClassName(), GetOrderName())

func Thank() -> String:
	return CustomerDialogueBook.GetThank(GetCustomerClassName())

func Complain(result: int) -> String:
	return CustomerDialogueBook.GetComplaint(GetCustomerClassName(), result)

func GetOrderName() -> String:
	return "\"" + Order.display_name + "\""
