using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	public float velocidad = 1;
	private float velrot= 1;
	private Vector3 lastposition;

	public Vector3 velocity = Vector3.zero;
	public float CORETNESSANLGE = 0.0f;
	public float alcance = 4.0f;
	public string correcion = "";

	static int ildeState = Animator.StringToHash("Base.ilde");  
	static int INITRUNState = Animator.StringToHash("Base.INITRUN");  




	Animator anim;

	// Use this for initialization
	void Start () {
	//	Debug.Log (" " + this.gameObject.name);
		anim = this.gameObject.GetComponent<Animator>();


	
		/*Debug.Log (" rotation "+transformpla.rotation.x.ToString()+" "+
		           transformpla.rotation.y.ToString()+" "+
		           transformpla.rotation.z.ToString()+" ");
		*/

		//rotacion alrrededor
		//Quaternion q = transformpla.rotation;
		//q.y = 90.0f;
		//transformpla.RotateAround = Quaternion.Euler(0,90.0f,0);
		//transformpla.LookAt ();
	

	
	}
	
	// Update is called once per frame
	void Update () {

		AnimatorStateInfo currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		
		correcion = "";
		if (currentBaseState.nameHash != ildeState) {
			CORETNESSANLGE = -270;
		} else {
			CORETNESSANLGE = -90;
		}

		if (InterfazNivel.singletonInterfaz.personal) {
			Vector3 posicion = this.transform.position;

			Vector3 dir =InterfazNivel.GetDireccionJoyStick();
			posicion+=(dir*0.5f);
			this.transform.position =  posicion;

		}
	//else
		{

				if (lastposition != null) {
						velocity = this.transform.position - lastposition;
		
				} 
				lastposition = this.transform.position;

				//determinar si se oprimio boton para correr
				float v = MyInput.My_Input.GetAxis ("Vertical");
				anim.SetFloat ("v1", v);
	
				bool run = v > 0;
				anim.SetBool ("run", run);

				
	

				///
				//rotacion alrrededor
				//transformpla.rotation = Quaternion.Euler(0, velrot, 0);
				//transformCubo.rotation = Quaternion.Euler(0,velrot,0);
				//Debug.Log (" rotation "+transformpla.rotation.x.ToString()+" "+
				//           transformpla.rotation.y.ToString()+" "+
				//           transformpla.rotation.z.ToString()+" ");
				//velrot++;

				//rotacion
				float angulo = InterfazNivel.GetangleJoyStick ();
				//Debug.Log(angulo);
				angulo += CORETNESSANLGE;
				//Debug.Log (angulo+"angulo");
				this.transform.rotation = Quaternion.Euler (0, -angulo, 0);

	
				//

				//direccion de movimiento  se comentareo todo por anin controller
				//Debug.Log (InterfazNivel.GetDireccionJoyStick ());
				//Debug.Log (InterfazNivel.GetDireccionJoyStick ().ToString()+"direccion ");
				//Debug.Log (InterfazNivel.GetDireccionJoyStick().ToString()+"dir joy");
				//Vector3 dirjoy = InterfazNivel.GetDireccionJoyStick ();

				//Vector3 direccion = (InterfazNivel.GetDireccionJoyStick () * velocidad)*Time.deltaTime;
				//Debug.Log (direccion+"direccion suerte");
				//direccion.y = 0;
				// this.transformpla2.position +=	direccion;

				//rigidbody.AddForce(dirjoy *100);
			
				//

				
				//habilitar o deshabilitar rootmotion anin controller gravedad
	
				bool ensuelo = anim.applyRootMotion;
				bool onfloor = false;
				bool flag = false;
				RaycastHit[] hits = Physics.RaycastAll (this.transform.position, -Vector3.up, 5);
				for (int i=0; i< hits.Length&& ! flag; i++) {
						RaycastHit r = hits [i];
						if (r.transform.gameObject.name.Equals ("Plane")) {
								flag = true;
								onfloor = r.distance <= 1f;
			
						}
				}
			      anim.applyRootMotion = onfloor; //&& !InterfazNivel.singletonInterfaz.personal;

				if (this.transform.position.y < 0.1f && onfloor) {
						Vector3 pos = this.transform.position;
			
						pos.y = 0.1f;
			
						this.transform.position = pos;
				}



	
				//


				//corretness fuera de platadorma
				if (this.transform.position.y < -20 && !anim.applyRootMotion) {
						Vector3 pos = this.transform.position;
						pos.x = 0;
						pos.y *= -1;
						pos.z = 0;
						this.transform.position = pos;
				}





	

				// dibujar punto de referencia
				//this.drawreferencePoint ();
	
			


				//	rotacion normal mirando A UN PUNTO
	
				// nuevo rayo desde la mitad del cuerpo
				flag = false;
				RaycastHit hitInfo;
				flag = Physics.Raycast (this.centerPoint (), -Vector3.up, out hitInfo, 5);

				if (flag && hitInfo.distance < 5) {

						if (hitInfo.transform.gameObject.tag.Equals ("flor")) {
						
						
								Vector3 myPos = this.transform.position;
			
								if (hitInfo.distance > 0) {
										float auxcorrecion = Mathf.Abs (hitInfo.distance - 5);
										//correcion ="distance "+hitInfo.distance;
										//correcion+=";  myPos"+myPos.ToString();
										myPos.y += auxcorrecion;
										//correcion+="; Nueva posicion"+myPos.ToString();
										this.transform.position = myPos;

								}
						}
				}
	

		}

			this.drawCenterPoint ();
				
	}


	public void OnTriggerEnter (  Collider other) {
		/*GameObject obj= other.gameObject;
		if (obj.name.Equals ("Plane")) {
			anim.applyRootMotion = true;
		

			}*/
	}

	public bool debuger =false;
		
	public  void OnGUI () {
		if(debuger)
	//	GUI.Label(new Rect(0, 0, Screen.width, 200),"");
			GUI.Label(new Rect(0, 40, Screen.width, Screen.height)," comprobacion "+ correcion);
	}


	public bool InMovement(){

		AnimatorStateInfo currentBaseState=	anim.GetCurrentAnimatorStateInfo(0);
		return currentBaseState.nameHash != ildeState && currentBaseState.nameHash != INITRUNState;
		
	}

	public Vector3 referencePoint(){
		Vector3 result = Vector3.zero;
		CapsuleCollider cp =	this.gameObject.collider as CapsuleCollider;// .bounds.size.y);
		if (cp != null) {


			Vector3 vec2 = 	this.transform.position;
			vec2.y = this.transform.position.y+((cp.height*2)*0.6f);
	
			result = vec2;
		}
		return 	result;
	}

	private Vector3 centerPoint(){
		Vector3 result = Vector3.zero;
		CapsuleCollider cp =	this.gameObject.collider as CapsuleCollider;// .bounds.size.y);
		if (cp != null) {

			Vector3 vec2 = 	this.transform.position;
			vec2.y = this.transform.position.y+((cp.height));
			
			result = vec2;
		}
		return 	result;
	}

	private void drawreferencePoint(){
		
		Vector3 vec2 = referencePoint ();
		if(vec2!= Vector3.zero)
			Debug.DrawLine(this.transform.position, vec2);
	}

	private void drawCenterPoint(){
		
		Vector3 vec2 = centerPoint ();
	
		if(vec2!= Vector3.zero)
			Debug.DrawLine(this.transform.position, vec2);
	}


}
