using UnityEngine;
using System.Collections;

public class Menus : MonoBehaviour {
	
	public Transform[] childs;
	
	private GameObject audioSource;
	
	public enum Telas{
		menuInicial,
		extras,
		opcoes,
		gameOver,
		novoJogo,
		personagens
	}
	public Telas telas;
	
	enum OpcoesMenuPrincipal{
		novoJogo,
		continuar,
		opcoes,
		extras,
		sair
	}	
	enum OpcoesMenuExtras{
		personagens,
		universo,
		concept,
		voltar
	}
	enum OpcoesMenuOpcoes{
		ruim,
		regular,
		bom,
		bonito,
		otimo,
		fantastico,
		voltar
	}
	enum OpcoesMenuPersonagens{
		chuck,
		asura,
		yami,
		lucia,
		honoo,
		voltar
	}
	enum OpcoesMenuGameOver{
		menuPrincipal,
		voltar,
		sair
	}
	enum OpcoesMenuNovoJogo{
		sim,
		nao
	}
	private OpcoesMenuPrincipal menus;
	private OpcoesMenuExtras menuExtras;
	private OpcoesMenuOpcoes menuOpcoes;
	private OpcoesMenuGameOver menuGameOver;
	private OpcoesMenuNovoJogo menuNovoJogo;
	private OpcoesMenuPersonagens menuPersonagens;
	
	private float delay = 0.0f;
	
	void Start(){
		audioSource = GameObject.FindGameObjectWithTag("AudioSource");
	}
	
