using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : NetComponent 
{

    private enum State
    {
        disconnected,
        waiting,
        loadingMap,
        playing
    }

    State _state = State.disconnected;

	// Use this for initialization
	protected override void OnStart () 
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<NetworkView>().group = 1;
	}

    void OnEnable()
    {
        Network.InitializeServer(8, 80, !Network.HavePublicAddress());
        _state = State.waiting;
    }
	
	void OnGUI() 
    {
        switch (_state)
        {
            case State.disconnected:
                {
                    GameObject.Destroy(gameObject);
                    SceneManager.LoadScene("Menu");
                } break;
            case State.waiting:
                {
                    string msg = "";
                    foreach (NetworkPlayer client in Network.connections)
                    {
                        msg += client.ipAddress + ":" + client.port + " connected\n";
                    }
                    GUIInfo.TextArea(0.1f, 0.1f, 0.8f, 0.6f, msg);

                    if (GUIInfo.Button(0.1f, 0.8f, 0.3f, 0.2f, "Start Game"))
                        Load();
                } break;
            case State.loadingMap:
                {
                    GUIInfo.Label(0.4f, 0.4f, 0.3f, 0.2f, "Loading...");
                } break;
            case State.playing:
                {
                } break;
        }	
	}

    int _mapLoaded;

    void Load()
    {
        _mapLoaded = 0;
        SendMessage("LoadLevel", "Circuito");
        _state = State.loadingMap;
        // networkView.RPC("LoadLevel", RPCMode.AllBuffered, "Circuito");
    }

    void MapLoaded()
    {
        if(++_mapLoaded == Network.connections.Length + 1)
        {
            LoadPlayers();
            SendMessage("StartGame");
            _state = State.playing;
        }
    }

    void LoadPlayers()
    {
        for (int i = 0; i < Network.connections.Length; ++i)
        {
            NetworkPlayer p = Network.connections[i];
            GameObject o = GameObject.Find("StartPoint" + i);
            SendMessage(p, o, "Create", p.ToString());
        }
    }
}
