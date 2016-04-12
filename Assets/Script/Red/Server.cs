using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que controla la escena del servidor.s
/// </summary>
public class Server : NetComponent {
    
    private enum State {disconnected, waitingConnections, loadingMap, loadingPlayers, playing}
    
    State _state = State.disconnected;
    
    private int _mapLoadedCount;
    
    void Awake () {
        DontDestroyOnLoad (this);
    }

	protected override void OnStart () {
        GetComponent<NetworkView> ().group = 1;              	
	}
    
    void OnEnable () {
        if (_state == State.disconnected) {
            NetworkConnectionError error = Network.InitializeServer (8, 80, !Network.HavePublicAddress());
            
            if (error == NetworkConnectionError.NoError) {
                _state = State.waitingConnections;
            }            
        }  
    }
	
	// Update is called once per frame
	void Update () {
	    if (Network.peerType == NetworkPeerType.Disconnected)
            _state = State.disconnected;
	}
    
    void OnGui () {
        
        switch (_state) {
            case State.disconnected:
                GameObject.Destroy (gameObject);
                SceneManager.LoadScene ("Menu");
            break;
            case State.waitingConnections:
                
                string msg = "";
                
                foreach (NetworkPlayer client in Network.connections) {
                    msg += client.ipAddress + ":" + client.port + " connected\n";
                }
                
                GUIInfo.TextArea (0.1f, 0.1f, 0.8f, 0.6f, msg);
                
                if (GUIInfo.Button (0.1f, 0.8f, 0.3f, 0.2f, "Start Game"))
                    Load();
                    
                if (GUIInfo.Button (0.6f, 0.8f, 0.3f, 0.2f, "Back"))
                    Network.Disconnect ();
                
            break;
            case State.loadingMap:
                GUIInfo.Label (0.4f, 0.4f, 0.3f, 0.2f, "Loading...");
            break;
            case State.playing:
            break;
        }     
    }
    
    /// <summary>
    /// Método que envia a los clientes el mensaje de que deben cargar los mapas.
    /// </summary>
    void Load () {
        _mapLoadedCount = 0;
        
        Network.RemoveRPCsInGroup (0);
        Network.RemoveRPCsInGroup (1);
        
        // TODO - Enviar mensaje de cargar nivel
        ((Client) GetComponent<Client> ()).RpcLoadLevel("Circuito");
        
        _state = State.loadingMap;
    }
    
    [Command]
    void CmdMapLoaded () {
        // Número de clientes más el servidor
        
        if (++_mapLoadedCount == Network.connections.Length +1) {
            
            //TODO - Comunicar comenzar juego
            ((Client) GetComponent<Client> ()).RpcStartGame();
            
            _state = State.playing;
            print ("Starting game");
        }
        
    }
}
