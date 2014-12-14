using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class OVRAndroid : MonoBehaviour {

    public GameObject Model;
    private OVRPlayerController Player;
    public bool ControllSide = false;

    private float initzeropoint = 0;
    private float setZplus = 20, setZminus = -20;

    protected const int DefaltScOscPort = 57110;
    protected Dictionary<string, ServerLog> _servers = null;
    protected Quaternion _objectQuaternion = new Quaternion();
    private Vector3 BufV = new Vector3();
    private int interval = 0;

    protected virtual void Awake()
    {
        Player = Model.GetComponent<OVRPlayerController>();

        OSCHandler.Instance.Init("", DefaltScOscPort);
        _servers = new Dictionary<string, ServerLog>();
    }

    protected void Update()
    {
        OSCHandler.Instance.UpdateLogs();
        _servers = OSCHandler.Instance.Servers;
        List<ServerLog> servLogs = new List<ServerLog>(_servers.Values);
        foreach (ServerLog servLog in servLogs)
        {
            if (servLog.log.Count > 0)
            {
                int lastPacketIndex = servLog.packets.Count - 1;
                if (OSCHandler.Instance.Clients.Count == 0)
                {
                    OSCHandler.Instance.Init(servLog.server.RemoteEndPoint.Address.ToString(), DefaltScOscPort);
                }
                float x = (float)servLog.packets[lastPacketIndex].Data[0];
                float y = (float)servLog.packets[lastPacketIndex].Data[1];
                float z = (float)servLog.packets[lastPacketIndex].Data[2];
                float w = (float)servLog.packets[lastPacketIndex].Data[3];
                _objectQuaternion = new Quaternion(x, y, z, w);
                Vector3 v = _objectQuaternion.eulerAngles;
                Vector3 bufv = v;
                bufv.y = bufv.y - BufV.y;
                if (bufv.y < 0) bufv.y *= -1;
                //if(bufv.z>180)bufv.z=bufv.z-360;
                if (bufv.x > 180) bufv.x = bufv.x - 360;
                if (bufv.y > 1)
                {
                    if (interval < 10) interval++;
                    Player.AndroidFront = true;
                }
                else
                {
                    if (interval > 0) interval--;
                    Player.AndroidFront = false;
                }
                //float bufz=bufv.z;
                float bufz = bufv.x;
                if (Input.GetKey(KeyCode.Z)) initzeropoint = bufz;
                bufz = bufz - initzeropoint;
                print(bufz.ToString());
                if (ControllSide && interval > 0)
                {
                    if (bufz > setZplus) Player.AndroidLeft = true;
                    else if (bufz < setZminus) Player.AndroidRight = true;
                    else
                    {
                        Player.AndroidRight = false;
                        Player.AndroidLeft = false;
                    }
                }
                else
                {
                    Player.AndroidRight = false;
                    Player.AndroidLeft = false;
                }
                //print ("front:"+(unitychan.androidY).ToString()+"side:"+(unitychan.androidZ).ToString());
                BufV = v;
            }
        }
    }
}
