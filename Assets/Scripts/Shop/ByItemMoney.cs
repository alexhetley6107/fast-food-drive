using UnityEngine;

public class ByItemMoney : MonoBehaviour
{
    public enum Types
    {
        REMOVE_ADS, OPEN_CITY, OPEN_MEGAPOLIS
    }

    public Types type;

    public void ByItem()
    {
        switch (type)
        {
            case Types.REMOVE_ADS:
                IAPManager.instance.ByNoAds();
                break;
            case Types.OPEN_CITY:
                IAPManager.instance.ByCityMap();
                break;
            case Types.OPEN_MEGAPOLIS:
                IAPManager.instance.ByMegapolisMap();
                break;

        }
    }
}
