using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
	public float endMoveToSleighDurationSeconds, endMoveSleighDurationSeconds;
	public AudioSource winSound;

	private SpriteRenderer sr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		//sr.sortingOrder = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Win")) return;

		PlayerMovement.Instance.DisableMovement();

		StartCoroutine(EndAnimation(collision.gameObject));
	}

	private IEnumerator EndAnimation(GameObject present)
	{
		float elapsed = 0;
		sr.sortingOrder = 1;

		present.transform.rotation = Quaternion.Euler(Vector3.zero);
		present.GetComponent<Rigidbody2D>().simulated = false;

		PlayerMovement.Instance.rb.simulated = false;
		PlayerMovement.Instance.SetFacingDirection(1);
		PlayerMovement.Instance.SwitchAnimation("air");

		Vector3 startPresentPosition = present.transform.position;
		Vector3 endPresentPosition = transform.position + (Vector3)present.GetComponent<Present>().endAnimationPosition;

		Vector3 startPlayerPosition = PlayerMovement.Instance.transform.position;
		Vector3 endPlayerPosition = transform.position + new Vector3(0.5f, 0);

		winSound.Play();

		// Move present and player to sleigh
		while (elapsed < endMoveToSleighDurationSeconds)
		{
			elapsed += Time.deltaTime;

			float t = elapsed / endMoveToSleighDurationSeconds;
			t = t * t * (3f - 2f * t);

			present.transform.position = Vector3.Lerp(startPresentPosition, endPresentPosition, t);
			PlayerMovement.Instance.transform.position = Vector3.Lerp(startPlayerPosition, endPlayerPosition, t);

			yield return null;
		}

		PlayerMovement.Instance.transform.SetParent(transform);
		present.transform.SetParent(transform);

		Vector3 startSleighPosition = transform.position;
		Vector3 endSleighPosition = transform.position + new Vector3(6f, 4f);

		StartCoroutine(NextLevel());
		PlayerMovement.Instance.SwitchAnimation("idle");
		GetComponent<Animator>().Play("Flying");

		// move sleigh offscreen
		elapsed = 0;
		while (elapsed < endMoveSleighDurationSeconds)
		{
			elapsed += Time.deltaTime;

			float t = elapsed / endMoveSleighDurationSeconds;
			t = t * t;

			transform.position = Vector3.Lerp(startSleighPosition, endSleighPosition, t);

			yield return null;
		}

		IEnumerator NextLevel()
		{
			yield return new WaitForSeconds(0.25f);
			LevelManager.Instance.LoadNextScene();
		}
	}
}
