using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement Instance { get; private set; }

	[HideInInspector] public Rigidbody2D rb;
	private Animator anim;
	private BoxCollider2D coll;
	[HideInInspector] public SpriteRenderer sr;
	private PlayerInputHandler inputHandler;
	public int facingDirection { get; private set; }

	public float moveSpeed = 10f;
	public float acceleration = 5f;

	public float jumpHeight = 15f;
	public const float coyoteTime = 0.15f;
	public float fallSpeedMultiplier = 1f;
	public float maxFallSpeed = 8f;
	public float variableJumpMultiplier = 0.4f;

	private bool isJumping;
	private float coyoteTimer = 0;
	[HideInInspector] public bool movementDisabled = false;

	private bool grounded;

	private string currentAnimation;

	public LayerMask groundLayer;
	public AudioSource jumpSound;
	public AudioSource moveSound;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else Instance = this;

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		coll = GetComponent<BoxCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		inputHandler = GetComponent<PlayerInputHandler>();

		facingDirection = 1;

		anim.SetBool("idle", true);
		currentAnimation = "idle";
	}

	private void Update()
	{
		if (movementDisabled) return;
		if (coyoteTimer > 0) coyoteTimer -= Time.deltaTime;

		HandleAnimations();
	}

	private void FixedUpdate()
	{
		if (movementDisabled) return;
		grounded = IsGrounded();

		if (grounded && inputHandler.MoveInput.x != 0 && !moveSound.isPlaying) moveSound.Play();
		if (inputHandler.MoveInput.x == 0 || !grounded) moveSound.Stop();

		if (inputHandler.JumpInput && CanJump()) Jump();
		if (isJumping) CheckVariableJumpHeight();

		// Horizontal Movement
		MoveTowardsVelocity(new Vector2(inputHandler.MoveInput.x * moveSpeed, rb.velocity.y), acceleration * Time.fixedDeltaTime);
		SetFacingDirectionToVelocity();

		if (grounded && rb.velocity.y <= 0.1f) coyoteTimer = coyoteTime;

		// Multiply and Clamp Fallspeed
		if (rb.velocity.y < 0)
		{
			SetVelocityY(rb.velocity.y * (1 + fallSpeedMultiplier * Time.fixedDeltaTime));
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpeed, float.MaxValue));
		}
	}

	void HandleAnimations()
	{
		string newAnimation;

		if (grounded)
		{
			if (inputHandler.MoveInput.x == 0) newAnimation = "idle";
			else newAnimation = "running";
		}
		else newAnimation = "air";

		SwitchAnimation(newAnimation);
	}

	public void Jump()
	{
		inputHandler.UseJumpInput();
		SetVelocityY(jumpHeight);
		isJumping = true;

		jumpSound.pitch = Random.Range(0.9f, 1.2f);
		jumpSound.Play();
		coyoteTimer = 0;
	}
	public void CheckVariableJumpHeight()
	{
		if (rb.velocity.y <= 0f) isJumping = false;
		else if (inputHandler.JumpReleased)
		{
			SetVelocityY(rb.velocity.y * variableJumpMultiplier);
			isJumping = false;
		}
	}
	public bool CanJump() => (coyoteTimer > 0);

	public bool IsGrounded()
	{
		return Physics2D.OverlapBox(new Vector2(coll.bounds.center.x, coll.bounds.min.y), new Vector2(coll.bounds.size.x - 0.05f, 0.05f), 0, groundLayer);
	}

	public void SwitchAnimation(string animBoolName)
	{
		if (currentAnimation == animBoolName) return;
		anim.SetBool(currentAnimation, false);
		anim.SetBool(animBoolName, true);
		currentAnimation = animBoolName;
	}

	#region Setters
	private void SetVelocityX(float vel) => rb.velocity = new Vector2(vel, rb.velocity.y);
	private void SetVelocityY(float vel) => rb.velocity = new Vector2(rb.velocity.x, vel);
	private void MoveTowardsVelocity(Vector2 target, float speed) => rb.velocity = Vector2.MoveTowards(rb.velocity, target, speed);

	public void SetFacingDirection(float direction)
	{
		direction = Mathf.Sign(direction);
		if (direction == 0f) return;
		facingDirection = (int)direction;
		sr.flipX = direction == -1f;
	}
	public void SetFacingDirectionToVelocity()
	{
		if (rb.velocity.x != 0) SetFacingDirection(Mathf.Sign(rb.velocity.x));
	}
	public void DisableMovement()
	{
		movementDisabled = true;
		moveSound.Stop();
		SwitchAnimation("idle");
	}
	#endregion
}
