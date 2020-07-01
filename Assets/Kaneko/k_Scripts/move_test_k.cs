using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_test_k : MonoBehaviour
{
    Rigidbody KyojyuuRb;

    void Start()
    {
        KyojyuuRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        KyojyuuRb.AddForce(-50, 0, 0);
    }
}
