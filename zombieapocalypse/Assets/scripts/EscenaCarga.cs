using UnityEngine;
using System.Collections;

public class EscenaCarga : MonoBehaviour {

	public Texture2D textura;
	private int contador =0;
	private int maximo = 3;
	private string[] array = {".","..","..."};
	public string Nivel="";
	private float tiempoLoad;
	private float tiempoaux;
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToLandscapeLeft = true;
		Nivel = PlayerPrefs.GetString ("Nivel");
		tiempoaux = Time.time;
		string img = "img/imagencarga_" + Nivel ;
		textura = Resources.Load<Texture2D>(img) ;
		loadLevel();
		/*
		Debug.Log(img);
		Debug.Log(Resources.Load(img));
		Debug.Log(Resources.Load(img+ ".jpg"));
		textura = Resources.Load<Texture2D>(img) ;
		Debug.Log(textura);
		*/
		//loadLevel();

	}
	
	// Update is called once per frame
	/*void Update () {
	

		bool val = false;


		if ((Time.time - tiempoaux) > 15) {
			val = true;
			tiempoaux= Time.time;
		}
		


		if (val) {
			//loadLevel();
		}


	}
*/

	private void loadLevel(){

		Application.LoadLevel(Nivel);
	/*
		switch(Nivel){
		case "Nivel1":
			Application.LoadLevel(Nivel);
			break;
		case "Nivel2":
			Application.LoadLevel(Nivel);
			break;
			
		}
		*/
	}
	
	
	void OnGUI(){

		if (tiempoLoad == 0 || (Time.time -tiempoLoad) > 1) {
				contador = (contador + 1) % maximo;
				tiempoLoad = Time.time;
		}

		if (textura != null)
			GUI.DrawTexture (new Rect (0, 0,Screen.width,Screen.height), textura);

		GUI.Label (new Rect (0, 100, 100, 30), "Cargando "+array[contador]);
	}
	void OnDestroy() {
		Debug.Log ("Destroy");
		textura = null;
	}
}
