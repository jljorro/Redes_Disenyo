using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interpolate : NetComponent
{
	private PlayerStatus _status;
	Vector3 _nextPosition;
	
	void Start()
	{
        _status = GetComponent<PlayerStatus>();
	}

    #region Helper Methods

    private static readonly char DATA_SEPARATOR = ';';
    private static readonly char VALUE_SEPARATOR = ',';

    IList<Vector3> positions = new List<Vector3>();
    public int _snapshotSize = 4;

    /// <summary>
    /// Serialize the data to send on Snapshot
    /// </summary>
    /// <returns>Serialized data on string</returns>
    private string _Serialize()
    {
        string ret = "";
        //Position
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 pos = positions[i];
            ret += pos.x.ToString() + VALUE_SEPARATOR;
            ret += pos.y.ToString() + VALUE_SEPARATOR;
            ret += pos.z.ToString();
            if (i < positions.Count - 1)
                ret += DATA_SEPARATOR;
        }
        return ret;
    }

    /// <summary>
    /// Deserializes received data
    /// </summary>
    /// <param name="raw">Serialized data on string</param>
    private void _DeSerialize(string raw)
    {
        //Deserialize Position
        string[] splittedPositionData = raw.Split(DATA_SEPARATOR);
        positions.Clear();
        for (int i = 0; i < _snapshotSize; i++)
        {
            string[] splittedVector3 = splittedPositionData[i].Split(VALUE_SEPARATOR);
            Vector3 next = new Vector3(
                float.Parse(splittedVector3[0]), //x
                float.Parse(splittedVector3[1]), //y
                float.Parse(splittedVector3[2]));//z
            interpolateTill(next);
            positions.Add(next);
        }
    }

    void interpolateTill(Vector3 next)
    {
    }

    #endregion
}
