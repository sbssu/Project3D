using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSample : MonoBehaviour
{
    [SerializeField] GameObject moon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            // 30도라는 회전량
            moon.transform.RotateAround(transform.position, Vector3.up, 30f);

        }
    }
}
