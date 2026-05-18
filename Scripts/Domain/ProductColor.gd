class_name ProductColor

var r: float
var g: float
var b: float
var a: float

func _init(p_r: float = 0.0, p_g: float = 0.0, p_b: float = 0.0, p_a: float = 1.0) -> void:
	r = p_r
	g = p_g
	b = p_b
	a = p_a

static func from_rgb255(p_r: float, p_g: float, p_b: float, p_a: float) -> ProductColor:
	return ProductColor.new(p_r / 255.0, p_g / 255.0, p_b / 255.0, p_a).clamped()

func with_alpha(alpha: float) -> ProductColor:
	return ProductColor.new(r, g, b, alpha).clamped()

func clamped() -> ProductColor:
	return ProductColor.new(clampf(r, 0.0, 1.0), clampf(g, 0.0, 1.0), clampf(b, 0.0, 1.0), clampf(a, 0.0, 1.0))

func to_godot_color() -> Color:
	return Color(r, g, b, a)
