using UnityEngine;
using System.Collections;

public class MapLoader : NetComponent {

	// Use this for initialization
	protected override void OnStart () 
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<NetworkView>().group = 1;
	}

    int _levelPrefix = -1;
	
    void LoadLevel(string level)
    {
        Network.SetSendingEnabled(0, false);

        Network.isMessageQueueRunning = false;

        Network.SetLevelPrefix(++_levelPrefix);

        Application.LoadLevel(level);

        Network.isMessageQueueRunning = true;

        Network.SetSendingEnabled(0, true);

        SendMessage("MapLoaded");
	}
}
