using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotImage : MonoBehaviour
{
   public float rotationSpeed = 45.0f; // Velocidad de rotación
    private Vector3 initialRotation;
    private Vector3 initialMousePosition;

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
        initialMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Cuando haces clic
        {
            initialMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Cuando dejas de hacer clic
        {
            transform.rotation = Quaternion.Euler(initialRotation);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - initialMousePosition;

            float rotationAngle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
           
            transform.Rotate(Vector3.forward, rotationAngle * rotationSpeed * Time.deltaTime);

            initialMousePosition = currentMousePosition;
        }
    }
}
