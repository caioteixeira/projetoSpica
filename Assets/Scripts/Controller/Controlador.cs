using UnityEngine;
using System.Collections;

public class ControladorMovimento
{
	//Velocidade ao andar
	public float walkSpeed = 3.0f;
	//Ao pressionar "Fire1", começamos a correr
	public float runSpeed = 10.0f;
	
	public float inAirControlAcceleration = 1.0f;
	
	//Gravidade
	public float gravity = 60.0f;
	public float maxFallSpeed = 20.0f;
	
	//Troca de velocidade
	public float speedSmothing = 5.0f;
	
	//Rotação
	public float rotationSmoothing = 5.0f;
	
	//A direção atual do movimento em x-y
	public Vector3 direction = Vector3.zero;
	
	//A velocidade vertical atual
	public float verticalSpeed = 0.0f;
	
	//A velocidade atual
	public float speed = 0.0f;
	
	//O jogador está se movendo?
	public bool isMoving = false;
	
	//A última colision flags retorna do controller.Move
	public CollisionFlags collisionFlags;
	
	public Vector3 velocity;
	
	public Vector3 inAirVelocity = Vector3.zero;
	
	//Tempo no ar
	public float hangTime = 0.0f;	
}

public class ControladorPulo
{
	//Posso pular?
	public bool enabled = true;
	
	//Altura do pulo
	public float height = 1.0f;
	
	public float doubleJumpHeight = 4.1f;
	
	//Com que velocidade posso pular outra vez?
	public float repeatTime = 0.05f;
	
	public float timeout = 0.15f;
	
	//Estamos pulando?
	public bool jumping = false;
	
	//Estamos no pulo duplo?
	public bool doubleJumping = false;
	
	//Posso fazer um pulo duplo?
	public bool canDoubleJump = false;
	
	public bool reachedApex = false;
	
	//última vez em que o botão "Jump" foi pressionado
	public float lastButtonTime = -10.0f;
	
	//último tempo em que o botão pulo foi pressionado
	public float lastTime = -1.0f;
	
	//A altura do pulo no extraJumpHeight
	public float lastStartHeight = 0.0f;
	
}
public class Controlador : MonoBehaviour 
{
	
	//Este script corresponde ao input?
	public bool canCantrol = true;
	
	public ControladorMovimento movement = new ControladorMovimento();
	public ControladorPulo jump = new ControladorPulo();
	
	//Movendo suporte da plataforma
	private Transform activePlatform;
	private Vector3 activeLocalPlatformPoint;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 lastPlatformVelocity;
	
	private CharacterController controller;
	
	void Awake () 
	{
		movement.direction = transform.TransformDirection(Vector3.forward);
		controller = GetComponent<CharacterController>();
	}
	
	void UpdateSmoothedMovementDirection()
	{
		float h = Input.GetAxisRaw("Horizontal");
		
		if(!canCantrol)
			h = 0.0f;
		
		movement.isMoving = Mathf.Abs(h) > 0.1f;
		
		if(movement.isMoving)
			movement.direction = new Vector3 (h,0,0);
		
		//Controles no chão
		if(controller.isGrounded)
		{
			//Suaviza a direção da velocidade atual
			float curSmooth = movement.speedSmothing * Time.deltaTime;
			
			float targetSpeed = Mathf.Min(Mathf.Abs(h),1.0f);
			
			//Pega o modificador de velocidade
			if(Input.GetButton("Fire2") && canCantrol)
			{
				targetSpeed *= movement.runSpeed;
			}
			else
			{
				targetSpeed *= movement.walkSpeed;
			}
			movement.speed = Mathf.Lerp(movement.speed,targetSpeed,curSmooth);
			
			movement.hangTime = 0.0f;
		}
		else
		{
			//Controles no ar
			movement.hangTime += Time.deltaTime;
			if(movement.isMoving)
				movement.inAirVelocity += new Vector3 (Mathf.Sign(h),0,0) * Time.deltaTime * movement.inAirControlAcceleration;
		}
		
	}
	void FixedUpdate()
	{
		transform.position = new Vector3(transform.position.x,transform.position.y,0);
	}
	
