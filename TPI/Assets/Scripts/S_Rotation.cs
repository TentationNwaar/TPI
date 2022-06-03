/*
Auteur : Maxence Weyermann
Date : 16.05.22
Lieu : ETML
Description : Script qui permet la rotation du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Rotation : MonoBehaviour
{
    //Gère la vitesse de rotation
    public float rotationSpeed = 100;

    //Défini si il est en rotation ou pas
    internal bool dragging = false;

    //Crée une variable avec le composant RigidBody
    internal Rigidbody rb;
    internal Vector3 lastRotation;
    internal Vector3 lastPosition;

    //On utilise le cube pour rotate
    public GameObject cube;

    //On fait appel au script du menu pour le gérer
    public S_PauseMenu s_PauseMenu;
    public S_Automate s_Automate;

    //internal float rotationArrowLeftRight;
    //internal float rotationArrowUpDown;

    // Start is called before the first frame update
    void Start()
    {
        //Au démarrage, on lui attribut le composant RigidBody
        rb = GetComponent<Rigidbody>();
        lastRotation = cube.transform.eulerAngles;
        lastPosition = cube.transform.localPosition;
        s_Automate = FindObjectOfType<S_Automate>();

        //Résout le bug du cube qui se déplace seul
        rb.constraints = RigidbodyConstraints.FreezeAll;
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetGame();
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
        float rotationArrowLeftRight = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float rotationArrowUpDown = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        if (!dragging && !s_Automate.isShuffuling && !s_Automate.isSolving)
        {
            transform.Rotate(rotationArrowLeftRight, rotationArrowUpDown, 0f);
        }

        //Si on bouge, on enlève les restrictions
        if (dragging && !s_Automate.isShuffuling && !s_Automate.isSolving)
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
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RightClick()
    {
        Input.GetMouseButtonDown(1);
    }
}
