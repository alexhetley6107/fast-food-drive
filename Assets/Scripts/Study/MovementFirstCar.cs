using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFirstCar : MonoBehaviour
{
    public GameObject canvasFirst, secondCar, secondCanvas, thirdCar;
    private bool isFirst;
    private CarControl _control;
    private bool x;

    private void Start()
    {
        _control = GetComponent<CarControl>();
    }
    void Update()
    {
        if (transform.position.x < 8f && !isFirst)
        {
            isFirst = true;
            _control.speed = 0f;
            canvasFirst.SetActive(true);
        }
       
    }
    private void OnMouseDown()
    {
        if (!isFirst || transform.position.x > 9f) 
            return;               
        
        _control.speed = 15f;
        canvasFirst.SetActive(false);
        secondCanvas.SetActive(true);
        secondCar.GetComponent<CarControl>().speed = 8f;
        thirdCar.GetComponent<CarControl>().speed = 8f;             

    }
}
