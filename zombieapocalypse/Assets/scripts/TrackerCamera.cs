using UnityEngine;
using System.Collections;

public class TrackerCamera : MonoBehaviour {

	private Transform transFormObjetivo;
	private Player player;
	private Vector3 defdistance = UtilsVector.defdistance;

	private Vector3 referencepoint = UtilsVector.referencepoint;

	private float tolerancereference = 0.3f;
	private float MAX_DISTANCE = 20;
	public 	float velocidad = 1f;
	public  float acelareacion =0.3f;
	const int lamda = -2;
	const int toreferencepointCamera = -1;
	const int ilde = 0;
	const int acelerate = 1;
	const int siguiendo = 2;
	private int estado = 0;

	

	// Use this for initialization
	void Start () {


		GameObject gamobj = GameObject.Find ("personaje2");
		player = 	gamobj.GetComponent<Player> ();
		transFormObjetivo = gamobj.transform;
		estado = lamda;

		transform.rotation = Quaternion.Euler(0,-UtilsVector.AngleDifPovite() , 0);
	}

	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.Euler(0,-UtilsVector.AngleDifPovite() , 0);
	
		// pintando
		this.drawreferencePoint();


		{

		

			Vector3 vectorobjetivo   =  getVectorObjetivo();
			Vector3 MyVector = this.gameObject.transform.position;

			Vector3 aux= vectorobjetivo- MyVector;

		if(aux.z < 0){

				//estado =toreferencepointCamera;
			}



		switch (estado) {
			case lamda:
				 if(!inreferencePoint ()){
					estado =toreferencepointCamera;

				}	
				else {

					estado =ilde ;
					referencepoint = player.referencePoint();
				}

				break;

			case toreferencepointCamera:

				
				vectorobjetivo =  getVectorObjetivo();
				 MyVector = this.transform.position;
				Vector3 direccion = vectorobjetivo - MyVector;

				if(direccion.sqrMagnitude <= tolerancereference){
					this.transform.position = vectorobjetivo;
					estado = ilde;
					referencepoint = player.referencePoint();
				}
				else{
					direccion.Normalize ();
					MyVector = MyVector + direccion ;//* Time.deltaTime;
					this.transform.position = MyVector;
				}


				

				break;

		case ilde:

			if (player.InMovement()) {
				
					direccion = this.referencepoint- this.player.transform.position;
					float magnitud = 	Mathf.Abs((direccion).magnitude);
				
					if(magnitud > MAX_DISTANCE){
						estado = acelerate;
					}

			}

			break;

		case acelerate:


				direccion = vectorobjetivo - MyVector;
			
				if(direccion.sqrMagnitude <= tolerancereference){
					this.transform.position = vectorobjetivo;
					estado = siguiendo;


				}
				else{
					direccion.Normalize ();
					
					MyVector = MyVector + direccion ;//* Time.deltaTime;
					this.transform.position = MyVector;
				}

			break;
		case  siguiendo:

				if (player.InMovement())
				{
					this.transform.position = vectorobjetivo;
				}
				else
				{
					estado = ilde;
					referencepoint = player.referencePoint();
				}
         
			break;
				}

		}

		/*
	float td =	Time.deltaTime;

		Vector3 vectorobjetivo = transFormObjetivo.position;
		Vector3 MyVector = this.transform.position;
		Vector3 direccion = vectorobjetivo - MyVector;
		if (direccion.sqrMagnitude > 100) {
						direccion.Normalize ();
		
						direccion = direccion * velocidad;
						MyVector = MyVector + direccion;
						this.transform.position = MyVector;
				}

*/

	}
	public  void OnGUI () {
		
		//GUI.Label (new Rect (0, 20, Screen.width, Screen.height), estado.ToString());
	}


	private Vector3 getVectorObjetivo(){
	
		Vector3 obj = player.referencePoint () + defdistance ;
		return obj;
	}


	private bool inreferencePoint(){

		Vector3 obj =this.getVectorObjetivo ();
		if(!obj.Equals(Vector3.zero)){
			return (this.transform.position - obj).sqrMagnitude <=tolerancereference;
		}
		else
			return false;

	}


	private void drawreferencePoint(){
		
		Vector3 vec2 = player.referencePoint();
		//Debug.Log (this.transformpla2.position.ToString()+" "+vec2.ToString()+" "+Time.deltaTime);
		if (vec2 != Vector3.zero) {
			Vector3 vec2b =	getVectorObjetivo();
			if (vec2b != Vector3.zero) 
				Debug.DrawLine (vec2, vec2b);
			
		}
		
	}

}
