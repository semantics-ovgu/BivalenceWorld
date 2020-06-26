using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Validator.World;

public class GUI_ValidateImage : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage = default;
    [SerializeField]
    private List<Container> _list = default;

    public void SetColor(EValidationResult validateResult)
    {
        ActivateImage(true);

        var obj = _list.Find(x => x.Type == validateResult);
        if (obj != null)
        {
            SetSpriteToImage(obj.Sprite);
        }
        else
        {
            Debug.LogError("Can not find type: " + validateResult.ToString());
        }
    }

    public void ActivateImage(bool isActiv)
    {
        _targetImage.gameObject.SetActive(isActiv);
    }

    private void SetSpriteToImage(Sprite sprite)
    {
        _targetImage.sprite = sprite;
    }

    [System.Serializable]
    public class Container
    {
	    public EValidationResult Type;
	    public Sprite Sprite;
    }
}
