using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    public static Vector3[] vertices;
    int[] triangles;

    public int xSize;
    public int zSize;

    // Start is called before the first frame update
    void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        createMesh();
        updateMesh();
    }

    void createMesh(){
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float maxHeight = 20f;

        //ground offset to get smoother looking terrain
        float zoff = 0f;

        //vertices
        for (int index = 0, z = 0; z <= zSize; z++){

            //ground offset to get smoother looking terrain
            float xoff = 0;

            for (int x = 0; x <= xSize; x++){

                //Note: if maxHeight is changed, adjust offsets accordingly -> increase maxHeight, decrease offset and vice versa
                float y = Mathf.PerlinNoise(xoff + 0f, zoff + 0f) * maxHeight;   //perlin noise map to randomize y height
                vertices[index] = new Vector3(x, y, z);
                index++;
                xoff += 0.02f;  //incrementing offset makes smoother gradation, smaller value = smoother terrain (i.e. bigger hills)
            }

            zoff += 0.02f;
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


        for (int i = 0; i < objectsToSpawn; i++)
        {
            SpreadObject();
        }
    }

    void updateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        //So you don't fall through terrain mesh
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    private void OnDrawGizmos(){
        if (vertices == null){
            return;
        }

        for (int i = 0; i < vertices.Length; i++){
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    public GameObject[] objectsToSpread;
    public int objectsToSpawn = 30;
    public float objectXSpread = 100;
    public float objectZSpread = 100;

    void SpreadObject()
    {
        int r = Random.Range(0, vertices.Length - 1);
        int r2 = Random.Range(0, objectsToSpread.Length - 1);
        Vector3 randPos = vertices[r];
        Instantiate(objectsToSpread[r2], randPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update(){
        
    }
}
