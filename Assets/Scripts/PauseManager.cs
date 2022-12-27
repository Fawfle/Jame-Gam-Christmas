using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseMenu;
    public AudioMixer playerAudio;

	private void Start()
	{
        playerAudio.SetFloat("Volume", 0f);
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
		{
            if (paused) UnPause();
            else Pause();
		}
    }

    public void Pause()
	{
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        playerAudio.SetFloat("Volume", -80f);

        paused = true;
	}

    public void QuitToMenu()
	{
        LevelManager.Instance.LoadScene(0);
        Destroy(GameObject.FindGameObjectWithTag("Game Music"));
	}

    public void UnPause()
	{
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        playerAudio.SetFloat("Volume", 0f);

        paused = false;
    }
}
