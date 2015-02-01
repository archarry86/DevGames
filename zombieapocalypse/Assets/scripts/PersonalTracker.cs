using UnityEngine;
using System.Collections;

public class PersonalTracker : MonoBehaviour {
	public Transform transFormObjetivo;
	// Use this for initialization
	void Start () {
		transFormObjetivo = GameObject.Find ("personaje2").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
		
		float angulo =InterfazNivel.GetangleJoyStick();
	
		
		Vector3 vm = new Vector3 ();
		vm.x= (Mathf.Cos(angulo*Mathf.PI / 180));
		int auxvv = 1;
		if(angulo< 0 || angulo > 180)
			auxvv = -1;
		vm.z= Mathf.Sqrt( Mathf.Abs(Mathf.Pow(vm.x,2)-1.0f))*auxvv;
		vm.y = 1;
		Vector3 VEC = transFormObjetivo.position;
		VEC += (vm*4);
		this.transform.position = VEC;


		angulo += -90;
		this.transform.rotation = Quaternion.Euler(0,-angulo , 0);
		//drawCenterPoint ();
	}

	
	private void drawCenterPoint(){
		
	
		
		Vector3 VEC = new Vector3 (50, 0, 50);//Vector3.forward;//new Vector3 (10, 10, 10);
		Debug.DrawLine(this.transform.position, VEC, Color.red);
	}
}
