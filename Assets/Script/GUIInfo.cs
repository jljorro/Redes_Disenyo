using UnityEngine;

/// <summary>
/// Clase que se encarga de mostrar el resultado de la carrera.
/// </summary>
public class GUIInfo : MonoBehaviour {
    
    [SerializeField] FinishLine finishLine;

    // Update is called once per frame
    void OnGUI() {
        // Marcador del lider
        Label (0, 0.3f, 0.2f, 0.1f, "Mejor tiempo de la partida");
        Label (0, 0.4f, 0.2f, 0.1f, "Lider: " + finishLine.NameLeader);
        Label (0, 0.5f, 0.2f, 0.1f, "Tiempo Lider:  " + finishLine.LeaderTime);
        
        // Marcador del jugador local 
        Label (0, 0.6f, 0.2f, 0.1f, "Tus tiempos");
        Label (0, 0.7f, 0.2f, 0.1f, "Vuelta: " + finishLine.CarLap);
        Label (0, 0.8f, 0.2f, 0.1f, "Última Vuelta: " + finishLine.LastLapTime);
        Label (0, 0.9f, 0.2f, 0.1f, "Mejor Tiempo: " + finishLine.BestLocalTime);
    }

    public static void Label(float left, float top, float width, float height, string text) {
        GUI.Label(new Rect(left * Screen.width, top * Screen.height, width * Screen.width, height * Screen.height), text);
    }
}
