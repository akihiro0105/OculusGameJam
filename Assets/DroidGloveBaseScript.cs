using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class DroidGloveBaseScript : MonoBehaviour
{
	public GameObject Model;
    private UnityChan_AndroidControll Player;
	public bool ControllSide=false;

    private float initzeropoint = 0;
    private float setZplus = 20, setZminus = -15;

	protected const int DefaltScOscPort = 57110;
	protected Dictionary<string, ServerLog> _servers = null;
	protected Quaternion _objectQuaternion = new Quaternion();
    private float BufFront = 0;
	private int interval = 0;

	protected virtual void Awake()
    {
		Player=Model.GetComponent<UnityChan_AndroidControll>();
		Player.androidY = 0;

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
				Vector3 v=_objectQuaternion.eulerAngles;

                float initfront = v.y;
                float front = initfront - BufFront;
                BufFront = initfront;
                if (front < 0) front *= -1;
                if (front > 1)
                {
                    if (interval < 10) interval++;
                }
                else
                {
                    if (interval > 0) interval--;
                }
                Player.androidY = 0.1f * interval;

                float side = v.x;
                if (side > 180) side = side - 360;
                side *= -1;
                if (Input.GetKey(KeyCode.Z)) initzeropoint = side;
                side = side - initzeropoint;
                print(side.ToString());
                if (ControllSide && interval > 0)
                {
                    if (side > setZplus) Player.androidZ = 1;
                    else if (side < setZminus) Player.androidZ = -1;
                    else Player.androidZ = 0;
                }
                else Player.androidZ = 0;
			}
	    }
    }
}
