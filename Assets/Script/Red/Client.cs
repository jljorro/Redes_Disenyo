using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// Clase que controla la escena del cliente.
/// </summary>
public class Client : NetComponent {
    
    private enum State {disconnected, waitingConnections, loading, playing}

    State _state = State.disconnected;
    
    public string _ip = "127.0.0.1";
    public string _port = "80";

	// Use this for initialization
	protected override void OnStart () {
	    GetComponent<NetworkView> ().group = 1;
	}
	
	// Update is called once per frame
	void Update () {	
        if (Network.peerType == NetworkPeerType.Disconnected)
            _state = State.disconnected;
        else if (_state == State.disconnected && Network.isClient)
            _state = State.waitingConnections;    
	}
    
    void OnGui () {
        
        switch (_state) {
            case State.disconnected:
            
                _ip = GUIInfo.TextField (0.1f, 0.1f, 0.3f, 0.1f, _ip);
                _port = GUIInfo.TextField (0.6f, 0.1f, 0.3f, 0.1f, _port);
                
                if (GUIInfo.Button (0.4f, 0.4f, 0.3f, 0.2f, "Connect")) {
                    NetworkConnectionError error = Network.Connect (_ip, int.Parse (_port));
                    
                    if (error != NetworkConnectionError.NoError) {
                        print (error);
                    }
                }
            
            break;
            case State.waitingConnections:
            
                string waitingMsg = "";
                foreach (NetworkPlayer client in Network.connections){
                    waitingMsg += client.ipAddress + ":" + client.port + " connected\n";
                }
                
                GUIInfo.TextArea (0.1f, 0.1f, 0.8f, 0.6f, waitingMsg);
                
                if (GUIInfo.Button (0.6f, 0.8f, 0.3f, 0.2f, "Back"))
                    Network.Disconnect ();
            
            break;
            case State.loading:
                GUIInfo.Label (0.4f, 0.4f, 0.3f, 0.2f, "Loading...");
            break;
            case State.playing:
            break;
        }        
    }
    
    [ClientRpc]
    public void RpcLoadLevel (string level) {
        _state = State.loading;
    }
    
    [ClientRpc]
    public void RpcStartGame () {
        _state = State.playing;
    }
}
