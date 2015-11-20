using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class snakeWGenerator : MonoBehaviour {

	public float radi, varRadi, longi, varLongi, pCorba, pSalt, factVarNormal;
	public int nPunts, nPuntsPlans, nSegments; 
	public Vector3 origen, normalOrigen;

	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list

	float texCoordV;
	int segActual, currentRingVertexIndex, lastRingVertexIndex;
	segment[] buffSegments;

	[HideInInspector, System.NonSerialized]
	public MeshRenderer Renderer;
	MeshFilter filter;

	private class llesca{
		public Vector3 centre;
		public Quaternion normal;
	}
	private class segment{
		public float length;
		public llesca origen;
		public llesca desti;
	}

	public void addSegment (){
		var offset = Vector3.zero;
		var texCoord = new Vector2(0f, texCoordV);
		var textureStepU = 1f / (nPunts - nPuntsPlans);
		var ang = 0f;
		var angInc = 2f * Mathf.PI * textureStepU;
		int aux = (segActual + 1) % nSegments;
		//init segment
			//init longitud
		buffSegments[aux].length = Random.Range(longi - varLongi, longi + varLongi);
		var r = Random.Range(radi - varRadi, radi + varRadi);
		texCoordV += 0.0625f * (buffSegments[aux].length + buffSegments[aux].length / r);
			//init llesca origen
		buffSegments[aux].origen.centre = buffSegments[segActual].desti.centre;
		buffSegments[aux].origen.normal = buffSegments[segActual].desti.normal;
			//init llesca desti
		Quaternion qaux = buffSegments[aux].origen.normal;
		qaux.eulerAngles += new Vector3 (Random.Range(-factVarNormal,factVarNormal), 0, Random.Range(-factVarNormal,factVarNormal));
		buffSegments[aux].desti.normal = qaux;
		buffSegments[aux].desti.centre = buffSegments[aux].origen.centre + buffSegments[aux].origen.normal * new Vector3(0f, buffSegments[aux].length, 0f);

		for (var n = 0; n <= nPunts - nPuntsPlans; n++, ang += angInc) //afegir vertex anell node
		{
			offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
			offset.z = r * Mathf.Sin(ang);
			vertexList.Add(buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset); // Add Vertex position
			uvList.Add(texCoord); // Add UV coord
			texCoord.x += textureStepU;
		}

		for (var currentRingVertexIndex = vertexList.Count - nPunts - nPuntsPlans - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) {
			triangleList.Add (lastRingVertexIndex + 1); // Triangle A
			triangleList.Add (lastRingVertexIndex);
			triangleList.Add (currentRingVertexIndex);
			triangleList.Add (currentRingVertexIndex); // Triangle B
			triangleList.Add (currentRingVertexIndex + 1);
			triangleList.Add (lastRingVertexIndex + 1);
		}

		lastRingVertexIndex = vertexList.Count  - nPunts - nPuntsPlans - 1;
		segActual = aux;
	}

	private void SetMesh()
	{
		// Get mesh or create one
		var mesh = filter.sharedMesh;
		if (mesh == null) 
			mesh = filter.sharedMesh = new Mesh();
		else 
			mesh.Clear();
		
		// Assign vertex data
		mesh.vertices = vertexList.ToArray();
		mesh.uv = uvList.ToArray();
		mesh.triangles = triangleList.ToArray();
		
		// Update mesh
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
	

	void OnEnable()
	{
		gameObject.isStatic = false;
		filter = gameObject.GetComponent<MeshFilter> ();
		if (filter == null)
			filter = gameObject.AddComponent<MeshFilter> ();
		Renderer = gameObject.GetComponent<MeshRenderer> ();
		if (Renderer == null)
			Renderer = gameObject.AddComponent<MeshRenderer> ();
	}

	// Use this for initialization
	void Start () {
		vertexList = new List<Vector3>();
		uvList = new List<Vector2>();
		triangleList = new List<int>();
		currentRingVertexIndex = 0;
		lastRingVertexIndex = 0;
		buffSegments = new segment[nSegments];
		segActual = 1;
		buffSegments [0] = new segment();
		buffSegments [0].length = longi;
		buffSegments [0].origen = new llesca();
		buffSegments [0].desti = new llesca();
		buffSegments [0].origen.normal = new Quaternion();
		buffSegments [0].origen.centre = origen;
		buffSegments [0].desti.normal = new Quaternion();
		buffSegments [0].desti.centre = origen;
		for (int i = 1; i < nSegments; ++i) {
			buffSegments [i] = new segment();
			buffSegments [i].origen = new llesca();
			buffSegments [i].desti = new llesca();
		}
		for (int i = 0; i < nSegments; ++i) addSegment();
		SetMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
