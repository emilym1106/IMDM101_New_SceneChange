using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideTrigger : MonoBehaviour
{
   private void OnTriggerEnter (Collider other)
   {
    if (other.tag == "Player")
    {
    // Print out the current scene's name
            Debug.Log(SceneManager.GetActiveScene().name);
            // Change scene
            SceneManager.LoadScene("Scene_2_test");
    }
   }

   }
