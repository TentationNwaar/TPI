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
    internal bool dragging = false;
    //Crée une variable avec le composant RigidBody
    Rigidbody rb;
    Quaternion lastRotation;
    public GameObject cube;

    public S_PauseMenu s_PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        //Au démarrage, on lui attribut le composant RigidBody
        rb = GetComponent<Rigidbody>();
        cube.transform.localRotation = lastRotation;
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

        //Si on appuye sur esc ou le bouton visuel de sortie, le menu s'affiche
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            if (s_PauseMenu.GameIsPaused)
            {
                s_PauseMenu.Resume();
            }

            else
            {
                s_PauseMenu.Pause();
            }
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
            //On utilise les axe d'unity XY pour faire tourner le cube en fonction 
            //du temps et de la vitesse de rotation
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            //On applique la force au RigidBody
            rb.AddTorque(Vector3.down*x);
            rb.AddTorque(Vector3.right*y);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //cube.transform.SetPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,90,0)));
            cube.transform.Rotate(0,90,0);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cube.transform.Rotate(0,-90,0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cube.transform.Rotate(90,0,0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cube.transform.Rotate(-90,0,0);
        }
    }
}
