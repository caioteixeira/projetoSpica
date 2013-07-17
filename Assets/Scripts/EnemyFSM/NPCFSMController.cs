using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using Serialization;

public class NPCFSMController : MonoBehaviour
{
	public float updateTime = 0.1f;
	public GameObject player;
	private FSMSystem fsm; //
	
	public float walkSpeed = 1;
	public float runSpeed = 3;
	
	public PatrolValues patrulha;
	public AtaqueValues ataque;
	public LutaValues luta;
	public float damageTimer;
	public float damageReceptTimer;
	
	public int semente;
	
	public EnemyActions enemyActions;
	
	#region Inicialização da maquina
	public void Start ()
	{
		MakeFSM (); //Instancializa a FSM
		StartCoroutine(UpdateFSM());//Inicia a Coroutine de atualização UpdateFSM
		
		enemyActions = new EnemyActions(gameObject);
	}

	private void MakeFSM ()
	{
		//Instacializa os estados
		
		Patrulha patrulha = new Patrulha();
		NPCDefend npcDefend = new NPCDefend();
		NPCAtack npcAtack = new NPCAtack();
		NPCDie npcDie = new NPCDie();
		NPCDamage npcDamage = new NPCDamage();
		Luta luta = new Luta();
		
		//Cria a maquina
		fsm = new FSMSystem (gameObject);
		
		
		//Adiciona os estados na maquina
		fsm.AddState(patrulha);
		fsm.AddState(npcDefend);
		fsm.AddState(npcAtack);
		fsm.AddState(npcDie);
		fsm.AddState(luta);
		fsm.AddState(npcDamage);
		
		if (showLog)
			Debug.Log ("NPCFSMController: Maquina construida!");
		fsm.CurrentState.DoBeforeEntering(gameObject);
	}
	#endregion
	
	#region Atualização da maquina
	//Método para setar a transição
	public void SetTransition (Transition t)
	{
		if (showLog) {
			Debug.Log ("NPCFSMController: Transicao " + fsm.CurrentState.ToString () + t.ToString ());	
		}
		fsm.PerformTransition (t); 
		
	}

	/*public void FixedUpdate ()
	{
		fsm.CurrentState.Act (player, gameObject);
		fsm.CurrentState.Reason (player, gameObject);
	}
	*/
	
	 IEnumerator UpdateFSM(){ //Coroutine UpdateFSM, tem como função gerenciar a maquina de estados
		while(true && updateTime!=0.0f){
			fsm.CurrentState.Act (player, gameObject);
			fsm.CurrentState.Reason (player, gameObject);
			AtualizarValores();
			yield return new WaitForSeconds(updateTime); // Aguarda por 0.1 segundos para próximo ciclo
		}		
	}
	
	public void AtualizarValores()
	{
		if(damageTimer > 0)
			damageTimer -= updateTime;
		damageReceptTimer += updateTime;
	}
	#endregion
	
	
	
	//Variaveis locais (Utilizadas pelo controlador)
	//Nota b(Boolean) + nome da variavel
	//Razão = conflito com nomes das mensagens
	
	#region Variaveis locais (PlayerController)
	public float campoDeVisao = 10.0f;
	
	public bool bTakingDamage;
	public bool bTouchGround;
	public bool bDie = false; //Morte
	public bool animationEnd;
	
	//Permitir exibição de Logs
	public bool showLog = false; 
	
	#endregion
	//Métodos que recebem mensagens externas
	/*Nota: Mensagens externas devem ser recebidas no corpo do controlador
	 * 
	 */
	
	#region Mensagens Externas
	public void OnDeserialized()
	{
		enemyActions = new EnemyActions(gameObject);
		//Debug.Log ("Serializou");
		if(bDie)
			enemyActions.Die();
	}

	public void AnimationEnd()
	{
		animationEnd = true;
		//if(showLog)
		//	Debug.Log ("NPCFSMController - Mensagem: AnimationEnd");
		
	}
	
	public void TouchGround ()
	{
		bTouchGround = true;
		if (showLog)
			Debug.Log ("NPCFSMController - Mensagem: TouchGround");
	}

