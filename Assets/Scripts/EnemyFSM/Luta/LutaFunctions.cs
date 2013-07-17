using UnityEngine;
using System.Collections;

public class LutaFunctions{
	 
	/// <summary>
	/// Inicializa os valores do estado Luta.
	/// Para ser utilizado junto com o componente NPCFSMController
	/// </summary>
	/// <param name='controller'>
	/// Componente NPCFSMController.
	/// </param>
	public static void Inicializar(NPCFSMController controller)
	{
		//Inicializa punhos
		foreach (Attack temp in controller.ataque.ataques)
		{
			temp.punhoDireito = controller.ataque.punhoDireito;
			temp.punhoEsquerdo = controller.ataque.punhoEsquerdo;
		}	
	}
	public static void Atualiza(NPCFSMController controller)
	{
		
	}
	public static Distancia.distancias testaDistancia(NPCFSMController controller, Vector3 npcPosition)
	{
		Vector3 targetPosition = controller.player.transform.position;
		LutaValues valores = controller.luta;
		
		float distancia = Vector3.Distance(npcPosition, targetPosition);
		
		float muitoPerto, perto, longe, muitoLonge;
		
		Distancia.distancias saida;
		
		//Testes de distância - Fuzzy
		muitoPerto = EnemyFunctions.EstaMuitoPerto(distancia, valores.muitoPerto.distanciaMax, valores.muitoPerto.tolerancia);
		perto = EnemyFunctions.EstaPerto(distancia, valores.perto.distanciaMax, valores.perto.tolerancia);
		longe = EnemyFunctions.EstaLonge(distancia, valores.longe.distanciaMin, valores.longe.distanciaMax, valores.longe.tolerancia);
		muitoLonge = EnemyFunctions.EstaMuitoLonge(distancia, valores.muitoLonge.distanciaMin, valores.muitoLonge.tolerancia);
		
		if(muitoPerto >= perto && muitoPerto > longe && muitoPerto > muitoLonge)
		{
			saida = Distancia.distancias.MuitoPerto;
		}
		else if(muitoPerto < 1 && perto > longe && perto > muitoLonge)
		{
			saida = Distancia.distancias.Perto;	
		}
		else if(longe > muitoPerto && longe > perto && longe > muitoLonge)
		{
			saida = Distancia.distancias.Longe;
		}
		else
		{
			saida = Distancia.distancias.MuitoLonge;
		}
		//Debug.Log (saida.ToString());
		return saida;			
	}
}
