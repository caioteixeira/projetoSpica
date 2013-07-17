using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;

public class PlayerFSMController : MonoBehaviour
{
	public GameObject player;
	private FSMSystem fsm; //
	
	#region Inicialização da maquina
	public void Start ()
	{
		MakeFSM (); //Instancializa a FSM
		StartCoroutine(UpdateFSM());//Inicia a Coroutine de atualização UpdateFSM
	}

	private void MakeFSM ()
	{
		//Instacializa os estados
		
		FSMState idle = new Idle ();
		FSMState walk = new Walk ();
		FSMState run = new Run ();
		FSMState jump = new Jump ();
		FSMState defend = new Defend ();
		FSMState atack = new Atack ();
		FSMState damage = new Damage ();
		FSMState die = new Die ();
       
		
		//Cria a maquina
		fsm = new FSMSystem (gameObject);
		
		//Adiciona os estados na maquina
		fsm.AddState (idle);
		fsm.AddState (walk);
		fsm.AddState (run);
		fsm.AddState (jump);
		fsm.AddState (defend);
		fsm.AddState (atack);
		fsm.AddState (damage);
		fsm.AddState (die);
		
		if (showLog)
			Debug.Log ("PlayerFSMController: Maquina construida!");
	}
	#endregion
	
	#region Atualização da maquina
	//Método para setar a transição
	public void SetTransition (Transition t)
	{
		if (showLog) {
			Debug.Log ("PlayerFSMController: Transicao " + fsm.CurrentState.ToString () + t.ToString ());	
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
		while(true){
			fsm.CurrentState.Act (player, gameObject);
			fsm.CurrentState.Reason (player, gameObject);
			yield return new WaitForSeconds(0.1f); // Aguarda por 0.1 segundos para próximo ciclo
		}		
	}
	#endregion
	
	
	//Variaveis locais (Utilizadas pelo controlador)
	//Nota b(Boolean) + nome da variavel
	//Razão = conflito com nomes das mensagens
	
	#region Variaveis locais (PlayerController)
	public bool bInputWalk;
	public bool bInputJump;
	public bool bInputRun;
	public bool bInputAtack;
	public bool bInputDefend;
	public bool bTakingDamage;
	public bool bTouchGround;
	public bool bDie; //Morte
	
	//Permitir exibição de Logs
	public bool showLog = false; 
	
	#endregion
	//Métodos que recebem mensagens externas
	/*Nota: Mensagens externas devem ser recebidas no corpo do controlador
	 * 
	 */
	
	#region Mensagens Externas
	public void InputWalk ()
	{
		bInputWalk = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputWalk");
	}

	public void InputWalkFalse ()
	{
		bInputWalk = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputWalk = false");
	}

	public void InputJump ()
	{
		bInputJump = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputJump");
	}

	public void InputJumpFalse ()
	{
		bInputJump = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputJump = false");
	}

	public void InputRun ()
	{
		bInputRun = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputRun");
	}

	public void InputRunFalse ()
	{
		bInputRun = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputRun = false");
	}

	public void InputAtack ()
	{
		bInputAtack = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputAtack");
	}

	public void InputAtackFalse ()
	{
		bInputAtack = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputAtack = false");
	}

	public void InputDefend ()
	{
		bInputDefend = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputDefend");
	}

	public void InputDefendFalse ()
	{
		bInputDefend = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: InputDefend = false");
	}

	public void TouchGround ()
	{
		bTouchGround = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: TouchGround");
	}

	public void TouchGroundFalse ()
	{
		bTouchGround = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: TouchGround = false");
	}

	public void TakingDamage ()
	{
		bTakingDamage = true;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: TakingDamage");
	}

	public void TakingDamageFalse ()
	{
		bTakingDamage = false;
		if (showLog)
			Debug.Log ("PlayerFSMController - Mensagem: TakingDamage = false");
	}

	public void Die ()
	{
		bDie = true;
		Debug.Log ("PlayerFSMController - Mensagem: Die");
	}
	#endregion
}

//Classes dos estados

/*
 * Estados devem sempre ter um método Act() e um método Reason()
 * Estados devem sempre herdar de FSMState
 */

//TODO: Fazer Act() de todos os estados

public class Idle : FSMState
{
	PlayerFSMController controller;