	public void TouchGroundFalse ()
	{
		bTouchGround = false;
		if (showLog)
			Debug.Log ("NPCFSMController - Mensagem: TouchGround = false");
	}

	public void TakingDamage ()
	{
		bTakingDamage = true;
		if (showLog)
			Debug.Log ("NPCFSMController - Mensagem: TakingDamage");
		
		damageReceptTimer = 0.0f;
	}

	public void TakingDamageFalse ()
	{
		bTakingDamage = false;
		if (showLog)
			Debug.Log ("NPCFSMController - Mensagem: TakingDamage = false");
	}

	public void DieState ()
	{
		bDie = true;
		if(showLog)
			Debug.Log ("NPCFSMController - Mensagem: Die");
	}
	#endregion
}

//Classes dos estados

/*
 * Estados devem sempre ter um método Act() e um método Reason()
 * Estados devem sempre herdar de FSMState
 */

//TODO: Fazer Act() de todos os estados

public class Patrulha: FSMState
{
	NPCFSMController controller;
	PatrolFunctions patrolFunctions;

	public Patrulha ()
	{       
		stateID = StateID.Patrulha;
		AddTransition(Transition.ToLuta, StateID.Luta);
		AddTransition(Transition.ToDie, StateID.Die);
		AddTransition(Transition.ToDamage, StateID.Damage);
		
		
	}
	public override void DoBeforeEntering (GameObject gameObject)
	{
		
		controller = gameObject.GetComponent<NPCFSMController> ();
		patrolFunctions = new PatrolFunctions(gameObject, controller.patrulha, controller.updateTime);
		patrolFunctions.walkSpeed = controller.walkSpeed;
		
		if(controller.showLog)
				Debug.Log ("NPCFSMController - Entrou no estado Patrulha!");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		patrolFunctions.UpdatePatrol();	
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		//Testa morte
		if(controller.bDie)
		{
			controller.SetTransition(Transition.ToDie);	
		}
		
		else if(EnemyFunctions.EnemyRay(controller.campoDeVisao,gameObject, Color.red, 8))
		{
			if(controller.showLog)
				Debug.Log ("NPCFSMController - Inimigo avistado!");
			controller.SetTransition(Transition.ToLuta);
		}
		else if(controller.bTakingDamage)
		{
			controller.SetTransition(Transition.ToDamage);	
		}
		
	}
} 
public class Luta: FSMState
{
	NPCFSMController controller;
	public Luta()
	{
		stateID = StateID.Luta;
		AddTransition(Transition.ToAtack,StateID.Atack);
		AddTransition(Transition.ToDefend, StateID.Defend);
		AddTransition(Transition.ToPatrulha, StateID.Patrulha);
		AddTransition(Transition.ToDie, StateID.Die);
		AddTransition(Transition.ToDamage, StateID.Damage);
	}
	public override void DoBeforeEntering (GameObject gameObject)
	{
		
		
		controller = gameObject.GetComponent<NPCFSMController> ();
		
		LutaFunctions.Inicializar(controller);
		if(controller.showLog)
				Debug.Log ("NPCFSMController - Entrou no estado Luta");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		switch (LutaFunctions.testaDistancia(controller, controller.transform.position))
		{
			case Distancia.distancias.MuitoPerto:
				int decisao; // 1 - Ataque / 2 - Defesa / 3 - Idle
				System.Random random = new System.Random(controller.semente);
			
				decisao = UnityEngine.Random.value>0.5f?1:2;
				
				switch(decisao)
				{
					case 1:
						int dAtaque = random.Next(0, controller.ataque.ataques.Length-1);
						controller.ataque.ataqueSelecionado = controller.ataque.ataques[1];
						controller.SetTransition(Transition.ToAtack);
						break;
					case 2:
						//controller.SetTransition(Transition.ToDefend);
						break;
				}
				if(controller.showLog)
				{
					Debug.Log (decisao);
				}
					
				
				break;
			
			case Distancia.distancias.Perto:
				controller.enemyActions.Walk(player.transform.position, controller.walkSpeed);
				break;
			
			case Distancia.distancias.Longe:
				controller.enemyActions.Run(player.transform.position, controller.runSpeed);
				break;

		}
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		//Testa morte
		
		if(controller.bDie)
		{
			controller.SetTransition(Transition.ToDie);
		}
		
		else if(LutaFunctions.testaDistancia(controller, controller.transform.position) == Distancia.distancias.MuitoLonge)
		{
			controller.SetTransition(Transition.ToPatrulha);	
		}
		else if(controller.bTakingDamage)
		{
			controller.SetTransition(Transition.ToDamage);	
		}
	}
}

