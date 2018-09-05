using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Light source, provided by teaching material of lab 5
/// </summary>
public class PointLight : MonoBehaviour {

    public Color color;

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
