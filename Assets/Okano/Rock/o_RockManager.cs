using System.Collections;
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
    int rocksCol = 8;
    [SerializeField]
    int rocksRow = 4;
    [SerializeField]
    o_Rock[,][] rocks = new o_Rock[2, 8][]
    {
        {
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null }
        },
        {
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null },
        new o_Rock[] { null, null, null, null }
        }
    };
    [SerializeField]
    /*public*/
    List<o_Rock[]> list_rocksR = new List<o_Rock[]>();
    /*public*/ List<o_Rock[]> list_rocksL =new List<o_Rock[]>();


    public int massR = 0, massL = 0;
    //岩の付いているbodyの回転　多分Z軸
    float bodyRotation;
    [SerializeField]
    o_RockManager frontRM=null;
    [SerializeField]
    o_RockManager backRM=null;


    // Start is called before the first frame update
    void Start()
    {
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            list_rocksR.Add(rocks[((int)MOVE.RIGHT), listNum]);
        }
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            list_rocksL.Add(rocks[((int)MOVE.LEFT), listNum]);
        }

        //rocks = new Rock[rocksCol, rocksRow];
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Rockからよう　移動　成功ならtrue失敗ならfalse
    public bool RockMoveM(o_Rock rockA/*, int col*/,int row, o_RockManager.MOVE caseNum)
    {
        int[] adr = new int[2]; 
        //Rockの位置検索
        for (int listNum = 0; listNum < rocksCol; listNum++)
        {
            if (rockA.Equals(list_rocksR[listNum][row]))
            {
                adr[0] = (int)MOVE.RIGHT;
                adr[1] = listNum;
                break;
            }
            else if(rockA.Equals(list_rocksL[listNum][row]))
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
                    adr2[2]++;                    
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
                    adr2[2]--;
                }
                break;
            default:
                break;
        }
        //移動　成功ならtrue失敗ならfalse
        switch (adr2[0])
        {
            case (int)MOVE.RIGHT:
                //移動先が空なら突っ込む
                if (moveToRockManager.list_rocksR[adr2[1]][adr2[2]] == null)
                {
                    moveToRockManager.list_rocksR[adr2[1]][adr2[2]] = rockA;
                }
                //移動先にあるなら乗せる
                else if (!moveToRockManager.list_rocksR[adr2[1]][adr2[2]].RideRock(rockA))
                {
                    //乗せるのに失敗したらfalse
                    return false;
                }
                //
                rockA.Move(moveToRockManager,(MOVE)adr2[0],adr2[1], adr2[2]);
                if (adr[0] == (int)MOVE.RIGHT)
                {
                    list_rocksR[adr[1]][row] = null;
                }
                else
                {
                    list_rocksL[adr[1]][row] = null;
                }
                return true;
                break;
            case (int)MOVE.LEFT:
                if (moveToRockManager.list_rocksL[adr2[1]][adr2[2]] == null)
                {
                    moveToRockManager.list_rocksL[adr2[1]][adr2[2]] = rockA;
                }
                else if (!moveToRockManager.list_rocksL[adr2[1]][adr2[2]].RideRock(rockA))
                {
                    return false;
                }
                rockA.Move(moveToRockManager, (MOVE)adr2[0],adr2[1], adr2[2]);
                if (adr[0] == (int)MOVE.RIGHT)
                {
                    list_rocksR[adr[1]][row] = null;
                }
                else
                {
                    list_rocksL[adr[1]][row] = null;
                }
                return true;
                break;
        }
        return false;
    }

    public void SetRock(o_Rock o_rock,MOVE right_left,int rockCol,int rockRow)
    {
        if (right_left == MOVE.RIGHT)
        {
            list_rocksR[rockCol][rockRow] = o_rock;
            return;
        }
        list_rocksL[rockCol][rockRow] = o_rock;
    }
}
