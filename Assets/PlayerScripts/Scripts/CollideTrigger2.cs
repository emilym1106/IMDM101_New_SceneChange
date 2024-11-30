using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideTrigger2 : MonoBehaviour
{
    public PlayerContolBee playerControlBee; // Reference to PlayerContolBee script

    private void Start()
    {
        if (playerControlBee == null)
        {
            playerControlBee = FindObjectOfType<PlayerContolBee>();
        }
    }
    [SerializeField] Animator transitionAnim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerControlBee.GetFlowerCount() >= 5)
        {
            // Print out the current scene's name
            Debug.Log(SceneManager.GetActiveScene().name);
            {
                StartCoroutine(LoadLevel());
                IEnumerator LoadLevel()
                {
                    transitionAnim.SetTrigger("End");
                    yield return new WaitForSeconds(1);
                    SceneManager.LoadScene("FinalScene");
                    transitionAnim.SetTrigger("Start");

                }
            }
        }

        else if (other.CompareTag("Player"))
        {
            Debug.Log("You need to collect 5 flowers to proceed!");
        }
        }
    } 