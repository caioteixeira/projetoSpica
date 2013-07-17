using UnityEngine;
using System.Collections;

public class CharacterAttack : MonoBehaviour {
	
	
	public float punchSpeed = 0.5f;//Controla a velocidade da animação de soco
	public float punchHitTime = 0.1f;//Controla a velocidade de repetição
	public float punchTime = 0.2f;//Controla o tempo da animação
	
	public float superPunchSpeed = 1;
	public float superPunchHitTime = 0.2f;
	public float superPunchTime = 0.4f;
	
	public bool showLog = false;
	
	private bool isAttaking = false;//Se verdadeiro não roda a animação, impede que uma animação sobrepõnha a outra antes q termine
	
	private bool leftPunch = false;
	private bool rightPunch = true;

	private float atackDelay = 0.5f;
	
	private Controlador contr;
	
	void Start()
	{
		contr = GetComponent<Controlador>();
		animation["left_punch"].speed = punchSpeed;
		
		animation["right_punch"].speed = punchSpeed;
		
		animation["strong_punch"].speed = superPunchSpeed;
		
		animation.Stop();
	
	}
	void Update()
	{
		if(animation.IsPlaying("idle"))
		{
			atackDelay += Time.deltaTime;	
		}
		if(Input.GetButton("Fire1") && !isAttaking && !contr.IsMoving()){
			atackDelay = 0;
			SendMessage("DidPunch");
			isAttaking = true;
			if(showLog)
				Debug.Log("Input Punch");
		}
		else if(Input.GetButton("Fire3") && !isAttaking && !contr.IsMoving()){
			atackDelay = 0;
			SendMessage("DidStrongPunch");
			isAttaking = true;
			if(showLog)
				Debug.Log("Input soco forte");
		}
		else if(animation.IsPlaying("idle") && atackDelay > 0.5f)
		{
			SendMessage("resetaPunhos");
		}
		
	}
	
	IEnumerator DidStrongPunch()
	{
		animation.Play("strong_punch");
			if(showLog)
				Debug.Log("We are in strong punch");
			
		yield return new WaitForSeconds(superPunchHitTime);
				
		yield return new WaitForSeconds(superPunchTime - superPunchHitTime);
		isAttaking = false;
		
	}
	IEnumerator DidSuperPunch()
	{
		animation.Play("super_punch");
			if(showLog)
				Debug.Log("We are in Super Punch");
			
		yield return new WaitForSeconds(superPunchHitTime);
				
		yield return new WaitForSeconds(superPunchTime - superPunchHitTime);
		isAttaking = false;
	}
	
	IEnumerator DidPunch()
	{
		if(rightPunch){
			animation.Play("right_punch");
				if(showLog)
					Debug.Log("We are in right punch");

			yield return new WaitForSeconds(punchHitTime);
				
			yield return new WaitForSeconds(punchTime - punchHitTime);
			isAttaking = false;
			rightPunch = false;
			leftPunch = true;
			
			
		}else if (leftPunch){
			animation.Play("left_punch");
				if(showLog)
					Debug.Log("We are in left punch");
			yield return new WaitForSeconds(punchHitTime);
				
			yield return new WaitForSeconds(punchTime - punchHitTime);
			isAttaking = false;
			leftPunch = false;
			rightPunch = true;
		}
			
	}
	public bool IsAttaking(){
		return this.isAttaking;
	}
}