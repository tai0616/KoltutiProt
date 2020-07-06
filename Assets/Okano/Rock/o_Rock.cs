using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_Rock : MonoBehaviour
{
    [SerializeField]
    o_RockManager rockManager;
    [SerializeField]
    bool canBreak = true;
    [SerializeField]
    bool canMove = true;
    [SerializeField]
    int mass = 1;
    public int massR = 1;
    
    [SerializeField]
    o_RockManager.MOVE right_left=o_RockManager.MOVE.RIGHT;
    //岩マネージャー内配列の中の位置
    [SerializeField]
    int colum = 0,row=0;
    [SerializeField]
    o_Rock ridingRock = null;
    [SerializeField]
    o_Rock ridedRock = null;
    int rideNum = 0;
    [SerializeField]
    GameObject o_RockUP_Prefab=null;
    static GameObject o_RockUP=null;

    private void Awake()
    {
        if (o_RockUP == null)
        {
            o_RockUP = Instantiate(o_RockUP_Prefab);
            o_RockUP.SetActive(false);
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //外部参照用Player用　岩移動要求
    public bool RockMove(o_RockManager.MOVE caseMove)
    {
        if (!canMove) return false;

        return rockManager.RockMoveM(this/*,colum*/, row, caseMove);
    }
    public bool RockMove(Vector3 vec3)
    {
        if (!canMove) return false;

        o_RockManager.MOVE caseMove;

        if (vec3 == Vector3.forward)
        {
            caseMove = o_RockManager.MOVE.FRONT;
        }
        else if (vec3 == Vector3.back)
        {
            caseMove = o_RockManager.MOVE.BACK;
        }
        else if (vec3 == Vector3.up)
        {
            caseMove = o_RockManager.MOVE.UP;
        }
        else if (vec3 == Vector3.down)
        {
            caseMove = o_RockManager.MOVE.DOWN;
        }
        else if (vec3 == Vector3.right)
        {
            if (right_left == o_RockManager.MOVE.RIGHT)
            {
                if (row < rockManager.rocksCol / 2)
                {
                    caseMove = o_RockManager.MOVE.DOWN;
                }
                else
                {
                    caseMove = o_RockManager.MOVE.UP;
                }
            }
            else
            {
                if (row < rockManager.rocksCol / 2)
                {
                    caseMove = o_RockManager.MOVE.UP;
                }
                else
                {
                    caseMove = o_RockManager.MOVE.DOWN;
                }
            }
        }
        else if (vec3 == Vector3.left)
        {
            if (right_left == o_RockManager.MOVE.RIGHT)
            {
                if (row < rockManager.rocksCol / 2)
                {
                    caseMove = o_RockManager.MOVE.UP;
                }
                else
                {
                    caseMove = o_RockManager.MOVE.DOWN;
                }
            }
            else
            {
                if (row < rockManager.rocksCol / 2)
                {
                    caseMove = o_RockManager.MOVE.DOWN;
                }
                else
                {
                    caseMove = o_RockManager.MOVE.UP;
                }
            }
        }
        else
        {
            Debug.Log("Argument is invalid");
            return false;
        }

        return rockManager.RockMoveM(this, row, caseMove);
    }

    //RockManager用
    public void Move(o_RockManager moveToRockManager,o_RockManager.MOVE right_left0, int colNum, int rowNum,Vector3 pos)
    {
        transform.parent = moveToRockManager.transform;
        if (pos != Vector3.zero)
        {
            if (ridingRock != null)
            {
                if (pos.x > 0.5f)
                {
                    pos.x += (pos.x - 0.5f)*2.0f;
                }
                else if (pos.x < -0.5f)
                {
                    pos.x += (pos.x + 0.5f) * 2.0f;
                }
                else if (pos.y > 0.5f)
                {
                    pos.y += (pos.y - 0.5f) * 2.0f;
                }
                else if (pos.y < -0.5f)
                {
                    pos.y += (pos.y + 0.5f) * 2.0f;
                }
            }
            transform.localPosition = pos;
        }
        rockManager = moveToRockManager;
        right_left = right_left0;
        colum = colNum;
        row = rowNum;


    }


    //RockManager用
    public bool RideRock(o_Rock rideR)
    {
        if (ridedRock == null)
        {
            rideR.GetOffRock();
            ridedRock = rideR;
            ridedRock.ridingRock = this;
            rideR.rideNum = rideNum + 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetOffRock()
    {
        if (ridingRock != null)
        {
            ridingRock.ridedRock = null;
            ridingRock = null;
            return true;
        }
        return false;
    }

    public bool RockDestroy()
    {
        if (!canBreak)
        {
            return canBreak;
        }

        if (rockManager.RockDestroyM(this, row))
        {
            GetOffRock();
            Destroy(gameObject);
        }

        return false;
    }

    public bool EqualsRocksPos(o_Rock rockA)
    {
        if (Equals(rockA))
        {
            return true;
        }
        else if(rockA!=null&&rockA.ridedRock!=null&& Equals(rockA.ridedRock))
        {
            return true;
        }
        return false;
    }

    public void CalcMass()
    {
        if (ridedRock != null)
        {
            massR = (int)((mass + ridedRock.mass) * rockManager.massCoe);
            return;
        }
        massR = mass;
    }

    public void OnSelected()
    {
        o_RockUP.SetActive(true);
        o_RockUP.transform.position = transform.position;
    }
    public void OffSelected()
    {
        o_RockUP.SetActive(false);
    }

}
