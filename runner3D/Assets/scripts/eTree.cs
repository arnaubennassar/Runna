using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class eTree : MonoBehaviour {

	public GameObject copa;
	private GameObject[] leafs;
	private class branca{
		public float texCoordV;
		public float lenght;
		public float rad;
		public float[] ringShape;
		public Quaternion quaternion;
		public Vector3 pos;
		public int nsons;
		public branca[] sons; 
		public int level;
		public bool leaf;
	}


	//posar fulles lleugerament endavant
	private branca dad;
	//Random Factors
	private int NumberOfSides;
	private float maxRad;
	private float minRad;
	private float maxLen;
	private float minLen;
	private int maxSon;
	private float ratio;	//nombre de vegades q creix mes rapid en y vs x
	private float rota;
	private float regular; //factor q fa q sigui mes o menys rodo un arbre, 0 = rodo
	private Vector3 obs;
	private float last;
	private int fill;
	private int maxfill;


	//control
	private float speed;
	private float sensi;
	public bool grown;
	private bool fgrown;

	List<Vector3> vertexList; // Vertex list
	List<Vector2> uvList; // UV list
	List<int> triangleList; // Triangle list


	[HideInInspector, System.NonSerialized]
	public MeshRenderer Renderer;
	MeshFilter filter;

	void OnEnable()
	{
		gameObject.isStatic = false;
		filter = gameObject.GetComponent<MeshFilter>();
		if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
		Renderer = gameObject.GetComponent<MeshRenderer>();
		if (Renderer == null) Renderer = gameObject.AddComponent<MeshRenderer>();

		//Random Values
		maxRad = Random.Range(12f, 20f);
		minRad = Random.Range(0.1f, 0.7f);
		maxLen = 20f;
		minLen = 3f;
		maxSon = 3;
		rota = 20f;
		regular = Random.Range(0f, 0.4f);
		ratio = Random.Range(1f, 7f);
		last = 0f;
		NumberOfSides = (int)Random.Range (8, 16);
		fill = 0;
		maxfill = (int)Random.Range(10f, 1000f);;
		//Control values
		speed = 1000f;
		sensi = 0.1f;
		grown = false;
		fgrown = true;
	}
	
	public void init(Vector3 posi){
		dad = new branca ();
		dad.rad = minRad;
		dad.lenght = minLen;
		dad.ringShape = new float[NumberOfSides + 1];
		dad.quaternion = new Quaternion ();
		dad.pos = posi;
		dad.texCoordV = 0f;
		for (var n = 0; n < NumberOfSides; n++) 
		{
			dad.ringShape[n] = Random.Range(1-regular, 1+regular);
		}
		dad.ringShape[NumberOfSides] = dad.ringShape[0];
		dad.nsons = 0;
		dad.level = 1;
		dad.leaf = false;
	}
	public void grow(float value, bool beat){
		if (dad.rad >= maxRad) {
			grown = true;
			if(fgrown){
				initLeaf(dad);
				leafs = GameObject.FindGameObjectsWithTag ("leaf");
				fgrown = false;
			}
			leafGrow(value);
		}
		else {
			if(value  * sensi > last){
				if (vertexList == null) // Create lists for holding generated vertices
				{
					vertexList = new List<Vector3>();
					uvList = new List<Vector2>();
					triangleList = new List<int>();
				}
				else // Clear lists for holding generated vertices
				{
					vertexList.Clear();
					uvList.Clear();
					triangleList.Clear();
				}
				dad.texCoordV = 0f;
				recGrow(dad, value, -1, beat);
				SetTreeMesh();
			}
			last = value;
		}
	}

	private void recGrow(branca node, float value, int lastRingVertexIndex, bool beat){

		//Carguem els vertex de l'anell del node
		var offset = Vector3.zero;
		var texCoord = new Vector2(0f, node.texCoordV);
		var textureStepU = 1f / NumberOfSides;
		var angInc = 2f * Mathf.PI * textureStepU;
		var ang = 0f;

		node.rad += Mathf.Min(((value*speed))/node.level, 0.2f);  //creixer (rad)
		node.lenght += Mathf.Min(0.2f,((value * speed*ratio))/node.level); //creixer (lenght)

		for (var n = 0; n <= NumberOfSides; n++, ang += angInc) //afegir vertex anell node
		{
			var r = node.ringShape[n] * node.rad;
			offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
			offset.z = r * Mathf.Sin(ang);
			vertexList.Add(node.pos + node.quaternion * offset); // Add Vertex position
			uvList.Add(texCoord); // Add UV coord
			texCoord.x += textureStepU;
		}


		//Conectem vertex amb nodes anteriors, si no era el primer node
		if (lastRingVertexIndex >= 0) { // After first base ring is added ...
			// Create new branch segment quads, between last two vertex rings
			for (var currentRingVertexIndex = vertexList.Count - NumberOfSides - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) {
				triangleList.Add (lastRingVertexIndex + 1); // Triangle A
				triangleList.Add (lastRingVertexIndex);
				triangleList.Add (currentRingVertexIndex);
				triangleList.Add (currentRingVertexIndex); // Triangle B
				triangleList.Add (currentRingVertexIndex + 1);
				triangleList.Add (lastRingVertexIndex + 1);
			}
		} 



		//si la ultima branca no te fills, per la seguent crida (no recursiva)
		if(node.nsons == 0 ){ 
			//Acabar en punta
			// Create a cap for ending the branch
			vertexList.Add(node.pos + node.quaternion * new Vector3(0f,node.lenght,0f)); // Add central vertex
			uvList.Add(texCoord + Vector2.one); // Twist UVs to get rings effect
			for (var n = vertexList.Count - NumberOfSides - 2; n < vertexList.Count - 2; n++) // Add cap
			{
				triangleList.Add(n);
				triangleList.Add(vertexList.Count - 1);
				triangleList.Add(n + 1);
			}
			if(/*fill < maxfill*/beat){ //treure n branquetes
				node.nsons = (int) Random.Range(0, maxSon);
				node.sons = new branca[node.nsons];  ///set random sons
				for(int i = 0; i < node.nsons; ++i){
					node.sons[i] = initSon(node);
					++fill;
				}
			}
			else node.leaf = true;
		}

		else {	//crides recursives als fills del node
			node.leaf = false;
			node.texCoordV += 0.0625f * (node.lenght + node.lenght / node.rad);
			lastRingVertexIndex = vertexList.Count - NumberOfSides - 1;
			for (int i = 0; i<node.nsons; ++i) {
				node.sons [i].texCoordV = node.texCoordV;
				node.sons[i].pos = node.pos + node.quaternion * new Vector3(0f, node.lenght, 0f);
				recGrow (node.sons [i], value, lastRingVertexIndex, beat);
			}
		}
	}
	private void initLeaf(branca node){
		if (node.leaf) {
			Instantiate(copa, node.pos + node.quaternion * new Vector3(0f,node.lenght,0f), Quaternion.identity);
		} else {
			for(int i = 0; i<node.nsons; ++i){
				initLeaf (node.sons[i]);
			}
		}
	}

	private void leafGrow(float value){
		value = Mathf.Min (value * 10 * speed, 8 * maxRad);
		for(int i = 0; i < leafs.Length; ++i){
			leafs[i].transform.localScale = new Vector3(value, value, value);
		}
	}

	private branca initSon(branca dadd){
		branca aux = new branca ();
		aux.rad = minRad;
		aux.lenght = minLen;
		aux.ringShape = new float[NumberOfSides + 1];
		Quaternion qaux = dadd.quaternion;
		qaux.eulerAngles += new Vector3 (Random.Range(-rota,rota), 0, Random.Range(-rota,rota));
		aux.quaternion = qaux;
		aux.texCoordV = 0f;
		for (var n = 0; n < NumberOfSides; n++) 
		{
			aux.ringShape[n] = Random.Range(1-regular, 1+regular);
		}
		aux.ringShape[NumberOfSides] = aux.ringShape[0];
		aux.nsons = 0;
		aux.level = dadd.level + 1;
		aux.leaf = false;
		return aux;
	}

	private void SetTreeMesh()
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
}
