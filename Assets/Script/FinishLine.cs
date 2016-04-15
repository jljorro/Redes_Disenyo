using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Clase que controla la meta y los tiempos de vuelta que se
/// van haciendo en el juego.
///
/// Cada clase FinishLine controla la meta del jugador local.
/// </summary>
public class FinishLine : NetworkBehaviour {
    
    // Variables que hay que sincronizar
    [SyncVar] string nameLeader;
    [SyncVar] float leaderTime;
    
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
    
    [Server]
    public void SetBestTimes (string nameLeader, float bestTime){
        this.nameLeader = nameLeader;
        leaderTime = bestTime;
    }

    void OnTriggerEnter(Collider other) {
      Debug.Log ("Entro en la meta");
      
      if (other.GetComponent<Player_NetworkSetup>().isLocalPlayer) {
            car = other.gameObject;
            
            // Comprobamos que ha pasado por todos los puntos de control
            foreach (GameObject cp in _checkpoints) {
                if (!cp.GetComponent<CheckPoint>().Checked)
                    return;
            }
            
            // Marcamos el tiempo de la última vuelta
            lastLapTime = Time.time - lastLapTime;
            
            if (bestLocalTime > lastLapTime)
                bestLocalTime = lastLapTime;
                
            if (leaderTime > bestLocalTime) {
                other.GetComponent<Player_NetworkIdentity> ().CmdSetBestTimes (car.name, bestLocalTime);
            }

            // Reseteamos todos los checkpoints para ese coche
            foreach (GameObject cp in _checkpoints) {
                cp.GetComponent<CheckPoint>().Checked = false;
            }            
        }      
    }
}
