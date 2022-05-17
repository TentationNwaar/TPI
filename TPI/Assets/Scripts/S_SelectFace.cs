/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de tourner les faces du cubes
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SelectFace : MonoBehaviour
{
    public S_Rotation s_Rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !s_Rotation.dragging)
        {
            Debug.Log("OK");
        }
    }
}
