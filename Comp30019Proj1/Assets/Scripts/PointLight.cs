using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Light source, provided by teaching material of Lab 5, COMP30019.
/// </summary>
/// Point Light is used to get the world postion of light and give color to the light falling
/// on the terrain and water.
public class PointLight : MonoBehaviour {

    public Color color;

    /// <summary>
    /// Gets the world position of the light source.
    /// </summary>
    /// <returns>The world position.</returns>
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
