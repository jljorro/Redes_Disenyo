using UnityEngine;
using System.Collections;

public class Client : NetComponent
{
    private enum State
    {
        disconnected,
        waiting,
        loadingMap,
        playing
    }

    State _state = State.disconnected;

    public string ip = "127.0.0.1";

    public string port = "80";

	// Use this for initialization
	protected override void OnStart () 
    {
        GetComponent<NetworkView>().group = 1;
	}

    void OnGUI()
    {
        switch(_state)
        {
            case State.disconnected:
                {
                    ip = GUIInfo.TextField(0.1f, 0.1f, 0.3f, 0.1f, ip);
                    port = GUIInfo.TextField(0.6f, 0.1f, 0.3f, 0.1f, port);
                    if (GUIInfo.Button(0.4f, 0.4f, 0.3f, 0.2f, "Connect"))
                    {
                        Network.Connect(ip, int.Parse(port));
                    }
                }break;
            case State.waiting:
                {
                    string msg = "";
                    foreach (NetworkPlayer client in Network.connections)
                    {
                        msg += client.ipAddress + ":" + client.port + " connected\n";
                    }
                    GUIInfo.TextArea(0.1f, 0.1f, 0.8f, 0.6f, msg);

                    if (GUIInfo.Button(0.1f, 0.8f, 0.3f, 0.2f, "Back"))
                        Network.Disconnect();
                }break;
            case State.loadingMap:
                {
                    GUIInfo.Label(0.4f, 0.4f, 0.4f, 0.2f, "Loading...");
                }break;

        }
    }

    void LoadLevel()
    {
        _state = State.loadingMap;
    }

    void StartGame()
    {
        _state = State.playing;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
            _state = State.disconnected;
        else if (_state == State.disconnected && Network.isClient)
            _state = State.waiting;
	}
}
