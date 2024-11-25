using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideTrigger : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(SceneManager.GetActiveScene().name);
        {
            StartCoroutine(LoadLevel());
            IEnumerator LoadLevel()
            {
                transitionAnim.SetTrigger("End");
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene("BeeScene");
                transitionAnim.SetTrigger("Start");

            }





        }
    
    }
}
