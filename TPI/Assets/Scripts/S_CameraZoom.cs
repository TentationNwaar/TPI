/*
Auteur : Maxence Weyermann
Date : 19.05.22
Lieu : ETML
Description : Script qui permet de gérer le zoom de la caméra
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraZoom : MonoBehaviour
{
     void Update()
     {
         if (Input.GetAxis("Mouse ScrollWheel") > 0)
         {
             GetComponent<Camera>().fieldOfView--;
         }

         if (Input.GetAxis("Mouse ScrollWheel") < 0)
         {
             GetComponent<Camera>().fieldOfView++;
         }
     }
}
