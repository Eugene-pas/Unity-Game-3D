using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public GameObject[] weapons;
    public int ammo;
    public int ammoPistol;
    public GameObject[] GetGameWeapons()
    {
        return weapons;
    }
    
}