	void Update(){
		
		float test = Input.GetAxisRaw("Vertical");
		
		delay += Time.deltaTime;
		
		bool down = false;
		bool up = false;
		
		if(test < -0.4f && delay > 0.5f){
			delay = 0.0f;
			down = true;
		}else if(test > 0.4f && delay > 0.5f){
			delay = 0.0f;
			up = true;
		}
		
		

		//Este switch é para tela de menu
		switch(telas){
		//Menu inicial
		case Telas.menuInicial:
			switch(menus){
				//Novo jogo
			case OpcoesMenuPrincipal.novoJogo:
				childs[0].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					if(LevelSerializer.SavedGames.Count > 0){
						Application.LoadLevel("NovoJogo");
					}else
						Application.LoadLevel("Intro");
				}
				if(down){
					menus = OpcoesMenuPrincipal.continuar;
					childs[0].renderer.material.color = Color.white;
				}else if(up){
					menus = OpcoesMenuPrincipal.sair;
					childs[0].renderer.material.color = Color.white;
				}
				break;
				//Continuar
			case OpcoesMenuPrincipal.continuar:
				childs[1].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					if(LevelSerializer.SavedGames.Count > 0)
						LevelSerializer.Resume();
					//else
						//Debug.Log("Nenhum save encontrado");
				}
				if(down){
					childs[1].renderer.material.color = Color.white;
					menus = OpcoesMenuPrincipal.opcoes;
				}else if(up){
					menus = OpcoesMenuPrincipal.novoJogo;
					childs[1].renderer.material.color = Color.white;
				}
				break;
				//Opções
			case OpcoesMenuPrincipal.opcoes: 
				childs[2].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					DontDestroyOnLoad(audioSource);
					Application.LoadLevel("Opcoes");
				}
				if(down){
					childs[2].renderer.material.color = Color.white;
					menus = OpcoesMenuPrincipal.extras;
				}else if(up){
					menus = OpcoesMenuPrincipal.continuar;
					childs[2].renderer.material.color = Color.white;
				}
				break;
				//Extras
			case OpcoesMenuPrincipal.extras:
				childs[3].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					DontDestroyOnLoad(audioSource);
					Application.LoadLevel("Extras");
				}	
				if(down){
					childs[3].renderer.material.color = Color.white;
					menus = OpcoesMenuPrincipal.sair;
				}else if(up){
					menus = OpcoesMenuPrincipal.opcoes;
					childs[3].renderer.material.color = Color.white;
				}
				break;
				//Sair
			case OpcoesMenuPrincipal.sair:
				childs[4].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					Application.Quit();
				}
				if(down){
					childs[4].renderer.material.color = Color.white;
					menus = OpcoesMenuPrincipal.novoJogo;
				}else if(up){
					menus = OpcoesMenuPrincipal.extras;
					childs[4].renderer.material.color = Color.white;
				}		
				break;
			}
			break;
			
			//Segunda tela: Extras
			case Telas.extras:
				switch( menuExtras ){
				//Personagens
				case OpcoesMenuExtras.personagens:
					childs[0].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						DontDestroyOnLoad(audioSource);
						Application.LoadLevel("Personagens");
					}
					if(down){
						menuExtras = OpcoesMenuExtras.universo;
						childs[0].renderer.material.color  = Color.white;
					}else if(up){
						menuExtras = OpcoesMenuExtras.voltar;
						childs[0].renderer.material.color  = Color.white;
					}
					break;
				//Universo do game
				case OpcoesMenuExtras.universo:
					childs[1].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						DontDestroyOnLoad(audioSource);
						//TODO: Fazer tela Universo do game
					}
					if(down){
						childs[1].renderer.material.color =  Color.white;
						menuExtras = OpcoesMenuExtras.concept;
					}else if(up){
						menuExtras = OpcoesMenuExtras.personagens;
						childs[1].renderer.material.color =  Color.white;
					}
				break;
				//Concept Art
				case OpcoesMenuExtras.concept: 
					childs[2].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						DontDestroyOnLoad(audioSource);
						//TODO: Fazer tela Concepts art
					}
					if(down){
						childs[2].renderer.material.color = Color.white;
						menuExtras = OpcoesMenuExtras.voltar;
					}else if(up){
						menuExtras = OpcoesMenuExtras.universo;
						childs[2].renderer.material.color = Color.white;	
					}
				break;
				//Voltar
				case OpcoesMenuExtras.voltar:
					childs[3].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						DestroyImmediate(audioSource);
						Application.LoadLevel("MenuInicial");
					}
					if(down){
						childs[3].renderer.material.color = Color.white;
						menuExtras = OpcoesMenuExtras.personagens;
					}else if(up){
						menuExtras = OpcoesMenuExtras.concept;
						childs[3].renderer.material.color = Color.white;
					}		
				break;
				}
			break;
			
			//Tela Opcoes
		case Telas.opcoes:
				switch(menuOpcoes){
			case OpcoesMenuOpcoes.ruim:
					childs[0].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						QualitySettings.SetQualityLevel(0);
					}
					if(down){
						childs[0].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.regular;
					}else if(up){
						childs[0].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.voltar;
					}
				break;
			case OpcoesMenuOpcoes.regular:
					childs[1].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						QualitySettings.SetQualityLevel(1);
					}
					if(down){
						childs[1].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.bom;
					}else if(up){
						childs[1].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.ruim;
					}
				break;
			case OpcoesMenuOpcoes.bom:
					childs[2].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						QualitySettings.SetQualityLevel(2);
					}
					if(down){
						childs[2].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.bonito;
					}else if(up){
						childs[2].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.regular;
					}
				break;
			case OpcoesMenuOpcoes.bonito:
					childs[3].renderer.material.color =  Color.red;
					if(Input.GetButton("Fire4")){
						QualitySettings.SetQualityLevel(3);
					}
					if(down){
						childs[3].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.otimo;
					}else if(up){
						childs[3].renderer.material.color = Color.white;
						menuOpcoes = OpcoesMenuOpcoes.bom;
					}
				break;
			case OpcoesMenuOpcoes.otimo:
				childs[4].renderer.material.color =  Color.red;
				if(Input.GetButton("Fire4")){
					QualitySettings.SetQualityLevel(4);
				}
				if(down){
					childs[4].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.fantastico;
				}else if(up){
					childs[4].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.bonito;
				}
				break;	
			case OpcoesMenuOpcoes.fantastico:
				childs[5].renderer.material.color =  Color.red;
				if(Input.GetButton("Fire4")){
					QualitySettings.SetQualityLevel(5);
				}
				if(down){
					childs[5].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.voltar;
				}else if(up){
					childs[5].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.otimo;
				}
				break;
			case OpcoesMenuOpcoes.voltar:
				childs[6].renderer.material.color =  Color.red;
				if(Input.GetButton("Fire4")){
					Destroy(audioSource);
					Application.LoadLevel("MenuInicial");
				}
				if(down){
					childs[6].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.ruim;
				}else if(up){
					childs[6].renderer.material.color = Color.white;
					menuOpcoes = OpcoesMenuOpcoes.fantastico;
				}
				break;
			}		
			break;
		case Telas.gameOver:
			switch(menuGameOver){
			case OpcoesMenuGameOver.menuPrincipal:
				childs[0].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4"))
					Application.LoadLevel("MenuInicial");
				if(down){
					childs[0].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.voltar;
				}else if(up){
					childs[0].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.sair;
				}
				break;
			case OpcoesMenuGameOver.voltar:
				childs[1].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4"))
					if(LevelSerializer.SavedGames.Count > 0)
						LevelSerializer.Resume();
					else
						Application.LoadLevel("Fase01");
				if(down){
					childs[1].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.sair;
				}else if(up){
					childs[1].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.menuPrincipal;
				}
				break;
			case OpcoesMenuGameOver.sair:
				childs[2].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4"))
					Application.Quit();
				if(down){
					childs[2].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.menuPrincipal;
				}else if(up){
					childs[2].renderer.material.color = Color.white;
					menuGameOver = OpcoesMenuGameOver.voltar;
				}
				break;
			}
			break;
		case Telas.novoJogo:
			switch(menuNovoJogo){
			case OpcoesMenuNovoJogo.sim:
				childs[0].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4")){
					LevelSerializer.SavedGames.Clear();
					LevelSerializer.SaveDataToPlayerPrefs();
					Application.LoadLevel("Intro");
				}if(down){
					childs[0].renderer.material.color = Color.white;
					menuNovoJogo = OpcoesMenuNovoJogo.nao;
				}else if(up){
					childs[0].renderer.material.color = Color.white;
					menuNovoJogo = OpcoesMenuNovoJogo.nao;
				}
				break;
			case OpcoesMenuNovoJogo.nao:
				childs[1].renderer.material.color = Color.red;
				if(Input.GetButton("Fire4"))
					Application.LoadLevel("MenuInicial");
				if(down){
					childs[1].renderer.material.color = Color.white;
					menuNovoJogo = OpcoesMenuNovoJogo.sim;
				}else if(up){
					childs[1].renderer.material.color = Color.white;
					menuNovoJogo = OpcoesMenuNovoJogo.sim;
				}
				break;
			}
			break;
			case Telas.personagens:
				switch(menuPersonagens){
				//Chuck
				case OpcoesMenuPersonagens.chuck:
					childs[0].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Chuck");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.asura;
						childs[0].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.voltar;
						childs[0].renderer.material.color = Color.white;
					}
					break;
					//Asura
				case OpcoesMenuPersonagens.asura:
					childs[1].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Asura");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.yami;
						childs[1].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.chuck;
						childs[1].renderer.material.color = Color.white;
					}
					break;
				case OpcoesMenuPersonagens.yami:
					childs[2].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Yami");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.lucia;
						childs[2].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.asura;
						childs[2].renderer.material.color = Color.white;
					}
					break;
				case OpcoesMenuPersonagens.lucia:
					childs[3].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Lucia");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.honoo;
						childs[3].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.yami;
						childs[3].renderer.material.color = Color.white;
					}
					break;
				case OpcoesMenuPersonagens.honoo:
					childs[4].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Honoo");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.voltar;
						childs[4].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.lucia;
						childs[4].renderer.material.color = Color.white;
					}
					break;
				case OpcoesMenuPersonagens.voltar:
					childs[5].renderer.material.color = Color.red;
					if(Input.GetButton("Fire4")){
						Application.LoadLevel("Extras");
					}
					if(down){
						menuPersonagens = OpcoesMenuPersonagens.chuck;
						childs[5].renderer.material.color = Color.white;
					}else if(up){
						menuPersonagens = OpcoesMenuPersonagens.honoo;
						childs[5].renderer.material.color = Color.white;
					}
					break;
				}
				break;
			}
	}
}
