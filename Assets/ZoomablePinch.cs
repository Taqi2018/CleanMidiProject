using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomablePinch : MonoBehaviour
{
    public float deltaMovement;

    public float distanceMoved;
    // Start is called before the first frame update
    void Start()
    {
        distanceMoved = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position-= new Vector3( deltaMovement*Time.deltaTime,0,0);


        }
 

        if (Input.GetKey(KeyCode.D))
        {
            
            transform.position += new Vector3(deltaMovement * Time.deltaTime, 0, 0);
           
            distanceMoved += deltaMovement;
            Debug.Log(transform.position.x);


/*            if (tag == "b")
            {
               
                Debug.Log(distanceMoved+400 );
                
            }*/

        }
/*        if (Input.GetKeyUp(KeyCode.D))
        {
           *//* GridManager.instance.distanceBetweenNotes += distanceMoved /100;*//*
        }*/

    }
}
