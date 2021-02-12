using UnityEngine;
using UnityEngine.SceneManagement;
public class SecondCar : MonoBehaviour
{
    public GameObject canSec;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Trigger Pass"))
        {
            PlayerPrefs.SetString("First Game", "No");
            LoadGame();
        }

        if (other.transform.CompareTag("TurnBlock Left"))
            canSec.SetActive(false);
    }  
       
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            PlayerPrefs.SetString("First Game", "No");
            Invoke("LoadGame", 4f);
            canSec.SetActive(false);
        }
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
    /*private void OnMouseDown()
    {
        canSec.SetActive(false);
    }*/



}
