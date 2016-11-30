using UnityEngine;
using System.Collections;

public class cameraCtr : MonoBehaviour
{
    Vector3 mousepos;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right;
        }
        if (Input.GetMouseButtonDown(1))
        {
            mousepos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 change = Input.mousePosition - mousepos;
            transform.eulerAngles += new Vector3(-change.y*0.5f, change.x*0.5f, 0);
            mousepos = Input.mousePosition;
        }
    }
}
