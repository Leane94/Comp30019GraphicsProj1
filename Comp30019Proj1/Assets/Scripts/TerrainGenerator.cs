using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    // Number of divisions per line (e.g. 5 vertices/line => 4*4 divisions)
    public int NumDivisions = 128;

    // Width/Length of the terrain in the world (terrain is a square)
    public float TerrainWidth = 30;

    // Max height of the terrain
    public float MaxHeight = 5;

    // Range for the random number to be added in each iteration
    public float HeightDecreaseRate = 0.5f;

    public Shader shader;

    // Array of all vertices
    private Vector3[] Vertices;

    // Total number of vertices
    private int NumVertices;

    // Array of UV vectors
    private Vector2[] UVs;

    // Array of all triangles on mesh
    private int[] Triangles;

	// Use this for initialization
	void Start () {
        CreateTerrain();
        ColourTerrain();
        this.gameObject.GetComponent<MeshRenderer>().material.shader = shader;
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateTerrain()
    {
        // Initialisation
        NumVertices = (NumDivisions + 1) * (NumDivisions + 1);
        Vertices = new Vector3[NumVertices];
        UVs = new Vector2[NumVertices];
        Triangles = new int[NumDivisions * NumDivisions * 6];

        float HalfWidth = TerrainWidth * 0.5f;
        float DivisionWidth = TerrainWidth / NumDivisions;

        // Create a new mesh
        Mesh TerrainMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = TerrainMesh;

        int TriangleOffset = 0;

        // Set up array of Vertices, UVs and Triangles
        for (int i = 0; i <= NumDivisions; i++)
        {
            for (int j = 0; j <= NumDivisions; j++)
            {
                // Make Terrain's center to be (0,0)
                Vertices[i * (NumDivisions + 1) + j] = new Vector3((-HalfWidth + j * DivisionWidth), 0.0f, (HalfWidth - i * DivisionWidth));
                UVs[i * (NumDivisions + 1) + j] = new Vector2((float)i / NumDivisions, (float)j / NumDivisions);

                if (i < NumDivisions && j < NumDivisions)
                {
                    int TopLeft = i * (NumDivisions + 1) + j;
                    int BottomLeft = (i + 1) * (NumDivisions + 1) + j;

                    // Triangles has to be clock-wise to be rendered
                    Triangles[TriangleOffset] = TopLeft;
                    Triangles[TriangleOffset + 1] = TopLeft + 1;
                    Triangles[TriangleOffset + 2] = BottomLeft;

                    Triangles[TriangleOffset + 3] = TopLeft + 1;
                    Triangles[TriangleOffset + 4] = BottomLeft + 1;
                    Triangles[TriangleOffset + 5] = BottomLeft;

                    TriangleOffset += 6;
                }
            }
        }

        DiamondSquare(TerrainMesh);

        // Update mesh
        TerrainMesh.vertices = Vertices;
        TerrainMesh.uv = UVs;
        TerrainMesh.triangles = Triangles;

        TerrainMesh.RecalculateBounds();
        TerrainMesh.RecalculateNormals();
    }

    // Diamond-Square algorithms
    private void DiamondSquare(Mesh TerrainMesh)
    {
        // Initialize corner of the terrain
        Vertices[0].y = Random.Range(-MaxHeight, MaxHeight);
        Vertices[NumDivisions].y = Random.Range(-MaxHeight, MaxHeight);
        Vertices[Vertices.Length - 1 - NumDivisions].y = Random.Range(-MaxHeight, MaxHeight);
        Vertices[Vertices.Length - 1].y = Random.Range(-MaxHeight, MaxHeight);

        int Iteration = (int)Mathf.Log(NumDivisions, 2);
        int SquareCounter = 1;
        int SquareSize = NumDivisions;
        float HeightOffset = MaxHeight;

        for (int i = 0; i < Iteration; i++)
        {
            int Row = 0;
            for (int j = 0; j < SquareCounter; j++)
            {
                int Col = 0;
                for (int k = 0; k < SquareCounter; k++)
                {
                    DSCalculator(Row, Col, SquareSize, HeightOffset);
                    Col += SquareSize;
                }
                Row += SquareSize;
            }
            SquareCounter *= 2;
            SquareSize /= 2;
            HeightOffset *= HeightDecreaseRate;
        }
    }

    // Diamond Square calculation
    private void DSCalculator(int Row, int Col, int Size, float Offset)
    {
        int HalfSize = (int)(0.5f * Size);
        int TopLeft = Row * (NumDivisions + 1) + Col;
        int BottomLeft = (Row + Size) * (NumDivisions + 1) + Col;

        // Diamond step
        int Mid = (Row + HalfSize) * (NumDivisions + 1) + (Col + HalfSize);
        Vertices[Mid].y = (Vertices[TopLeft].y + Vertices[BottomLeft].y + Vertices[TopLeft + Size].y + Vertices[BottomLeft + Size].y) * 0.25f
            + Random.Range(-Offset, Offset);

        // Square step
        int Top = TopLeft + HalfSize;
        int Left = Mid - HalfSize;
        int Bottom = BottomLeft + HalfSize;
        int Right = Mid + HalfSize;

        // If vertex on top edge of terrain
        if (Top <= NumDivisions)
        {
            Vertices[Top].y = (Vertices[TopLeft].y + Vertices[Mid].y + Vertices[TopLeft + Size].y) / 3 
                + Random.Range(-Offset, Offset);
        }
        else
        {
            int Temp = (Row - HalfSize) * (NumDivisions + 1) + Col + HalfSize;
            Vertices[Top].y = (Vertices[TopLeft].y + Vertices[Mid].y + Vertices[TopLeft + Size].y + Vertices[Temp].y) * 0.25f
                + Random.Range(-Offset, Offset);
        }

        // If vertex on left edge of terrain
        if (Left % (NumDivisions+1) == 0)
        {
            Vertices[Left].y = (Vertices[TopLeft].y + Vertices[Mid].y + Vertices[BottomLeft].y) / 3 
                + Random.Range(-Offset, Offset);
        }
        else
        {
            Vertices[Left].y = (Vertices[TopLeft].y + Vertices[Mid].y + Vertices[BottomLeft].y + Vertices[Mid-Size].y) * 0.25f
                + Random.Range(-Offset, Offset);
        }

        // If vertex on bottom edge of terrain
        if (Bottom >= (NumVertices - 1 - NumDivisions)){
            Vertices[Bottom].y = (Vertices[BottomLeft].y + Vertices[Mid].y + Vertices[BottomLeft + Size].y) / 3 
                + Random.Range(-Offset, Offset);
        }
        else
        {
            int Temp = (Row + 3 * HalfSize) + (NumDivisions + 1) + Col + HalfSize;
            Vertices[Bottom].y = (Vertices[BottomLeft].y + Vertices[Mid].y + Vertices[BottomLeft + Size].y + Vertices[Temp].y) * 0.25f
                + Random.Range(-Offset, Offset);
        }

        // If vertex on right edge of terrain
        if ((Right+1) % (NumDivisions + 1) == 0)
        {
            Vertices[Right].y = (Vertices[TopLeft + Size].y + Vertices[Mid].y + Vertices[BottomLeft + Size].y) / 3 
                + Random.Range(-Offset, Offset);
        }
        else
        {
            Vertices[Right].y = (Vertices[TopLeft + Size].y + Vertices[Mid].y + Vertices[BottomLeft + Size].y + Vertices[Mid+Size].y) * 0.25f
                + Random.Range(-Offset, Offset);
        }
    }

    // Set colour of terrain based on height of vertex
    private void ColourTerrain()
    {
        Mesh TerrainMesh = GetComponent<MeshFilter>().mesh;
        Color[] ColourArray = new Color[Vertices.Length];
        for (int i = 0; i < Vertices.Length; i++)
        {
            if (Vertices[i].y < 0.1f)
            {
                ColourArray[i] = Color.yellow;
            }
            else if (Vertices[i].y > MaxHeight * 0.9f)
            {
                ColourArray[i] = Color.white;
            }
            else
            {
                ColourArray[i] = Color.green;
            }
        }
        TerrainMesh.colors = ColourArray;

        TerrainMesh.RecalculateBounds();
        TerrainMesh.RecalculateNormals();

    }

}
