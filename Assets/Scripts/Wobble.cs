using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float rotationSpeed = 1, scaleSpeed = 0.1f;

    public float maxAngle = 4;
	public float minScale = 1;
	public float maxScale = 1.2f;

	float scaleT = 0, rotateT = 0;

	public float rotateOffset, scaleOffset;
	public bool randomOffset = false;

	private void Start()
	{
		rotateT += rotateOffset;
		scaleT += scaleOffset;

		if (randomOffset)
		{
			rotateT += Random.Range(0f, 2f);
			scaleT += Random.Range(0f, 2f);
		}

	}

	private void Update()
	{
		rotateT += Time.deltaTime * rotationSpeed;
		scaleT += Time.deltaTime * scaleSpeed;

		float rt = Mathf.PingPong(rotateT, 1);
		rt = rt * rt * (3f - 2f * rt);

		float rZ = -maxAngle + maxAngle * 2 * rt;
		transform.rotation = Quaternion.Euler(0, 0, rZ);

		float rs = Mathf.PingPong(scaleT, 1);
		rs = rs * rs * (3f - 2f * rs);

		float scale = minScale + (maxScale - minScale) * rs;
		transform.localScale = Vector3.one * scale;
	}
}
