using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyojyuManager : MonoBehaviour
{
    /// <summary>
    /// レイの当たり判定
    /// </summary>
    Ray ray;
    RaycastHit hit;            //Rayが当たったオブジェクトの情報を入れる箱
    public float ray_distance;
    public Transform ray_startpos;

    /// <summary>
    /// 巨獣の動くスピード関連
    /// </summary>
    Rigidbody KyojyuuRb;
    public float speed = 1;
    public float side_move_speed = 1;
    private bool movenow = false;
    private bool Kyojyu_Dont_Sidemove = false;
    private float time = 0.0f;
    public float Dontmove_time;

    private GameObject[] BodyObj = new GameObject[4];


    private float[] RailPos = { -200, -100, 0, 100, 200 };
    //public Transform[] Railpos;

    private int now_RailNumber = 3;
    private int next_RailNumber = 3;

    /// <summary>
    /// 巨獣の重さ関連
    /// </summary>
    public float R_weight;
    public float L_weight;
    public float R_old_weight;//古い情報格納用と、最初に当たる岩をどっちに避けるかに使う
    public float L_old_weight;

    ////最初の岩だけどっちに移動するか直接書き込めるようにとかいうめんどくさい処理
    //public bool first_side_move_R;
    //private bool first_rock = true;

    private float Body_rotation = 0;
    private float Save_rotation = 0;

    public bool Kyojyu_rotation_now = false;

    public float tenmethu_time;
    private float alpha_Sin;
    private float hogetime;

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

        //RとLが動かせる古い情報を格納
        if (R_weight != L_weight)
        {
            R_old_weight = R_weight;//古い重さ情報格納
            L_old_weight = L_weight;
        }

        ///////////////レイで岩山を判断///////////////
        ray = new Ray(ray_startpos.transform.position, transform.TransformDirection(new Vector3(0, 0, ray_distance)));

        //レイを可視化
        Debug.DrawRay(ray_startpos.transform.position, transform.TransformDirection(new Vector3(0, 0, ray_distance)), Color.yellow);

        //岩山を自動で避ける時
        if (Physics.Raycast(ray, out hit, ray_distance))//Rayの当たり判定
        {
            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.tag == "MountainRock" && movenow == false && Kyojyu_Dont_Sidemove == false)
            {
                if (L_weight < R_weight)//右の方が重かった時
                {
                    next_RailNumber++;
                    movenow = true;

                    hogetime = 0;
                    Body_rotation = 0;
                    Save_rotation = BodyObj[0].transform.localEulerAngles.z;
                }
                else if (L_weight > R_weight)//左の方が重かった時
                {
                    next_RailNumber--;
                    movenow = true;

                    hogetime = 0;
                    Body_rotation = 0;
                    Save_rotation = BodyObj[0].transform.localEulerAngles.z;
                }
                else//重さが等しい時 
                {
                    if (L_old_weight < R_old_weight)//前の右の方が重かった時
                    {
                        next_RailNumber++;
                        movenow = true;

                        hogetime = 0;
                        Body_rotation = 0;
                        Save_rotation = BodyObj[0].transform.localEulerAngles.z;
                    }
                    else if (L_old_weight > R_old_weight)//前の左の方が重かった時
                    {
                        next_RailNumber--;
                        movenow = true;

                        hogetime = 0;
                        Body_rotation = 0;
                        Save_rotation = BodyObj[0].transform.localEulerAngles.z;
                    }
                }
                KyojyuuRb.velocity = Vector3.zero;
                KyojyuuRb.angularVelocity = Vector3.zero;
            }
        }


        //ケンジャクで巨獣を動かすとき
        if (R_weight - L_weight * 2 >= 0 && movenow == false && Kyojyu_Dont_Sidemove == false)//右が左の２倍以上重くなった時
        {
            next_RailNumber++;
            movenow = true;
            KyojyuuRb.velocity = Vector3.zero;
            KyojyuuRb.angularVelocity = Vector3.zero;

            Kyojyu_Dont_Sidemove = true;
            hogetime = 0;

            Body_rotation = 0;
            Save_rotation = BodyObj[0].transform.localEulerAngles.z;

        }

        if (L_weight - R_weight * 2 >= 0 && movenow == false && Kyojyu_Dont_Sidemove == false)
        {
            next_RailNumber--;
            movenow = true;
            KyojyuuRb.velocity = Vector3.zero;
            KyojyuuRb.angularVelocity = Vector3.zero;

            Kyojyu_Dont_Sidemove = true;
            hogetime = 0;

            Body_rotation = 0;
            Save_rotation = BodyObj[0].transform.localEulerAngles.z;

        }


        //Debug.Log(Kyojyu_Dont_Sidemove);

        //巨獣がケンジャクで動いた後で数秒間動けない
        if (Kyojyu_Dont_Sidemove == true)
        {
            time += Time.deltaTime;

            if (time > Dontmove_time)
            {
                time = 0;
                Kyojyu_Dont_Sidemove = false;
            }
        }


        //横に移動処理
        if (movenow == true)//横に移動中
        {
            
            hogetime += Time.deltaTime;

            //点滅処理
            Color _color = BodyObj[0].GetComponent<Renderer>().material.color;
            if (hogetime <= tenmethu_time)
            {
                alpha_Sin = Mathf.Sin(Time.time) / 1.2f + 0.1f;
                _color.a = alpha_Sin;
            }
            else
            {
                _color.a = 1.0f;
            }
            BodyObj[0].GetComponent<Renderer>().material.color = _color;


            //巨獣の動き＋回転
            if (next_RailNumber - now_RailNumber > 0)//右に移動
            {
                KyojyuuRb.AddForce(side_move_speed, 0, 0);

                if (hogetime >= tenmethu_time)//点滅終わったら
                {
                    if (Body_rotation > -180.0f)
                    {
                        Body_rotation -= 0.2f;
                    }
                    else
                    {
                        Body_rotation = -180;
                    }
                }

                RightStopHantei();
            }
            else//左に移動
            {
                KyojyuuRb.AddForce(-side_move_speed, 0, 0);

                if (hogetime >= tenmethu_time)//点滅終わったら
                {
                    if (Body_rotation < 180.0f)
                    {
                        Body_rotation += 0.2f;
                    }
                    else
                    {
                        Body_rotation = 180;
                    }
                }

                LeftStopHantei();
            }

            BodyObj[0].transform.rotation = Quaternion.Euler(0, 0, Save_rotation + Body_rotation);

        }

        //Debug.Log(Body_rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MountainRock")
        {
            //Destroy(this.gameObject);
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

