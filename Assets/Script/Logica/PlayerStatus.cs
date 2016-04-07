using UnityEngine;
using System.Collections;

public class PlayerStatus : NetComponent
{
    public Vector3 Velocity;

    void Awake()
    {
        Velocity = new Vector3(0, 0, 0);
    }

    protected override void OnStart()
    {
    }

    void ChangeName(string newName)
    {
        name = newName;
    }

    void IsLocalPlayer(string newName)
    {
        if (newName == Network.player.ToString())
        {
            // Tomo el control
            GameObject.FindWithTag("MainCamera").
                GetComponent<CameraController>().Target = gameObject;
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<CalculateVelocity>().enabled = true;
            GetComponent<GUIInfo>().enabled = true;
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Serialize(ref Velocity);
            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
        }
        else
        {
            stream.Serialize(ref Velocity);
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);
            Vector3 error = pos - transform.position;
            if(error.magnitude > 10)
                transform.position = pos;
        }

        //stream.Serialize(ref Velocity);
        //Vector3 pos = transform.position;
        //stream.Serialize(ref pos);
        //transform.position = pos;
    }
}
