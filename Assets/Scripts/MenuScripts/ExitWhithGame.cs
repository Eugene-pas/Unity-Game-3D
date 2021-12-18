using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWhithGame : MonoBehaviour
{
    public Animator anim;
    public void ExitGame()
    {
        anim.SetBool("fadeExit",true);
        ExitGame();
    }
}
