using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

	public Animator anim;
	public float transitionTime;

	public bool transitioning;

	private void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadScene(int index)
	{
		StartCoroutine(TransitionScene(index));
	}

	public void LoadNextScene()
	{
		StartCoroutine(TransitionScene(SceneManager.GetActiveScene().buildIndex + 1));
	}
	public void ResetScene()
	{
		StartCoroutine(TransitionScene(SceneManager.GetActiveScene().buildIndex));
	}

	private IEnumerator TransitionScene(int buildIndex)
	{
		// play
		anim.SetTrigger("Load");
		// wait
		transitioning = true;
		yield return new WaitForSeconds(transitionTime);
		transitioning = false;
		// load
		SceneManager.LoadScene(buildIndex);
	}
}
