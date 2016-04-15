using UnityEngine;
using UnityEngine.Networking;

public class Player_NetworkIdentity : NetworkBehaviour {
    
    [SyncVar] private string playerUniqueId;
    private NetworkInstanceId playerNetId;
    private Transform transformPlayer;
    
    public override void OnStartLocalPlayer () {
        GetNetIdentity ();
        SetIdentity ();
    }

	// Use this for initialization
	void Awake () {
	    transformPlayer = transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (transformPlayer.name == "" || transformPlayer.name == "Car(Clone)"){
            SetIdentity ();
        }
	}
    
    [Client]
    void GetNetIdentity () {
        playerNetId = GetComponent<NetworkIdentity> ().netId;
        
        CmdTellServerMyIdentity (MakeUniqueIdentity());
    }
    
    void SetIdentity () {
        if (!isLocalPlayer) {
            transformPlayer.name = playerUniqueId;
        } else {
            transformPlayer.name = MakeUniqueIdentity();
        }
    }
    
    string MakeUniqueIdentity () {
        string uniqueId = "Player " + playerNetId.ToString ();
        return uniqueId;
    }
    
    [Command]
    void CmdTellServerMyIdentity (string identity) {
        playerUniqueId = identity;
    }
    
    [Command]
    public void CmdSetBestTimes (string nameLeader, float leaderTime) {
        GameObject.Find ("Circuit/FinishLine").GetComponent<FinishLine> ().SetBestTimes (nameLeader, leaderTime);
    }
}
