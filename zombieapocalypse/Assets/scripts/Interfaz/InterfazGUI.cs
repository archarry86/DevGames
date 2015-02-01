using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterfazGUI : MonoBehaviour {

	private Dictionary<string,UIControl> controles= new Dictionary<string,UIControl> ();
	protected bool canGUI= true;
	protected bool isMobile;
	public static List<Vector2> _points = new List<Vector2>();
	protected MyInput MyInput = MyInput.My_Input;
	
	// Use this for initialization
	 void Start() {
		isMobile = false;
		switch (Application.platform) {
			case RuntimePlatform.IPhonePlayer: 
				isMobile = true;
				break;
			case RuntimePlatform.Android :
				isMobile = true;
				break;
		}
		Inicio();

	}
	public virtual void Inicio(){

    }
	// Update is called once per frame
	public void ActualizarTouches () {
	
		if (isMobile) 
			PointsTouch();
		// procesar por cada boton si fue oprimido o no

						foreach (UIControl c in 	controles.Values) {
								VirtualButton2D vb = c as VirtualButton2D;
								if (vb != null)
										vb.Update ();
						}


	}

	
	public void OnGUI () {
		if (canGUI) {
						foreach (UIControl c in 	controles.Values)
								c.OnGUI ();
				}

		if (!isMobile) {
			//tomar todos los valores de posicion del mouse
			PointsMouse();
		}
		
	}


	private void PointsTouch(){


		if (isMobile) {
			_points.Clear();
			int count = Input.touchCount;
			int i = 0;
			if (count > 0) {
				while (i< count) {
					Touch t = Input.GetTouch(i);
					// DADO QUE LA ORINTACION ESTAL LAND SCAPE LEFT
					Vector2 auxp= new Vector2( t.position.x,(Screen.height-t.position.y));
					if (auxp.y > Screen.height)
						auxp.y = Screen.height;
					else if(auxp.y < 0)
						auxp.y = 0;
					
					_points.Add(auxp);
					i++;
				}
			} 
		}
	
	}

	private void PointsMouse(){

		if ((Event.current.type == EventType.MouseDrag) || (Event.current.type == EventType.MouseDown)) {
						_points.Clear ();
						Vector2 vec = Event.current.mousePosition;
						_points.Add (new Vector2 (vec.x, vec.y));
		} else if (Event.current.type == EventType.MouseUp) {
			_points.Clear ();
		}
	}


	public UIControl GetControl(string ID){
		return	controles [ID];
	}
	
	public void AddControl(string ID,UIControl control){
		control.ID = ID;
		controles.Add (ID, control);

	}



	public Rect FactoryRectangle(object x, object y , object w, object h){

		return new Rect ((typeof(float).IsInstanceOfType(x)) ? (float)(Screen.width * 	((float)x)) : ((int)x),
		                 (typeof(float).IsInstanceOfType(y)) ? (float)(Screen.height * 	((float)y)) : ((int)y),
		                 (typeof(float).IsInstanceOfType(w)) ? (float)(Screen.width  *	((float)w)) : ((int)w),
		                 (typeof(float).IsInstanceOfType(h)) ? (float)(Screen.height *	((float)h)) : ((int)h));
	}

	public void LimpiarControles(){
		canGUI= false;
		controles.Clear ();
		canGUI= true;
	}
}




public abstract class UIControl{
	internal string ID { get; set; }
	Rect Rectangulo { get; set;}
	public abstract void  OnGUI();
}

public interface UIActionEvent{
	
	void listener(object sender);
}


public class UIButton:UIControl{

	public string Text { get; set;}
	public UIActionEvent Listener {get;set;}
	
	protected Rect rectangulo;
	public  Rect Rectangulo{ get{ return this.rectangulo; }set{ rectangulo = value; }}
	
	
	public	override void OnGUI(){
		
		bool var = GUI.Button (rectangulo, Text);
		
		if ( var && Listener != null) {
			Listener.listener(this);
		}
	}
}



public class UITextField:UIControl{

	public string TextLabel{ get; set;}
	public string Text { get; set;}
	public bool ReadOnly { get; set;}
	protected Rect rectangulo;
	private  Rect rectangulotext;
	public  Rect Rectangulo { 
		get{ 
			return this.rectangulo; 
		}
		set{ 
			rectangulo = value;
			rectangulotext = value;
			rectangulotext.y+=value.height; 
		}
	}
	
	
	public override	void OnGUI(){
		GUI.Label (rectangulo, TextLabel);
		Text = GUI.TextField (rectangulotext, Text);
	}
}


public class UITextArea:UIControl{
	
	public string TextLabel{ get; set;}
	public string Text { get; set;}

	public bool ReadOnly { get; set;}
	protected Rect rectangulo;
	private  Rect rectangulotext;
	public  Rect Rectangulo { 
		get{ 
			return this.rectangulo; 
		}
		set{ 
			rectangulo = value;
		 
		}
	}
	
	
	public override	void OnGUI(){
		

			if(ReadOnly)
				GUI.TextArea(rectangulo, Text);
			else
				Text = GUI.TextArea(rectangulo, Text);
	
		
	}
}


