using UnityEngine;
using System.Collections;

public class FimDemo : MonoBehaviour {

	public string nomeMenuPricipal;
	public GUISkin guiSkin;  		

	void OnGUI(){

		GUI.skin = guiSkin;
		//Faz uma caixa de fundo
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "Obrigado!");
		//Menu principal
		if(GUI.Button(new Rect(Screen.width /2 - 100,Screen.height /2 - 50,250,50), "Menu principal")){
				Application.LoadLevel(nomeMenuPricipal);
		}
		if(GUI.Button(new Rect(Screen.width /2 - 100,Screen.height /2 ,250,50), "Tentar outra vez?"))
	    	Application.LoadLevel("Fase01");		
		//Quit game
		if (GUI.Button (new Rect (Screen.width /2 - 100,Screen.height /2 + 50,250,50), "Sair do jogo"))
			Application.Quit();
	}
}
