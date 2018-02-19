using UnityEngine;

// This class corresponds to the 3rd person camera features.
public class ThirdPersonOrbitCamBasic : MonoBehaviour 
{
	public Transform player;                                           // Player's reference. //El Player.
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);       // Offset to repoint the camera. //Rotación de la cámara 
	public Vector3 camOffset   = new Vector3(0.4f, 0.5f, -2.0f);       // Offset to relocate the camera related to the player position. //Posicion de la cámara .
	public float smooth = 10f;                                         // Speed of camera responsiveness. //Velocidad de la cámara
	public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed //Velocidad de giro de la cámara de lado a lado.
	public float verticalAimingSpeed = 6f;                             // Vertical turn speed. //velocida de giro de la cámara de arriba a abajo
	public float maxVerticalAngle = 30f;                               // Camera max clamp angle.  //Maxima distancia que la cámara puede llegar hacia abajo.
	public float minVerticalAngle = -60f;                              // Camera min clamp angle. //Máxima distábncia que la cámara puede llegar hacia arriba.
	public string XAxis = "Analog X";                                  // The default horizontal axis input name. 
	public string YAxis = "Analog Y";                                  // The default vertical axis input name.

	private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement. //Guardamos en este float el ángulo movimiento del raton
	private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement. //guardamos en este float el angulo del movimiento del ratón
	private Transform cam;                                             // This transform. //La camara
	private Vector3 relCameraPos;                                      // Current camera position relative to the player.
	private float relCameraPosMag;                                     // Current camera distance to the player. //Distancia con el player.
	private Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation. //Velocida de rotación de la cámara 
	private Vector3 smoothCamOffset;                                   // Camera current offset on interpolation. //Velocidad de recolocación/popsicion de la cámara
	private Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate. //El pivote del target que queremos.(Player)
	private Vector3 targetCamOffset;                                   // Camera offset target to interpolate. //La posicion del target que queremos. (Player)
	private float defaultFOV;                                          // Default camera Field of View.  //Lo que se ve
	private float targetFOV;                                           // Target camera Field of View. //El target que queremos
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.  //Máximo giro vertical hacia abajo del target.

    // Get the camera horizontal angle.
    public static ThirdPersonOrbitCamBasic instance;
	public float GetH { get { return angleH; } }

	void Awake()
	{
        instance = this;
		// Reference to the camera transform.
        //La cámara es este transform.(Que es la cámara xd)
		cam = transform;

		// Set camera default position.
        //la posicion de la cámara es igual a la posición del player(player.position) con una rotación(quat.iden*pivotOffset) más la posición máxima.
		cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        //La rotación es un quaternion.
		cam.rotation = Quaternion.identity;

		// Get camera position relative to the player, used for collision test.
        //Esta variable sirve para saber la distancia entre player y cámara, para que nunca se colapsen.
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;

		// Set up references and default values.
		smoothPivotOffset = pivotOffset; //Rapidez del giro
		smoothCamOffset = camOffset; //Rapidez de la posicion
		defaultFOV = cam.GetComponent<Camera>().fieldOfView;  //Lo que la cámara ve.
		angleH = player.eulerAngles.y; //El float es eel ángulo Y del palyer.

		ResetTargetOffsets ();
		ResetFOV (); //
		ResetMaxVerticalAngle();
	}

	void Update()
	{
		// Get mouse movement to orbit the camera.
		// Mouse:
		angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;//posicion vertical de la cámara, el movimiento del ratón para calcular la posición multiplicado por la rapidez.
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed; //posicion vertical de la cámara, el movimiento del ratón para calcular la posición multiplicado por la rapidez.
		// Joystick:
		//angleH += Mathf.Clamp(Input.GetAxis(XAxis), -1, 1) * 60 * horizontalAimingSpeed * Time.deltaTime;
		//angleV += Mathf.Clamp(Input.GetAxis(YAxis), -1, 1) * 60 * verticalAimingSpeed * Time.deltaTime;

		// Set vertical movement limit.
		angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);
        //Le damos mínimo angulo y el máximo angulo que queremos que gire AngleV //giro hacia arriba //giro hacia abajo
		// Set camera orientation.
        //Rotacion de la cámara en Y, hacemos euler con el angulo sacado del movimiento horizontal.
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        //rotacion de la cmara en Y y X.
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
        //rotacion de la camara es igual al quaternion que hemos conseguido.
		cam.rotation = aimRotation;

		// Set FOV.
        //le damos a la cámara lo que queremos que se vea. //un lerp entre la visión, y nuestro target, y el tiempo que queremos que se vea. todo el rato
		cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera>().fieldOfView, targetFOV,  Time.deltaTime);

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset; //30
        //Colision entre camara y player.
		Vector3 noCollisionOffset = targetCamOffset;
		                                //distancia entre player y camara

		// Repostition the camera.
        //Rotacion de la camara = Lerp, con la rotacion, la rotacion entre el target y la velocidad.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
        //Posicion de la camara
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);
        //posicion de la camara, en la posicion del player con la rotacion de la camara y la velocida del giro, con la rotacion entera y la velocidad de la posicion.
		cam.position =  player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
	}

	// Set camera offsets to custom values.
    //le damos a la camara el nuevo pivote y el offset que queremos.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
	}

	// Reset camera offsets to default values.
    //reseteamos la posicion y el pivote de la camara al normal.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = pivotOffset;
		targetCamOffset = camOffset;
	}

	// Reset the camera vertical offset. //reseteamos el offset de la camara en Y(posicion)
	public void ResetYCamOffset()
	{
		targetCamOffset.y = camOffset.y;
	}

    // Set camera vertical offset. //damos nuevo offset de la camara en Y(posicion)
    public void SetYCamOffset(float y)
	{
		targetCamOffset.y = y;
	}

    // Set camera horizontal offset. //damos el offset de la camara en X(posicion)
    public void SetXCamOffset(float x)
	{
		targetCamOffset.x = x;
	}

	// Set custom Field of View. //Damos nueva vision a la camara.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value. //Reseteamos la vision.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle. //Damos neuvo angulo de rotacion.
	public void SetMaxVerticalAngle(float angle)
	{
		this.targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value. //Reseteamos el angulo de rotacion.
	public void ResetMaxVerticalAngle()
	{
		this.targetMaxVerticalAngle = maxVerticalAngle;
	}

	// Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
	

	// Check for collision from camera to player. //MIRAMOS SI HAY COLISION DE LA CAMARA AL PLAYER.
	bool ViewingPosCheck (Vector3 checkPos, float deltaPlayerHeight) //
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something... //USAMOS UN RAYCAST QUE MARCA LA POSICION DEL PLAYER Y POSICION DE LA CAMARA.
		if(Physics.Raycast(checkPos, player.position+(Vector3.up* deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
		{
			// ... if it is not the player... //SI NO SE ESTAN TOCANDO, PUES NO HAY COLISION.
			if(hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				// This position isn't appropriate.
				return false;
			}
		}
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		return true;
	}

	// Check for collision from player to camera.


	// Get camera magnitude.
	public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}
