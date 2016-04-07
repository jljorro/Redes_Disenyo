using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerStatus))]
public class CalculateVelocity : NetComponent
{
    public float _acel = 4;
    public float _drag = 1;
    public float _brakeAcel = 12;
    public float _maxVelocity = 10;
    private PlayerStatus _status;
    float _vInput = 0;
    float _hInput = 0;

    void SetVInput(float v)
    {
        _vInput = v;
    }

    void SetHInput(float h)
    {
        _hInput = h;
    }

    void FixedUpdate()
    {
        Vector3 a = CalculateAcel();
        Vector3 v = CalculateVel(a);
    }

    protected override void OnStart()
    {  
        _status = GetComponent<PlayerStatus>();
    }

    Vector3 CalculateAcel()
    {
        Vector3 acel;
        if (_vInput < 0)
            acel = _brakeAcel * transform.forward * _vInput;
        else if (_vInput > 0)
            acel = _acel * transform.forward * _vInput;
        else
            acel = -_drag * transform.forward;

        acel += transform.right * _hInput * _status.Velocity.magnitude;

        return acel;
    }

    Vector3 CalculateVel(Vector3 acel)
    {
        _status.Velocity = _status.Velocity + 
                                acel * Time.fixedDeltaTime;

        if (_status.Velocity.magnitude > _maxVelocity)
            _status.Velocity = _status.Velocity.normalized *
                                                    _maxVelocity;

        if (_status.Velocity != Vector3.zero &&
            Vector3.Angle(_status.Velocity,
                                        transform.forward) > 90)
            _status.Velocity = Vector3.zero;

        return _status.Velocity;
    }	
}
