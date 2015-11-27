using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ground : MonoBehaviour {

	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list
	
	public int triu, trid, trit;

	int  currentRingVertexIndex, lastRingVertexIndex;
	float texCoordV;
	bool first = true;
	Vector3 prevXo, prevYo, prevXf, prevYf;

//	[HideInInspector, System.NonSerialized]
//	public MeshRenderer Renderer;
//	MeshFilter filter;
	public MeshFilter gMesh = null;


	public void addGroundSeg (Vector3 xo, Vector3 xf, Vector3 yo, Vector3 yf){
		var offset = Vector3.zero;
		var texCoord = new Vector2(0,0);
		xo.y += 0.1f;
		xf.y += 0.1f;
		yo.y += 0.1f;
		yf.y += 0.1f;
	/*	xo.z -= 1f;
		xf.z += 1f;
		yo.z -= 1f;
		yf.z += 1f;   */

		//if (!first) {
			vertexList.Add (xo); // Add Vertex position
			uvList.Add (texCoord); 
			texCoord = new Vector2(0,1);
			vertexList.Add (yo); // Add Vertex position
			uvList.Add (texCoord); 
			texCoord = new Vector2(1,0);
			vertexList.Add (xf); // Add Vertex position
			uvList.Add (texCoord); 
			texCoord = new Vector2(1,1);
			vertexList.Add (yf); // Add Vertex position
			uvList.Add (texCoord); 
		
			currentRingVertexIndex = vertexList.Count - 4;

			triangleList.Add (currentRingVertexIndex + 1); // Triangle A
			triangleList.Add (currentRingVertexIndex + 0);
			triangleList.Add (currentRingVertexIndex + 3);
			triangleList.Add (currentRingVertexIndex + 2); // Triangle B
			triangleList.Add (currentRingVertexIndex + 3);
			triangleList.Add (currentRingVertexIndex + 0);
	//	}
	//	first = false;
		prevXo = xo;
		prevYo = yo;
		prevXf = xf;
		prevYf = yf;
	//	lastRingVertexIndex = vertexList.Count - 2;
	//	SetMesh ();

	}
	public void SetMesh()
	{
		Mesh ret = new Mesh();

		ret.vertices = vertexList.ToArray();
		ret.uv = uvList.ToArray();
		ret.triangles = triangleList.ToArray();
		
		// Update mesh
		ret.RecalculateNormals();
		ret.RecalculateBounds();
		gMesh.mesh = ret;
	}

	void OnEnable()
	{
		gameObject.isStatic = false;
		vertexList = new List<Vector3>();
		uvList = new List<Vector2>();
		triangleList = new List<int>();
		currentRingVertexIndex = 0;
		lastRingVertexIndex = 0;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
