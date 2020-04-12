using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Makes a <see cref="GameObject"/> face the rendering <see cref="Camera"/>. 
/// Only works on <see cref="GameObject"/> that have a <see cref="Renderer"/> component attached and if it's visible.
/// </summary>
[ExecuteInEditMode]
public class RenderAsBillboard : MonoBehaviour
{

    [SerializeField]
    private Camera _camera = default;
    /// <summary> What Camera property to face when rendering. </summary>
    public enum Mode
    {
        /// <summary> Rotates towards the camera position. </summary>
        Position,
        /// <summary> Rotates so that the forward vectors match. </summary>
        Rotation
    }

    public Mode BillboardMode = Mode.Position;
    /// <summary> Used to restrict/enable rotation to certain axes. 0.0f disables rotation around that axis, 1.0f enables it. </summary>
    public Vector3 Axes = new Vector3(1.0f, 1.0f, 1.0f);


    private void Awake()
    {
	    var manager = GameManager.Instance;
	    if (manager != null)
	    {
		    var cameraManager = manager.GetCameraManager();
		    if (cameraManager != null)
		    {
                cameraManager.CameraChangedEvent.AddEventListener(CameraChangedListener);
		    }
	    }
    }

    private void CameraChangedListener(Camera arg0)
    {
	    _camera = arg0;
    }

    //private void OnWillRenderObject()
    //{
    //    if (!enabled)
    //        return;

    //    Vector3 dir = GetDirection();
    //    dir.Scale(Axes);
    //    transform.forward = dir;
    //}

    private void LateUpdate()
    {
        if(_camera == null)
        {
	        _camera = GameManager.Instance.GetMainCamera();
        }
        else
        {
            Vector3 dir = GetDirection();
            dir.Scale(Axes);
            transform.forward = dir;
        }
    }

    private Vector3 GetDirection()
    {
        if (BillboardMode == Mode.Rotation)
            return _camera.transform.forward;

        else if (BillboardMode == Mode.Position)
            return transform.position - _camera.transform.position;

        Debug.Assert(false, "Unknown " + typeof(Mode).Name + ": " + BillboardMode, gameObject);
        return Vector3.zero;
    }
}
