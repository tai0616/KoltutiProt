using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyojyuManager : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;            //Rayが当たったオブジェクトの情報を入れる箱
    public float ray_distance;
  

    Rigidbody KyojyuuRb;
    public float speed = 1;
    public float side_move_speed = 1;

    private GameObject[] BodyObj = new GameObject[4];


    private float[] RailPos = {-40,-20,0,20,40 };
    //public Transform[] Railpos;

    private int now_RailNumber = 3;
    private int next_RailNumber = 3;

    public float R_weight;
    public float L_weight;

    private bool movenow = false;

    private float Body_rotation = 0;

    public bool Kyojyu_rotation_now = false;

    private void Start()
    {
        KyojyuuRb = GetComponent<Rigidbody>();

        //子オブジェクトの回転する体を取得
        BodyObj[0] = transform.Find("Body01").gameObject;
        BodyObj[1] = transform.Find("Body02").gameObject;
        BodyObj[2] = transform.Find("Body03").gameObject;
        BodyObj[3] = transform.Find("Body04").gameObject;
    }

    private void Update()
    {
        KyojyuuRb.AddForce(0, 0, speed);

        Kyojyu_rotation_now = movenow;//今は横に動いてるとき常に回転している

        ///////////////レイで岩山を判断///////////////
        ray = new Ray(transform.position, transform.TransformDirection(new Vector3(0,0, ray_distance)));

        //レイを可視化
        Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, 0, ray_distance)), Color.yellow);

        if (Physics.Raycast(ray, out hit, ray_distance))//Rayの当たり判定
        {
            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.tag == "MountainRock" && movenow == false)
            {
                if (L_weight < R_weight)//右の方が重かった時
                {
                    next_RailNumber++;
                    movenow = true;
                }
                else if (L_weight > R_weight)//左の方が重かった時
                {
                    next_RailNumber--;
                    movenow = true;
                }
                else//重さが等しい時 
                { 
                
                }
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;
            }
        }

        if (movenow == true)//横に移動中
        {
            if (next_RailNumber - now_RailNumber > 0)//右に移動
            {
                KyojyuuRb.AddForce(side_move_speed, 0, 0);
                Body_rotation -= 0.5f;

                RightStopHantei();
            }
            else//左に移動
            {
                KyojyuuRb.AddForce(-side_move_speed, 0, 0);
                Body_rotation += 0.5f;

                LeftStopHantei();
            }

            for (int count = 0; count <= 3; count++)
                BodyObj[count].transform.rotation = Quaternion.Euler(0, 0, Body_rotation);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MountainRock")
        {
            Destroy(this.gameObject);
        }
    }

    private void RightStopHantei()
    {
        if (next_RailNumber == 1)
        {
            if (transform.position.x > RailPos[0])
            {

                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 2)
        {
            if (transform.position.x > RailPos[1])
            {

                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 3)
        {
            if (transform.position.x > RailPos[2])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 4)
        {
            if (transform.position.x > RailPos[3])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }

        }
        else if (next_RailNumber == 5)
        {
            if (transform.position.x > RailPos[4])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }

    }

    private void LeftStopHantei()
    {
        if (next_RailNumber == 1)
        {
            if (transform.position.x < RailPos[0])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 2)
        {
            if (transform.position.x < RailPos[1])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 3)
        {
            if (transform.position.x < RailPos[2])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
        else if (next_RailNumber == 4)
        {
            if (transform.position.x < RailPos[3])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }

        }
        else if (next_RailNumber == 5)
        {
            if (transform.position.x < RailPos[4])
            {
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;

                now_RailNumber = next_RailNumber;
                movenow = false;
            }
        }
    }
}

