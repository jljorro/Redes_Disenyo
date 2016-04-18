using UnityEngine;

/// <summary>
/// Clase que controla la meta y los tiempos de vuelta que se
/// van haciendo en el juego.
///
/// Cada clase FinishLine controla la meta del jugador local.
/// </summary>
public class FinishLine : MonoBehaviour {
    
    // Variables que hay que sincronizar
    string nameLeader;
    float leaderTime;
    
    // Variables locales a cada juagdor
    int carLap;
    float lastLapTime;
    //float carTime;
    float bestLocalTime;
    
    GUIInfo _guiInfo;
    GameObject[] _checkpoints;
    GameObject car;

    public string NameLeader {
        get {return nameLeader;}
    }
    
    public float LeaderTime {
        get {return leaderTime;}
    }
    
    public int CarLap {
        get {return carLap;}
    }
    
    public float LastLapTime {
        get {return lastLapTime;}
    }
    
    public float BestLocalTime {
        get {return bestLocalTime;}
    }

	// Use this for initialization
	void Awake () {
        nameLeader = "";
        leaderTime = 99;
        carLap = 0;
        lastLapTime = 0;
        bestLocalTime = 99;
        //carTime = 0;
	}

     void Start() {
        _checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
    }

    void Update () {}
    
    public void SetBestTimes (string nameLeader, float bestTime){
        this.nameLeader = nameLeader;
        leaderTime = bestTime;
    }

    void OnTriggerEnter(Collider other) {
      Debug.Log ("Entro en la meta");
    }
}
