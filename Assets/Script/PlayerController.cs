using UnityEngine;

/// <summary>
/// Clase que controla las acciones del jugador y las
/// representa en el juego.
/// </summary>
public class PlayerController : MonoBehaviour {
    
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
}
