using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ValidateImage : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;

    [SerializeField]
    private Color _wrongColor = default;

    [SerializeField]
    private Color _rightColor = default;


    public void SetColor(bool isCorrect)
    {
        ActivateImage(true);
        if (isCorrect == false)
        {
            SetColorToImage(_wrongColor);
        }
        else
        {
            SetColorToImage(_rightColor);
        }
    }

    public void ActivateImage(bool isActiv)
    {
        _targetImage.gameObject.SetActive(isActiv);
    }

    private void SetColorToImage(Color color)
    {
        _targetImage.color = color;
    }
}
