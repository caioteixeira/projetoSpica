using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour {

	public Golpe golpe;
	private Player player;
	public GameObject root;
	
	void Awake()
	{
		player = root.gameObject.GetComponent<Player>();
		
	}
	void OnTriggerEnter(Collider collider) 
	{
		
		GameObject target = collider.gameObject; //Inicializa target como o gameObject do collider
		Player targetPlayer;
		if(target.tag == "Enemy" && golpe.power > 0) //Se o target for um Player e o golpe não for nulo
		{
			/*
			 * Garante que a variavel Player seja iniciliazada
			 */ 
			
			//Caso ataque atinja um dos membros periféricos (punhos ou pernas)
			if(target.GetComponent<Player>() == null) 
			{
				//Pega o componente Player do GameObject do topo da hierarquia
				GameObject parent = GameObject.Find(target.transform.root.name);
				targetPlayer = parent.GetComponent<Player>();
			}
			//Caso atinja diretamente o CharacterController (topo da hierarquia)
			else
			{
				targetPlayer = target.GetComponent<Player>(); //Pega o componente Player
			}
			
			//Envia os valores para calculo de dano de acordo com o tipo do ataque;
			switch (golpe.tipoDoGolpe) 
			{
				case Golpe.TiposGolpe.Fisico:
					targetPlayer.RecebeDano(player.stamina, golpe.power);
					break;
				case Golpe.TiposGolpe.Misto:
					targetPlayer.RecebeDano(player.stamina, player.poderEspiritual, player.manipulacao,golpe.power);	
					break;
				case Golpe.TiposGolpe.Espiritual:
					targetPlayer.RecebeDano(player.poderEspiritual,player.manipulacao, golpe.power);
					break;
			}
		}
	}
}