	public Idle ()
	{
		stateID = StateID.Idle;	
		
		this.AddTransition (Transition.ToWalk, StateID.Walk);
		this.AddTransition (Transition.ToDefend, StateID.Defend);
		this.AddTransition (Transition.ToJump, StateID.Jump);
		this.AddTransition (Transition.ToAtack, StateID.Atack);
		this.AddTransition (Transition.ToDie, StateID.Die);
		this.AddTransition (Transition.ToDamage, StateID.Damage);
		    
	}
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		gameObject.SendMessage("Idle");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
		
		if (controller.bDie) {
			controller.SetTransition (Transition.ToDie);	
			return;
		}
		if (controller.bInputWalk) {
			controller.SetTransition (Transition.ToWalk);
			return;
		}
		if (controller.bInputJump) {
			controller.SetTransition (Transition.ToJump);	
			return;
		}
		if (controller.bInputDefend) {
			controller.SetTransition (Transition.ToDefend);	
			return;
		}
		if (controller.bTakingDamage) {
			controller.SetTransition (Transition.ToDamage);	
			return;
		}
		if (controller.bInputAtack)
		{
			controller.SetTransition(Transition.ToAtack);	
			return;
		}
		
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("IdleFalse");
	}
}

public class Walk : FSMState
{
	PlayerFSMController controller;

	public Walk ()
	{
		stateID = StateID.Walk;
		
		this.AddTransition (Transition.ToRun, StateID.Run);
		this.AddTransition (Transition.ToAtack, StateID.Atack);
		this.AddTransition (Transition.ToJump, StateID.Jump);
		this.AddTransition (Transition.ToIdle, StateID.Idle);
		this.AddTransition (Transition.ToDamage, StateID.Damage);
	}
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		
		gameObject.SendMessage("Walk");
	}
	public override void Act (GameObject player, GameObject gameObject)
	{
		 
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
		if (controller.bDie) {
			controller.SetTransition (Transition.ToIdle);	
			return;
		}
		if (controller.bInputWalk == false) {
			controller.SetTransition (Transition.ToIdle);
			return;
		}
		if (controller.bInputRun) {
			controller.SetTransition (Transition.ToRun);
			return;
		}
		if (controller.bInputJump) {
			controller.SetTransition (Transition.ToJump);	
			return;
		}
		if (controller.bInputAtack) {
			controller.SetTransition (Transition.ToAtack);	
			return;
		}
		
		if (controller.bTakingDamage) {
			controller.SetTransition (Transition.ToDamage);	
			return;
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("WalkFalse");
	}
}

public class Run : FSMState
{
	PlayerFSMController controller;

	public Run ()
	{
		stateID = StateID.Run;

		this.AddTransition (Transition.ToIdle, StateID.Idle);
		this.AddTransition (Transition.ToWalk, StateID.Walk);
		this.AddTransition (Transition.ToJump, StateID.Jump);
		this.AddTransition (Transition.ToAtack, StateID.Atack);
		this.AddTransition (Transition.ToDamage, StateID.Damage);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		
		gameObject.SendMessage("Run");
	}
	
	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
		if (controller.bDie) {
			controller.SetTransition (Transition.ToIdle);	
			return;
		}
		if(controller.bInputRun == false)
		{
			if(controller.bInputWalk)
			{
				controller.SetTransition(Transition.ToWalk);
			}
			else
			{
				controller.SetTransition(Transition.ToIdle);
			}
			return;
		}
		if(controller.bInputWalk == false)
		{
			controller.SetTransition(Transition.ToIdle);
			return;
		}
		if(controller.bInputJump)
		{
			controller.SetTransition(Transition.ToJump);
			return;
		}
		if(controller.bInputAtack)
		{
			controller.SetTransition(Transition.ToAtack);
			return;
		}
		if(controller.bTakingDamage)
		{
			controller.SetTransition(Transition.ToDamage);
			return;
		}	
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("RunFalse");
	}
}

public class Jump : FSMState
{
	PlayerFSMController controller;