public class NPCDefend: FSMState
{
	NPCFSMController controller;

	public NPCDefend ()
	{       
		stateID = StateID.Defend;
		AddTransition(Transition.ToDie, StateID.Die);
		AddTransition(Transition.ToLuta, StateID.Luta);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<NPCFSMController> ();
		
		if(controller.showLog)
		{
			Debug.Log ("NPCFSMController - Entrou no estado Defend");	
		}
		controller.enemyActions.Defend();
		controller.bTakingDamage = false;
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		if(controller.bTakingDamage)
		{
			controller.damageTimer = 4.0f;
			controller.bTakingDamage = false;
		}
		
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		//Testa morte
		if(controller.bDie)
		{
			controller.SetTransition(Transition.ToDie);	
		}
		if(controller.damageReceptTimer > 1.0f)
		{
			controller.SetTransition(Transition.ToLuta);	
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		controller.enemyActions.DefendFalse();
		controller.SetTransition(Transition.ToLuta);	
	}
} 

public class NPCAtack: FSMState
{
	NPCFSMController controller;

	public NPCAtack ()
	{       
		stateID = StateID.Atack;
		AddTransition(Transition.ToLuta, StateID.Luta);
		AddTransition(Transition.ToDie, StateID.Die);
		AddTransition(Transition.ToDamage,StateID.Damage);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		
		controller = gameObject.GetComponent<NPCFSMController> ();
		controller.enemyActions.Attack(controller.ataque.ataqueSelecionado);
		
		if(controller.showLog)
				Debug.Log ("NPCFSMController - Entrou no estado Ataque!");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		//Testa morte
		if(controller.bDie)
		{
			controller.SetTransition(Transition.ToDie);	
		}
		if(controller.animationEnd)
		{
			controller.SetTransition(Transition.ToLuta);	
		}
		if(controller.bTakingDamage)
		{
			controller.SetTransition(Transition.ToDamage);	
		}
		
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		controller.enemyActions.DesativaPunhos();
		controller.animationEnd = false;
	}
} 


public class NPCDamage: FSMState
{
	NPCFSMController controller;
	

	public NPCDamage ()
	{       
		stateID = StateID.Damage;
		AddTransition(Transition.ToLuta, StateID.Luta);
		AddTransition(Transition.ToDie, StateID.Die);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
	
		
		gameObject.SendMessage("TakingDamageAnimation");
		controller = gameObject.GetComponent<NPCFSMController> ();
		
		if(controller.showLog)
			Debug.Log ("NPCFSMController - Entrou no estado Damage");
		controller.damageTimer = 2.0f;
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		if(controller.showLog)
			Debug.Log ("NPCFSMController - Damage Timer:" + controller.damageTimer);
		if(controller.bTakingDamage)
		{
			controller.damageTimer = 2.0f;
			controller.bTakingDamage = false;
			gameObject.SendMessage("TakingDamageAnimation");
		}
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		//Testa morte
		
		if(controller.animationEnd )
		{
			if(controller.bDie)
			{
				controller.SetTransition(Transition.ToDie);	
			}
			else
			{
				controller.SetTransition(Transition.ToLuta);
			}
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		
		controller.animationEnd = false;
	}
} 


public class NPCDie: Die //Herança de Die (PlayerFSMController.cs)  
{
	NPCFSMController controller;

	public NPCDie ()
	{       
		stateID = StateID.Die;
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<NPCFSMController> ();
		controller.enemyActions.Die();
		controller.updateTime = 0;
		GameObject.Destroy(gameObject.GetComponent<CharacterController>());
		GameObject.Destroy(controller);
		
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
	}
} 