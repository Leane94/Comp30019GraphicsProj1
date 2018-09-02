using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOrbit : MonoBehaviour {

    public float OrbitSpeed;
    public float coordinateX;
    public float coordinateY;
    private float counter = 0;

    void FixedUpdate()
    {
        counter += Time.deltaTime * OrbitSpeed;
        float x = Mathf.Cos(counter) * coordinateX;
        float y = Mathf.Sin(counter) * coordinateY;
        this.transform.position = new Vector3(x, y, 0.0f);
    }
}