public class UISlider:UIControl{
	
	protected Rect rectangulo;
	private  Rect rectangulotext;
	
	public Rect Rectangulo { get{ return this.rectangulo; }set{ rectangulo = value;rectangulotext = value; rectangulotext.y+=value.height; }}
	public float Value{ get; set;}
	
	
	
	public	override void OnGUI(){
		
		Value=	GUI.HorizontalSlider (rectangulo, Value, 0.1f, 1.0f);
		PlayerPrefs.SetFloat ("valueslider", Value);
		GUI.Label(rectangulotext, Value.ToString()+" ; "+(float)(Value * 100.0f)+"%");
		
	}
}


public class VirtualButton2D:UIControl{
	
	public float Wmedios = 0.0f;
	public Rect rectangulopos;
	public Rect rectangulo;
	public Texture2D textura;
	public Texture2D textJoyContainer;
	public bool IsJoy = false;
	public float angulo2D;
	public UIActionEvent btnPressed;
	public UIActionEvent btnReleased;
	private bool pressed= false;

	public bool Pressed{
		get{

			return pressed;
		}
	}

	public  Rect Rectangulo{
		get{ 
			return this.rectangulo; 
		}
		set{

			rectangulo = value;
			float val= 0;
			if(rectangulo.width == rectangulo.height){
				val  = rectangulo.width;
			}
			else{
				
				if(rectangulo.width < rectangulo.height){
					val  = rectangulo.width;
				}
				else{
					val  = rectangulo.height;
				}
			}
			Wmedios = val/2.0f;
			float wcuartos=  Wmedios/2.0f;
			rectangulopos = new Rect(rectangulo.x+wcuartos,rectangulo.y+wcuartos,Wmedios ,Wmedios );
		}
	}

	internal void Update(){

			List<Vector2> vector = InterfazGUI._points;
				if (vector.Count > 0) {
						bool val = false;

						for (int i = 0; i< vector.Count && !val; i++) {
								if (IsJoy)
										val = Intersecto(vector[i]);
								else
										val = IntersectoNormal(vector[i]);
						}
						/*
						if (pressed) 
			            {
								if (val && btnPressed != null)
										btnPressed.listener (this);
								else if (!val && btnReleased != null) {
										btnReleased.listener (this);
								}
						} else 
						{
								if (val && btnPressed != null)
										btnPressed.listener (this);
						}*/
						pressed = val;
						if (btnPressed != null)
							btnPressed.listener(this);
					
				} else {
						
						if(pressed){
							//if(btnReleased != null)
							//btnReleased.listener (this);
							//this.ResetPosition();
							pressed = false;
							if (btnPressed != null)
								btnPressed.listener(this);
						}

				}


	}
	
	
	public override  void  OnGUI(){
		
		if (IsJoy) {
			if (textJoyContainer != null) {
				GUI.DrawTexture (rectangulo, textJoyContainer);
			}
			if (textura != null) {
				GUI.DrawTexture (rectangulopos, textura);
			}
			
		} else {

			if (textura != null) {
				GUI.DrawTexture (rectangulo, textura);
			}
			
		}
	}
	
	// la idea es lansar efectivamente 
	public bool IntersectoNormal(Vector2 point){
		bool val = rectangulo.Contains (point);
		return val;
	}	
	// la idea es lansar efectivamente 
	public bool Intersecto(Vector2 point){
		bool val = rectangulo.Contains (point);
		if (val) {
			float radiomitad= rectangulopos.width/2.0f;
			rectangulopos = new Rect(point.x-radiomitad,point.y-radiomitad,rectangulopos.width,rectangulopos.width);
		}
		return val;
	}	
	
	public void ResetPosition(){
		float wcuartos=  Wmedios/2.0f;
		rectangulopos = new Rect(rectangulo.x+wcuartos,rectangulo.y+wcuartos,Wmedios ,Wmedios );
	}
	
	public Vector3 GetDireccionXZ(){
		
		Vector3 result = new Vector3 ();
		
		float wmediospos = (float)(rectangulopos.width / 2.0f);
		float cpocisionx = rectangulopos.x + wmediospos;
		float cpocisionz = rectangulopos.y + wmediospos;
		Vector3 JOY = new Vector3 (cpocisionx,0,cpocisionz);	
		
		float wmediosrec =(float)( rectangulo.width/ 2.0f);
		float crecx = rectangulo.x + wmediosrec;
		float crecz = rectangulo.y + wmediosrec;
		Vector3 centroRectangulo = new Vector3 (crecx,0,crecz);
		
		result =  JOY -  centroRectangulo;
		result.z *= -1;
		
		
		
		this.angulo2D =	((float)(Mathf.Atan2(result.z,result.x)* 180.0/ Mathf.PI))%360;

		if (float.IsNaN( this.angulo2D))
			this.angulo2D = 0;
	
		return result;
	}
}



