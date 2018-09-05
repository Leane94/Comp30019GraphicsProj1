using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Control the movement of sun
/// </summary>
// Created by Mayank
public class SphereOrbit : MonoBehaviour {

    public float OrbitSpeed = -0.2f;
    public float coordinateX = 20f;
    public float coordinateY = 20f;
    private float counter = 0;

    void Update()
    {
        counter += Time.deltaTime * OrbitSpeed;
        float x = Mathf.Cos(counter) * coordinateX;
        float y = Mathf.Sin(counter) * coordinateY;
        this.transform.position = new Vector3(x, y, 0.0f);
    }
}
