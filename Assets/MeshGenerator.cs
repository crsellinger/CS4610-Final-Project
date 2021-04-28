using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        createMesh();
        updateMesh();
    }

    void createMesh(){
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        //vertices
        for (int index = 0, z = 0; z <= zSize; z++){
            for (int x = 0; x <= xSize; x++){
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f; //perlin noise map to randomize y height
                vertices[index] = new Vector3(x, y, z);
                index++;
            }
        }

        //drawing triangles
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++){
            for (int i = 0; i < xSize; i++){
            //first triangle of quad
            triangles[tris] = vert;                 //(0,0,0) coord on quad
            triangles[tris + 1] = vert + xSize + 1; //(0,0,1)
            triangles[tris + 2] = vert + 1;          //(1,0,0)

            //second triangle of quad
            triangles[tris + 3] = vert + 1;         //(1,0,0)
            triangles[tris + 4] = vert + xSize + 1; //(0,0,1)
            triangles[tris + 5] = vert + xSize + 2; //(1,0,1)

            vert++;
            tris += 6;
            }
            vert++;
        }
    }

    void updateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos(){
        if (vertices == null){
            return;
        }

        for (int i = 0; i < vertices.Length; i++){
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }
}
