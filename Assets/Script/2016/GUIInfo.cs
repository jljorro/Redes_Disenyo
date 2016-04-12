using UnityEngine;

/// <summary>
/// Método que se encarga de mostrar el resultado de la carrera.
/// </summary>
public class GUIInfo : MonoBehaviour {
    
    private int _lap = 0;
    private string _lider = "";
    private float _lastLap = 99;
    private float _bestLap = 99;

    // Update is called once per frame
    void OnGUI() {
        Label(0, 0, 0.2f, 0.1f, "Lap " + _lap);
        Label(0, 0.1f, 0.2f, 0.1f, "Lider " + _lider);
        Label(0, 0.2f, 0.2f, 0.1f, "Last Lap " + _lastLap);
        Label(0, 0.3f, 0.2f, 0.1f, "Best Lap " + _bestLap);
    }

    public void LapTime(float time) {
        _lastLap = time;
        if (_lastLap < _bestLap)
            _bestLap = _lastLap;
    }

    public void Lap(int lap) {
        _lap = lap;
    }

    public void NewLider(string lider) {
        _lider = lider;
    }

    public static void Label(float left, float top, float width, float height, string text)
    {
        GUI.Label(new Rect(left * Screen.width, top * Screen.height, width * Screen.width, height * Screen.height), text);
    }

    public static string TextField(float left, float top, float width, float height, string text)
    {
        return GUI.TextField(new Rect(left * Screen.width, top * Screen.height, width * Screen.width, height * Screen.height), text);
    }

    public static string TextArea(float left, float top, float width, float height, string text)
    {
        return GUI.TextArea(new Rect(left * Screen.width, top * Screen.height, width * Screen.width, height * Screen.height), text);
    }

    public static bool Button(float left, float top, float width, float height, string text)
    {
        return GUI.Button(new Rect(left * Screen.width, top * Screen.height, width * Screen.width, height * Screen.height), text);
    }
}
