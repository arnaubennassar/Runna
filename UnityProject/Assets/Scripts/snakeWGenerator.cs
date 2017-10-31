using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class snakeWGenerator : MonoBehaviour {
	
	public float radi, varRadi, longi, varLongi, pTini, pTfi, dSalt, pSalt, pMony, factVarNormal;
	public int nPunts, angPla, nSegments, maxPendent, segmentsXobstacle, dificultLvl;
	public Vector3 origen, normalOrigen;
	public GameObject pendentD, pendentE, final;
	public GameObject[] simples, combos1, combos2, mony;
	private int segPava = 0;
	private int sXo;
	private bool over = false;
	private float pTrampa, pStep;
	public bool isLast = false;
	private bool lastSeg = false;
	private Vector3 nextXO, nextYO;
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
		public float maxZ, minZ, maxX, minX;
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
		buffSegments [aux].minZ = 99999999;
		Vector3 xo, yo, xf, yf;
		xf = new Vector3 ();
		yf = new Vector3 ();
		
		++sXo;
		//init segment
		//init longitud
		if (pSalt >= Random.Range (0.0f, 1.0f) && prevSalt == false && sXo == segmentsXobstacle && aux < nSegments - 2 && aux > 1) {
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
			if (n == 0){
				xf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
			//	nextXO = xf;
				if(buffSegments [aux].maxZ < xf.z)buffSegments [aux].maxZ = xf.z;
				else if(buffSegments [aux].minZ > xf.z){buffSegments [aux].minZ = xf.z;}
			}
			else if (n == nPunts - 1) {
				yf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
		//		nextYO = yf;
				if(buffSegments [aux].maxZ < yf.z)buffSegments [aux].maxZ = yf.z;
				else if(buffSegments [aux].minZ > yf.z){buffSegments [aux].minZ = yf.z;}
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
				if (n == 0){
					xf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
		/*		//	nextXO = xf;
					if(buffSegments [aux].maxZ < xf.z)buffSegments [aux].maxZ = xf.z;
					else if(buffSegments [aux].minZ > xf.z){buffSegments [aux].minZ = xf.z;Debug.Log("minz2 "+xf.z);}	*/
				}
				else if (n == nPunts - 1) {
					yf = (buffSegments [aux].desti.centre + buffSegments [aux].origen.normal * offset);
				//	nextYO = yf;
			/*		if(buffSegments [aux].maxZ < yf.z)buffSegments [aux].maxZ = yf.z;
					if(buffSegments [aux].minZ > yf.z){buffSegments [aux].minZ = yf.z;Debug.Log("minz3 "+yf.z);}		*/
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
	/*	if(buffSegments [aux].maxZ < xo.z)buffSegments [aux].maxZ = xo.z;
		else if(buffSegments [aux].minZ > xo.z){buffSegments [aux].minZ = xo.z;Debug.Log("minz4 "+xo.z);}		*/
		ang = ((270 - angPla / 2) * Mathf.PI) / (180);
		offset.x = r * Mathf.Cos (ang); // Get X, Z vertex offsets
		offset.z = r * Mathf.Sin (ang);
		yo = new Vector3 (0f, 0f, 0f);
		if (aux == 0) yo = (buffSegments [aux].origen.centre + buffSegments [aux].origen.normal * offset);
		if (!isSalt && !prevSalt && sXo == segmentsXobstacle && segPava != nSegments - 1) {
			sXo = 0;
			if (pTrampa >= Random.Range (0.0f, 1.0f)) {

				if (dificultLvl == 1 && aux > 5) {
					int auxObs = Random.Range (0, simples.Length) % simples.Length;
					buffSegments [aux].ground = Instantiate (simples [auxObs], nextYO, Quaternion.identity) as GameObject;
					buffSegments [aux].ground.transform.LookAt (yf);
					prevSalt = true;
				} else if (dificultLvl == 2 && aux > 1) {
					int auxObs = Random.Range (0, combos1.Length) % combos1.Length;
					buffSegments [aux].ground = Instantiate (combos1 [auxObs], nextYO, Quaternion.identity) as GameObject;
					buffSegments [aux].ground.transform.LookAt (yf);
					prevSalt = true;
				} else {
					int auxObs = Random.Range (0, combos2.Length) % combos2.Length;
					buffSegments [aux].ground = Instantiate (combos2 [auxObs], nextYO, Quaternion.identity) as GameObject;
					buffSegments [aux].ground.transform.LookAt (yf);
					prevSalt = true;
				}
			}
		}
		if (lastSeg) {
			GameObject auxGO = Instantiate (final, nextYO, Quaternion.identity) as GameObject;
			auxGO.transform.LookAt (yf);
			auxGO.GetComponentInChildren<Text> ().text = "LEVEL "+dificultLvl.ToString();
		}
		if (pMony >= Random.Range (0.0f, 1.0f)&& aux > 1) {
			int auxObs = Random.Range (0, mony.Length) % mony.Length;
			buffSegments [aux].money = Instantiate (mony[auxObs],nextYO, Quaternion.identity) as GameObject;
			buffSegments [aux].money.transform.LookAt (yf);
		}
		if (aux != 1) {
			buffSegments [aux].lE = Instantiate (pendentE, nextYO, Quaternion.identity) as GameObject;
			buffSegments [aux].lE.transform.LookAt (yf);
			Vector3 auxS = buffSegments [aux].lE.transform.localScale;
			auxS.z = Vector3.Distance (nextYO, yf);
			buffSegments [aux].lE.transform.localScale = auxS;
			buffSegments [aux].lD = Instantiate (pendentD, nextXO, Quaternion.identity) as GameObject;
			buffSegments [aux].lD.transform.LookAt (xf);
			auxS = buffSegments [aux].lD.transform.localScale;
			auxS.z = Vector3.Distance (nextXO, xf);
			buffSegments [aux].lD.transform.localScale = auxS;
		}



		buffSegments [aux].minZ = Mathf.Min (Mathf.Min (nextYO.z, nextXO.z), Mathf.Min (xf.z, yf.z));
		buffSegments [aux].maxZ = Mathf.Max (Mathf.Max (nextYO.z, nextXO.z), Mathf.Max (xf.z, yf.z));
		buffSegments [aux].minX = Mathf.Min (Mathf.Min (nextYO.x, nextXO.x), Mathf.Min (xf.x, yf.x));
		buffSegments [aux].maxX = Mathf.Max (Mathf.Max (nextYO.x, nextXO.x), Mathf.Max (xf.x, yf.x));

		if (aux == 1) {
			buffSegments[aux].maxX += 10;
			buffSegments[aux].minX -= 10;
			buffSegments[aux].maxZ += 10;
			buffSegments[aux].minZ -= 10;

		}

		nextYO = yf;
		nextXO = xf;


		//CREAR COLLIDERS LATERALS A MANOPLISIMA!!!!!
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
	
	public Quaternion nouse(float z, float x){

		//	Comentar be per reportar bugz

		int i = segPava;
		if (i == 0)	i = 1;
		while (i != nSegments && !(buffSegments[i].maxZ >= z && buffSegments[i].minZ <= z && buffSegments[i].maxX >= x && buffSegments[i].minX <= x)) {
		//	Debug.Log("NOOOOOOPE try seg "+i+" z pava, minim maxim segmen elegit "+z+"  ,  "+ buffSegments[i].minZ+"  ,  "+buffSegments[i].maxZ+"   x pava, minim maxim segmen elegit "+x+"  ,  "+ buffSegments[i].minX+"  ,  "+buffSegments[i].maxX);
			++i;
		}
		if (segPava < i - 2) {
			i = Mathf.Min (nSegments - 1, i);
			Debug.Log("BUG deteccio normal pava seg act "+segPava+" try seg "+i+" z pava, minim maxim segmen elegit "+z+"  ,  "+ buffSegments[segPava].minZ+"  ,  "+buffSegments[segPava].maxZ+"   x pava, minim maxim segmen elegit "+x+"  ,  "+ buffSegments[segPava].minX+"  ,  "+buffSegments[segPava].maxX);
			i = segPava;
		}
		if (i == nSegments) {
			over = true;
		//	Debug.Log("SEGMENT NO ELEGIT, FRAGMENT ACABAAAAAAAAAT");
		}
		else {
			segPava = i;
		//	Debug.Log("segment elegit "+i);
		//	Debug.Log("z pava, minim maxim segmen elegit "+z+"  ,  "+ buffSegments[i].minZ+"  ,  "+buffSegments[i].maxZ+"   x pava, minim maxim segmen elegit "+x+"  ,  "+ buffSegments[i].minX+"  ,  "+buffSegments[i].maxX);
		}
		if (buffSegments [segPava].ground != null) {
			Debug.Log ("porta detectada");
			if (buffSegments [segPava].ground.transform.name == "gate(Clone)") {
				Debug.Log ("porta detectada1");
				if(buffSegments [segPava].ground.GetComponentInChildren<gateQTE> () != null){
					Debug.Log ("porta detectada2");
					if (buffSegments [segPava].ground.GetComponentInChildren<gateQTE> ().enable != true) {
						Debug.Log ("porta detectada3");
						GameObject.FindWithTag ("Player").GetComponent<playerMovements> ().qtStart ();
						buffSegments [segPava].ground.GetComponentInChildren<gateQTE> ().enable = true;
					}
				}
			}
		}
		return buffSegments[segPava].origen.normal;
	}

	public bool isUp(){
		if (buffSegments [segPava].origen.centre.y < buffSegments [segPava].desti.centre.y)
			return true;
		return false;
	}

	public bool isFalling(float y){
		if (y < Mathf.Min(buffSegments [segPava % nSegments].origen.centre.y , buffSegments [segPava % nSegments].desti.centre.y))
			return true;
		else
			return false;	
	}
	
	public Vector3 inici (){
		return buffSegments [segPava].origen.centre + new Vector3 (0,radi+2, 2);
	}

	public Vector3 posIniTroll(){
		if (segPava == 0) return buffSegments [segPava].origen.centre + new Vector3 (0,radi, 2);
		else return buffSegments [segPava - 1].desti.centre + new Vector3 (0,radi, 2);
	}
	
	public bool isOver(){
		return over;
	}

	public Vector3 getPosition(int index){
		if (index == nSegments)
			return Vector3.zero;
		return buffSegments [index].origen.centre + new Vector3 (0,radi + 3,0);
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
	//	Debug.Log("SEGMENT INICIAL PASAT X FUNCIO, minX minZ                "+inici.minX+"    "+inici.minZ);
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
		nextYO = inici.desti.centre;
		nextXO = inici.desti.centre;
		//	gScript = terra.GetComponent<ground >();
		for (int i = 1; i < nSegments; ++i) {
			buffSegments [i] = new segment();
			buffSegments [i].origen = new llesca();
			buffSegments [i].desti = new llesca();
		}
		for (int i = 0; i < nSegments; ++i) {
			if(i == nSegments - 1) lastSeg = true;
			addSegment ();
		}
		SetMesh ();
	//	return finalObsPos;
		//	gScript.SetMesh ();
	//	for(int i = 0; i < nSegments; ++i){
	//		Debug.Log("Segment "+i+"  minX, maxX: "+buffSegments [i].minX+"    ,   "+buffSegments [i].maxX+"   minZ, maxZ: "+buffSegments [i].minZ+"   ,  "+buffSegments [i].maxZ);
	//	}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}