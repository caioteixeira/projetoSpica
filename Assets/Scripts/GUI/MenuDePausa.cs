using UnityEngine;
using System.Collections;

public class MenuDePausa : MonoBehaviour {
	
	public GUISkin gui;  
	
	private bool pause = false;	
	private bool showQualilySetings = false;

	void Start(){
		pause = false;
		Time.timeScale = 1;
		AudioListener.volume = 1;
		Screen.showCursor = false;
	}
	void Update(){

		if(Input.GetButtonDown("Escape")){
	
		//Checa se o jogo já está pausado	
		if(pause == true){ 
			//"despausa" o jogo
			pause = false;
			Time.timeScale = 1;
			AudioListener.volume = 1;
			Screen.showCursor = false;			
		}
		
		//se jogo não está pausado, então pausa o jogo
		else if(pause == false){
			pause = true;
			AudioListener.volume = 0;
			Time.timeScale = 0;
			Screen.showCursor = true;
		}
	}
}
	void OnGUI(){

		GUI.skin = gui;
		GUI.skin.box.fontSize = 28;

		if(pause == true){
		
			//Faz uma caixa de fundo
			GUI.Box(new Rect(Screen.width /2 - 100,Screen.height /2 - 100,250,100), "Pausado");
		
			//Menu principal
			if(GUI.Button(new Rect(Screen.width /2 - 100,Screen.height /2 - 50,250,50), "Menu principal")){
				Application.LoadLevel("MenuInicial");
			}
			if (GUI.Button (new Rect (Screen.width /2 - 100,Screen.height /2,250,50), "Opcoes"))
			{
				if(showQualilySetings){
					showQualilySetings = false;
				}else
					showQualilySetings = true;
			}
			if(showQualilySetings == true){
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 ,250,50), "Fastest")){
					QualitySettings.SetQualityLevel(0);
				}
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 + 50,250,50), "Fast")){
					QualitySettings.SetQualityLevel(1);
				}
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 + 100,250,50), "Simple")){
					QualitySettings.SetQualityLevel(2);
				}
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 + 150,250,50), "Good")){
					QualitySettings.SetQualityLevel(3);
				}
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 + 200,250,50), "Beautiful")){
					QualitySettings.SetQualityLevel(4);
				}
				if(GUI.Button(new Rect(Screen.width /2 + 150,Screen.height /2 + 250,250,50), "Fantastic")){
					QualitySettings.SetQualityLevel(5);
				}
			
				if(Input.GetButtonDown("Escape")){
					showQualilySetings = false;
				}
			}
			if (GUI.Button (new Rect (Screen.width /2 - 100,Screen.height /2 + 50,250,50), "Sair do jogo")){
				Application.Quit();
			}
				
		}		
	}
}
