using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collidetrigger3 : MonoBehaviour
{

    [SerializeField] Animator transitionAnim;
    private void OnTriggerEnter(Collider other)
    {
        WaitForSecondsRealtime(11);
        Debug.Log(SceneManager.GetActiveScene().name);
        {
            StartCoroutine(LoadLevel());
            IEnumerator LoadLevel()
            {
                transitionAnim.SetTrigger("End");
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene("TerrainBear");
                transitionAnim.SetTrigger("Start");
            }
        }
    }

    private void WaitForSecondsRealtime(int v)
    {
        throw new NotImplementedException();
    }
}