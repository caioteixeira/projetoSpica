using UnityEngine;
using System.Collections;

public static class CalculadoraDeDanos {
	public static float luckSoma = 0.5f;
	
	//Ataque fisico
	public static float CalculoDeDano(float staminaAt, float defesaDef, float power)
	{
		float luck = Random.value + luckSoma;
		return (staminaAt/defesaDef)* power * luck;
	}
	//Ataque espiritual
	public static float CalculoDeDano(float poderEspiritualAt, float manipulacaoAt, float defEspiritualDef, float manipulacaoDef, float power)
	{
		float luck = Random.value + luckSoma;
		
		return ((poderEspiritualAt+manipulacaoAt)/(defEspiritualDef+manipulacaoDef)) * power * luck;
	}
	//Ataque misto
	public static float CalculoDeDano(float staminaAt,float poderEspiritualAt, float manipulacaoAt, float defesaDef, float defEspiritualDef, float manipulacaoDef , float power)
	{
		float luck = Random.value + luckSoma;
		
		return ((staminaAt + poderEspiritualAt + manipulacaoAt)/(defesaDef + defEspiritualDef + manipulacaoDef)) * power * luck;
	}
}
