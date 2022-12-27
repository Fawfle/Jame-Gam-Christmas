using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameMusic : MonoBehaviour
{
	private void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Game Music");
		if (go != null) Destroy(go);
	}
}
