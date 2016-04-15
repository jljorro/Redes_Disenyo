using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour {
    
    [SyncVar]
    private Vector3 syncPosition;
    
    [SerializeField] Transform transformPlayer;
    [SerializeField] float lerpRate = 15;

	
	// Update is called once per frame
	void FixedUpdate () {
	    TransmitPosition ();
        LerpPosition ();
	}
    
    void LerpPosition () {
        if (!isLocalPlayer) {
             transformPlayer.position = Vector3.Lerp (transformPlayer.position, syncPosition, Time.deltaTime * lerpRate);
        }        
    }
    
    [Command]
    void CmdProvidePositionToServer (Vector3 position) {
        syncPosition = position;
    }
    
    [ClientCallback]
    void TransmitPosition () {
        if (isLocalPlayer) {
            CmdProvidePositionToServer (transformPlayer.position);
        }
    }
}
