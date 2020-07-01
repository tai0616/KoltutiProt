using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject obj;
    public GameObject Camera;
    public float radius;

    public float Camera_radius;
    //巨獣との差
    private float zPosition;
    float x;
    float y;
    float z;

    float angle_ = 0.0f;

    bool actionMode_ = false;
    MoveCamera sc_;
    // Use this for initialization
    void Start()
    {
        zPosition = 0.5f;
        sc_ = Camera.GetComponent<MoveCamera>();
        Camera_radius = 6.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            actionMode_ = !actionMode_;
            sc_.ModeChange();

        }
        if (!actionMode_)
        {
            // 左に移動
            if (Input.GetKey(KeyCode.W))
            {
                zPosition += 6.0f * Time.deltaTime;
                //this.transform.Translate(-0.1f, 0.0f, 0.0f);
            }
            // 右に移動
            if (Input.GetKey(KeyCode.S))
            {
                zPosition -= 6.0f * Time.deltaTime;
            }
            // 前に移動
            if (Input.GetKey(KeyCode.A))
            {
                angle_ -= 3.0f * Time.deltaTime;
                //this.transform.Translate(0.0f, 0.0f, 0.1f);
            }
            // 後ろに移動
            if (Input.GetKey(KeyCode.D))
            {
                angle_ += 3.0f * Time.deltaTime;
                //this.transform.Translate(0.0f, 0.0f, -0.1f);
            }


            //transform.position = obj.transform.position;
            x = radius * Mathf.Sin(angle_) + obj.transform.position.x;
            z = zPosition + obj.transform.position.z;
            y = radius * Mathf.Cos(angle_) + obj.transform.position.y;

            transform.position = new Vector3(x, y, z);
        }
        else
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                radius -= 4.0f * Time.deltaTime;
                //this.transform.Translate(-0.1f, 0.0f, 0.0f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                radius += 4.0f * Time.deltaTime;
                //this.transform.Translate(-0.1f, 0.0f, 0.0f);
            }

            x = radius * Mathf.Sin(angle_) + obj.transform.position.x;
            z = zPosition + obj.transform.position.z;
            y = radius * Mathf.Cos(angle_) + obj.transform.position.y;

            transform.position = new Vector3(x, y, z);

            //カメラの位置計算
            x = (radius + Camera_radius) * Mathf.Sin(angle_) + obj.transform.position.x;
            z = zPosition + obj.transform.position.z;
            y = (radius + Camera_radius) * Mathf.Cos(angle_) + obj.transform.position.y;
            
            Camera.transform.position = new Vector3(x, y, z);

        }
    }

    public float GetRadius()
    {
        return radius;
    }
}