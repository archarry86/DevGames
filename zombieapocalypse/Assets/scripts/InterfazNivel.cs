using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class UtilsVector
{
	/// <summary>
	/// vector utilizado para calculos de la camara
	/// </summary>
	public static Vector3 pivote = new Vector3 (0, 0, -1);

	/// <summary>
	/// vector de referencia con respecto al player
	/// </summary>
	public static  Vector3 referencepoint = Vector3.zero;
	
	public static Vector3 defdistance = new Vector3(0.0f, 0.0f, -50.0f);//(35.3f, 0, -35.3f);//(0.0f, 0.0f, -50.0f);//(-50.0f, 0.0f, 0.0f); //(0.0f, 0.0f, -50.0f);(-35.3f, 0, 35.3f);//(35.3f, 0, 35.3f);//(50.0f, 0.0f, 0.0f);//(35.3f, 0, -35.3f);

	public static Vector3 defdistanceunit = new Vector3 (0.7071f, 0, -0.7071f);

	public static float AngleXZ(Vector3 vector){
		return ((float)(Mathf.Atan2(vector.z,vector.x)* 180.0/ Mathf.PI))%360;
	}

	public static float AngleXZDefDistance{
		get{
			return AngleXZ(defdistance);
		}
	}


	public static float AngleXZPovite{
		get{
			return AngleXZ( pivote);
		}
	}


	public static float AngleDifPovite(){
		return AngleXZDefDistance - AngleXZPovite;
	}

	public static float AngleDifPovite(float angleparam){
		return AngleXZDefDistance - angleparam;
	}


}


// delema detectar el anuglo y enviar le mensaje para cambiar la direccon del jugador
public class InterfazNivel  : InterfazGUI,UIActionEvent {

	public static InterfazNivel singletonInterfaz;
	public VirtualButton2D joystyck;
	public VirtualButton2D correr;

	//private bool isMobile = false;

	private Vector3 lastcameraposition;
	private Quaternion qua;
	private TrackerCamera tacker;

	public bool personal= false;
	
	// Use this for initialization
	public override void Inicio () {

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		singletonInterfaz = this;

		Rect aux = FactoryRectangle (0.90f,5, 0.10f, 0.08f);
		AddControl ("btnSalir", new UIButton (){ Rectangulo = aux, Listener=this,Text="Salir"  });

	
		Rect aux2  =FactoryRectangle (0.85f,40, 0.15f, 0.08f);

		aux2.y = aux.y + aux.height;
		aux = aux2;
		AddControl ("btnmodjuego", new UIButton (){ Rectangulo = aux, Listener=this,Text="Cambiar modo"  });
	
		//
		aux  = FactoryRectangle (0.4f,0.9f, 0.1f, 0.08f);
		AddControl ("btnPausa", new UIButton (){ Rectangulo = aux, Listener=this,Text="Pausa"  });

		aux  = new Rect (0,
		                 Screen.height-250,
		                 175,
		                 175);

		AddControl ("btnJoy",joystyck = new VirtualButton2D (){ IsJoy= true ,Rectangulo = aux ,textura = Resources.Load<Texture2D>("img/controls/joy_cuadrado"),textJoyContainer  = Resources.Load<Texture2D>("img/controls/btn_disparar"),btnPressed = this, btnReleased = this });
		aux  = new Rect (Screen.width-175,
		                 Screen.height-250,
		                 175,
		                 175);
		AddControl ("btnCorrer",correr = new VirtualButton2D (){ Rectangulo = aux,textura = Resources.Load<Texture2D>("img/controls/btn_disparar") , textJoyContainer = Resources.Load<Texture2D>("img/controls/joy_cuadrado"), btnPressed = this, btnReleased = this });

      //  aux = FactoryRectangle(0.5f, 0.01f, 0.10f, 0.10f);
	
	}
	static string loger = "";

	//
	private  bool press= false;
	private  bool pressjoy = true;
	private  bool pressWalk = true;


	void Update () {
	

		//try{
		ActualizarTouches();

		//loger = "Touches version 10 ";
		/*
			if (isMobile) {
						touchesloger += "(mobile)"; 
						int count = Input.touchCount;
						touchesloger += " Count " + count.ToString();
			for(int i = 0;i< _points.Count;i++){
				touchesloger += "["+i.ToString();
				touchesloger += ""+_points[i].ToString()+"]-";
			}
	   }
		}catch(UnityException ex){
			touchesloger = "Error "+ex.Message+" "+ex.StackTrace.ToString();
				}
		*/

	}

    private  float vel =10.0f;


	public void listener(object sender){
		UIControl control = sender as UIControl;

		switch (control.ID) {

			case "btnSalir":
				string nivel = "MenuInicial";
				PlayerPrefs.SetString("Nivel",nivel);
				PlayerPrefs.Save();
				Application.LoadLevel("EscenaCarga");
			break;
			case "btnPausa":
			
			break;

			case "btnmodjuego":

				personal = !personal;

				if(personal){
					//lastcameraposition = this.transform.position;
					qua =this.transform.rotation;
					this.tacker = this.GetComponent<TrackerCamera>();
					DestroyObject(this.GetComponent<TrackerCamera>());
					this.gameObject.AddComponent<PersonalTracker>();
					this.correr.IsJoy= true ;
				}else{

					this.transform.position = this.transform.position+ UtilsVector.defdistance;
					this.transform.rotation = qua;
					this.gameObject.AddComponent<TrackerCamera>();
					DestroyObject(this.GetComponent<PersonalTracker>());
					this.correr.IsJoy= false;
				}

			break;
			case "btnCorrer":
				//
			// Debug.Log("EL BOTON  "+control.ID+ "");
				if(correr.Pressed){
				//Debug.Log("btnCorrer PRESSED "+correr.Pressed +" "+Time.time);
				MyInput.SetAxis("Vertical",(MyInput.GetAxis("Vertical")+0.01F));
				}
				else{
				//Debug.Log("btnCorrer NO PRESSED "+correr.Pressed +" "+Time.time);
				MyInput.SetAxis("Vertical",0);
				}
				//Debug.Log("btnCorrer R"+MyInput.GetAxis("Vertical")	);
			break;
			case "btnJoy":
			
			
			
			break;
		}
	}





