using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UEvent : MonoBehaviour
{
    public UnityEvent action;

    public void Run()
	{
		action?.Invoke();
	}
}
