using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    void Update()
    {
        UpDown();
        LeftRight();

        //카메라 회전
        transform.rotation = Quaternion.Euler(transform.rotation.x + Input.mousePosition.y * ySpeed * -1, transform.rotation.y - 30 + Input.mousePosition.x * xSpeed, 0);
    }

    //상하 이동
    void UpDown()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(0, -1, 0);
        }
    }

    //앞뒤, 좌우 이동
    void LeftRight()
    {
        if (Input.GetButton("Horizontal"))
        {
            transform.position += transform.right * Input.GetAxis("Horizontal");
        }
        if (Input.GetButton("Vertical"))
        {
            transform.position += transform.forward * Input.GetAxis("Vertical");
        }
    }
}
