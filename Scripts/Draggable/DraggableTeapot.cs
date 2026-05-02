using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableTeapot : Node2D, IDraggable
{
	[Export] Sprite2D liquidTop;
	[Export] Sprite2D liquidBottom;
	[Export] DragAreaTeapotInside insideArea;
	[Export] Vector2 dragOffset;
	[Export] AnimationPlayer animationPlayer;
	Vector2 originalPosition;
	bool hasContent = false;
	public bool HasContent => hasContent;
	ProductExpression liquidContent;
	public ProductExpression LiquidContent => liquidContent;

	public override void _Process(double delta)
	{
		if (InputManager.Instance?.currentDragItem == this)
		{
			var targetPosition = GetGlobalMousePosition() + dragOffset;
			Position = Position.Lerp(targetPosition, 20f * (float)delta);
			ZIndex = DraggableUtil.DragZIndex; // Ensure the dragged item is on top
		}
		else
		{
			Position = Position.Lerp(originalPosition, 10f * (float)delta);
			ZIndex = 0; // Reset ZIndex when not being dragged
		}

		liquidTop.Visible = hasContent;
		liquidBottom.Visible = hasContent;
		if (hasContent)
		{
			liquidTop.Modulate = liquidContent.Color.ToGodotColor();
			liquidBottom.Modulate = liquidContent.Color.ToGodotColor();
		}
	}

	public override void _Ready()
	{
		originalPosition = Position;
	}

	public void OnPick()
	{
		GD.Print("Teapot picked up!");
		SoundManager.Play(SFXType.TeapotPick);
	}

	public void OnDrop(DragArea dropArea)
	{
		if (dropArea is DragAreaContainer)
		{
			var container = dropArea as DragAreaContainer;
			if (hasContent && container.TryFill(liquidContent))
			{
				hasContent = false;
				liquidContent = null;
				SoundManager.Play(SFXType.TeapotPour);
			}
			else
			{
				GD.Print("Failed to pour teapot into container.");
			}
		}
		else if (dropArea is DragAreaTrash)
		{
			GD.Print("Teapot dropped into trash. Emptying teapot.");
			(dropArea as DragAreaTrash).OnTrash();
			hasContent = false;
			liquidContent = null;
			SoundManager.Play(SFXType.TeapotPour);
			return;
		}
		GD.Print($"Teapot dropped on {dropArea?.Name}!");
	}

	public void OnCancelDrag()
	{
		GD.Print("Teapot drag cancelled.");
	}

	public bool TryFill(ProductExpression liquid)
	{
		if (!liquid.Is(ProductCategory.Liquid))
		{
			GD.Print("Cannot fill teapot with non-liquid product.");
			return false;
		}
		if (hasContent)
		{
			var mix = new MixedLiquidExpression(liquidContent, liquid);
			liquidContent = mix;
			GD.Print($"Teapot already has content. Mixed to {mix.DisplayName}.");
			animationPlayer.Play("shake");
			return true;
		}
		liquidContent = liquid;
		hasContent = true;
		GD.Print($"Teapot filled with {liquid.DisplayName}.");
		animationPlayer.Play("shake");
		return true;
	}
	
	public bool CanBrew()
	{
		return hasContent & insideArea.HasLeaf();
	}

	public bool TryBrew()
	{
		if (!hasContent)
		{
			GD.Print("Cannot brew tea with an empty teapot.");
			return false;
		}
		if (!insideArea.HasLeaf())
		{
			GD.Print("Cannot brew tea without a leaf inside the teapot.");
			return false;
		}

		var leaf = insideArea.GetLeaf();
		var brewed = new TeaExpression(leaf, liquidContent);
		insideArea.SetLeaf(new BrewedLeafExpression(leaf, liquidContent));
		liquidContent = brewed;
		GD.Print($"Brewed tea in teapot. Now contains {brewed.DisplayName}.");
		
		animationPlayer.Play("shake");
		SoundManager.Play(SFXType.TeapotBrew);

		return true;
	}
}
