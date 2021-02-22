using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = default;
    [SerializeField]
    private Camera _orthoCamera = default;
    [SerializeField]
    private Vector3 _defaultPos = new Vector3(-18, 30, -27);
    [SerializeField]
    private Vector3 _defaultRot = new Vector3(35, 90, 0);
    [SerializeField]
    private Vector3 _orthoPos = new Vector3(22, 48, -36);
    [SerializeField]
    private Vector3 _orthoRot = new Vector3(90, 90, 0);
    public GenericEvent<CameraArgs> CameraChangedEvent = new GenericEvent<CameraArgs>();
    private Camera _currentCamera = default;

    public CameraMode GetCurrentCamMode()
    {
        if (_currentCamera == _orthoCamera)
        {
            return CameraMode.Cam2D;
        }
        else
        {
            return CameraMode.Cam3D;
        }
    }


    public void SetCameraDefault()
    {
        //SetCamera(_defaultPos, _defaultRot);
        _camera.gameObject.SetActive(true);
        _orthoCamera.gameObject.SetActive(false);
        _currentCamera = _camera;
        CameraChangedEvent.InvokeEvent(new CameraArgs
        {
            Camera = _camera,
            CameraMode = CameraMode.Cam3D
        });

    }

    public void SetCameraOrthogonal()
    {
        //SetCamera(_orthoPos, _orthoRot);
        _camera.gameObject.SetActive(false);
        _orthoCamera.gameObject.SetActive(true);
        _currentCamera = _orthoCamera;
        CameraChangedEvent.InvokeEvent(new CameraArgs
        {
            Camera = _orthoCamera,
            CameraMode = CameraMode.Cam2D
        });
    }

    private void SetCamera(Vector3 pos, Vector3 rot)
    {
        _camera.transform.position = pos;
        _camera.transform.rotation = Quaternion.Euler(rot);
    }

    public Camera GetCurrentCamera()
    {
        return _currentCamera;
    }

    public class CameraArgs
    {
        public Camera Camera;

        public CameraMode CameraMode;
    }

    public enum CameraMode
    {
        Cam2D,
        Cam3D
    }

}
