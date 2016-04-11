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
    
}
