using UnityEngine;

public class DeleteHorn : MonoBehaviour
{
    public float timetodelete = 2f;
   private void Start()
    {
        Destroy(gameObject, timetodelete);
    }   
}
