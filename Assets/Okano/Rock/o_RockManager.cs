﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_RockManager : MonoBehaviour
{
    public enum MOVE
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        FRONT,
        BACK
    }
    [SerializeField]
    public int rocksCol = 8;
    [SerializeField]
    int rocksRow = 4;
    [SerializeField]
    //o_Rock[,][] rocks = new o_Rock[2, 8][]
    //{
    //    {
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null }
    //    },
    //    {
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null },
    //    new o_Rock[] { null, null, null, null }
    //    }
    //};
    //[SerializeField]
    /*public*/
    List<o_Rock[]> list_rocksR = new List<o_Rock[]>();
    /*public*/ List<o_Rock[]> list_rocksL =new List<o_Rock[]>();


    public int massR = 0, massL = 0;
    [Tooltip ("上乗せ時の係数")]
    public float massCoe=2.0f;
    //岩の付いているbodyの回転　多分Z軸
    float bodyRotation;
    [SerializeField]
    o_RockManager frontRM=null;
    [SerializeField]
    o_RockManager backRM=null;

    [SerializeField]
    GameObject rockPrefab1 = null;
    [SerializeField]
    GameObject rockPrefab2 = null;
    [SerializeField]
    GameObject rockPrefab3 = null;
    [SerializeField]
    int[] CreateRocksR = new int[32]
{
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0
};
    [SerializeField]
    int[] CreateRocksL = new int[32]
{
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0 ,
        0,0,0,0
};

    //[SerializeField]
    Transform[] childrocks;

    // Start is called before the first frame update
    void Start()
    {
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            list_rocksR.Add(new o_Rock[] { null, null, null, null });
        }
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            list_rocksL.Add(new o_Rock[] { null, null, null, null });
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (CreateRocksR[i * rocksRow + j] != 0)
                {
                    GameObject createObj;
                    switch (CreateRocksR[i * rocksRow + j])
                    {
                        case 1:
                            createObj = Instantiate(rockPrefab1);
                            break;
                        case 2:
                            createObj = Instantiate(rockPrefab2);
                            break;
                        case 3:
                            createObj = Instantiate(rockPrefab3);
                            break;
                        default:
                            createObj = Instantiate(rockPrefab1);
                            break;
                    }
                    while (createObj.GetComponent<o_Rock>() == null) { };
                    SetRock(createObj.GetComponent<o_Rock>(), MOVE.RIGHT, i, j);
                    createObj.transform.localScale = new Vector3(1.0f / (rocksCol / 2.0f), 1.0f / (rocksCol / 2.0f), 1.0f / (rocksCol / 2.0f));
                }
                if (CreateRocksL[i * 4 + j] != 0)
                {
                    GameObject createObj;
                    switch (CreateRocksL[i * 4 + j])
                    {
                        case 1:
                            createObj = Instantiate(rockPrefab1);
                            break;
                        case 2:
                            createObj = Instantiate(rockPrefab2);
                            break;
                        case 3:
                            createObj = Instantiate(rockPrefab3);
                            break;
                        default:
                            createObj = Instantiate(rockPrefab1);
                            break;
                    }
                    while (createObj.GetComponent<o_Rock>() == null) { };
                    SetRock(createObj.GetComponent<o_Rock>(), MOVE.LEFT, i, j);
                    createObj.transform.localScale = new Vector3(1.0f / (rocksCol / 2.0f), 1.0f / (rocksCol / 2.0f), 1.0f / (rocksCol / 2.0f));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Rockからよう　移動　成功ならtrue失敗ならfalse
    public bool RockMoveM(o_Rock rockA/*, int col*/,int row, o_RockManager.MOVE caseNum)
    {
        int[] adr = new int[2] { 0, 0 }; 
        //Rockの位置検索
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            if (rockA.EqualsRocksPos(list_rocksR[listNum][row]))
            {
                adr[0] = (int)MOVE.RIGHT;
                adr[1] = listNum;
                break;
            }
            else if(rockA.EqualsRocksPos(list_rocksL[listNum][row]))
            {
                adr[0] = (int)MOVE.LEFT;
                adr[1] = listNum;
                break;
            }
        }
        //移動先岩マネージャー
        o_RockManager moveToRockManager=this;
        //移動先用アドレス
        int[] adr2 = { adr[0], adr[1], row };
        //移動先調整
        switch (caseNum)
        {       
            case MOVE.UP:
                adr2[1]--;
                if (adr2[1] < 0)
                {
                    adr2[1]++;
                    if (adr[0] == (int)MOVE.RIGHT)
                    {
                        adr2[0] = (int)MOVE.LEFT;
                    }
                    else
                    {
                        adr2[0] = (int)MOVE.RIGHT;
                    }
                }
                break;
            case MOVE.DOWN:
                adr2[1]++;
                if (adr2[1] >= rocksCol)
                {
                    adr2[1]--;
                    if (adr[0] == (int)MOVE.RIGHT)
                    {
                        adr2[0] = (int)MOVE.LEFT;
                    }
                    else
                    {
                        adr2[0] = (int)MOVE.RIGHT;
                    }
                }
                break;
            case MOVE.FRONT:
                adr2[2]--;
                if (adr2[2] < 0)
                {
                    if (moveToRockManager.frontRM == null)
                    {
                        return false;
                    }
                    moveToRockManager = moveToRockManager.frontRM;
                    adr2[2]=moveToRockManager.rocksRow-1;                    
                }
                break;
            case MOVE.BACK:
                adr2[2]++;
                if (adr2[2] >= rocksRow)
                {
                    if (moveToRockManager.backRM == null)
                    {
                        return false;
                    }
                    moveToRockManager = moveToRockManager.backRM;
                    adr2[2]=0;
                }
                break;
            default:
                break;
        }
        //Debug.Log("adr2:" + adr2[0] + adr2[1] + adr2[2]);
        //移動　成功ならtrue失敗ならfalse
        switch (adr2[0])
        {
            case (int)MOVE.RIGHT:
                //移動先が空なら突っ込む
                if (moveToRockManager.list_rocksR[adr2[1]][adr2[2]] == null)
                {
                    
                    moveToRockManager.list_rocksR[adr2[1]][adr2[2]] = rockA;
                    if (!rockA.GetOffRock())
                    {
                        if (adr[0] == (int)MOVE.RIGHT)
                        {
                            list_rocksR[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksL[adr[1]][row] = null;
                        }
                    }
                    else
                    {
                        if (adr[0] == (int)MOVE.RIGHT)
                        {
                            list_rocksR[adr[1]][row].CalcMass();
                        }
                        else
                        {
                            list_rocksL[adr[1]][row].CalcMass();
                        }
                    }
                }
                //移動先にあるなら乗せる
                else if (moveToRockManager.list_rocksR[adr2[1]][adr2[2]].RideRock(rockA))
                {

                    if (adr[0] == (int)MOVE.RIGHT)
                    {
                        if (rockA.Equals(list_rocksR[adr[1]][row]))
                        {
                            list_rocksR[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksR[adr[1]][row].CalcMass();
                        }
                    }
                    else
                    {
                        if (rockA.Equals(list_rocksL[adr[1]][row]))
                        {
                            list_rocksL[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksL[adr[1]][row].CalcMass();
                        }
                    }
                }
                else 
                {
                    //乗せるのに失敗したらfalse
                    return false;
                }

                //
                rockA.Move(moveToRockManager,(MOVE)adr2[0],adr2[1], adr2[2],CalcMoveToPosition(adr2[0], adr2[1], adr2[2]));
                moveToRockManager.list_rocksR[adr2[1]][adr2[2]].CalcMass();
                CalcMassRL();
                return true;
                break;
            case (int)MOVE.LEFT:
                if (moveToRockManager.list_rocksL[adr2[1]][adr2[2]] == null)
                {
                    moveToRockManager.list_rocksL[adr2[1]][adr2[2]] = rockA;
                    if (!rockA.GetOffRock())
                    {
                        if (adr[0] == (int)MOVE.RIGHT)
                        {
                            list_rocksR[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksL[adr[1]][row] = null;
                        }
                    }
                    else
                    {
                        if (adr[0] == (int)MOVE.RIGHT)
                        {
                            list_rocksR[adr[1]][row].CalcMass();
                        }
                        else
                        {
                            list_rocksL[adr[1]][row].CalcMass();
                        }
                    }

                }
                else if (moveToRockManager.list_rocksL[adr2[1]][adr2[2]].RideRock(rockA))
                {
                    if (adr[0] == (int)MOVE.RIGHT)
                    {
                        if (rockA.Equals(list_rocksR[adr[1]][row]))
                        {
                            list_rocksR[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksR[adr[1]][row].CalcMass();
                        }
                    }
                    else
                    {
                        if (rockA.Equals(list_rocksL[adr[1]][row]))
                        {
                            list_rocksL[adr[1]][row] = null;
                        }
                        else
                        {
                            list_rocksL[adr[1]][row].CalcMass();
                        }
                    }
                }
                else
                {
                    //乗せるのに失敗したらfalse
                    return false;
                }

                rockA.Move(moveToRockManager, (MOVE)adr2[0],adr2[1], adr2[2],CalcMoveToPosition(adr2[0], adr2[1], adr2[2]));
                moveToRockManager.list_rocksL[adr2[1]][adr2[2]].CalcMass();
                CalcMassRL();
                return true;
                break;
        }
        return false;
    }


    //Rockからよう　移動　成功ならtrue失敗ならfalse
    public bool RockDestroyM(o_Rock rockA, int row)
    {
        //Rockの位置検索
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            if (rockA.EqualsRocksPos(list_rocksR[listNum][row]))
            {
                if (rockA.Equals(list_rocksR[listNum][row]))
                {
                    list_rocksR[listNum][row] = null;
                }
                else
                {
                    list_rocksR[listNum][row].CalcMass();
                }
                return true;
            }
            else if (rockA.EqualsRocksPos(list_rocksL[listNum][row]))
            {
                if (rockA.Equals(list_rocksL[listNum][row]))
                {
                    list_rocksL[listNum][row] = null;
                }
                else
                {
                    list_rocksL[listNum][row].CalcMass();
                }
                return true;
            }
        }
        return false;
    }

    public void SetRock(o_Rock o_rock,MOVE right_left,int rockCol,int rockRow)
    {
        o_rock.Move(this, right_left, rockCol, rockRow, CalcMoveToPosition((int)right_left, rockCol, rockRow));
        if (right_left == MOVE.RIGHT)
        {
            list_rocksR[rockCol][rockRow] = o_rock;
            return;
        }
        list_rocksL[rockCol][rockRow] = o_rock;
    }

    Vector3 CalcMoveToPosition(int right_left,int col,int row)
    {
        Vector3 toPos = new Vector3(1.0f / rocksCol + 0.5f, 1.0f / rocksCol + 0.5f, 0.5f - 1.0f / rocksCol);

        if (rocksCol / 4 > col)
        {
            toPos.x -= (rocksCol / 4 - col) * 2.0f / rocksCol;
        }
        else if (rocksCol * 3 / 4 <= col)
        {
            toPos.x += (rocksCol * 3 / 4 - col - 1) * 2.0f / rocksCol;
            toPos.y *= -1;
        }
        else
        {
            toPos.y -= (col - rocksCol / 4 + 1) * 2.0f / rocksCol;
        }

        toPos.z -= row * 2.0f / rocksCol;

        if (right_left == (int)MOVE.LEFT)
        {
            toPos.x *= -1;
        }
        //Debug.Log(toPos + "toPos");
        return toPos;
    }

    public void CalcMassRL()
    {
        massR = 0;
        massL = 0;

        for (int colum = 0; colum < rocksCol; colum++)
        {
            for (int row = 0; row < rocksRow; row++)
            {
                if (list_rocksR[colum][row] != null)
                {
                    massR += list_rocksR[colum][row].massR;
                }
                if (list_rocksL[colum][row] != null)
                {
                    massL += list_rocksL[colum][row].massR;
                }
            }
        }
    }

    public void RotateRocks(int rotate)
    {
        int movelist = (int)(rocksCol * rotate / 180);
        Debug.Log("rotate:" + movelist);
        if (movelist < 0)
        {
            for (int i = 0; i > movelist; i--)
            {
                list_rocksR.Insert(0, list_rocksL[0]);
                list_rocksL.RemoveAt(0);
                list_rocksL.Add(list_rocksR[rocksCol]);
                list_rocksR.RemoveAt(rocksCol);
            }
        }
        else
        {
            for (int i = 0; i < movelist; i++)
            {
                
                list_rocksL.Insert(0, list_rocksR[0]);
                list_rocksR.RemoveAt(0);
                list_rocksR.Add(list_rocksL[rocksCol]);
                list_rocksL.RemoveAt(rocksCol);
            }
        }

        childrocks = new Transform[transform.childCount];
        for (int i = 0,childCnt= transform.childCount; i < childCnt; i++)
        {
            childrocks[i] = transform.GetChild(0);
            childrocks[i].parent = null;
        }
        transform.rotation = Quaternion.identity;
        for (int i = 0, childCnt = childrocks.Length; i < childCnt; i++)
        {
            childrocks[i].parent = transform;
        }

        CalcMassRL();
    }

    public void SetRocksTrans()
    {
        childrocks = new Transform[transform.childCount];
        for (int i = 0, childCnt = transform.childCount; i < childCnt; i++)
        {
            childrocks[i] = transform.GetChild(0);
        }
    }
}
