using UnityEngine;
using UnityEngine.Networking;

public class Player_Sync : NetworkBehaviour {
    
    [SyncVar]
    private Vector3 syncPosition;
    
    [SerializeField] Transform transform;
    [SerializeField] float lerpRate = 15;

	
	// Update is called once per frame
	void FixedUpdate () {
	    TransmitPosition ();
        LerpPosition ();
	}
    
    void LerpPosition () {
        if (!isLocalPlayer) {
             transform.position = Vector3.Lerp (transform.position, syncPosition, Time.deltaTime * lerpRate);
        }        
    }
    
    [Command]
    void CmdProvidePositionToServer (Vector3 position) {
        syncPosition = position;
    }
    
    [ClientCallback]
    void TransmitPosition () {
        if (isLocalPlayer) {
            CmdProvidePositionToServer (transform.position);
        }
    }
}
