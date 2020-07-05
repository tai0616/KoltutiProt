using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_RockTest : MonoBehaviour
{
    [SerializeField]
    o_RockManager moveRockM;
    [SerializeField]
    o_Rock moveRock;
    [SerializeField]
    o_RockManager.MOVE right_left = o_RockManager.MOVE.RIGHT;
    [SerializeField]
    int rockCol = 0, rockRow = 0;

    [SerializeField]
    o_Rock destroyRock = null;

    [SerializeField]
    int rotateRocks = 180;
    [SerializeField]
    GameObject rockPrefab = null;
    [SerializeField]
    bool[] CreateRocksR = new bool[32]
    {
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false ,
        false,false,false,false
    };

    // Start is called before the first frame update
    void Start()
    {
        moveRockM.SetRock(moveRock,right_left ,rockCol, rockRow);

        //for(int i = 0; i < 8; i++)
        //{
        //    for (int j = 0; j < 4; j++)
        //    {
        //        if (CreateRocksR[i*4+j])
        //        {
        //            GameObject createObj = Instantiate(rockPrefab);
        //            createObj.transform.localScale = Vector3.one;
        //            moveRockM.SetRock(createObj.GetComponent<o_Rock>(), o_RockManager.MOVE.RIGHT, i, j);
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveRock.RockMove(o_RockManager.MOVE.UP);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveRock.RockMove(o_RockManager.MOVE.DOWN);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRock.RockMove(o_RockManager.MOVE.FRONT);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveRock.RockMove(o_RockManager.MOVE.BACK);
        }

        if (Input.GetKeyDown(KeyCode.D) && destroyRock != null)
        {
            destroyRock.RockDestroy();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            moveRockM.SetRocksTrans();
            moveRockM.CalcMassRL();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateRocksTest();
        }
    }

    void RotateRocksTest()
    {
        moveRockM.transform.Rotate(new Vector3(0, 0, rotateRocks));
        moveRockM.RotateRocks(rotateRocks);
    }
}
