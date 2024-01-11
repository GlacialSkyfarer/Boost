using Godot;
using System;
using System.Linq.Expressions;

public enum PlayerState
{
	Idle,
	Walking,
	Jumping,
	Falling
}

public partial class Player : Actor
{
	
	#region References

	[ExportCategory("References")]

	[Export]
	public NodePath vmHolderPath;
	public Node3D vmHolder;

	#endregion

	#region Variables

	[Export]
	public PlayerState currentState = PlayerState.Idle;

	[ExportCategory("Movement")]
	[ExportGroup("Horizontal Movement")]

	[Export]
	public float movementSpeed = 20f;
	[Export]
	public float groundTraction = 1f;
	[Export]
	public float airTraction = 0.3f;

	[ExportGroup("Vertical Movement")]

	[ExportSubgroup("Jumping")]
	[Export]
	public float jumpHeight = 50f;
	[Export]
	public float maximumCoyoteTime = 0.2f;
	[Export]
	public float maximumJumpBuffer = 0.2f;
	float coyoteTime = 0f;
	float jumpBuffer = 0f;

	[ExportGroup("Gunplay")] //yokoi
	[Export]
	public Godot.Collections.Array<string> gunsOwned;

	public Gun currentGun;
	public int currentGunIndex = 0;
	public Godot.Collections.Array<Gun> guns;
	Godot.Collections.Array<string> gunsSpawned;

    #endregion

    public override void _Ready()
    {

		coyoteTime = 0f;
		jumpBuffer = 0f;
		Velocity = Vector3.Zero;
		vmHolder = GetNode<Node3D>(vmHolderPath);
		gunsSpawned = new();
		guns = new();

		foreach (string gun in gunsOwned) 
		{

			Gun g = GD.Load<PackedScene>("res://Scenes/Weapons/" + gun + ".tscn").Instantiate<Gun>();
			vmHolder.AddChild(g);
			guns.Add(g);
			gunsSpawned.Add(gun);

		}

		if (guns.Count > 0)  
		{
			
			currentGun = guns[0];
			currentGun.current = true;

		}

		currentGunIndex = 0;

    }

    public override void _Process(double delta)
    {

		coyoteTime = Mathf.Max(0f, coyoteTime - (float)delta);
		jumpBuffer = Mathf.Max(0f, jumpBuffer - (float)delta);

		if (gunsOwned.Count > gunsSpawned.Count) {

			for (int i = 1; i < gunsOwned.Count - gunsSpawned.Count; i++) 
			{

				Gun g = GD.Load<PackedScene>("res://Scenes/Weapons/" + gunsOwned[gunsOwned.Count - i] + ".tscn").Instantiate<Gun>();
				vmHolder.AddChild(g);
				guns.Add(g);
				gunsSpawned.Add(gunsOwned[i]);

			}

		}

		if (Input.IsActionJustReleased("ScrollUp") && guns.Count > 1) {

			currentGun.current = false;
			currentGunIndex = Mathf.Wrap(currentGunIndex - 1, 0, guns.Count);
			currentGun = guns[currentGunIndex];
			currentGun.current = true;
			currentGun.SwapIn();

		}
		if (Input.IsActionJustReleased("ScrollDown") && guns.Count > 1) {

			currentGun.current = false;
			currentGunIndex = Mathf.Wrap(currentGunIndex + 1, 0, guns.Count);
			currentGun = guns[currentGunIndex];
			currentGun.current = true;
			currentGun.SwapIn();

		}

		if (Input.IsActionJustPressed("Jump")) jumpBuffer = maximumJumpBuffer;
		if (Input.IsActionPressed("Fire") && currentGun != null && currentGun.fCool <= 0 && !currentGun.overheated) currentGun.Fire(this);
		if (Input.IsActionPressed("AltFire") && currentGun != null && currentGun.altFCool <= 0 && !currentGun.overheated) currentGun.AltFire(this);

    }

    public override void _PhysicsProcess(double delta)
    {
        
		Vector3 velocity = Velocity;
		Vector2 inputVector = Input.GetVector("Left", "Right", "Forward", "Back").Normalized();

		Vector3 movementDirection = inputVector.X * Transform.Basis.X + inputVector.Y * Transform.Basis.Z;

		switch(currentState) {

			case PlayerState.Idle:
			{

				if (IsOnFloor()) {

					coyoteTime = maximumCoyoteTime;

				}

				if (coyoteTime <= 0f) {

					currentState = PlayerState.Falling;

				} else {

					if (jumpBuffer > 0f) {

						velocity.Y = jumpHeight;
						jumpBuffer = 0f;
						coyoteTime = 0f;
						currentState = PlayerState.Jumping;

					} else if (inputVector != Vector2.Zero) {

						currentState = PlayerState.Walking;

					} else {

						velocity = velocity.Lerp(new(0, velocity.Y, 0), 0.1f * groundTraction);

					}

				}				
				
				break;
			}
			case PlayerState.Walking:
			{

				if (IsOnFloor()) coyoteTime = maximumCoyoteTime;

				if (coyoteTime <= 0f) {

					currentState = PlayerState.Falling;

				} else {

					if (jumpBuffer > 0f) {

						velocity.Y = jumpHeight;
						jumpBuffer = 0f;
						coyoteTime = 0f;
						currentState = PlayerState.Jumping;
					
					} else if (inputVector == Vector2.Zero) {

						currentState = PlayerState.Idle;

					} else {

						velocity = velocity.Lerp(new(movementDirection.X * movementSpeed, velocity.Y, movementDirection.Z * movementSpeed), 0.1f * groundTraction);

					}
				} 

				break;
			}
			case PlayerState.Jumping:
			{

				velocity.Y += gravity * (float)delta;

				if (IsOnFloor()) {

					currentState = PlayerState.Idle;

				} else {

					velocity = velocity.Lerp(new(movementDirection.X * movementSpeed, velocity.Y, movementDirection.Z * movementSpeed), 0.1f * airTraction);

					if (velocity.Y <= 0f)
					{

						currentState = PlayerState.Falling;

					} else if (Input.IsActionJustReleased("Jump")) {


						velocity.Y *= 0.5f;
						currentState = PlayerState.Falling;

					}

				}

				break;
			}
			case PlayerState.Falling:
			{

				velocity.Y += gravity * (float)delta;

				if (IsOnFloor()) {

					currentState = PlayerState.Idle;

				} else {

					velocity = velocity.Lerp(new(movementDirection.X * movementSpeed, velocity.Y, movementDirection.Z * movementSpeed), 0.1f * airTraction);

				}

				break;
			}

			default:
			{
				GD.PrintErr("Invalid player state!");
				currentState = PlayerState.Idle;
				break;
			}

		}

		Velocity = velocity;

		MoveAndSlide();

    }

}
