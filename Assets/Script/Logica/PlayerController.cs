using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : NetComponent
{
    private PlayerStatus _status;
    private CharacterController _controller;

    void FixedUpdate()
    {
        MovePlayer(_status.Velocity);
    }

    protected override void OnStart()
    {  
        _status = GetComponent<PlayerStatus>();
        _controller = GetComponent<CharacterController>();
    }

    void MovePlayer(Vector3 vel)
    {
        Vector3 motion = vel * Time.fixedDeltaTime;

        _controller.Move(motion);

        if(vel != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(vel);
    }
	
}
