/*
Auteur : Maxence Weyermann
Date : 24.05.22
Lieu : ETML
Description : Script qui permet la gestion de la résolution du cube
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class S_SolveTwoPhase : MonoBehaviour
{
    public S_ReadCube s_ReadCube;
    public S_CubeState s_CubeState;
    public S_Automate s_Automate;
    public S_Rotation s_Rotation;
    internal int solveCount;
    private bool doOnce = false;
    List<S_Automate.Move> reverse = new List<S_Automate.Move>();

    // Start is called before the first frame update
    void Start()
    {
        s_ReadCube = FindObjectOfType<S_ReadCube>();
        s_CubeState = FindObjectOfType<S_CubeState>();
        s_Automate = FindObjectOfType<S_Automate>();
        s_Rotation = FindObjectOfType<S_Rotation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (S_CubeState.started && doOnce)
        {
            doOnce = true;
            Solver();
        }
    }
    /// <summary>
    /// Méthode pour lancer la résolution
    /// </summary>
    public void Solver()
    {
        s_Automate.cube.transform.eulerAngles = s_Rotation.lastRotation;
        s_Automate.cube.transform.localPosition = s_Rotation.lastPosition;
        StartCoroutine(WaitBeforeSolve());
    }
    /// <summary>
    /// Permet de résoudre le cube après un laps de temps
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeSolve()
    {
        yield return new WaitForSeconds(0.5f);
        s_ReadCube.ReadState();

        // Récup l'historique et inverser les mouvements
        List<S_Automate.Move> reverseHistory = Enumerable.Reverse(s_Automate.history).ToList();
        s_Automate.history.Clear();
        S_Automate.moveList.Clear();
        List<S_Automate.Move> reverseMoves = new List<S_Automate.Move>();

        foreach (var move in reverseHistory)
        {
            S_Automate.Move reverseMove = Reverse(move);
            reverseMoves.Add(reverseMove);
            solveCount++;
        }
        S_Automate.moveList = reverseMoves;
    }

    /// <summary>
    /// Liste des mouvements à effectué à l'envers
    /// </summary>
    /// <param name="moveToReverse"></param>
    /// <returns></returns>
    public S_Automate.Move Reverse(S_Automate.Move moveToReverse)
    {
        switch (moveToReverse)
        {
            case S_Automate.Move.U:
                return S_Automate.Move.UPrime;
            case S_Automate.Move.D:
                return S_Automate.Move.DPrime;
            case S_Automate.Move.L:
                return S_Automate.Move.LPrime;
            case S_Automate.Move.R:
                return S_Automate.Move.RPrime;
            case S_Automate.Move.B:
                return S_Automate.Move.BPrime;
            case S_Automate.Move.F:
                return S_Automate.Move.FPrime;
            case S_Automate.Move.UPrime:
                return S_Automate.Move.U;
            case S_Automate.Move.DPrime:
                return S_Automate.Move.D;
            case S_Automate.Move.LPrime:
                return S_Automate.Move.L;
            case S_Automate.Move.RPrime:
                return S_Automate.Move.R;
            case S_Automate.Move.BPrime:
                return S_Automate.Move.B;
            case S_Automate.Move.FPrime:
                return S_Automate.Move.F;
            default:
                return S_Automate.Move.U;
        }
    }
}
