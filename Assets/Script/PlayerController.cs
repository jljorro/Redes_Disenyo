using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Clase que controla las acciones del jugador y las
/// representa en el juego.
/// </summary>
public class PlayerController : NetworkBehaviour {
    
    // Constantes del comportamiento del coche
    public float _acel = 4;
    public float _drag = 1;
    public float _brakeAcel = 12;
    public float _maxVelocity = 10;
    
    // Controlador
    private CharacterController _controller;
        
    // Velocidad del jugador
    Vector3 velocity;
        
    public Vector3 Velocity {
        get {return velocity;}
        set {velocity = value;}
    }
    
    // Registro de los input del jugador
    float _vInput = 0;
    float _hInput = 0;
    
    void Start (){
        if (isLocalPlayer) {
            _controller = GetComponent<CharacterController> ();
            
            CameraController cameraCtr = GameObject.Find ("Main Camera").GetComponent<CameraController> ();
            cameraCtr.AddTarget (gameObject);
        }
    }
    
    void Update () {
        if (isLocalPlayer) {
            // Capturar los input jugador
            float vInput = Input.GetAxisRaw ("Vertical");
            if (_vInput != vInput)
                _vInput = vInput;
                
            float hInput = Input.GetAxisRaw ("Horizontal");
            if (_hInput != hInput)
                _hInput = hInput;
            
            // Calcular los variables de acel y velocidad
            Vector3 acel = CalculateAceleration ();
            CalculateVelocity (acel);
            
            // Mover Jugador
            MovePlayer ();
        }
    }
    
    Vector3 CalculateAceleration () {
        Vector3 acel;
        
        if (_vInput > 0) {
            acel = transform.forward * _vInput * _acel;
        } else if (_vInput == 0) {
            acel = transform.forward * -_drag;
        } else {
            acel = transform.forward * _vInput * _brakeAcel;
        }
        
        acel += transform.right * _hInput * velocity.magnitude;
        
        return acel;
    }
    
    void CalculateVelocity (Vector3 acel) {
        velocity += acel* Time.fixedDeltaTime;
        
        if (velocity.magnitude > _maxVelocity) {
            velocity *= _maxVelocity / velocity.magnitude;
        } else if (velocity != Vector3.zero && Vector3.Angle (velocity, transform.forward)>90) {
            velocity = Vector3.zero;
        }
    }
    
    void MovePlayer () {
        Vector3 motion = velocity * Time.fixedDeltaTime;
        
        _controller.Move (motion);
        
        if (velocity != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation (velocity);
        }
    }
}
