using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenWaypoints : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypointsTransform;
    private int waypointIndex = 0;

    public bool addStart = false;

    [SerializeField] private float timeBetweenWaypoints;

    private void Awake()
    {
        if (addStart) waypointsTransform.Add(transform.position);
    }

    private void Start()
    {
        StartCycle();
    }

    private IEnumerator SmoothLerp(Vector2 startPos, Vector2 endPos, float seconds, bool loop = true)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.deltaTime;
            
            float t = elapsed / seconds;
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        transform.position = endPos;

        // stuff after move is over
        waypointIndex++;
        if (waypointIndex >= waypointsTransform.Count) waypointIndex = 0;
        if (loop) StartCycle();
    }

    public void StartCycle()
    {
        StartCoroutine(SmoothLerp(transform.position, waypointsTransform[waypointIndex], timeBetweenWaypoints));
    }

    public void StopWaypointTransform()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothLerp(transform.position, new Vector3(0, 1), 0.6f));
    }
}
