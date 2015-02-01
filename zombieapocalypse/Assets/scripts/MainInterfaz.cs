using UnityEngine;
using System.Collections;

public class MainInterfaz : InterfazGUI,UIActionEvent {
	const int NORMAL = 0;
	const int DISPLAYMENU = 1;
	const int DISPLAYCREDITOS = 2;


	private int state = NORMAL;

	// Use this for initialization
	public  override void Inicio () {

		//IniciarControlesCreditos ();
		IniciarControlesPrincipales ();
	}
	
	// Update is called once per frame
	void Update () {
	
		switch (state) {
		case DISPLAYMENU:
			this.LimpiarControles();
			IniciarControlesPrincipales();
			break;
		case DISPLAYCREDITOS:
			this.LimpiarControles();
			IniciarControlesCreditos();
			break;

				}
	}

	public void listener(object sender){
		UIControl control = sender as UIControl;
		switch (control.ID) {

			case "btnInicio":
			// si tenia cargado un nivel antes
			string nivel = "Nivel1";
			//
			PlayerPrefs.SetString("Nivel",nivel);
			PlayerPrefs.Save();
			Application.LoadLevel("EscenaCarga");

			break;

			case "btnOpciones":
			// pintar los botones de pintar creditos

			break;

			case "btnCreditos":
			// pintar los botones de pintar creditos
			//vaciar controles y colocar lso controles que son
			this.state = DISPLAYCREDITOS;
			break;

			case "btnOk":
			this.state= DISPLAYMENU;
			break;
		case "Salir":
			Application.Quit();
			break;
		}
	}

	void IniciarControlesPrincipales(){

		
		Rect aux = FactoryRectangle (0.05f, 0.05f, 0.9f, 60);
		AddControl ("btnInicio", new UIButton (){ Rectangulo = aux, Listener=this,Text="Inicio"  });
		
		aux  = new Rect(aux.x,aux.y+ aux.height+ 5,aux.width,aux.height);
		AddControl ("btnOpciones", new UIButton (){ Rectangulo = aux , Listener=this,Text="Opciones"  });
		
		aux  = new Rect(aux.x,aux.y+ aux.height+ 5,aux.width,aux.height);
		AddControl ("btnCreditos", new UIButton (){ Rectangulo = aux , Listener=this,Text="Creditos"  });

		aux  = new Rect(aux.x,aux.y+ aux.height+ 5,aux.width,aux.height);
		AddControl ("Salir", new UIButton (){ Rectangulo = aux , Listener=this,Text="Salir"  });
	}

	void IniciarControlesCreditos(){

		Rect aux =  FactoryRectangle (0.05f, 0.05f, 0.9f, 0.85f);
		AddControl ("textCreditos", new UITextArea (){ Rectangulo = aux,Text="Desarrollado por \n Juan sebastian suares: email@email \n Andres Vargas: email@email" ,ReadOnly= true });

		aux = FactoryRectangle (0.4f, 0.9f, 0.2f, 0.08f);
		AddControl ("btnOk", new UIButton (){ Rectangulo = aux, Listener=this,Text="Inicio"  });

	}
}
