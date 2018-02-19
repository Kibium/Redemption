using UnityEngine;
using System.Collections.Generic;

// This class manages which player behaviour is active or overriding, and call its local functions.
// Contains basic setup and common functions used by all the player behaviours.
public class BasicBehaviour : MonoBehaviour
{
	public Transform playerCamera;                        // Reference to the camera that focus the player. //El transform de la cámara del palyer
	public float turnSmoothing = 0.06f;                   // Speed of turn when moving to match camera facing.  //El giro del player frente a la camara
	public float sprintFOV = 70f;                        // the FOV to use on the camera when player is sprinting. //LO QUE SE VE CUANDO ESTÁ ESPRITANDO
	public string sprintButton = "Sprint";                // Default sprint button input name. //El boton para esprintar

	private float h;                                      // Horizontal Axis. //El axis horizontal
	private float v;                                      // Vertical Axis. //El axis vertical
	private int currentBehaviour;                         // Reference to the current player behaviour.  //La referencia al  player
	private int defaultBehaviour;                         // The default behaviour of the player when any other is not active. //La referencia por defecto
	private int behaviourLocked;                          // Reference to temporary locked behaviour that forbids override. //Refrencia temporal al behavior.
	private Vector3 lastDirection;                        // Last direction the player was moving. //La ultima direccion del player donde se ha movido.
	private Animator anim;                                // Reference to the Animator component. //Referencia a la animacion
	private ThirdPersonOrbitCamBasic camScript;           // Reference to the third person camera script. //Referencia a la cámara
	private bool sprint;                                  // Boolean to determine whether or not the player activated the sprint mode. //Boleano para saber si está esprintando o no
	private bool changedFOV;                              // Boolean to store when the sprint action has changed de camera FOV. //Booleano para saber si ha cambiado de vision o no.
	private int hFloat;                                   // Animator variable related to Horizontal Axis. //Variable asociado a la animacion en horizontal
	private int vFloat;                                   // Animator variable related to Vertical Axis. //Variable asociada a la animacion en vertical
	private List<GenericBehaviour> behaviours;            // The list containing all the enabled player behaviours. //Lista de GenericBehavior(La clase se crea mas abajo), COMPORTAMIENTOS.
	private List<GenericBehaviour> overridingBehaviours;  // List of current overriding behaviours. ////Lista de GenericBehavior(La clase se crea mas abajo) COMPORTAMIENTOS PRIMORDIALES.
    private Rigidbody rBody;                              // Reference to the player's rigidbody. //El rigidbody del player
	private int groundedBool;                             // Animator variable related to whether or not the player is on the ground. //para saber si esta en el suelo
	private Vector3 colExtents;                           // Collider extents for ground test. 
  
	// Get current horizontal and vertical axes.
	public float GetH { get { return h; } } //Cogemos el horizontal
	public float GetV { get { return v; } } //Cogemos el vertical

	// Get the player camera script.
	public ThirdPersonOrbitCamBasic GetCamScript { get { return camScript; } } //Retornamos la cmara del player

	// Get the player's rigid body.
	public Rigidbody GetRigidBody { get { return rBody; } } //Retorna el rigidbody del player

	// Get the player's animator controller.
	public Animator GetAnim { get { return anim; } } //retorna la animacion del player

	// Get current default behaviour.
	public int GetDefaultBehaviour {  get { return defaultBehaviour; } } //Retorna el COMPORTAMIENTO por defecto.

    void Awake ()
	{
     
        // Set up the references.
        behaviours = new List<GenericBehaviour> (); //Nueva lista.
		overridingBehaviours = new List<GenericBehaviour>();
		anim = GetComponent<Animator> (); //cogemos el naimator que existe.
		hFloat = Animator.StringToHash("H"); //Genera un parametro con la letra H(Horizontal)
		vFloat = Animator.StringToHash("V"); //Genera un parametro con la letra V(Vertical)
        camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic> (); //Cogemos la cámara.
		rBody = GetComponent<Rigidbody> (); //Cogemos el rigidbody del player.

		// Grounded verification variables.
		groundedBool = Animator.StringToHash("Grounded"); //si sta en el suelo
		colExtents = GetComponent<Collider>().bounds.extents;
    }
   
