using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class Weapons : MonoBehaviour
{
    public float health;
    public GameObject buttonReturn;
    public GameObject buttonExit;
    public GameObject[] weapons;
    public int ammo;
    public int ammoPistol;
    private int pres = 1;
    public GameObject[] GetGameWeapons()
    {
        return weapons;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Escape))
        {
            pres++;
        }
        if (pres % 2 == 0) 
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0.0f;
            buttonExit.SetActive(true);
            buttonReturn.SetActive(true);
        }
        if (pres % 2 == 1) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            buttonExit.SetActive(false);
            buttonReturn.SetActive(false);
        }
    }

    public void ReturnToGame()
    {
        pres = 2;
    }
}
