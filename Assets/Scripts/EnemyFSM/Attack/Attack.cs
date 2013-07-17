using UnityEngine;
using System.Collections;


/// <summary>
/// Valores necessários para cada golpe(parte de um ataque).
/// </summary>
[System.Serializable]
public class Golpe
{
	public enum TiposGolpe
	{
		Fisico,
		Espiritual,
		Misto
	}
	public enum ColisoresGolpe
	{
		punhoDireito,
		punhoEsquerdo,
		pernaDireita,
		pernaEsquerda	
	}
	public TiposGolpe tipoDoGolpe;
	public ColisoresGolpe colisorDoGolpe;
	public float power;
}
/// <summary>
/// Valores necessarios para um ataque.
/// </summary>
[System.Serializable]
public class Attack{
	public Distancia.distancias distancia;
	public string nomeDaAnimacao;
	[HideInInspector] public GameObject punhoDireito;
	[HideInInspector] public GameObject punhoEsquerdo;
	public Golpe[] golpes;
	[HideInInspector] public int index;
}

/// <summary>
/// Valores necessarios para o estado Ataque do NPCFSMController. 
/// </summary>

[System.Serializable]
public class AtaqueValues{	
	public GameObject punhoDireito;
	public GameObject punhoEsquerdo;
	
	public Attack[] ataques;
	
	public Attack ataqueSelecionado;
}


