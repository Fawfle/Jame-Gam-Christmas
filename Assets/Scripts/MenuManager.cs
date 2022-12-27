    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public float fallAnimationDuration;
	public Animator santaAnim;
	public Animator creditsAnim;
	public Animator levelSelectAnim;
	public Animator titleAnim;

	public List<Animator> bottomAnims;

    public void StartGame(int index)
	{
		StartCoroutine(StartGameRoutine(index));
	}

	IEnumerator StartGameRoutine(int index, bool levelSelect = false)
	{
		if (!levelSelect) SlideOutBottomUI();
		else levelSelectAnim.SetTrigger("Slide Out");
		titleAnim.SetTrigger("Slide Out");

		yield return new WaitForSeconds(1f);
		// START ANIMATION
		santaAnim.SetTrigger("Play");

		yield return new WaitForSeconds(fallAnimationDuration);
		LevelManager.Instance.LoadScene(index);
	}

	public void StartGameLevelSelect(int index)
	{
		StartCoroutine(StartGameRoutine(index, true));
	}

	public void SlideOutBottomUI()
	{
		StartCoroutine(SlideOutBottomUIRoutine());
	}

	private IEnumerator SlideOutBottomUIRoutine()
	{
		foreach (Animator anim in bottomAnims)
		{
			anim.SetTrigger("Slide Out");
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void SlideInBottomUI()
	{
		StartCoroutine(SlideInBottomUIRoutine());
	}

	private IEnumerator SlideInBottomUIRoutine()
	{
		for (int i = bottomAnims.Count - 1; i >= 0; i--)
		{
			bottomAnims[i].SetTrigger("Slide In");
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void GoToLevelSelect()
	{
		StartCoroutine(GoToLevelSelectRoutine());
	}

	private IEnumerator GoToLevelSelectRoutine()
	{
		StartCoroutine(SlideOutBottomUIRoutine());
		yield return new WaitForSeconds(0.5f);
		levelSelectAnim.SetTrigger("Slide In");
	}

	public void GoBackFromLevelSelect()
	{
		StartCoroutine(GoBackFromLevelSelectRoutine());
	}

	private IEnumerator GoBackFromLevelSelectRoutine()
	{
		levelSelectAnim.SetTrigger("Slide Out");
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(SlideInBottomUIRoutine());
	}

	public void SlideCreditsMenu(bool slideIn) => creditsAnim.SetTrigger(slideIn ? "Slide In" : "Slide Out");
}
