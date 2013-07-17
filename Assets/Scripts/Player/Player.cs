using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float hp;
	public float hpMax;
	public float ee;
	public float eeMax;
	public float stamina;
	public float poderEspiritual;
	public float manipulacao;
	public float defesa;
	public float defesaEspiritual;
	
	public bool showLog;
	float tmpDefesa;
	float tmpDefesaEspiritual;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(hp <= 0)
		{
			SendMessage("DieState",SendMessageOptions.DontRequireReceiver);
			
		}
	}
	public void Defend()
	{
		tmpDefesa = defesa;
		tmpDefesaEspiritual = defesaEspiritual;
		
		defesa = defesa*3;
		defesaEspiritual = defesaEspiritual*3;
	}
	public void DefendFalse()
	{
		defesa = tmpDefesa;
		defesaEspiritual = tmpDefesaEspiritual;
	}
	//Ataque fisico
	public void RecebeDano(float staminaAt, float power)
	{
		float dano = CalculadoraDeDanos.CalculoDeDano(staminaAt, defesa, power);
		if(showLog)
			Debug.Log(gameObject.name + " recebeu dano fisico de "+ dano);
		hp -= dano;
		gameObject.SendMessage("TakingDamage");
	}
	//Ataque espiritual
	public void RecebeDano(float poderEspiritualAt, float manipulacaoAt, float power)
	{
		float dano = CalculadoraDeDanos.CalculoDeDano(poderEspiritualAt, manipulacaoAt, defesaEspiritual, manipulacao, power);
		if(showLog)
			Debug.Log(gameObject.name + " recebeu dano espiritual de "+ dano);
		
		hp -= dano;
		
		gameObject.SendMessage("TakingDamage");
	}
	//Ataque misto
	public void RecebeDano(float staminaAt, float poderEspiritualAt, float manipulacaoAt, float power)
	{
		float dano = CalculadoraDeDanos.CalculoDeDano(staminaAt, poderEspiritualAt, manipulacaoAt, defesa, poderEspiritual,manipulacao, power);
		if(showLog)	
			Debug.Log(gameObject.name + " recebeu dano misto de "+ dano);
		
		hp-= dano;
		gameObject.SendMessage("TakingDamage");
		
	}
	
}
