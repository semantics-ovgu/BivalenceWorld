using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ConstantDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text = default;
    [SerializeField]
    private Image _image = default;

    private void Awake()
    {
        GameManager.Instance.GetCameraManager().CameraChangedEvent.AddEventListener(CameraChangedListener);
    }

    private void CameraChangedListener(CameraRotation.CameraArgs args)
    {
        switch (args.CameraMode)
        {
            case CameraRotation.CameraMode.Cam2D:
                transform.localPosition += new Vector3(1, 10);
                break;
            case CameraRotation.CameraMode.Cam3D:
                transform.localPosition += new Vector3(-1, -10);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetText(string txt)
    {
        //ToDo Resize
        _text.text = txt;
        Canvas.ForceUpdateCanvases();
        _image.rectTransform.sizeDelta = new Vector2(_text.rectTransform.sizeDelta.x + 2, _image.rectTransform.sizeDelta.y);
    }
}
