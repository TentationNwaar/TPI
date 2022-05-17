/*
Auteur : Maxence Weyermann
Date : 16.05.22
Lieu : ETML
Description : Script qui permet la rotation du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Rotation : MonoBehaviour
{
    //Gère la vitesse de rotation
    public float rotationSpeed = 100;
    //Défini si il est en rotation ou pas
    bool dragging = false;
    //Crée une variable avec le composant RigidBody
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //Au démarrage, on lui attribut le composant RigidBody
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si le clic gauche de la souris est levé, il ne tourne pas
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            //On bloque tous les mouvements
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        
    }

    //Méthode qui vérifie un mouvement en cours
    void OnMouseDrag()
    {
        dragging = true;
    }

    //Méthode qui effectue des actions tous les tels frames
    void FixedUpdate()
    {
        //Si on bouge, on enlève les restrictions
        if (dragging)
        {
            rb.constraints = RigidbodyConstraints.None;
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
            rb.AddTorque(Vector3.down*x);
            rb.AddTorque(Vector3.right*y);
        }
    }
}
