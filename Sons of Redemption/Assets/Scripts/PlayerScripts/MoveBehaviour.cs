using UnityEngine;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;                 // Default walk speed. //LA VELOCIDA DE CAMINAR
	public float runSpeed = 1.0f;                   // Default run speed. //LA VELOCIDA DE CORRER
	public float sprintSpeed = 2.0f;                // Default sprint speed. //LA VELOCIDAD DE ESPRINTAR
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed. //LA VELOCIDAD DE CAMBIAR ANIMACIONES
	public string jumpButton = "Jump";              // Default jump button. //EL BOTON DE SALTO
	public float jumpHeight = 1.5f;                 // Default jump height. //CUANTO SALTA XD
	public float jumpIntertialForce = 10f;          // Default horizontal inertial force when jumping.

	private float speed, speedSeeker;               // Moving speed. //LA VELOCIDAD WTF
	private int jumpBool;                           // Animator variable related to jumping. //ANIMACION ASOCIADA AL SALTO.
	private int groundedBool;                       // Animator variable related to whether or not the player is on ground. //ANIMACION ASOCIADO SI ESTA AL SUELO
	private bool jump;                              // Boolean to determine whether or not the player started a jump. //SI HA SALTADO
	private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle. //SI ESTA COLISIONANDO AMIGO

	// Start is always called after any Awake functions.
	void Start() 
	{
		// Set up the references.
		jumpBool = Animator.StringToHash("Jump"); //Le damos un valor string a un int. El string s un parámetro que nos servirá, 
		groundedBool = Animator.StringToHash("Grounded");//Le damos un valor string a un int.
        behaviourManager.GetAnim.SetBool (groundedBool, true);
        
		// Subscribe and register this behaviour as the default behaviour.
		behaviourManager.SubscribeBehaviour (this); //Añadimos este comportamiento.
		behaviourManager.RegisterDefaultBehaviour (this.behaviourCode);  //Es generico, añadimos este comportamiento como default.
		speedSeeker = runSpeed; //Damos valor al speed.
	}

	// Update is used to set features regardless the active behaviour.
	void Update ()
	{
		// Get jump input.
        //Si no está saltando, y apretamos al salto, y 
		if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Call the basic movement manager.
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV); //Llamamos a la funcion de moverse.(Funcion aqui, parametros en BasicBehavior)

		// Call the jump manager.
		//JumpManagement(); //Llamamos a la funcion de saltar(no tendrá)
	}

    // Execute the idle and walk/run jump movements.
    /*void JumpManagement()
	{
		// Start a new jump.
		if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
		{
			// Set jump related parameters.
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(jumpBool, true);
			// Is a locomotion jump?
			if(behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
			{
				// Temporarily change player friction to pass through obstacles.
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0f;
				// Set jump vertical impulse velocity.
				float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
				velocity = Mathf.Sqrt(velocity);
				behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
			}
		}
		// Is already jumping?
		else if (behaviourManager.GetAnim.GetBool(jumpBool))
		{
			// Keep forward movement while in the air.
			if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
			{
				behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
			}
			// Has landed?
			if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
			{
				behaviourManager.GetAnim.SetBool(groundedBool, true);
				// Change back player friction to default.
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
				// Set jump related parameters.
				jump = false;
				behaviourManager.GetAnim.SetBool(jumpBool, false);
				behaviourManager.UnlockTempBehaviour(this.behaviourCode);
			}
		}
	}*/

	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical) //MOVIMIENTO
	{
		// On ground, obey gravity. 
		if (behaviourManager.IsGrounded()) //SI ESTA EN EL SUELO(FUNCION EN BASICBHAVIOR)
            
			behaviourManager.GetRigidBody.useGravity = true; //USAMOS GRAVEDAD
            
		// Call function that deals with player orientation.
		Rotating(horizontal, vertical); //ROTAMOS!!!!!!! (FUNCION AQUI)

		// Set proper speed.
        //ESTO NOS DA LA VELOCIDAD DEL JUGADOR y su respectiva animacon
		Vector2 dir = new Vector2(horizontal, vertical); //direccion horizontal y vertical.
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude; //sped = vector direccion y la cantidad de speed que queremos.
		// This is for PC only, gamepads control speed via analog stick.
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting()) //si sta corriendo
		{
			speed = sprintSpeed; //cambiamo la velosida
		}

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime); //Le damos la velocidad y la animacion.
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical) //rotamos el player sgun donde este mirando la camara
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);//Forward(hacia delante)

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized; //noramlized hace que por defecto sea esta la direccion que quremos mirar.

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x); //Vector3 right es un vector que señala hacia los lados.
		Vector3 targetDirection;//direccion del target
		targetDirection = forward * vertical + right * horizontal; //hacia delante * vertical + hacia el lado * horizontal

		// Lerp current direction to calculated target direction.
		if((behaviourManager.IsMoving() && targetDirection != Vector3.zero)) //Si el player está moviendose y la direccion no es 0.
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection); //target rotation es igual a la direccion que estemos mirando

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
            //La nueva rotacion es                 //slerp  //rotacion desde                    //dond queremos rotar   //tiempo que tarda
			behaviourManager.GetRigidBody.MoveRotation (newRotation); //damos la rotacion
			behaviourManager.SetLastDirection(targetDirection); //la last direction es la targetdirection actual.
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if(!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning(); //Reposicionamos l camara
		}

		return targetDirection; //retornamos la targetdirection.
	}

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		isColliding = true;
	}
	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
	}
}
