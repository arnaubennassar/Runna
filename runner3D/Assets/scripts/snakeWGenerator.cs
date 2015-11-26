using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class snakeWGenerator : MonoBehaviour {

	public float radi, varRadi, longi, varLongi, pCorba, dSalt, pSalt, factVarNormal;
	public int nPunts, angPla, nSegments, maxPendent; 
	public Vector3 origen, normalOrigen;
	public GameObject terra;
	private ground gScript;

	bool prevSalt = false;
	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list

	float texCoordV;
	int segActual, currentRingVertexIndex, lastRingVertexIndex;
	segment[] buffSegments;


/*	[HideInInspector, System.NonSerialized]
	public MeshRenderer Renderer;
	MeshFilter filter;	*/
	Mesh mesh;

	private class llesca{
		public Vector3 centre;
		public Quaternion normal;
	}
	private class segment{
		public float length;
		public llesca origen;
		public llesca desti;
		public GameObject ground;
		public ground gS;
	}

	public void addSegment (){
		bool isSalt = false;
		var offset = Vector3.zero;
		var texCoord = new Vector2(0f, texCoordV);
		var textureStepU = 1f / (nPunts);
		var ang = 0f;
		var angInc = 2f * Mathf.PI * textureStepU;
		int aux = (segActual + 1) % nSegments;
		Vector3 xo, yo, xf, yf;
		xf = new Vector3 ();
		yf = new Vector3 ();

		//init segment
			//init longitud
		if (pSalt >= Random.Range (0.0f, 1.0f) && prevSalt == false) {
			isSalt = true;
			prevSalt = true;
			buffSegments [aux].length = dSalt;
		} else {
			buffSegments [aux].length = Random.Range (longi - varLongi, longi + varLongi);
			prevSalt = false;
		}
		var r = radi;//Random.Range(radi - varRadi, radi + varRadi);
		texCoordV += 0.0625f * (buffSegments[aux].length + buffSegments[aux].length / r);
			//init llesca origen
		buffSegments[aux].origen.centre = buffSegments[segActual].desti.centre;
		buffSegments[aux].origen.normal = buffSegments[segActual].desti.normal;
			//init llesca desti
		Quaternion qaux = buffSegments[aux].origen.normal;
		qaux.eulerAngles += new Vector3 (Random.Range(-factVarNormal,factVarNormal), Random.Range(-factVarNormal,factVarNormal), 0);
		Vector3 angleAux;
		angleAux.y = qaux.eulerAngles.y;
		angleAux.z = qaux.eulerAngles.z;
		if (qaux.eulerAngles.x > 90 + maxPendent) angleAux.x = 90 + maxPendent;
		else if (qaux.eulerAngles.x < 90 - maxPendent) angleAux.x = 90 - maxPendent;
		else angleAux.x = qaux.eulerAngles.x;
	//	if (isSalt)	angleAux.x = -maxPendent;
		qaux.eulerAngles = angleAux;
		buffSegments[aux].desti.normal = qaux;
		buffSegments[aux].desti.centre = buffSegments[aux].origen.centre + buffSegments[aux].origen.normal * new Vector3(0f, buffSegments[aux].length, 0f);
		angInc = ((360- angPla)*Mathf.PI)/(180 * (nPunts-1));
		ang = ((270 + angPla/2)*Mathf.PI)/(180);
		for (var n = 0; n <= nPunts-1 /*- 1*/; n++, ang += angInc) //afegir vertex anell node
		{
				offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin(ang);
				vertexList.Add(buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset); // Add Vertex position
			if(!isSalt){
				uvList.Add(texCoord);
				texCoord.x += textureStepU;
			} // Add UV coord
			else uvList.Add(new Vector2(0,0));
			if (n == 0) xf = (buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset);	
			else if(n == nPunts-1){
				yf = (buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset);
				ang = ((270 + angPla/2)*Mathf.PI)/(180);
				offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin(ang);
				vertexList.Add(buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset); // Add Vertex position
				if(!isSalt){
					uvList.Add(texCoord);
					texCoord.x += textureStepU;
				} // Add UV coord
				else uvList.Add(new Vector2(0,0));
			}

		}

		if (!isSalt) {
			for (var currentRingVertexIndex = vertexList.Count - nPunts - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) {
				triangleList.Add (lastRingVertexIndex + 1); // Triangle A
				triangleList.Add (lastRingVertexIndex);
				triangleList.Add (currentRingVertexIndex);
				triangleList.Add (currentRingVertexIndex); // Triangle B
				triangleList.Add (currentRingVertexIndex + 1);
				triangleList.Add (lastRingVertexIndex + 1);
			}
		} else {
			int current = vertexList.Count - nPunts - 1;
			for (int i = 0; i < nPunts; ++i) {
				triangleList.Add (current + i); // Triangle A
				triangleList.Add (current + (i+1)%nPunts);
				triangleList.Add (current + (i+nPunts/2)%nPunts);
			}
			ang = ((270 + angPla/2)*Mathf.PI)/(180);
			for (var n = 0; n <= nPunts-1 /*- 1*/; n++, ang += angInc) //afegir vertex anell node
			{
				offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin(ang);
				vertexList.Add(buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset); // Add Vertex position
					uvList.Add(texCoord);
					texCoord.x += textureStepU;
				if (n == 0) xf = (buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset);	
				else if(n == nPunts-1){
					yf = (buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset);
					ang = ((270 + angPla/2)*Mathf.PI)/(180);
					offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
					offset.z = r * Mathf.Sin(ang);
					vertexList.Add(buffSegments[aux].desti.centre + buffSegments[aux].origen.normal * offset); // Add Vertex position
						uvList.Add(texCoord);
						texCoord.x += textureStepU;
				}
				
			}
		} 
		lastRingVertexIndex = vertexList.Count  - nPunts - 1;

	//	buffSegments[aux].ground = Instantiate(terra, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
		ang = ((270 + angPla/2)*Mathf.PI)/(180);
		offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
		offset.z = r * Mathf.Sin(ang);
		xo = (buffSegments[aux].origen.centre + buffSegments[aux].origen.normal * offset);
		ang = ((270 - angPla/2)*Mathf.PI)/(180);
		offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
		offset.z = r * Mathf.Sin(ang);
		yo = (buffSegments[aux].origen.centre + buffSegments[aux].origen.normal * offset);






			  
		//buffSegments[aux].gS = buffSegments[aux].ground.GetComponent<ground >();                 



	/*	buffSegments [aux]. */if(!isSalt)gScript.addGroundSeg (xo, yo, xf, yf);
		/*	buffSegments[aux].ground.transform.localScale = new Vector3(r * Mathf.Sin(angPla)+2, buffSegments[aux].length+2, 1);
		buffSegments [aux].ground.transform.Rotate (buffSegments[aux].desti.normal.eulerAngles );
		*/
		segActual = aux;
	}

	private void SetMesh()
	{
		// Get mesh or create one
	/*	var mesh = filter.sharedMesh;
		if (mesh == null) 
			mesh = filter.sharedMesh = new Mesh();
		else 
			mesh.Clear();	*/
		mesh.Clear ();
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
	/*	filter = gameObject.GetComponent<MeshFilter> ();
		if (filter == null)
			filter = gameObject.AddComponent<MeshFilter> ();
		Renderer = gameObject.GetComponent<MeshRenderer> ();
		if (Renderer == null)
			Renderer = gameObject.AddComponent<MeshRenderer> ();	*/
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	// Use this for initialization
	void Start () {
		vertexList = new List<Vector3>();
		uvList = new List<Vector2>();
		triangleList = new List<int>();
		currentRingVertexIndex = 0;
		lastRingVertexIndex = 0;
		buffSegments = new segment[nSegments];
		segActual = 0;
		buffSegments [0] = new segment();
		buffSegments [0].length = 100;
		buffSegments [0].origen = new llesca();
		buffSegments [0].desti = new llesca();
		buffSegments [0].origen.normal = new Quaternion();
		//buffSegments [0].origen.normal.SetLookRotation (normalOrigen, new Vector3 (1.0f, 0.0f, 0.0f));
		buffSegments [0].origen.normal.eulerAngles = normalOrigen;
		buffSegments [0].origen.centre = origen;
		buffSegments [0].desti.normal = new Quaternion();
		buffSegments [0].desti.normal = buffSegments [0].origen.normal;
		buffSegments [0].desti.centre = origen;
		gScript = terra.GetComponent<ground >();
		for (int i = 1; i < nSegments; ++i) {
			buffSegments [i] = new segment();
			buffSegments [i].origen = new llesca();
			buffSegments [i].desti = new llesca();
		}
		for (int i = 0; i < nSegments; ++i) addSegment();
		SetMesh ();
		gScript.SetMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
