using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayGame : MonoBehaviour
{
 private Animator anim;
 public int levelToLoad;

 private void Start()
 {
  Cursor.lockState = CursorLockMode.Confined;
  anim = GetComponent<Animator>();
 }

 public void FadeToLevel()
 {
  Time.timeScale = 1f;
   anim.SetBool("fade",true);
 }



 public void OnFadeComplete()
 {
  SceneManager.LoadScene(levelToLoad);
 }
}
