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
    // Start is called before the first frame update
    void Start()
    {
        moveRock.Move(moveRockM,right_left ,rockCol, rockRow,Vector3.zero);
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
    }
}
