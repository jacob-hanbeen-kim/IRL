using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotMovementScript : MonoBehaviour {

    //public GameObject arm;
    //private Rigidbody rb;
    private Vector3 mousePosition;
    public float moveSpeed = 0.05f;

    bool demo = false;
    Vector3 startPos;
    Vector3 initMousePos;

    // Use this for initialization
    void Start () {
	    //rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            if (!demo)
            {
                demo = true;
                startPos = transform.position;
                initMousePos = Input.mousePosition;
            }
            if (demo)
            {
                mousePosition = Input.mousePosition;
                //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 delta = mousePosition - startPos;

                transform.position = Vector2.Lerp(transform.position, startPos + delta, moveSpeed * Time.deltaTime);
            }
        } else {
            demo = false;
        }
    }
    /*
    private void OnMouseDown()
    {
        Debug.Log("hi");
        rb.AddForce(-1 *transform.up * 1, ForceMode.Acceleration);
        //transform.position = transform.position + transform.up;
        //rb.useGravity = true;
    }*/


}
