using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public float speed = 5.0f;
    private float xMax;
    private float zMax;
    private float yMax;
    private TerrainGenerator terraingenerator;

    void Start()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        terraingenerator = new TerrainGenerator();
        xMax = terraingenerator.terrainWidth / 2  ;
        zMax = terraingenerator.terrainWidth / 2 ;
        yMax = terraingenerator.maxHeight;

        Vector3 initialPosition = new Vector3(-xMax, yMax*2, -zMax);

        this.transform.position = initialPosition;
        this.transform.eulerAngles += new Vector3(25.0f, 60.0f, 0.0f);

    }


    void Update(){

        float dx = 0.0f, dz = 0.0f;
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
