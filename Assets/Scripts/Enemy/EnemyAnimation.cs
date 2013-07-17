using UnityEngine;
using System.Collections;
using Serialization;

[SerializeAll]
public class EnemyAnimation : MonoBehaviour {
	
	/// <summary>
	/// Referencias para cada animação.
	/// </summary>
	enum Animations
	{
		idle,
		walk,
		run,
		jump,
		attack1,
		attack2,
		defend,
		takingDamage,
		takingDamageSuperNova,
		dead
	}
	Animations anima;
	
	void Start()
	{
		animation["walk"].speed = 3;
		animation["run"].speed = 3;
		animation["dead"].speed = 0.1f;
		animation["defend"].speed = 2.5f;
		animation["atack1"].speed = 0.4f;
		animation["atack2"].speed = 0.4f;
		
	}
	void Update()
	{
		StartCoroutine(UpdateAnimation());
	}
	
	#region Mensagens da FSM
	void IdleAnimation(){
		anima = Animations.idle;
		animation.wrapMode = WrapMode.Loop;
		animation.CrossFade("idle");
	}
	
	void WalkAnimation(){ 
		anima = Animations.walk;
		animation.wrapMode = WrapMode.Loop;
		
		animation.CrossFade("walk");
	}
	
	void RunAnimation(){
		anima = Animations.run;
		animation.wrapMode = WrapMode.Loop;
		animation.CrossFade("run");
	}
	
	void DefendAnimation()
	{
		anima = Animations.defend;
		animation.wrapMode = WrapMode.Once;
		animation.CrossFade("defend");
	}
	
	void JumpAnimation(){
		anima = Animations.jump;
	}
	
	public void PlayAnimation(string animationName)
	{
		animation.wrapMode = WrapMode.Once;
		animation.Play(animationName);
		anima = Animations.attack1;
	}
	
	void TakingDamageAnimation(){
		animation.wrapMode = WrapMode.Once;
		anima = Animations.takingDamage;
		animation.Play("danos");
	}
	
	void TakingDamageSuperNova(){
		anima = Animations.takingDamageSuperNova;
	}
	
	void DieAnimation(){
		//Debug.Log("DieAnimation");
		
		animation.wrapMode = WrapMode.Once;
		animation.Play("dead");
	}
	#endregion
	
	IEnumerator UpdateAnimation()
	{
		while(true)
		{
			switch(anima)
			{
			case Animations.idle:
				
				break;
			case Animations.walk:
				
				break;
			case Animations.run:
				
				break;
			case Animations.jump:
				
				break;
			case Animations.attack1:
				
				if(!animation.isPlaying)
				{
					gameObject.SendMessage("AnimationEnd", SendMessageOptions.DontRequireReceiver);
				}
				
				break;
			case Animations.attack2:
				
				if(!animation.isPlaying)
				{
					gameObject.SendMessage("AnimationEnd", SendMessageOptions.DontRequireReceiver);
				}
				
				break;
			case Animations.takingDamage:
				
				if(!animation.isPlaying)
				{
					gameObject.SendMessage("AnimationEnd", SendMessageOptions.DontRequireReceiver);
				}
				break;
			case Animations.takingDamageSuperNova:
				
				if(!animation.isPlaying)
				{
					gameObject.SendMessage("AnimationEnd", SendMessageOptions.DontRequireReceiver);
				}
				
				break;
			case Animations.dead:
				
				if(!animation.isPlaying)
				{
					gameObject.SendMessage("AnimationEnd", SendMessageOptions.DontRequireReceiver);
				}
				
				break;
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	}	
	
}
