using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CarControl : MonoBehaviour
{
    public AudioClip crash;
    public AudioClip[] accelerates;
    public float speed = 7f, force = 900f;
    private Rigidbody carRb;
    public bool rightTurn = false, leftTurn = false, moveFromUp;
    private float originRotationY, rotMultRight = 7f, rotMultLeft = 5f;
    private Camera mainCam;
    public LayerMask carLayer;
    private bool isMovingFast, carCrashed;
    [NonSerialized] public bool carPassed;
    [NonSerialized] public static bool isLose;
    public GameObject turnLeftSignal, turnRightSignal, explotion, exhaut;
    [NonSerialized] public static int countCars;
    

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        originRotationY = transform.eulerAngles.y;
        mainCam = Camera.main;

       if (leftTurn)
            StartCoroutine(TurnSignals(turnLeftSignal));
        else if (rightTurn)
            StartCoroutine(TurnSignals(turnRightSignal));

        IEnumerator TurnSignals(GameObject turnsignal)
        {
            while (!carPassed){
                turnsignal.SetActive(!turnsignal.activeSelf);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    private void FixedUpdate()
    {
        carRb.MovePosition(transform.position - transform.forward * speed * Time.fixedDeltaTime);
        
    }    
    private void Update()
    {
#if UNITY_EDITOR
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
#else
        if (Input.touchCount == 0)
            return;
        Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
#endif
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, carLayer)) {
            string carName = hit.transform.gameObject.name;
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !isMovingFast && gameObject.name == carName)
#else
            if (Input.GetTouch(0).phase == TouchPhase.Began && !isMovingFast && gameObject.name == carName)
#endif
            {
                GameObject vfx = Instantiate(exhaut, 
                    new Vector3(transform.position.x, transform.position.y +1.5f, transform.position.z), 
                    Quaternion.Euler(90f,0,0)) as GameObject;
                Destroy(vfx, 2f);

                speed *= 3f;
                isMovingFast = true;
                if (PlayerPrefs.GetString("music") != "No")
                {
                    GetComponent<AudioSource>().clip = accelerates[Random.Range(0, accelerates.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car") && !carCrashed) {
            carCrashed = true;
            isLose = true;
            speed = 0f;
            other.gameObject.GetComponent<CarControl>().speed = 0f;

            GameObject vfx = Instantiate(explotion, transform.position, Quaternion.identity) as GameObject;
            Destroy(vfx, 5f);

            if(isMovingFast)
                force*= 1.5f;
            carRb.AddRelativeForce(Vector3.back * force);
            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().clip = crash;
                GetComponent<AudioSource>().Play();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Car") && other.GetComponent<CarControl>().carPassed) 
            other.GetComponent<CarControl>().speed = speed + 5f;           
    }    
    private void OnTriggerStay(Collider other)
    {
        if (carCrashed)
            return;

        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
            RotateCar(rotMultRight);
        else if (other.transform.CompareTag("TurnBlock Left") && leftTurn)
            RotateCar(rotMultLeft, -1);
    }    
    private void OnTriggerExit(Collider other)
    {
        if (carCrashed)
            return;

        if (other.transform.CompareTag("Trigger Pass")) {
            if (carPassed)
                return;

            carPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider col in colliders)
                col.enabled = true;
            countCars++;
                    }
        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
            carRb.rotation = Quaternion.Euler(0, originRotationY + 90f, 0);
        else if (other.transform.CompareTag("TurnBlock Left") && leftTurn)
            carRb.rotation = Quaternion.Euler(0, originRotationY - 90f, 0);
        else if (other.transform.CompareTag("Delete Trigger")) 
            Destroy(gameObject);        
    }
    private void RotateCar(float speedRotate, int dir = 1)
    {
        if (carCrashed)
            return;
        if (dir == -1 && transform.localRotation.eulerAngles.y < originRotationY - 90f)
            return;
        if (dir == -1 && moveFromUp && transform.localRotation.eulerAngles.y > 250f && transform.localRotation.eulerAngles.y < 270f)
            return; 

        float rotateSpeed = speed * rotMultRight * dir;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, y: rotateSpeed, 0) * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);
    }
}
