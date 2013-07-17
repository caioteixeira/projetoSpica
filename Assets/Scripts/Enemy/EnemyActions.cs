using UnityEngine;
using System.Collections;
using Serialization;

/// <summary>
/// Classe responsavel por todas ações do inimigo
/// Ex: Ataque, Defesa, Andar, Correr, etc...
/// </summary>
[System.Serializable]
[SerializeAll]
public class EnemyActions {
	
	GameObject gameobject;
	CharacterController controller;
	Transform transform;
	public Attack localEnemyAttack;
	
	public EnemyActions()
	{
		
	}
	public EnemyActions(GameObject enemy)
	{
		this.gameobject = enemy;
		this.controller = enemy.GetComponent<CharacterController>();
		this.transform = enemy.transform;
	}
	
	/// <summary>
	/// Anda com determinada velocidade.
	/// </summary>
	/// <param name='rightSide'>
	/// Personagem andará para a direita?.
	/// </param>
	/// <param name='speed'>
	/// Velocidade do movimento.
	/// </param>
	public void Walk(bool rightSide, float speed )
	{
		Debug.Log("Walk");
		Vector3 direction = Vector3.forward;
		direction = gameobject.transform.TransformDirection(direction);
		direction *= speed/50;
		
		
		if(rightSide && transform.rotation.y != 90)
		{
			transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
			
		}
		else
		{
			transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
		}
		
		gameobject.SendMessage("WalkAnimation");
		controller.Move (direction);
		
	}
	/// <summary>
	/// Anda em direção a um alvo em determinada velocidade.
	/// </summary>
	/// <param name='target'>
	/// Alvo (direção do movimento).
	/// </param>
	/// <param name='speed'>
	/// Velocidade do movimento.
	/// </param>
	public void Walk(Vector3 target, float speed )
	{
		transform.LookAt(target);
		Vector3 direction = Vector3.forward;
		direction = gameobject.transform.TransformDirection(direction);
		direction *= speed/50;
		gameobject.SendMessage("WalkAnimation");
		controller.Move (direction);
		transform.rotation = new Quaternion(0,transform.rotation.y, 0, transform.rotation.w);
		
	}
	/// <summary>
	/// Corre em determinada velocidade.
	/// </summary>
	/// <param name='rightSide'>
	/// Personagem correrá para a direita?.
	/// </param>
	/// <param name='speed'>
	/// Velocidade do movimento.
	/// </param>
	public void Run(bool rightSide, float speed )
	{
		Debug.Log("Run");
		Vector3 direction = Vector3.forward;
		direction = gameobject.transform.TransformDirection(direction);
		direction *= speed/50;
		
		
		if(rightSide && transform.rotation.y != 90)
		{
			transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
			
		}
		else
		{
			transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
		}
		
		gameobject.SendMessage("RunAnimation");
		controller.Move(direction);
		
		
	}
	public void Run(Vector3 target, float speed )
	{
		transform.LookAt(target);
		Vector3 direction = Vector3.forward;
		direction = gameobject.transform.TransformDirection(direction);
		direction *= speed/50;
		gameobject.SendMessage("RunAnimation");
		controller.Move(direction);
		transform.rotation = new Quaternion(0,transform.rotation.y, 0, transform.rotation.w);
		
	}
	/// <summary>
	/// Coloca o personagem em estado parado (Idle).
	/// </summary>
	public void Idle()
	{
		gameobject.SendMessage("IdleAnimation");
	}
	/// <summary>
	/// Faz o personagem "morrer" e todos os processos relacionados.
	/// </summary>
	public void Die ()
	{
		gameobject.SendMessage("DieAnimation");	
	}
	/// <summary>
	/// Faz o personagem atacar e todos os processos relacionados.
	/// </summary>
	/// <param name='enemyAttack'>
	/// Valores necessarios para execução do ataque.
	/// </param>
	public void Attack(Attack enemyAttack)
	{
		this.localEnemyAttack = enemyAttack;
		localEnemyAttack.index = 0;
		gameobject.GetComponent<EnemyAnimation>().PlayAnimation(localEnemyAttack.nomeDaAnimacao);
		localEnemyAttack.punhoDireito.GetComponent<Punch>().enemyActions = this;
		localEnemyAttack.punhoEsquerdo.GetComponent<Punch>().enemyActions = this;
		AtualizaPunhos();
		
	}
	/// <summary>
	/// Atualiza os valores de cada punhos(colisores de ataque).
	/// Deve ser usado após a execução de cada golpe
	/// </summary>
	public void AtualizaPunhos()
	{
		Punch punch;
		if(localEnemyAttack.golpes[localEnemyAttack.index].colisorDoGolpe == Golpe.ColisoresGolpe.punhoDireito)
		{
			punch = localEnemyAttack.punhoDireito.GetComponent<Punch>();
			punch.golpe.power = localEnemyAttack.golpes[localEnemyAttack.index].power;
		}
		else
		if(localEnemyAttack.golpes[localEnemyAttack.index].colisorDoGolpe == Golpe.ColisoresGolpe.punhoEsquerdo)
		{
			punch = localEnemyAttack.punhoEsquerdo.GetComponent<Punch>();
			punch.golpe.power = localEnemyAttack.golpes[localEnemyAttack.index].power;
		}
	}
	/// <summary>
	/// Reseta os valores dos punhos e os valores do ataque.
	/// Deve ser usado no fim do processo de Ataque;
	/// </summary>
	public void DesativaPunhos()
	{
		Punch punhoEsq = localEnemyAttack.punhoEsquerdo.GetComponent<Punch>();
		Punch punhoDir = localEnemyAttack.punhoDireito.GetComponent<Punch>();
		
		punhoEsq.golpe.power = 0;
		punhoDir.golpe.power = 0;
		this.localEnemyAttack = null;
					
	}
	/// <summary>
	/// Atualiza os valores para o próximo golpe.
	/// Se não houver um próximo golpe, finaliza o ataque;
	/// </summary>
	public void ProximoGolpe()
	{
		localEnemyAttack.index++;
		if(localEnemyAttack.index >= localEnemyAttack.golpes.Length)
		{
			localEnemyAttack.index = 0;	
		}
		else
		{
			this.AtualizaPunhos();
		}
	}
	/// <summary>
	/// Faz a ação "Defender" e processos relacionados.
	/// </summary>
	public void Defend()
	{
		gameobject.SendMessage("Defend");
		gameobject.SendMessage("DefendAnimation");
	}
	/// <summary>
	/// Faz as ações de finalização da ação "Defender".
	/// </summary>
	public void DefendFalse()
	{
		gameobject.SendMessage("DefendFalse");
	}
}
