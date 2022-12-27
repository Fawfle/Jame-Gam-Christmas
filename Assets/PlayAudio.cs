using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();
    
    public void Play(int index) => audioSources[index].Play();
}
