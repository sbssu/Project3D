using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float lerpAmount;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, destination, lerpAmount * Time.deltaTime);
    }
}
