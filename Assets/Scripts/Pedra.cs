using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedra : MonoBehaviour
{
    Vector3 PlayerPos;
    float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, PlayerPos, step);
    }
}