	void Update()
    {
       
        // Store the input axes.
        h = Input.GetAxis("Horizontal"); //H es AD
		v = Input.GetAxis("Vertical"); //V es WS

		// Set the input axes on the Animator Controller.
		anim.SetFloat(hFloat, h, 0.1f, Time.deltaTime); //hfloat es El parametro, h es la tecla, el float la velocidad, y el timedeltatime.
		anim.SetFloat(vFloat, v, 0.1f, Time.deltaTime); //hfloat es El parametro, h es la tecla, el float la velocidad, y el timedeltatime.

        // Toggle sprint by input.
        sprint = Input.GetButton (sprintButton); //Sprint es igual al botón de sprint.(shift)

		// Set the correct camera FOV for sprint mode.
		if(IsSprinting()) //Si esta corriendo
		{
			changedFOV = true; //HA CAMBIADO DE VISION
            //le damos otro punto de vista
			camScript.SetFOV(sprintFOV); //Le damos otro punto d ivision.
		}
		else if(changedFOV) //Si ha cambiado.
		{
			camScript.ResetFOV(); //Reseteamos el punto de vision.
			changedFOV = false; //No cambia la vision.
		}
		// Set the grounded test on the Animator Controller.
		anim.SetBool(groundedBool, IsGrounded());  //Damos un setbool.
	}

	// Call the FixedUpdate functions of the active or overriding behaviours.
	void FixedUpdate()
	{
		// Call the active behaviour if no other is overriding.
        //ACTIVA UN COMPORTAMIENTO SI NO HAY NINGUNO MAS IMPORTAMENTE.
		bool isAnyBehaviourActive = false; //si ningun comportamiento esta activo
		if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
		{
			foreach (GenericBehaviour behaviour in behaviours) //por cada comportamiento en comportamientos.
			{
				if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
				{
					isAnyBehaviourActive = true; //Activa un comportamiento.
					behaviour.LocalFixedUpdate();
				}
			}
		}
		// Call the overriding behaviours if any.
		else
		{
			foreach (GenericBehaviour behaviour in overridingBehaviours) //
			{
				behaviour.LocalFixedUpdate();
			}
		}

		// Ensure the player will stand on ground if no behaviour is active or overriding.
		if (!isAnyBehaviourActive && overridingBehaviours.Count == 0) //si no se esta haciendo ningun comportamiento
		{
			rBody.useGravity = true;
			Repositioning (); //se reposiciona el player
		}
	}

	// Call the LateUpdate functions of the active or overriding behaviours.
	private void LateUpdate()
	{
		// Call the active behaviour if no other is overriding.
		if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
		{
			foreach (GenericBehaviour behaviour in behaviours)
			{
				if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
				{
					behaviour.LocalLateUpdate();
				}
			}
		}
		// Call the overriding behaviours if any.
		else
		{
			foreach (GenericBehaviour behaviour in overridingBehaviours)
			{
				behaviour.LocalLateUpdate();
			}
		}

	}

	// Put a new behaviour on the behaviours watch list.
	public void SubscribeBehaviour(GenericBehaviour behaviour)
	{
		behaviours.Add (behaviour); //SE AÑADE UN NUEVO COMPORTAMIENTO (MOVERSE O VOLAR)
	}

	// Set the default player behaviour.
	public void RegisterDefaultBehaviour(int behaviourCode) //SE REGISTRA EL COMPORTAMIENTO POR DEFECTO.
	{
		defaultBehaviour = behaviourCode;
		currentBehaviour = behaviourCode;
	}

	// Attempt to set a custom behaviour as the active one.
	// Always changes from default behaviour to the passed one.
	public void RegisterBehaviour(int behaviourCode) //damos un comportamiento al actual comportamiento.
	{
		if (currentBehaviour == defaultBehaviour) //si el comportamiento es igual al que hay por defecto
		{
			currentBehaviour = behaviourCode; //EL CURRENT ES EL QUE LE PASAMOS.
		}
	}

