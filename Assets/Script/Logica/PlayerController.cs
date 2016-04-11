using UnityEngine;
using System.Collections;

enum movementAcel { gas, brake, drag }
enum TurnAcel { left, right, drag }

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : NetComponent {
    public float _acel = 4;
    public float _drag = 1;
    public float _brakeAcel = 12;
    public float _maxVelocity = 10;
    
    private CharacterController _controller;
    private PlayerStatus _status;
    
    float _vInput = 0;
    float _hInput = 0;
    
    protected override void OnStart () {
        _controller = GetComponent<CharacterController> ();
        _status = GetComponent<PlayerStatus> ();
    }
    
    void FixedUpdate () {
        Vector3 acel = calculateAceleration ();
        calculateVelocity (acel);
        movePlayer ();
    }
    
    /// <summary>
    /// Método que calcula la aceleración a partir del Input del usuario.
    /// </summary>
    /// <returns>Devuelve la velocidad nueva.</returns>
    Vector3 calculateAceleration () {
        Vector3 acel;
        
        if (_vInput > 0) 
            acel = transform.forward * _vInput * _acel;
        else if (_vInput == 0)
            acel = transform.forward * -_drag;
        else
            acel = transform.forward * _vInput * _brakeAcel;
            
        acel += transform.right * _hInput * _status.Velocity.magnitude; 
        
        return acel;
    }
    
    void calculateVelocity (Vector3 acel) {
        _status.Velocity += acel * Time.fixedDeltaTime;
        
        if (_status.Velocity.magnitude > _maxVelocity) {
            _status.Velocity *= _maxVelocity / _status.Velocity.magnitude;
        } else if (_status.Velocity != Vector3.zero && Vector3.Angle (_status.Velocity, transform.forward) > 90) {
            _status.Velocity = Vector3.zero;
        }
    }
    
    void movePlayer () {
        Vector3 motion = _status.Velocity * Time.fixedDeltaTime;
        
        _controller.Move (motion);
        
        if (_status.Velocity != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation (_status.Velocity);
        }
    }
    
    void SetVInput (float vInput) {
        _vInput = vInput;
    }
    
    void SetHInput (float hInput) {
        _hInput = hInput;
    }
}
