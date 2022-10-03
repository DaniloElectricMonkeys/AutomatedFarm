using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxisRaw("Horizontal") * speed, 0, 0);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            transform.Translate(0, 0, Input.GetAxisRaw("Vertical") * speed);
        }
    }
}
