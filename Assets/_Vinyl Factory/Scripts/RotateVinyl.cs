using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateVinyl : MonoBehaviour
{

    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
      objectToRotate.transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));  
    }
}
