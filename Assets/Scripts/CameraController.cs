using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float followSpeed;
    public float minX;
    public float maxX;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX), transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * followSpeed);
    }
}
