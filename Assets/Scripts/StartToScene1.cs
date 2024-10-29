using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartToScene1 : MonoBehaviour
{

    public void GoToScene1() 
    {
        SceneManager.LoadScene("BearScene");
    
    }



}
