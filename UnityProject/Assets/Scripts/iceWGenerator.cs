using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class iceWGenerator : MonoBehaviour {
	
	public float radi, varRadi, longi, varLongi, pTini, pTfi, dSalt, pSalt, pMony, factVarNormal;
	public int nPunts, angPla, nSegments, maxPendent, segmentsXobstacle, dificultLvl;
	public Vector3 origen, normalOrigen;
	public GameObject mony1, mony2, mony3 , mony4, mony5, final;
	public GameObject[] simples, combos1, combos2;
	private int segPava = 0;
	private int sXo;
	private bool over = false;
	private float pTrampa, pStep;
	public bool isLast = false;
	private bool lastSeg = false;
	
	//private ground gScript;
	
	bool prevSalt = false;
	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list
	List<Vector3> colvertexList;
	List<int> coltriangleList;
	
	float texCoordV;
	int segActual, currentRingVertexIndex, lastRingVertexIndex;
	segment[] buffSegments;
	MeshCollider meshc;
	
	/*      [HideInInspector, System.NonSerialized]
        public MeshRenderer Renderer;
        MeshFilter filter;      */
	Mesh mesh;
	
	public struct llesca{
		public Vector3 centre;
		public Quaternion normal;
	}
	public struct segment{
		public float length;
		public llesca origen;
		public llesca desti;
		public GameObject ground;
		public GameObject lD;
		public GameObject lE;
		public GameObject money;
	//	public ground gS;
	}
	
	private void addSegment (){
		bool isSalt = false;
		var offset = Vector3.zero;
		var texCoord = new Vector2 (0f, texCoordV);
		var textureStepU = 1f / (nPunts);
		var ang = 0f;
		var angInc = 2f * Mathf.PI * textureStepU;
		int aux = (segActual + 1) % nSegments;
		Vector3 xo, yo, xf, yf;
		xf = new Vector3 ();
		yf = new Vector3 ();

		++sXo;
		//init segment
		//init longitud
		if (pSalt >= Random.Range (0.0f, 1.0f) && prevSalt == false && sXo == segmentsXobstacle && aux < nSegments -2 && aux > 1) {
			sXo = 0;
			isSalt = true;
			prevSalt = true;
			buffSegments [aux].length = dSalt;
		} else {
			buffSegments [aux].length = Random.Range (longi - varLongi, longi + varLongi);
			prevSalt = false;
		}
		var r = radi;//Random.Range(radi - varRadi, radi + varRadi);
		texCoordV += 0.0625f * (buffSegments [aux].length + buffSegments [aux].length / r);
		//init llesca origen
		buffSegments [aux].origen.centre = buffSegments [segActual].desti.centre;
		buffSegments [aux].origen.normal = buffSegments [segActual].desti.normal;
		//init llesca desti
		Quaternion qaux = buffSegments [aux].origen.normal;
		qaux.eulerAngles += new Vector3 (Random.Range (-factVarNormal, factVarNormal), Random.Range (-factVarNormal, factVarNormal), 0);
		Vector3 angleAux;
		angleAux.y = qaux.eulerAngles.y;
		angleAux.z = qaux.eulerAngles.z;
		if (qaux.eulerAngles.x > 90 + maxPendent)
			angleAux.x = 90 + maxPendent;
		else if (qaux.eulerAngles.x < 90 - maxPendent)
			angleAux.x = 90 - maxPendent;
		else
			angleAux.x = qaux.eulerAngles.x;
		//      if (isSalt)     angleAux.x = -maxPendent;
		qaux.eulerAngles = angleAux;
		buffSegments [aux].desti.normal = qaux;
		buffSegments [aux].desti.centre = buffSegments [aux].origen.centre + buffSegments [aux].origen.normal * new Vector3 (0f, buffSegments [aux].length, 0f);
		angInc = ((360 - angPla) * Mathf.PI) / (180 * (nPunts - 1));
		ang = ((270 + angPla / 2) * Mathf.PI) / (180);
		for (var n = 0; n <= nPunts-1 /*- 1*/; n++, ang += angInc) { //afegir vertex anell node
			offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
			offset.z = r * Mathf.Sin (ang);
			vertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
			colvertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); 
			if (!isSalt) {
				uvList.Add (texCoord);
				texCoord.x += textureStepU;
			} // Add UV coord
			else
				uvList.Add (new Vector2 (0, 0));
			if (n == 0)
				xf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
			else if (n == nPunts - 1) {
				yf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
				ang = ((270 + angPla / 2) * Mathf.PI) / (180);
				offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin (ang);
				vertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
				colvertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
				if (!isSalt) {
					uvList.Add (texCoord);
					texCoord.x += textureStepU;
				} // Add UV coord
				else
					uvList.Add (new Vector2 (0, 0));
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
				coltriangleList.Add (lastRingVertexIndex + 1); // Triangle A
				coltriangleList.Add (lastRingVertexIndex);
				coltriangleList.Add (currentRingVertexIndex);
				coltriangleList.Add (currentRingVertexIndex); // Triangle B
				coltriangleList.Add (currentRingVertexIndex + 1);
				coltriangleList.Add (lastRingVertexIndex + 1);
			}
		} else {
			int current = vertexList.Count - nPunts - 1;
			for (int i = 0; i < nPunts; ++i) {
				triangleList.Add (current + i); // Triangle A
				triangleList.Add (current + (i + 1) % nPunts);
				triangleList.Add (current + (i + nPunts / 2) % nPunts);
				coltriangleList.Add (current + i); // Triangle A
				coltriangleList.Add (current + (i + 1) % nPunts);
				coltriangleList.Add (current + (i + nPunts / 2) % nPunts);
			}
			ang = ((270 + angPla / 2) * Mathf.PI) / (180);
			for (var n = 0; n <= nPunts-1 /*- 1*/; n++, ang += angInc) { //afegir vertex anell node
				offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
				offset.z = r * Mathf.Sin (ang);
				vertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
				colvertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
				uvList.Add (texCoord);
				texCoord.x += textureStepU;
				if (n == 0)
					xf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
				else if (n == nPunts - 1) {
					yf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
					ang = ((270 + angPla / 2) * Mathf.PI) / (180);
					offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
					offset.z = r * Mathf.Sin (ang);
					vertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
					colvertexList.Add (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset); // Add Vertex position
					uvList.Add (texCoord);
					texCoord.x += textureStepU;
				}
				
			}
		}
		lastRingVertexIndex = vertexList.Count - nPunts - 1;
		
		//      buffSegments[aux].ground = Instantiate(terra, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
		ang = ((270 + angPla / 2) * Mathf.PI) / (180);
		offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
		offset.z = r * Mathf.Sin (ang);
		xo = (buffSegments [aux].origen.centre + buffSegments [aux].origen.normal * offset);
		ang = ((270 - angPla / 2) * Mathf.PI) / (180);
		offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
		offset.z = r * Mathf.Sin (ang);
		yo = (buffSegments [aux].origen.centre + buffSegments [aux].origen.normal * offset);
		
		if (!isSalt && !prevSalt && sXo == segmentsXobstacle && segPava != nSegments -1) {
			sXo = 0;
			if (pTrampa >= Random.Range (0.0f, 1.0f)){
				Debug.Log("hey");
				if(dificultLvl == 0){
					Debug.Log("0");
					int auxObs = Random.Range (0, simples.Length)%simples.Length;
					buffSegments[aux].ground = Instantiate(simples[auxObs], buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r+0.1f, 0));
					prevSalt = true;
				}
				else if(dificultLvl == 1){
					Debug.Log("1");
					int auxObs = Random.Range (0, combos1.Length)%combos1.Length;
					buffSegments[aux].ground = Instantiate(combos1[auxObs], buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r+0.1f, 0));
					prevSalt = true;
				}
				else if(dificultLvl == 2){
					Debug.Log("2");
					int auxObs = Random.Range (0, combos2.Length)%combos2.Length;
					buffSegments[aux].ground = Instantiate(combos2[auxObs], buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r+0.1f, 0));
					prevSalt = true;
				}

				/*
				if(pValla >= Random.Range (0.0f, 1.0f)){
					buffSegments[aux].ground = Instantiate(valla, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r, 0));
					prevSalt = true;
				}
				else if (pPorta >= Random.Range (0.0f, 1.0f)){
					buffSegments[aux].ground = Instantiate(porta, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r, 0));
					prevSalt =true;
				}
				else if(pPinxus >= Random.Range (0.0f, 1.0f)){
					float p = Random.Range (0.0f, 1.0f);
					if(p < 0.33)buffSegments[aux].ground = Instantiate(pinxus, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					else if(p > 0.33 && p < 0.66)buffSegments[aux].ground = Instantiate(pinxusR, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					else buffSegments[aux].ground = Instantiate(pinxusL, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), Quaternion.identity) as GameObject;
					buffSegments[aux].ground.transform.LookAt(buffSegments[aux].origen.centre + new Vector3(0, r, 0));
					prevSalt =true;
				}
				*/
			}
		}
		if (lastSeg && isLast) {
			Instantiate(final, yf, buffSegments [aux].desti.normal);
		}
		if (pMony >= Random.Range (0.0f, 1.0f)) {
			float p = Random.Range (0.0f, 1.0f);
			if(p < 0.2) buffSegments[aux].money = Instantiate(mony1, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), buffSegments [aux].desti.normal) as GameObject;
			else if(p<0.4) buffSegments[aux].money = Instantiate(mony2, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), buffSegments [aux].desti.normal) as GameObject;
			else if(p<0.6) buffSegments[aux].money = Instantiate(mony3, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), buffSegments [aux].desti.normal) as GameObject;
			else if(p<0.8) buffSegments[aux].money = Instantiate(mony5, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), buffSegments [aux].desti.normal) as GameObject;
			else buffSegments[aux].money =  Instantiate(mony4, buffSegments[aux].desti.centre + buffSegments[aux].desti.normal * new Vector3(0f,0f,-r), buffSegments [aux].desti.normal) as GameObject;
		}
	//	buffSegments[aux].lE = Instantiate(pendentE, yf, buffSegments [aux].desti.normal) as GameObject;
	//	buffSegments[aux].lD = Instantiate(pendentD, xf, buffSegments [aux].desti.normal) as GameObject;
		pTrampa += pStep;
		segActual = aux;
	}

	public segment createFirstSeg(){
		segPava = 4;
		segment saux = new segment();
		saux.length = 100;
		saux.origen = new llesca();
		saux.desti = new llesca();
		saux.origen.normal = new Quaternion();
		//buffSegments [0].origen.normal.SetLookRotation (normalOrigen, new Vector3 (1.0f, 0.0f, 0.0f));
		saux.origen.normal.eulerAngles = normalOrigen;
		saux.origen.centre = origen;
		saux.desti.normal = new Quaternion();
		saux.desti.normal.eulerAngles = normalOrigen;
		saux.desti.centre = origen;
		return saux;
	}
	public segment getLastSeg(){
		return buffSegments [nSegments - 1];
	}
	private void SetMesh()
	{
		// Get mesh or create one
		/*      var mesh = filter.sharedMesh;
                if (mesh == null)
                        mesh = filter.sharedMesh = new Mesh();
                else
                        mesh.Clear();   */
		mesh.Clear ();
		// Assign vertex data
		mesh.vertices = vertexList.ToArray();
		mesh.uv = uvList.ToArray();
		mesh.triangles = triangleList.ToArray();
		
		// Update mesh
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		Mesh newMesh = new Mesh ();
		newMesh.vertices = colvertexList.ToArray ();
		newMesh.triangles = coltriangleList.ToArray ();
		meshc.sharedMesh = newMesh;
	}
	
	
	void OnEnable()
	{
		gameObject.isStatic = false;
		vertexList = new List<Vector3>();
		uvList = new List<Vector2>();
		triangleList = new List<int>();
		colvertexList = new List<Vector3>();
		coltriangleList = new List<int>();
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

	//	Destroy(rigidbody);
	/*	Rigidbody rigid = gameObject.AddComponent(typeof (Rigidbody)) as Rigidbody;
		rigid.useGravity = false;
		rigid.isKinematic = true;
		rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;	*/
		meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshc.sharedMesh = GetComponent<MeshFilter>().mesh;
	}

	public Quaternion nouse(float z){
		int i = 0;
		int last_seg = segPava;
		while (i < nSegments && !(z < buffSegments[i].desti.centre.z && z > buffSegments[i].origen.centre.z) ){
			++i;
		}
		if (i >= nSegments)
			segPava = last_seg;
		else segPava = i;
		
		if (z > buffSegments [nSegments - 1].desti.centre.z) {
			over = true;
			
		}
		return buffSegments[segPava].origen.normal;
	}

	public bool isFalling(float y){
		if (y < Mathf.Min(buffSegments [segPava % nSegments].origen.centre.y - radi, buffSegments [segPava % nSegments].origen.centre.y -radi))
			return true;
		else
			return false;

	}

	public Vector3 inici (){
		return buffSegments [segPava].origen.centre + new Vector3 (0,radi + 1, 1);
	}

	public bool isOver(){
		return over;
	}

	public void destroySons(){

		for (int i = 0; i < nSegments; ++i){
			Destroy (buffSegments [i].lD);
			Destroy (buffSegments [i].lE);
			if(buffSegments[i].ground != null)Destroy (buffSegments [i].ground);
			if(buffSegments[i].money != null)Destroy (buffSegments [i].money);
		}  
		mesh.Clear ();
		meshc.sharedMesh.Clear ();
	}
	
	// Use this for initialization
	public void addFragment (segment inici, float pIni, float pFi, int level) {
		pTrampa = pIni;
		pStep = (pFi - pIni) / nSegments;
		dificultLvl = level;
		vertexList.Clear ();
		uvList.Clear();
		triangleList.Clear();
		colvertexList.Clear();
		coltriangleList.Clear();
		currentRingVertexIndex = 0;
		lastRingVertexIndex = 0;
		buffSegments = new segment[nSegments];
		segActual = 0;
		buffSegments[0] = inici;
		sXo = 0;
	//	gScript = terra.GetComponent<ground >();
		for (int i = 1; i < nSegments; ++i) {
			buffSegments [i] = new segment();
			buffSegments [i].origen = new llesca();
			buffSegments [i].desti = new llesca();
		}
		for (int i = 0; i < nSegments; ++i) {
			if(i == nSegments - 1) {lastSeg = true;Debug.Log("is last");}
			addSegment ();
		}
		SetMesh ();
	//	gScript.SetMesh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}