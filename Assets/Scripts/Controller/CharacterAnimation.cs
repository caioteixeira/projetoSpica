using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	
	public bool showLog = false;
	
	/*Ajusta a velocidade da animação "walk"
	public float modificadorVelAndar = 2.5f;
	//Ajusta a velocidade da animação "run"
	public float modificadorVelCorrer= 1.5f;
	//Ajusta a velocidade da animação "jump"
	public float modificadorVelPulo = 2.0f;
	
	public float punchSpeed = 1;*/

	enum Animations
	{
		idle,
		walk,
		run,
		idle_jump,
		walk_jump,
		run_jump,
		double_jump,
		left_punch,
		right_punch,
		jump_punch,
		double_jump_punch,
		strong_punch,
		super_punch,
		taking_damage,
		dead
	}
	Animations anima;
	
	void Awake(){
		animation.wrapMode = WrapMode.Loop;
	}
	
	void Start()
	{
		animation.Stop();
		
		
		
		//Animação "jump" sobrepõe todas as outras, pode acontecer qdo o personagem está parado, andando ou correndo
		/*int layerPulo = 1;
		AnimationState pulo = animation["idle_jump"];
		pulo.layer = layerPulo;
		pulo.speed *= modificadorVelPulo;
		pulo.wrapMode = WrapMode.Once;
		
		AnimationState andando =  animation["walk"];
		andando.speed *= modificadorVelAndar;
		
		AnimationState correndo = animation["run"];
		correndo.speed *= modificadorVelCorrer;
		
		animation["left_punch"].speed = punchSpeed;
		animation["right_punch"].speed = punchSpeed;
		animation["strong_punch"].speed = punchSpeed;
		animation["super_punch"].speed = punchSpeed;*/
	}
	void Update()
	{
		anima = Animations.idle;
		StartCoroutine(UpdateAnimation());
	}
	
	#region Mensagens
	public void IdleAnimationPlayer(){anima = Animations.idle;}
	
	public void WalkAnimationPlayer(){ anima = Animations.walk;}
	
	public void Run(){anima = Animations.run;}
	
	public void Jump(){anima = Animations.idle_jump;}
	
	public void DoubleJump(){anima = Animations.double_jump;}
	
	public void TakingDamageAnimation(){anima = Animations.taking_damage;}
	
	public void DeadAnimation(){anima = Animations.dead;}
	
	public void LeftPunchAnimation(){anima = Animations.left_punch;}

	public void RightPunchAnimation(){anima = Animations.right_punch;}
	
	public void JumpPunchAnimation(){anima = Animations.jump_punch;}
	
	public void DoubleJumpPunchAnimation(){anima = Animations.double_jump_punch;}
	
	public void StrongPunchAnimation(){anima = Animations.strong_punch;}
	
	public void SuperPunchAnimation(){anima = Animations.super_punch;}
	#endregion
	
	IEnumerator UpdateAnimation()
	{
		while(true)
		{
			switch(anima)
			{
			case Animations.idle:
				animation.CrossFade("idle",0.5f);
				if(showLog)
					Debug.Log("Idle");
				break;
			case Animations.walk:
				animation.CrossFade("walk");
				if(showLog)
					Debug.Log("Walk");
				break;
			case Animations.run:
				animation.CrossFade("run");
				if(showLog)
					Debug.Log("Run");
				break;
			case Animations.idle_jump:
				animation.Play("idle_jump");
				if(showLog)
					Debug.Log("Jump");
				break;
			case Animations.double_jump:
				animation.PlayQueued("double_jump",QueueMode.PlayNow);
				if(showLog)
					Debug.Log("DoubleJump");
				break;
			case Animations.taking_damage:
				animation.Play("taking_damage");
				if(showLog)
					Debug.Log("Damage");
				break;
			case Animations.dead:
				animation.Play("dead");
				if(showLog)
					Debug.Log("Dead");
				break;
			case Animations.left_punch:
				//animation.CrossFadeQueued("left_punch",0.1f,QueueMode.PlayNow);
				if(showLog)
					Debug.Log("LeftPunch");
				break;
			case Animations.right_punch:
				//animation.CrossFadeQueued("right_punch",0.1f,QueueMode.PlayNow);
				if(showLog)
					Debug.Log("RightPunch");
				break;
			case Animations.super_punch:
				//animation.CrossFadeQueued("super_punch",0.1f,QueueMode.PlayNow);
				if(showLog)
					Debug.Log("SuperPunch");
				break;
			case Animations.strong_punch:
				//animation.CrossFadeQueued("strong_punch",0.1f,QueueMode.PlayNow);
				if(showLog)
					Debug.Log("StrongPunch");
				break;
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	}
	public bool LeftPunchTest()
	{
		return animation.IsPlaying("left_punch");
	}
}
