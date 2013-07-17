using UnityEngine;
using System.Collections;
using System;

public class PlayerStatusBar : MonoBehaviour {
	
	public GameObject player;
	
	public GameObject EEGui;
	public GameObject HPGui;
	
	GUITexture eeGUIText;
	GUITexture hpGUIText;
	
	private Player jogador; //Componente Player
	float hpAtual; //HP Atual
	float hpMax; //HP Maximo
	float eeAtual; //ee Atual
	float eeMax; //ee Maximo
	
	
	public bool exibeHp; //Exibir HP?
	public bool exibeEe; //Exibir EE?
	//Nota: Desligar essas bools, desliga completamente os métodos de atualização.
	
	//Vetores com todas as texturas
	public Texture2D[] texturasHp;
	public Texture2D[] texturasEE;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		//Inicialização de valores
		jogador = player.GetComponent<Player>();
		hpMax = jogador.hpMax;
		hpAtual = jogador.hp;
		eeMax = jogador.eeMax;
		eeAtual = jogador.ee;
		
		
		
		EEGui = GameObject.Find("/HUD/EE");
		HPGui = GameObject.Find("/HUD/HP");
		
		eeGUIText = EEGui.GetComponent<GUITexture>();
		hpGUIText = HPGui.GetComponent<GUITexture>();
		
		//Inicia Coroutine UpdateStatus
		StartCoroutine(UpdateStatus());
	
	}
	IEnumerator UpdateStatus() //Coroutine que atualiza todas as barras;
	{
		while(true)
		{
			//Testa se atualizará as barras ou não.
			if(exibeHp)
				UpdateHP();
			if(exibeEe)
				UpdateEE();
			
			
			yield return new WaitForSeconds(0.05f); //Define tempo de atualização
		}
	}
	
	void UpdateHP() //Atualiza barra de HP
	{
		hpAtual = jogador.hp;
		hpGUIText.texture = defineImagem(hpMax,hpAtual,texturasHp);
		
	}
	void UpdateEE() //Atualiza barra de EE
	{
		eeAtual = jogador.ee;
		eeGUIText.texture = defineImagem(eeMax,eeAtual,texturasEE);
		
	}
	
	// Método padrão para calculo da imagem a ser usada (padrão para ambas as barras)
	private Texture2D defineImagem(float max, float atual, Texture2D[] vetorImagens)
	{
		if(atual >= max)
		{
			return vetorImagens[vetorImagens.Length -1];
		}
		//Calcula porcentagem
		float porcentagem = (atual/max)*100.0f;
		
		//Usando porcentagem e quantidade de imagens, define qual imagem será usada...
		int tamanhoVetor = vetorImagens.Length;
		int resultado;
		resultado = Mathf.RoundToInt((tamanhoVetor*porcentagem)/100); //Define qual imagem será usada
		
		//Debug.Log("Porcentagem = "+porcentagem+"% Imagem = "+resultado);
		
		return vetorImagens[resultado>0? resultado-1: 0];
		
	}
}
