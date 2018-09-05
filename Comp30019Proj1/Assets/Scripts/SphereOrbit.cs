using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Sun Revolution Around The Terrain
/// </summary>
/// Created by Mayank
public class SphereOrbit : MonoBehaviour {

    // Declare and assign orbit speed, terrain instance variable to
    // get terrain width and set the radius.
    public float OrbitSpeed = -0.2f;
    private TerrainGenerator terrainGenerator;
    private float Radius;
    // Set angle to zero.
    private float Angle = 0;

    /// <summary>
    /// Initiate the radius of the circular orbit using the terrain width.
    /// </summary>
    private void Start()
    {
        // Assign radius equal to the terrain width.
        terrainGenerator = GameObject.Find("Terrain").GetComponent<TerrainGenerator>();
        Radius = terrainGenerator.terrainWidth;
    }
    /// <summary>
    /// Apply the revolution to the sun (sphere).
    /// </summary>
    /// Learnt Implementation from https://answers.unity.com/questions/133373/moving-object-in-a-ellipse-motion.html, modified the code
    /// by adding orbit speed and deltatime to control and smoothen the speed.
    /// The radius is also fixed for both x and y axis to get a perfect circular orbit.
    void Update()
    {
        // Increment the angle based on orbit speed and delta time.
        Angle += Time.deltaTime * OrbitSpeed;
        // Update the position of the sun (sphere) based on cos(angle) and sin(angle).
        transform.position = new Vector3(Mathf.Cos(Angle) * Radius, Mathf.Sin(Angle) * Radius, 0.0f);
    }
}
