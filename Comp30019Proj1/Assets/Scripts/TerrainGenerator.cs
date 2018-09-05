using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Terrain generator which uses diamond-square algorithm to generate random terrain,
/// and color terrain based on height
/// </summary>
// Created by Chao Li
public class TerrainGenerator : MonoBehaviour
{
    //Light source for the world
    public PointLight pointLight;

    // Number of divisions per line (e.g. 5 vertices/line => 4*4 divisions)
    public int numDivisions = 128;

    // Width/Length of the terrain in the world (terrain is a square)
    public float terrainWidth = 30;

    // Max height of the terrain
    public float maxHeight = 5;

    // Range for the random number to be added in each iteration
    public float heightDecreaseRate = 0.5f;

    // Total number of vertices
    private int numVertices;

    // Use this for initialization
    void Start()
    {
        numVertices = (numDivisions + 1) * (numDivisions + 1);
        CreateWater();
        CreateTerrain();
        ColourTerrain();
    }
    // Update is called once per frame
    void Update()
    {
        // Update light position for each shader
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());

        MeshRenderer waterRenderer = this.transform.Find("Water").gameObject.GetComponent<MeshRenderer>();
        waterRenderer.material.SetColor("_PointLightColor", this.pointLight.color);
        waterRenderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());

    }

    /// <summary>
    /// Create a flat mesh
    /// </summary>  
    public Mesh CreateMesh()
    {
        // Initialisation    
        // Array of all vertices
        Vector3[] vertices = new Vector3[numVertices];
        // Array of UV vectors
        Vector2[] uvs = new Vector2[numVertices];
        // Array of all triangles on mesh
        int[] triangles = new int[numDivisions * numDivisions * 6];

        float HalfWidth = terrainWidth * 0.5f;
        float DivisionWidth = terrainWidth / numDivisions;

        // Create a new mesh
        Mesh flatMesh = new Mesh();

        int TriangleOffset = 0;

        // Set up array of vertices, uvs and triangles
        for (int i = 0; i <= numDivisions; i++)
        {
            for (int j = 0; j <= numDivisions; j++)
            {
                // Make Terrain's center to be (0,0)
                vertices[i * (numDivisions + 1) + j] = new Vector3((-HalfWidth + j * DivisionWidth), 0.0f, (HalfWidth - i * DivisionWidth));
                uvs[i * (numDivisions + 1) + j] = new Vector2((float)i / numDivisions, (float)j / numDivisions);

                if (i < numDivisions && j < numDivisions)
                {
                    int topLeft = i * (numDivisions + 1) + j;
                    int bottomLeft = (i + 1) * (numDivisions + 1) + j;

                    // triangles has to be clock-wise to be rendered
                    triangles[TriangleOffset] = topLeft;
                    triangles[TriangleOffset + 1] = topLeft + 1;
                    triangles[TriangleOffset + 2] = bottomLeft;

                    triangles[TriangleOffset + 3] = topLeft + 1;
                    triangles[TriangleOffset + 4] = bottomLeft + 1;
                    triangles[TriangleOffset + 5] = bottomLeft;

                    TriangleOffset += 6;
                }
            }
        }

        flatMesh.vertices = vertices;
        flatMesh.uv = uvs;
        flatMesh.triangles = triangles;

        flatMesh.RecalculateBounds();
        flatMesh.RecalculateNormals();

        return flatMesh;
    }

    /// <summary>
    /// Create water mesh
    /// </summary>
    private void CreateWater()
    {
        Mesh waterMesh = CreateMesh();
        this.transform.Find("Water").gameObject.GetComponent<MeshFilter>().mesh = waterMesh;
        this.transform.Find("Water").gameObject.GetComponent<MeshCollider>().sharedMesh = waterMesh;

        waterMesh.RecalculateBounds();
        waterMesh.RecalculateNormals();
    }

    /// <summary>
    /// Create terrain mesh
    /// </summary>
    private void CreateTerrain()
    {        
        // Create a new mesh for terrain
        Mesh terrainMesh = CreateMesh();
        GetComponent<MeshFilter>().mesh = terrainMesh;

        DiamondSquare(terrainMesh);

        GetComponent<MeshCollider>().sharedMesh = terrainMesh;
    }

    /// <summary>
    /// Apply diamond-square algorithm to a mesh
    /// </summary>
    /// <param name="terrainMesh">Mesh to be manipulate</param>
    private void DiamondSquare(Mesh terrainMesh)
    {
        // Initialize corner of the terrain
        Vector3[] vertices = terrainMesh.vertices;
        vertices[0].y = Random.Range(-maxHeight, maxHeight);
        vertices[numDivisions].y = Random.Range(-maxHeight, maxHeight);
        vertices[vertices.Length - 1 - numDivisions].y = Random.Range(-maxHeight, maxHeight);
        vertices[vertices.Length - 1].y = Random.Range(-maxHeight, maxHeight);

        int iteration = (int)Mathf.Log(numDivisions, 2);
        int squareCounter = 1;
        int squareSize = numDivisions;
        float heightOffset = maxHeight;

        for (int i = 0; i < iteration; i++)
        {
            int row = 0;
            for (int j = 0; j < squareCounter; j++)
            {
                int col = 0;
                for (int k = 0; k < squareCounter; k++)
                {
                    DSCalculator(row, col, squareSize, heightOffset, vertices);
                    col += squareSize;
                }
                row += squareSize;
            }
            squareCounter *= 2;
            squareSize /= 2;
            heightOffset *= heightDecreaseRate;

        }

        // update mesh
        terrainMesh.vertices = vertices;

        terrainMesh.RecalculateBounds();
        terrainMesh.RecalculateNormals();
    }

    /// <summary>
    /// Calculate position for each vertices
    /// </summary>
    /// <param name="row">row number of squares</param>
    /// <param name="col">colum number of squares</param>
    /// <param name="size">size of square</param>
    /// <param name="offset">offset for random number to be add</param>
    /// <param name="vertices">array of all vertices</param>
    private void DSCalculator(int row, int col, int size, float offset, Vector3[] vertices)
    {
        int halfSize = (int)(0.5f * size);
        int topLeft = row * (numDivisions + 1) + col;
        int bottomLeft = (row + size) * (numDivisions + 1) + col;

        // Diamond step
        int mid = (row + halfSize) * (numDivisions + 1) + (col + halfSize);
        vertices[mid].y = (vertices[topLeft].y + vertices[bottomLeft].y + vertices[topLeft + size].y + vertices[bottomLeft + size].y) * 0.25f
            + Random.Range(-offset, offset);

        // Square step
        int top = topLeft + halfSize;
        int left = mid - halfSize;
        int bottom = bottomLeft + halfSize;
        int right = mid + halfSize;

        // If vertex on top edge of terrain
        if (top <= numDivisions)
        {
            vertices[top].y = (vertices[topLeft].y + vertices[mid].y + vertices[topLeft + size].y) / 3
                + Random.Range(-offset, offset);
        }
        else
        {
            int temp = (row - halfSize) * (numDivisions + 1) + col + halfSize;
            vertices[top].y = (vertices[topLeft].y + vertices[mid].y + vertices[topLeft + size].y + vertices[temp].y) * 0.25f
                + Random.Range(-offset, offset);
        }

        // If vertex on left edge of terrain
        if (left % (numDivisions + 1) == 0)
        {
            vertices[left].y = (vertices[topLeft].y + vertices[mid].y + vertices[bottomLeft].y) / 3
                + Random.Range(-offset, offset);
        }
        else
        {
            vertices[left].y = (vertices[topLeft].y + vertices[mid].y + vertices[bottomLeft].y + vertices[mid - size].y) * 0.25f
                + Random.Range(-offset, offset);
        }

        // If vertex on bottom edge of terrain
        if (bottom >= (numVertices - 1 - numDivisions))
        {
            vertices[bottom].y = (vertices[bottomLeft].y + vertices[mid].y + vertices[bottomLeft + size].y) / 3
                + Random.Range(-offset, offset);
        }
        else
        {
            int temp = (row + 3 * halfSize) + (numDivisions + 1) + col + halfSize;
            vertices[bottom].y = (vertices[bottomLeft].y + vertices[mid].y + vertices[bottomLeft + size].y + vertices[temp].y) * 0.25f
                + Random.Range(-offset, offset);
        }

        // If vertex on right edge of terrain
        if ((right + 1) % (numDivisions + 1) == 0)
        {
            vertices[right].y = (vertices[topLeft + size].y + vertices[mid].y + vertices[bottomLeft + size].y) / 3
                + Random.Range(-offset, offset);
        }
        else
        {
            vertices[right].y = (vertices[topLeft + size].y + vertices[mid].y + vertices[bottomLeft + size].y + vertices[mid + size].y) * 0.25f
                + Random.Range(-offset, offset);
        }
    }

    /// <summary>
    /// Set colour of terrain based on height of vertex
    /// </summary>
    private void ColourTerrain()
    {
        Mesh terrainMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = terrainMesh.vertices;
        Color[] colourArray = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y < 0.1f)
            {
                colourArray[i] = Color.yellow;
            }
            else if (vertices[i].y > maxHeight * 0.9f)
            {
                colourArray[i] = Color.white;
            }
            else if (vertices[i].y <= maxHeight * 0.9f && vertices[i].y >= maxHeight * 0.8f)
            {
                colourArray[i] = new Color(0.68f, 0.51f, 0.06f, 1);
            }
            else
            {
                colourArray[i] = Color.green;
            }
        }
        terrainMesh.colors = colourArray;

        terrainMesh.RecalculateBounds();
        terrainMesh.RecalculateNormals();

    }

}
