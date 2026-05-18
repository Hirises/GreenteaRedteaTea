class_name ClickAreaCandle
extends ClickArea

@export var candleMaxTime: float = 3.0
@export var teapot: DraggableTeapot
@export var flameOn: Node2D
@export var flameOff: Node2D
@export var particle: GPUParticles2D
var candleTime := 0.0
var isLit := false

func OnClick() -> void:
	if isLit:
		isLit = false
		SoundManager.Stop(SFXType.TEAPOT_BREWING)
		SoundManager.Play(SFXType.CANDLE_EXTINGUISH)
		print("Extinguishing candle %s." % name)
		_set_flame_state(false)
		return
	if !teapot.CanBrew():
		print("Cannot lit candle %s because teapot is unable to brew." % name)
		SoundManager.Play(SFXType.CANDLE_LIT_FAIL)
		return
	print("Lit candle %s!" % name)
	isLit = true
	candleTime = 0.0
	_set_flame_state(true)
	SoundManager.Play(SFXType.CANDLE_LIT)
	SoundManager.Play(SFXType.TEAPOT_BREWING)

func _process(delta: float) -> void:
	super._process(delta)
	if isLit:
		candleTime += delta
		if candleTime >= candleMaxTime:
			isLit = false
			print("Candle %s has burned out. Trying brew the tea in teapot..." % name)
			teapot.TryBrew()
			_set_flame_state(false)
			return
		if InputManager.Instance.currentDragItem == teapot:
			isLit = false
			SoundManager.Stop(SFXType.TEAPOT_BREWING)
			print("Teapot is lifted while candle %s is lit. Extinguishing candle." % name)
			_set_flame_state(false)

func _set_flame_state(lit: bool) -> void:
	flameOff.visible = !lit
	flameOn.visible = lit
	particle.emitting = lit

func CanClick() -> bool:
	return teapot.CanBrew()
