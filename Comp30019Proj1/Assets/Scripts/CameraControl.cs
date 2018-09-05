using UnityEngine;
using System.Collections;
/// <summary>
/// Camera control and collision detection
/// </summary>
// Created by Judd
public class CameraControl : MonoBehaviour
{
    // Speed of the camera movement
    public float speed = 5.0f;
    // Terrain size
    private float xMax;
    private float zMax;
    private float yMax;
    private TerrainGenerator terrainGenerator;

    void Start()
    {
        // Initialize position of camera based on terrain size
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        terrainGenerator = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainGenerator>();
        xMax = terrainGenerator.terrainWidth / 2  ;
        zMax = terrainGenerator.terrainWidth / 2 ;
        yMax = terrainGenerator.maxHeight;

        Vector3 initialPosition = new Vector3(-xMax, yMax*2, -zMax);

        this.transform.position = initialPosition;
        this.transform.eulerAngles += new Vector3(30.0f, 45.0f, 0.0f);

    }


    void Update(){

        float yaw = 0.0f;
        float pitch = 0.0f;
        float amountToMove = Time.deltaTime * speed;
        Vector3 futurePosition ;
                                 
   
            if (Input.GetKey(KeyCode.W))
            {
                futurePosition = this.transform.position + this.transform.forward * amountToMove;
                if (futurePosition.x < xMax && futurePosition.x > -xMax
                    && futurePosition.z < zMax && futurePosition.z > -zMax)
                {
                    this.transform.position += this.transform.forward * amountToMove;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                futurePosition = this.transform.position - this.transform.forward * amountToMove;
                if (futurePosition.x < xMax && futurePosition.x > -xMax
                     && futurePosition.z < zMax && futurePosition.z > -zMax)
                {
                    this.transform.position -= this.transform.forward * amountToMove;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                futurePosition = this.transform.position - this.transform.right * amountToMove;
                if (futurePosition.x < xMax && futurePosition.x > -xMax
                     && futurePosition.z < zMax && futurePosition.z > -zMax)
                {
                    this.transform.position -= this.transform.right * amountToMove;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                futurePosition = this.transform.position + this.transform.right * amountToMove;
                if (futurePosition.x < xMax && futurePosition.x > -xMax
                     && futurePosition.z < zMax && futurePosition.z > -zMax)
                {
                    this.transform.position += this.transform.right * amountToMove;
                }
            }
                

        yaw += speed * Input.GetAxis("Mouse X");
        pitch -= speed * Input.GetAxis("Mouse Y");

        this.transform.eulerAngles += new Vector3(pitch, yaw, 0.0f);

       
    }

  
}
