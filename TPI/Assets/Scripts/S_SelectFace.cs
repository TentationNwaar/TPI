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
    //On appelle les autres scripts pour pouvoir
    //les utiliser ici
    public S_ReadCube s_ReadCube;
    public S_CubeState s_CubeState;
    public S_Rotation s_Rotation;
    public S_PivotRotation s_PivotRotation;

    //Cette variable permet de définir quel layerMask utiliser
    int layerMask = 1 << 3;
    // Start is called before the first frame update
    void Start()
    {
        s_CubeState = FindObjectOfType<S_CubeState>();
        s_ReadCube = FindObjectOfType<S_ReadCube>();
        s_Rotation = FindObjectOfType<S_Rotation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !s_Rotation.dragging)
        {      
            //On regarde l'état actuel du cube
            s_ReadCube.ReadState();

            //Raycast depuis la souris pour voir quel face est touchée
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;

                //On fait une liste de tous les côtés(liste des faces des gameobjects)
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    s_CubeState.up,
                    s_CubeState.down,
                    s_CubeState.left,
                    s_CubeState.right,
                    s_CubeState.front,
                    s_CubeState.back,
                };
                //Si la face touchée existe
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        s_CubeState.Pickup(cubeSide);
                        cubeSide[16].GetComponent<S_PivotRotation>().Rotate(cubeSide);                 
                    }
                }
            }
        }
    }
}
