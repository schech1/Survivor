using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speed;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()

    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
