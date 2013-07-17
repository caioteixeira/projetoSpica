using UnityEngine;
using System.Collections;

public class Animacao : MonoBehaviour {

	//Ajusta a velocidade da animação "walk"
	public float modificadorVelAndar = 2.5f;
	//Ajusta a velocidade da animação "run"
	public float modificadorVelCorrer= 1.5f;
	//Ajusta a velocidade da animação "jump"
	public float modificadorVelPulo = 2.0f;
	
	private Controlador controlador;
	
	void Start () {
		animation.Stop();
		
		//Loop "default" para todas as animações
		animation.wrapMode = WrapMode.Loop;
		
		//Animação "jump" sobrepõe todas as outras, pode acontecer qdo o personagem está parado, andando ou correndo
		int layerPulo = 1;
		AnimationState pulo = animation["idle_jump"];
		pulo.layer = layerPulo;
		pulo.speed *= modificadorVelPulo;
		pulo.wrapMode = WrapMode.Once;
		
	
		AnimationState andando =  animation["walk"];
		andando.speed *= modificadorVelAndar;
		
		AnimationState correndo = animation["run"];
		correndo.speed *= modificadorVelCorrer;
		
		AnimationState takingDamage = animation["taking_damage"];
		takingDamage.layer = 10;
		takingDamage.wrapMode = WrapMode.Once;
		
	}
	void Update () {
		
		controlador = GetComponent<Controlador>();
		
		if(controlador.IsMoving()){
			if (Input.GetButton ("Fire2"))
				animation.CrossFade ("run");
			else
				animation.CrossFade ("walk");
		}
		// Se não está se movendo, volta a ficar parado
		else
			animation.CrossFade ("idle", 0.5f);
	}
	
	void DidJump(){
		animation.Play("idle_jump");
	}
	void DidDoubleJump(){
		animation.Play("idle_jump");
	}
	void TakingDamage(){
		animation.Play("taking_damage");
	}
	void DieState(){
		Destroy(GetComponent<Controlador>());
		Destroy(GetComponent<Animacao>());
		Destroy(GetComponent<CharacterAttack>());
		Destroy(GetComponent<CharacterController>());
		Destroy(GetComponent<ControladorPunhos>());
		Destroy(GetComponent<Player>());
		gameObject.AddComponent<ControladorDie>();
	}
	
	
}