	public Jump ()
	{
		stateID = StateID.Jump;
		
		this.AddTransition (Transition.ToIdle, StateID.Idle);
		this.AddTransition (Transition.ToAtack, StateID.Atack);
		this.AddTransition (Transition.ToWalk, StateID.Walk);
		this.AddTransition (Transition.ToRun, StateID.Run);
		this.AddTransition (Transition.ToDamage, StateID.Damage);
		
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		gameObject.SendMessage("Jump");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		if (controller.bTouchGround) {
			switch (previousStateID) {
			case StateID.Idle:
				controller.SetTransition (Transition.ToIdle);
				break;
			case StateID.Walk:
				controller.SetTransition (Transition.ToWalk);
				break;
			case StateID.Run:
				controller.SetTransition (Transition.ToRun);
				break;									
			}
			controller.bInputJump = false;
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("JumpFalse");
	}
}
	
public class Defend: FSMState
{
	PlayerFSMController controller;

	public Defend ()
	{
        
		stateID = StateID.Defend;
		
		AddTransition (Transition.ToIdle, StateID.Idle);
		AddTransition (Transition.ToDamage, StateID.Damage);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		gameObject.SendMessage("Defend");
	}
	
	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
		if (controller.bDie) {
			controller.SetTransition (Transition.ToIdle);	
			return;
		}
		if(controller.bInputDefend == false)
		{
			controller.SetTransition(Transition.ToIdle);
			return;
		}
		if(controller.bTakingDamage)
		{
			controller.SetTransition(Transition.ToDamage);	
			return;
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("DefendFalse");
	}
}

public class Atack: FSMState
{
	PlayerFSMController controller;

	public Atack ()
	{
        
		stateID = StateID.Atack;
		
		AddTransition (Transition.ToIdle, StateID.Idle);
		AddTransition (Transition.ToJump, StateID.Jump);
		AddTransition (Transition.ToWalk, StateID.Walk);
		AddTransition (Transition.ToRun, StateID.Run);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		gameObject.SendMessage("Atack");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		if(controller.bDie)
		{
			controller.SetTransition(Transition.ToIdle);
			return;
		}
		if(controller.bInputAtack == false)
		{	
		 	switch (previousStateID) {
			case StateID.Idle:
				controller.SetTransition (Transition.ToIdle);
				break;
			case StateID.Walk:
				controller.SetTransition (Transition.ToWalk);
				break;
			case StateID.Run:
				controller.SetTransition (Transition.ToRun);
				break;									
			
			}
			return;
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("AtackFalse");
	}
}

public class Damage: FSMState
{
	PlayerFSMController controller;

	public Damage ()
	{       
		stateID = StateID.Damage;
		AddTransition (Transition.ToIdle, StateID.Idle);
		AddTransition (Transition.ToDefend, StateID.Defend);
		AddTransition (Transition.ToJump, StateID.Jump);
		AddTransition (Transition.ToRun, StateID.Run);
		AddTransition (Transition.ToWalk, StateID.Walk);
	}
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		
		gameObject.SendMessage("Damage");
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
		if(controller.bTakingDamage == false)
		{	
		 	switch (previousStateID) {
			case StateID.Idle:
				controller.SetTransition (Transition.ToIdle);
				break;
			case StateID.Walk:
				controller.SetTransition (Transition.ToWalk);
				break;
			case StateID.Run:
				controller.SetTransition (Transition.ToRun);
				break;
			case StateID.Defend:
				controller.SetTransition(Transition.ToDefend);
				break;
			case StateID.Jump:
				controller.SetTransition(Transition.ToJump);
				break;		
			}
			return;
		}
	}
	public override void DoBeforeLeaving (GameObject gameObject)
	{
		gameObject.SendMessage("DamageFalse");
	}
}

public class Die: FSMState
{
	PlayerFSMController controller;
	
	public override void DoBeforeEntering (GameObject gameObject)
	{
		controller = gameObject.GetComponent<PlayerFSMController> ();
		
		gameObject.SendMessage("Die");
	}
	
	public Die ()
	{       
		stateID = StateID.Die;
	}

	public override void Act (GameObject player, GameObject gameObject)
	{
		
	}

	public override void Reason (GameObject player, GameObject gameObject)
	{
		
	}
} 