	// Attempt to deactivate a player behaviour and return to the default one.
	public void UnregisterBehaviour(int behaviourCode) //le damos el comportamiento por defecto al actual comportamiento.
	{
		if (currentBehaviour == behaviourCode) //si el comportamiento es igual al que hay actuando
		{
			currentBehaviour = defaultBehaviour; //el comportamiento es igual al que hay por defecto.
		}
	}

	// Attempt to override any active behaviour with the behaviours on queue.
	// Use to change to one or more behaviours that must overlap the active one (ex.: aim behaviour).
    //Esto nos sirve para sobreescribir cualquier comportamiento activo con los que estane n la cola. Lo usamos para cambiar comportamientos.
	public bool OverrideWithBehaviour(GenericBehaviour behaviour) 
	{
		// Behaviour is not on queue.
		if (!overridingBehaviours.Contains(behaviour)) //Si el comportamiento no esta en la cola
		{
			// No behaviour is currently being overridden.
			if (overridingBehaviours.Count == 0) //Y no hay ninguno en la cola, si los comporta´mientos estan vacios
			{
				// Call OnOverride function of the active behaviour before overrides it.
				foreach (GenericBehaviour overriddenBehaviour in behaviours) //por cada comportamiento.
				{
					if (overriddenBehaviour.isActiveAndEnabled && currentBehaviour == overriddenBehaviour.GetBehaviourCode())
					{
						overriddenBehaviour.OnOverride();
						break;
					}
				}
			}
			// Add overriding behaviour to the queue.
			overridingBehaviours.Add(behaviour); //añadimos el comportamiento a la cola
			return true;//retornamos falso
		}
		return false; //si el comportamiento esta en la cola, retornamos falso.
	}

	// Attempt to revoke the overriding behaviour and return to the active one.
	// Called when exiting the overriding behaviour (ex.: stopped aiming).
    //Esto no sirve para quitar cualquier comportamiento.
	public bool RevokeOverridingBehaviour(GenericBehaviour behaviour)
	{
		if (overridingBehaviours.Contains(behaviour)) //si existe en la cola de comportamientos
		{
			overridingBehaviours.Remove(behaviour); //borramos el comportamiento.
			return true;
		}
		return false;
	}

	// Check if any or a specific behaviour is currently overriding the active one.
    //Miramos si el comportamiento anula el que está haciendose actualment
	public bool IsOverriding(GenericBehaviour behaviour = null) //Bool esPrimordial?
	{
		if (behaviour == null) //si no hay ningun comportamiento 
			return overridingBehaviours.Count > 0; //retornamos que los primordiales
		return overridingBehaviours.Contains(behaviour); //sino, retornamos si está o no.
	}

	// Check if the active behaviour is the passed one.
	public bool IsCurrentBehaviour(int behaviourCode) //miramos si el comportamiento activo es el que acaba de hacerse
	{
		return this.currentBehaviour == behaviourCode;
	}

	// Check if any other behaviour is temporary locked.
	public bool GetTempLockStatus(int behaviourCodeIgnoreSelf = 0) //miramos si algun otro comportaient esta bloqueado.
	{
		return (behaviourLocked != 0 && behaviourLocked != behaviourCodeIgnoreSelf);
	}

	// Atempt to lock on a specific behaviour.
	//  No other behaviour can overrhide during the temporary lock.
	// Use for temporary transitions like jumping, entering/exiting aiming mode, etc.
	public void LockTempBehaviour(int behaviourCode) //bloquea un comportamiento mientras se est haciendo el comportamiento. Ningun otro se podrá hacer
	{
		if (behaviourLocked == 0)
		{
			behaviourLocked = behaviourCode;
		}
	}

	// Attempt to unlock the current locked behaviour.
	// Use after a temporary transition ends.
	public void UnlockTempBehaviour(int behaviourCode) //Desbloquea un comportamiento para que pueda hacer otro comportamiento.
	{
		if(behaviourLocked == behaviourCode)
		{
			behaviourLocked = 0;
		}
	}

	// Common functions to any behaviour:

	// Check if player is sprinting.
	public virtual bool IsSprinting() //Mira si el player está corriendo.
	{
		return sprint && IsMoving() && CanSprint(); //Si está pulsando al sprint, y se está moviendo y puede correr. Retorna si está corriendo.
	}