	public  void OnGUI () {
		base.OnGUI();
		GUI.Label(new Rect(0,0,Screen.width,Screen.height), loger);

	}

	public static Vector3 GetDireccionJoyStick(){

		Vector3 result = Vector3.zero;
		//if (singletonInterfaz.correr.Pressed )
		{

			loger ="";
						if (UtilsVector.AngleDifPovite () == 0.0f){

								if (!singletonInterfaz.personal){
									result = singletonInterfaz.joystyck.GetDireccionXZ ();
									result.Normalize ();
								}else{
									//algoritmo 1
									singletonInterfaz.correr.GetDireccionXZ();
									singletonInterfaz.joystyck.GetDireccionXZ ();
									
									float angulo2Drun = singletonInterfaz.correr.angulo2D;
									if(angulo2Drun< 0)
										angulo2Drun += 360;	

									float angulo2Djoy = singletonInterfaz.joystyck.angulo2D;
									angulo2Djoy	-= 180;
				
									float pivote = angulo2Djoy-UtilsVector.AngleXZ(UtilsVector.pivote);

									

									float angulo2D= angulo2Drun+pivote;
									if (singletonInterfaz.correr.Pressed )
									{
										result.x = (Mathf.Cos (angulo2D * Mathf.PI / 180));
										int auxvv = 1;
										if (angulo2D < 0 || angulo2D > 180)
											auxvv = -1;
										
										result.z = Mathf.Sqrt (Mathf.Abs (Mathf.Pow (result.x, 2) - 1.0f)) * auxvv;
										result.y= 0;
										
									}

								}
								
						}else {
								//algoritmo 1
								singletonInterfaz.correr.GetDireccionXZ();
								singletonInterfaz.joystyck.GetDireccionXZ ();

								float angulo2D = singletonInterfaz.joystyck.angulo2D;
								float angulo2Dr = singletonInterfaz.correr.angulo2D;
			
								if(angulo2D < 0)
									angulo2D+=360;

								if(angulo2Dr < 0)
									angulo2Dr+=360;
				//loger+= angulo2D.ToString()+";";
				//loger+= angulo2Dr.ToString()+";";
								float dif = (  angulo2Dr-angulo2D);
				//loger+=  dif.ToString()+";";


								angulo2D+= dif;

								if(angulo2D >= 180 && angulo2D <= 360 )
									angulo2D-=360;

				//loger+= angulo2D.ToString()+";";
								angulo2D += UtilsVector.AngleDifPovite();
								
								if(angulo2D >= 180 && angulo2D <= 360 )
									angulo2D-=360;

				//loger+= angulo2D.ToString()+";";
							
								if (singletonInterfaz.correr.Pressed )
								{
								result.x = (Mathf.Cos (angulo2D * Mathf.PI / 180));
								int auxvv = 1;
								if (angulo2D < 0 || angulo2D > 180)
										auxvv = -1;
			
								result.z = Mathf.Sqrt (Mathf.Abs (Mathf.Pow (result.x, 2) - 1.0f)) * auxvv;
								result.y= 0;

								}
						}
		}
		return result;
	}
    public static float GetangleJoyStick()
    {
		float angulo2D = 0;
		Vector2 result = singletonInterfaz.joystyck.GetDireccionXZ ();
	
		
		//algoritmo para incluir la orientacion del a camara
		if (UtilsVector.AngleDifPovite() == 0.0f)
			angulo2D = singletonInterfaz.joystyck.angulo2D;	
		else
		{
		
			 angulo2D = singletonInterfaz.joystyck.angulo2D;
			
			//if(angulo2D < 0)
		//angulo2D+=360;

			angulo2D += UtilsVector.AngleDifPovite();
		
		
		}


		return angulo2D;

    }
	

}


public class MyInput{

	// estaticos
	private static  MyInput _myInput = new MyInput();

	public static MyInput My_Input{
		get{
			return _myInput;
		}
	}
	//

	private Hashtable htable ;

	private MyInput(){
	
		switch (Application.platform) {
		case RuntimePlatform.IPhonePlayer: 
			htable = new Hashtable();
			break;
		case RuntimePlatform.Android :
			htable = new Hashtable();
			break;
		default:
			//htable = new Hashtable();
			break;
			
		}

	}

	public float GetAxis(string axisName){
		float val = 0.0f;
			if (IsMyInput()) {

						if(!htable.Contains(axisName)){
							htable[axisName] = val;
						}
						else{
							val= (float)htable[axisName];
						}
			} else {

			val = Input.GetAxis(axisName);

			}
		return val;
	
	}


	public void SetAxis(string axisName,float val){
		if (IsMyInput()) 
		{

						if (!htable.Contains(axisName)) {
							htable.Add (axisName, 0.0f);
						}
						htable [axisName] = val;
		}
	}

	public bool IsMyInput(){
		return 	htable != null;
	}
}

