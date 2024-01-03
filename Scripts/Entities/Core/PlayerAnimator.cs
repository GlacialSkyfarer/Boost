using Godot;
using System;

public partial class PlayerAnimator : AnimationTree
{

	[Export]
	public NodePath playerPath;

	public Player player;

	float groundedBlendFactor = 0;
	float aerialBlendFactor = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		player = GetNode<Player>(playerPath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

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

	}
}