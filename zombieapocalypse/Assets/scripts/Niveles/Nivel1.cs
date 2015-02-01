using UnityEngine;
using System.Collections;

public class Nivel1 : MonoBehaviour {


	// Use this for initialization
	void Start () {

	GameObject obj=	GameObject.Find("Zombie");
		for(int i=0 ;i<-1;i++){

			Vector3 vector = VectoraleatorioXZ();
			vector.y= obj.transform.position.y;
			GameObject aux =	Instantiate(obj,vector ,  Quaternion.identity) as GameObject;
			aux.GetComponent<Zombie>().velocidad = (float)Random.Range(3,5);
			aux.name+=i.ToString();
				}

	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Update Nivel1"+Time.frameCount);
	}

   // 
	private Vector3 VectoraleatorioXZ(){
	
		Vector3 result = new Vector3 ();
		float angulo = Random.Range (0.0f,1.0f);
	
				//sen = co/hip; co = asin(teta)*hip
				//cos = ca/hip; ca = acos(teta)*hip
		float magnitude = Random.Range(20,50);
	
		result.x = Mathf.Acos(angulo)*magnitude;
		result.z =  Mathf.Asin(angulo)*magnitude;

		angulo = Mathf.Rad2Deg * angulo;

		int rango =Random.Range(0,12);
		rango = rango % 4;
		angulo += rango * 80;

		if(angulo > 90 && angulo <270)
			result.x =result.x*-1;


		if(angulo > 180 && angulo <360)
			result.z =result.z*-1;

		return result;
	}




}
