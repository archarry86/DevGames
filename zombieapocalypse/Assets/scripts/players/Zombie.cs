using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {


	public Transform transFormObjetivo;

	public float velocidad = 2.0f;
	// Use this for initialization
	void Start () {
		transFormObjetivo = GameObject.Find ("personaje").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (transFormObjetivo != null) {
						Vector3 vectorobjetivo = transFormObjetivo.position;
						Vector3 MyVector = this.transform.position;
						Vector3 direccionaux = vectorobjetivo - MyVector;

						vectorobjetivo.y= 0.0f;
						MyVector.y= 0.0f;
						Vector3 direccion = vectorobjetivo - MyVector;

						if ( Mathf.Abs( direccion.magnitude) > 5) {
						direccionaux.Normalize ();
			
						direccionaux = direccionaux * velocidad * Time.deltaTime;
						
						this.transform.position +=  direccionaux;
						}
				}
	}
}