	void AplicaPulo()
	{
		//Previne de pulo rapido demais
		if(jump.lastTime + jump.repeatTime > Time.time)
			return;
		
		if(controller.isGrounded)
		{
			//Pulo
			if(jump.enabled && Time.time < jump.lastButtonTime + jump.timeout)
			{
				movement.verticalSpeed = CalculaVelocidadePulo(jump.height);
				movement.inAirVelocity = lastPlatformVelocity;
				Pulou();
				SendMessage("DidJump");
			}
		}
	}
	void AplicaGravidade()
	{	
		if(jump.jumping && !jump.reachedApex && movement.verticalSpeed <= 0.0f)
		{
			jump.reachedApex = true;
		}
		//Teste pulo duplo
		if((jump.jumping && Input.GetButtonUp("Jump") && !jump.doubleJumping)
			|| (!controller.isGrounded && !jump.jumping && !jump.doubleJumping && movement.verticalSpeed < -12.0f))
		{
			jump.canDoubleJump = true;
		}
		//pulo duplo
		if(jump.canDoubleJump && Input.GetButtonDown("Jump") && !IsTouchingCeiling())
		{
			jump.doubleJumping = true;
			movement.verticalSpeed = CalculaVelocidadePulo(jump.height);
			jump.canDoubleJump =false;
			SendMessage("DidDoubleJump");
		}
		
		if(controller.isGrounded)
		{
			movement.verticalSpeed = -movement.gravity * Time.deltaTime;
			jump.canDoubleJump = false;
		}
		else
			movement.verticalSpeed -= movement.gravity * Time.deltaTime;
		
		//Controla velocidade de descida
		movement.verticalSpeed = Mathf.Max(movement.verticalSpeed,-movement.maxFallSpeed);	
	}
	float CalculaVelocidadePulo(float targetJumpHeight)
	{
		return Mathf.Sqrt(2*targetJumpHeight*movement.gravity);
	}
	void Pulou()
	{
		jump.jumping = true;
		jump.reachedApex = false;
		jump.lastTime = Time.time;
		jump.lastStartHeight = transform.position.y;
		jump.lastButtonTime = -10;
	}
	
	void Update () 
	{
		if(Input.GetButtonDown("Jump") && canCantrol)
		{
			jump.lastButtonTime = Time.time;
		}
		UpdateSmoothedMovementDirection();
		//Aplica gravidade
		AplicaGravidade();
		//Aplica lógica do pulo
		AplicaPulo();
		
		//Movimento sobre a plataforma
		if(activePlatform != null)
		{
			Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
			Vector3 moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
			transform.position = transform.position + moveDistance;
			lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint)/Time.deltaTime;
		}else
		{
			lastPlatformVelocity = Vector3.zero;
		}
		activePlatform = null;
		
		//Salva última posição do jogador
		Vector3 lastPosition = transform.position;
		
		//Calcula movimento atual
		Vector3 currentMovementOffset = movement.direction*movement.speed+ new Vector3(0,movement.verticalSpeed,0)+movement.inAirVelocity;
		
		currentMovementOffset *= Time.deltaTime;
		
		//Move o jogador
		movement.collisionFlags = controller.Move(currentMovementOffset);
		
		//Calcula velocidade baseado na posição anterior
		movement.velocity = (transform.position - lastPosition)/Time.deltaTime;
		
		if(activePlatform != null)
		{
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);
		}
		//Rotação
		if(movement.direction.sqrMagnitude > 0.01f)
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movement.direction),Time.deltaTime * movement.rotationSmoothing);
		
		if(controller.isGrounded)
		{
			movement.inAirVelocity = Vector3.zero;
			if(jump.jumping)
			{
				jump.jumping = false;
				jump.doubleJumping = false;
				jump.canDoubleJump = false;
				
				Vector3 jumpMoveDirection = movement.direction * movement.speed + movement.inAirVelocity;
				if(jumpMoveDirection.sqrMagnitude > 0.01f)
					movement.direction = jumpMoveDirection.normalized;
			}
		}
	}
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(hit.moveDirection.y > 0.01f)
			return;
		
		if(hit.moveDirection.y < -0.9 && hit.normal.y > 0.9)
		{
			activePlatform = hit.collider.transform;
		}
	}
	bool IsTouchingCeiling()
	{
		return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
	}
	public void SetControllable(bool controllable)
	{
		canCantrol = controllable;
	}
	public bool IsMoving(){
		return movement.isMoving;
	}
}


