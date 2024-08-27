using Godot;
using System;

public partial class PlayerAnimator : AnimationTree
{

	[ExportGroup("References")]
	[Export]
	public NodePath cameraAnimatorPath;
	public AnimationTree cameraAnimator;

	[Export]
	public NodePath playerPath;

	public Player player;

	[ExportGroup("Properties")]

	[Export]
	public bool strafeAnimation = true;
	float strafeDirection = 0;

	float groundedBlendFactor = 0;
	float aerialBlendFactor = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		cameraAnimator = GetNode<AnimationTree>(cameraAnimatorPath);

		player = GetNode<Player>(playerPath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Vector2 inputVector = Input.GetVector("Left", "Right", "Forward", "Back");
		
		if (strafeAnimation) {

			strafeDirection = Mathf.Lerp(strafeDirection, inputVector.X, 0.1f);

		} else {

			strafeDirection = 0;

		}

		switch(player.currentState)
		{

			case PlayerState.Idle:
			{

				groundedBlendFactor = Mathf.Lerp(groundedBlendFactor, 0, 0.1f);
				aerialBlendFactor = Mathf.Lerp(aerialBlendFactor, 0, 0.1f);

				break;

			}

			case PlayerState.Walking:
			{

				groundedBlendFactor = Mathf.Lerp(groundedBlendFactor, 1, 0.1f);
				aerialBlendFactor = Mathf.Lerp(aerialBlendFactor, 0, 0.1f);

				break;

			}

			case PlayerState.Jumping:
			{

				groundedBlendFactor = Mathf.Lerp(groundedBlendFactor, 0, 0.1f);
				aerialBlendFactor = Mathf.Lerp(aerialBlendFactor, 1, 0.1f);

				break;

			}

			case PlayerState.Falling:
			{

				groundedBlendFactor = Mathf.Lerp(groundedBlendFactor, 0, 0.1f);
				aerialBlendFactor = Mathf.Lerp(aerialBlendFactor, -1, 0.1f);

				break;

			}

		}

		Set("parameters/GroundedBlend/blend_amount", groundedBlendFactor);
		Set("parameters/AerialBlend/blend_amount", aerialBlendFactor);
		cameraAnimator.Set("parameters/Strafe/blend_amount", strafeDirection);
	}
}
