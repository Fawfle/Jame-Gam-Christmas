using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
	public float jumpBuffer = 0.1f;
	public Vector2 MoveInput { get; private set; } = Vector2.zero;
	public bool JumpInput { get; private set; } = false;
	public bool JumpReleased { get; private set; } = false;
	public void OnMoveInput(InputAction.CallbackContext context)
	{
		MoveInput = Vector2Int.RoundToInt(context.ReadValue<Vector2>().normalized);
	}

	public void OnJumpInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			JumpInput = true;
			JumpReleased = false;
			StartCoroutine(UseJumpCoroutine());
		}
		else if (context.canceled) JumpReleased = true;
	}

	public void OnResetInput(InputAction.CallbackContext context)
	{
		if (context.started && !LevelManager.Instance.transitioning) LevelManager.Instance.ResetScene();
	}

	public void UseJumpInput()
	{
		StopCoroutine(UseJumpCoroutine());
		JumpInput = false;
	}

	private IEnumerator UseJumpCoroutine()
	{
		yield return new WaitForSeconds(jumpBuffer);
		JumpInput = false;
	}
}
