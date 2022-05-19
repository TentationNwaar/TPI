/*
Auteur : Maxence Weyermann
Date : 17.05.22
Lieu : ETML
Description : Script qui permet de voir l'état du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CubeState : MonoBehaviour
{
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pickup(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            //On attache le parent de chaque face (petit cube) au parent de l'index (cube du millieu), sauf si c'est déjà le cas
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
    }
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }
}
