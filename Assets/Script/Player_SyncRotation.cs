using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncRotation : NetworkBehaviour {
    
    [SyncVar] 
    private Quaternion syncPlayerRotation;

    [SerializeField] private Transform transformPlayer;
    [SerializeField] private float lerpRate = 15;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    TransmitRotation ();
        LerpRotation ();
	}
    
    void LerpRotation () {
        if (!isLocalPlayer){
            transformPlayer.rotation = Quaternion.Lerp (transformPlayer.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
        }
    }
    
    [Command]
    void CmdProvideRotationToServer (Quaternion rotation) {
        syncPlayerRotation = rotation;
    }
    
    [ClientCallback]
    void TransmitRotation () {
        if (isLocalPlayer){
            CmdProvideRotationToServer (transformPlayer.rotation);
        }
    }
}
