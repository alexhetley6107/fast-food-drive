using UnityEngine;

public class IsNoAds : MonoBehaviour
{
    void Start()
    {/*
        PlayerPrefs.DeleteKey("NoAds");
        PlayerPrefs.DeleteKey("City");
        PlayerPrefs.DeleteKey("Megapolis");
        PlayerPrefs.SetInt("Coins", 115);
        PlayerPrefs.SetInt("NowMap", 1);*/

        if (PlayerPrefs.GetString("NoAds") == "yes")
            Destroy(gameObject);
    }

   
}
