using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class NetComponent : MonoBehaviour
{
    public bool _monojugador = true;
    public bool _server = true;
    public bool _client = true;

    protected void Start()
    {
        if (Network.peerType == NetworkPeerType.Disconnected &&
                !_monojugador)
            Component.Destroy(this);

        else if (Network.peerType == NetworkPeerType.Server &&
                !_server)
            Component.Destroy(this);

        else if (Network.peerType == NetworkPeerType.Client &&
                !_client)
            Component.Destroy(this);
        else
            OnStart();
    }

    /// <summary>
    /// Sustituye al Start() de Unity. De esa manera ejecutamos el Start() aqu�
    /// destruyendo los componentes que no se necesiten en funci�n del modo de red.
    /// </summary>
    protected virtual void OnStart()
    {
    }

    /// <summary>
    /// Sustituye el SendMessage de Unity. Si se env�a un mensaje y ning�n
    /// script lo recibe se env�a dicho mensaje por red para que se ejecute
    /// en el otro extremo.
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <param name="broadcast">Par�metro opcional. Si es true se env�a por red
    /// a todos los clientes y servidor</param>
    public new void SendMessage(string methodName, object value = null, bool broadcast = false)
    {
        SendMessage(gameObject, methodName, value, broadcast);
    }

    /// <summary>
    /// Sustituye el SendMessage de Unity. Si se env�a un mensaje y ning�n
    /// script lo recibe se env�a dicho mensaje por red para que se ejecute
    /// en el otro extremo.
    /// </summary>
    /// <param name="receivedGameObject">gameObject al que se envia el mensaje</param>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <param name="broadcast">Par�metro opcional. Si es true se env�a por red
    /// a todos los clientes y servidor</param>
    /// <returns>True si alg�n componente tiene el m�todo requerido o se envi� por red</returns>
    public bool SendMessage(GameObject receivedGameObject, string methodName, object value = null, bool broadcast = false)
    {
        bool anyReceiver = UnitySendMessage(methodName, value, receivedGameObject);

        // Si estamos conectados lo enviamos por red
        if (GetComponent<NetworkView>() != null && Network.peerType != NetworkPeerType.Disconnected)
        {
            // Si el env�o es forzado se hace broadcast a todos los clientes aunque sea un cliente el que lo env�a
            NetSendMessage(methodName, value, receivedGameObject, broadcast);
            return true;
        }
        else if (!anyReceiver)
        {
            Debug.LogWarning("SendMessage " + methodName + " has no receiver!");
        }
        return anyReceiver;
    }

    /// <summary>
    /// Env�a un mensaje de red a un �nico player.
    /// </summary>
    /// <param name="player">NetworkPlayer al que se le env�a el mensaje de red</param>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <returns>True si alg�n componente tiene el m�todo requerido o se envi� por red</returns>
    public bool SendMessage(NetworkPlayer player, string methodName, object value = null)
    {
        return SendMessage(player, gameObject, methodName, value);
    }

    /// <summary>
    /// Env�a un mensaje de red a un �nico player.
    /// </summary>
    /// <param name="player">NetworkPlayer al que se le env�a el mensaje de red</param>
    /// <param name="receivedGameObject">gameObject al que se envia el mensaje</param>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <returns>True si alg�n componente tiene el m�todo requerido o se envi� por red</returns>
    public bool SendMessage(NetworkPlayer player, GameObject receivedGameObject, string methodName, object value = null)
    {
        NetSendMessage(player, methodName, value, receivedGameObject);
        return true;
    }

    /// <summary>
    /// Env�a un mensaje a los componentes de "este lado", por introspecci�n
    /// como hace Unity pero devolviendo un booleano.
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <param name="receivedGameObject">gameObject al que se envia el mensaje. Por 
    /// defecto se env�a al propio GameObject</param>
    /// <returns>True si alg�n componente tiene el m�todo requerido</returns>
    private bool UnitySendMessage(string methodName, object value = null, GameObject receivedGameObject = null)
    {
        if (receivedGameObject == null)
            receivedGameObject = gameObject;
        bool anyReceiver = false;
        foreach (MonoBehaviour c in receivedGameObject.GetComponents<MonoBehaviour>())
        {
            MethodInfo methodInfo;
            if (value != null)
                methodInfo = c.GetType().GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new Type[] { value.GetType() }, null);
            else
            {
                methodInfo = c.GetType().GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (methodInfo != null)
            {
                anyReceiver = true;
                object[] parameters;
                if (value != null)
                    parameters = new object[] { value };
                else
                    parameters = new object[] { };
                methodInfo.Invoke(c, parameters);
            }
        }
        return anyReceiver;
    }

    /// <summary>
    /// Env�a un mensaje al otro extremo. El m�todo invocado se deduce a partir del
    /// nombre del m�todo m�s el tipo ya que las llamadas con RCP no pueden ser a
    /// m�todos sobrecargados.
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <param name="receivedGameObject">gameObject al que se envia el mensaje. Por 
    /// defecto se env�a al propio GameObject</param>
    /// <param name="toOthers">caso especial que solo debe usarse para ser true
    /// cuando el servidor recibe un mensaje que debe reenviar a todos los clientes menos
    /// al invocante</param>
    private void NetSendMessage(string methodName, object value = null, GameObject receivedGameObject = null, bool toOthers = false)
    {
        if (receivedGameObject == null)
            receivedGameObject = gameObject;
        string netMethodName = "RPCSendMessageFromOtherSide";
        object[] parameters;
        RPCMode mode = toOthers || Network.peerType == NetworkPeerType.Server ? RPCMode.Others : RPCMode.Server;
        if (value == null)
            parameters = new object[] { methodName };
        else
        {
            netMethodName += value.GetType().Name;
            parameters = new object[] { methodName, value };
        }
        receivedGameObject.GetComponent<NetworkView>().RPC(netMethodName, mode, parameters);
    }

    /// <summary>
    /// Env�a un mensaje a un jugador concreto. El m�todo invocado se deduce a partir del
    /// nombre del m�todo m�s el tipo ya que las llamadas con RCP no pueden ser a
    /// m�todos sobrecargados.
    /// </summary>
    /// <param name="player">Player al que queremos enviar el mensaje</param>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional. Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    /// <param name="receivedGameObject">gameObject al que se envia el mensaje. Por 
    /// defecto se env�a al propio GameObject</param>
    private void NetSendMessage(NetworkPlayer player, string methodName, object value = null, GameObject receivedGameObject = null)
    {
        if (receivedGameObject == null)
            receivedGameObject = gameObject;
        string netMethodName = "RPCSendMessageFromOtherSide";
        object[] parameters;
        if (value == null)
            parameters = new object[] { methodName };
        else
        {
            netMethodName += value.GetType().Name;
            parameters = new object[] { methodName, value };
        }
        print(netMethodName + " " + parameters);
        receivedGameObject.GetComponent<NetworkView>().RPC(netMethodName, player, parameters);
    }


    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional del m�todo Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    private void SendMessageFromOtherSide(string methodName, object value)
    {
        if (!UnitySendMessage(methodName, value, gameObject))
        {
            Debug.LogWarning("SendMessage " + methodName + " from the other side has no receiver!");
        }
    }

    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    [RPC]
    private void RPCSendMessageFromOtherSide(string methodName)
    {
        SendMessageFromOtherSide(methodName, null);
    }

    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional del m�todo Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    [RPC]
    private void RPCSendMessageFromOtherSideSingle(string methodName, float value)
    {
        SendMessageFromOtherSide(methodName, value);
    }

    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional del m�todo Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    [RPC]
    private void RPCSendMessageFromOtherSideInt32(string methodName, int value)
    {
        SendMessageFromOtherSide(methodName, value);
    }

    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional del m�todo Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    [RPC]
    private void RPCSendMessageFromOtherSideString(string methodName, string value)
    {
        SendMessageFromOtherSide(methodName, value);
    }

    /// <summary>
    /// Env�a un mensaje que ha sido solicitado desde el otro extremo 
    /// (cliente o servidor)
    /// </summary>
    /// <param name="methodName">Nombre del m�todo que se quiere invocar</param>
    /// <param name="value">Par�metro opcional del m�todo Si es null se presupone que
    /// el m�todo no tiene par�metros</param>
    [RPC]
    private void RPCSendMessageFromOtherSideVector3(string methodName, Vector3 value)
    {
        SendMessageFromOtherSide(methodName, value);
    }
}
