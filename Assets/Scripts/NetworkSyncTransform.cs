using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkSyncTransform : NetworkBehaviour
{
    [SerializeField]
    private float _posLerpRate = 15;
    [SerializeField]
    private float _rotLerpRate = 15;
    [SerializeField]
    private bool UseRotationAnim;
    [SerializeField]
    private float _posThreshold = 0.1f;
    [SerializeField]
    private float _rotThreshold = 1f;

    [SyncVar]
    private Vector3 _lastPosition;

    [SyncVar]
    private Vector3 _lastRotation;

    private void Start()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.eulerAngles;
    } 

    void Update()
    {
        if (IsMaster)
            return;

        InterpolatePosition();
        InterpolateRotation();
    }

    bool IsMaster
    {
        get { return isLocalPlayer || hasAuthority; }
    }

    void FixedUpdate()
    {
        if (!IsMaster)
            return;

        var posChanged = IsPositionChanged();

        if (posChanged)
        {
            CmdSendPosition(transform.position);
            _lastPosition = transform.position;
        }

        var rotChanged = IsRotationChanged();

        if (rotChanged)
        {
            CmdSendRotation(transform.localEulerAngles);
            _lastRotation = transform.localEulerAngles;
        }
    }

    private void InterpolatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, _lastPosition, Time.deltaTime * _posLerpRate);
    }

    private void InterpolateRotation()
    {
        if (UseRotationAnim) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_lastRotation), Time.deltaTime * _rotLerpRate);
        else transform.rotation = Quaternion.Euler(_lastRotation);
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdSendPosition(Vector3 pos)
    {
        _lastPosition = pos;
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdSendRotation(Vector3 rot)
    {
        _lastRotation = rot;
    }

    private bool IsPositionChanged()
    {
        return Vector3.Distance(transform.position, _lastPosition) > _posThreshold;
    }

    private bool IsRotationChanged()
    {
        return Vector3.Distance(transform.localEulerAngles, _lastRotation) > _rotThreshold;
    }

    public override int GetNetworkChannel()
    {
        return Channels.DefaultUnreliable;
    }

    public override float GetNetworkSendInterval()
    {
        return 0.01f;
    }
}
