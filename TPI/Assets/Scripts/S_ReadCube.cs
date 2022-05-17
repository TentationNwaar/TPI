/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de lire les faces du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ReadCube : MonoBehaviour
{
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();
    private int layerMask = 1 << 3; //Pour les faces

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
