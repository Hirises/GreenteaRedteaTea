class_name CustomerManager
extends Node

var customerCount := 0
var easyBorichaScore := 0
var normalBorichaScore := 0
var hardBorichaScore := 0
var _random := RandomNumberGenerator.new()
var CustomerCount: int:
	get: return customerCount

func init() -> void:
	_random.randomize()
	easyBorichaScore = _random.randi_range(4, 7)
	normalBorichaScore = _random.randi_range(10, 15)
	hardBorichaScore = _random.randi_range(18, 27)

func GenerateNextCustomer(score: int, rating: int) -> Customer:
	customerCount += 1
	var customer: Customer
	if score <= 7:
		customer = BorichaCustomer.new(customerCount) if score == easyBorichaScore else EasyCustomer.new(customerCount)
	elif score == 8:
		customer = PutinCustomer.new(customerCount)
	elif score <= 15:
		customer = BorichaCustomer.new(customerCount) if score == normalBorichaScore else NormalCustomer.new(customerCount)
	elif score == 16:
		customer = MiniBossCustomer.new(customerCount)
	elif score <= 29:
		customer = BorichaCustomer.new(customerCount) if score == hardBorichaScore else HardCustomer.new(customerCount)
	elif score <= 32:
		customer = RyuCustomer.new(customerCount, score - 30)
	else:
		customer = BadCustomer.new(customerCount) if _random.randi_range(0, 9) == 0 else RandomCustomer.new(customerCount)
	customer.GenerateOrder()
	return customer