	// Check if player can sprint (all behaviours must allow).
	public bool CanSprint() //Mira si puede correr.
	{
		foreach (GenericBehaviour behaviour in behaviours)
		{
			if (!behaviour.AllowSprint ())
				return false;
		}
		foreach(GenericBehaviour behaviour in overridingBehaviours)
		{
			if (!behaviour.AllowSprint())
				return false;
		}
		return true;
	}

	// Check if the player is moving on the horizontal plane.
	public bool IsHorizontalMoving() //Mira si se esta moviendo en horizontal
	{
		return h != 0;
	}

	// Check if the player is moving.
	public bool IsMoving() //mira si esta moviendose 
	{
		return (h != 0)|| (v != 0);
	}

	// Get the last player direction of facing.
	public Vector3 GetLastDirection() //reotrna la ultima direccion a la que esta mirando
	{
		return lastDirection;
	}

	// Set the last player direction of facing.
	public void SetLastDirection(Vector3 direction) //damos la ultima direccion que esta mirando
	{
		lastDirection = direction;
	}

	// Put the player on a standing up position based on last direction faced. //REPOSICIONAMOS EL PLAYER EN LA DIRECCION QUE ESTA MIRANDO
	public void Repositioning() //
	{
		if(lastDirection != Vector3.zero) //Si la ultima direccion no es 0
		{ 
			lastDirection.y = 0; //LA Y ES 0
			Quaternion targetRotation = Quaternion.LookRotation (lastDirection); //la rotacion es igual a la ultima direccion que hemos cogido
			Quaternion newRotation = Quaternion.Slerp(rBody.rotation, targetRotation, turnSmoothing); 
                                                        //rotacion actual   //rotacion donde queremos rotar //tiempo que tarda
			rBody.MoveRotation (newRotation); //rotamos el personaje.
		}
	}

	// Function to tell whether or not the player is on ground.
	public bool IsGrounded()  //Miramos si esta al suelo o no.
	{
		Ray ray = new Ray(this.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
                            //la posicion del transform    , el vector del suelo
		return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f); //reotnra si esta en el suel o ono.
	}
}

// This is the base class for all player behaviours, any custom behaviour must inherit from this.
// Contains references to local components that may differ according to the behaviour itself.
public abstract class GenericBehaviour : MonoBehaviour //NOS CREAMOS UN COMPORTAMIENTO GENERICO!!!!!!!!!!! GENERA COMPORTAMIENTOS!!!
{
	//protected Animator anim;                       // Reference to the Animator component.
	protected int speedFloat;                      // Speed parameter on the Animator. //Es el parámetro que le damos, la velocidad deanimacion
	protected BasicBehaviour behaviourManager;     // Reference to the basic behaviour manager. //referencia a este script
	protected int behaviourCode;                   // The code that identifies a behaviour. IDENTIFICA UN COMPORTAMIENTO, EL CODIGO QUE LO IDENTIFICA!
	protected bool canSprint;                      // Boolean to store if the behaviour allows the player to sprint. //booleano para saber si puede correr.


	void Awake()
	{
		// Set up the references.
		behaviourManager = GetComponent<BasicBehaviour> ();
		speedFloat = Animator.StringToHash("Speed"); //el speedFloat es un int con valor de un string.
		canSprint = true; //SI Puede correr

		// Set the behaviour code based on the inheriting class.
		behaviourCode = this.GetType().GetHashCode(); //este codigo.
	}

	// Protected, virtual functions can be overridden by inheriting classes.
	// The active behaviour will control the player actions with these functions:
	
	// The local equivalent for MonoBehaviour's FixedUpdate function.
	public virtual void LocalFixedUpdate() { }
	// The local equivalent for MonoBehaviour's LateUpdate function.
	public virtual void LocalLateUpdate() { }
	// This function is called when another behaviour overrides the current one.
	public virtual void OnOverride() { }

	// Get the behaviour code.
	public int GetBehaviourCode()
	{
		return behaviourCode; //retornamos este codigo
	}

	// Check if the behaviour allows sprinting.
	public bool AllowSprint() //si puede correr pues retorna que si xD.
	{
		return canSprint;
	}

    
}
