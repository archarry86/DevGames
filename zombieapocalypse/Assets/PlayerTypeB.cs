using UnityEngine;
using System.Collections;

public class PlayerTypeB  : MonoBehaviour {
	Transform transformpla;
	 
	Animator anim;
	public float velocidad = 1.3f;
	public float CORETNESSANLGE = 0.0f;
	

	// Use this for initialization
	void Start () {
		transformpla = GameObject.Find ("personaje3").transform;
		anim = GameObject.Find ("personaje3").GetComponent<Animator>();
		
	}
	public float angulo ;
	// Update is called once per frame
	void Update () {
		

		

	
		
		//ROTAR MUÑECO
		angulo =InterfazNivel.GetangleJoyStick();
		angulo += CORETNESSANLGE;
		transformpla.rotation = Quaternion.Euler(0,-angulo , 0);
	
		//MOVER
		Vector3 direccion = (InterfazNivel.GetDireccionJoyStick () * velocidad)*Time.deltaTime;
		direccion.y = 0;
		this.transformpla.position += (direccion);//* Time.deltaTime);
		

		//determinar si se oprimio boton para correr
		float v =  Input.GetAxis("Vertical");
		anim.SetFloat("v1",v);
		
		bool run = direccion != Vector3.zero;
		
		anim.SetBool ("run", run);

		
		//DETECTAR GRAVEDAD
		bool onfloor = false;
		bool flag = false;
		RaycastHit[] hits = Physics.RaycastAll (transformpla.position, -Vector3.up, 5);
		for (int i=0; i< hits.Length&& ! flag; i++) {
			RaycastHit r = hits [i];
			if (r.transform.gameObject.name.Equals ("Plane")) {
				flag = true;
				onfloor =  r.distance <= 1f;
				
			}
		}


		anim.applyRootMotion = onfloor;

	}
	
	

	
	
	public  void OnGUI () {
		
		GUI.Label(new Rect(0, 40, Screen.width, 30),angulo.ToString());
	}
	
	
	public bool InMovement(){
		

		return false;
		
	}
	
	public Vector3 referencePoint(){
		Vector3 result = Vector3.zero;
		CapsuleCollider cp =	this.gameObject.collider as CapsuleCollider;// .bounds.size.y);
		if (cp != null) {
			
			
			Vector3 vec2 = 	this.transformpla.position;
			vec2.y = this.transformpla.position.y+((cp.height*2)*0.6f);
			
			result = vec2;
		}
		return 	result;
	}
	
	private void drawreferencePoint(){
		
		Vector3 vec2 = referencePoint();
		//Debug.Log (this.transformpla2.position.ToString()+" "+vec2.ToString()+" "+Time.deltaTime);
		if(vec2!= Vector3.zero)
			Debug.DrawLine(this.transformpla.position, vec2);
		
	}
	
	
}
