using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject Player;
    private bool ActionMode_;

    public float c_forward;
    public float c_up;
    float angle_;
    // Start is called before the first frame update
    void Start()
    {
        c_forward = c_up = 2.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ActionMode_)
        {

            Vector3 move_ = Player.transform.position;
            move_.z -= 15f;
            transform.position = move_;
        }
        else
        {
            this.transform.LookAt(Player.transform);
            transform.position += transform.forward * c_forward;
            transform.position += transform.up * c_up;
        }

    }

    public void ModeChange()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        ActionMode_ = !ActionMode_;
    }
}
