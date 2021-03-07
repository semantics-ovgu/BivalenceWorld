using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Validator;
using Validator.Game;

public class DragObject : MonoBehaviour
{
    private Vector3 _mOffset;
    [SerializeField]
    private MeshRenderer _renderer = default;
    private Material _normalMaterial = default;
    [SerializeField]
    private PredicateObj _predicate = default;
    private Vector3 _startPos = default;
    private Vector3 _dragOffset = default;
    [SerializeField]
    private Transform _rootObj = default;

    private void Start()
    {
        _normalMaterial = _renderer.material;
    }

    public void OnMouseDownEvent()
    {
        if (GameManager.Instance.GUIGame != null && GameManager.Instance.GUIGame.IsGameRunning)
        {
            if (GameManager.Instance.GUIGame.GetLastGUIGameElement() is GUI_GameElement_Question questionElement)
            {
                if (questionElement.Question.PossibleAnswers.Any(s => s.SelectionType == Question.Selection.SelectionTypes.WorldObject))
                {
                    var currentWorldObject = new WorldObject(new List<string>(), new List<string>(), new List<object> { _predicate.GetField().GetX(), _predicate.GetField().GetZ() });
                    var selection = questionElement.SetWorldObjSelection(currentWorldObject);

                    if (selection != null)
                    {
                        var element = GameManager.Instance.GetSelectionManager().TargetHoveredElement;
                        if (element is SelectableObject selectable)
                        {
                            foreach (var field in GameManager.Instance.GetCurrentBoard().GetFieldElements())
                            {
                                field.RemoveTemporaryConstants();
                            }
                            var selectedFIeld = selectable.GetRootObj().GetComponent<Field>();
                            if (selectedFIeld != null && !selectedFIeld.GetConstantsList().Any())
                            {
                                selectedFIeld.AddTemporaryConstant(selection.WorldObject.Consts.First());
                            }
                        }
                    }
                }
            }
        }

        var asd = Physics.RaycastAll(GameManager.Instance.GetScreenToRay());
        _startPos = _rootObj.position;
        if (asd.Any())
        {
            _dragOffset = asd[0].point - _rootObj.position;
        }
    }

    public void OnMouseDragEvent()
    {
        var asd = Physics.RaycastAll(GameManager.Instance.GetScreenToRay());

        if (asd.Length > 0)
        {
            for (int i = 0; i < asd.Length; i++)
            {
                if (asd[i].collider.gameObject.layer == LayerMask.NameToLayer("Selectable"))
                {
                    var pos = asd[i].point;

                    //Anhand der größe offseten
                    _rootObj.position = new Vector3(pos.x, pos.y, pos.z) + _dragOffset / 2;
                }
            }
        }
    }

    public void OnMouseUpEvent()
    {
        SelectionManager selection = GameManager.Instance.GetSelectionManager();

        ISelectable target = selection.TargetHoveredElement;
        if (target != null)
        {
            var field = target.GetRootObj().GetComponent<Field>();

            if (CheckIfFieldIsEmpty(field))
            {
                bool changeField = _predicate.GetField() != field;

                _predicate.GetField().ResetPredicate();
                field.AddPredicateObj(_predicate);

                if (changeField)
                {
                    selection.SelectObj();
                }
            }
            else
            {
                _rootObj.position = _startPos;
            }
        }
        else
        {
            _predicate.CurrentField.DestroyPredicateObj();
            WorldChanged();
        }
    }

    private void WorldChanged()
    {
        var manager = GameManager.Instance;
        if (manager != null)
        {
            manager.GetValidation().SetPresentationLayout();
            manager.GetTextInputField().ResetValidationOnTexts();
        }
    }

    private bool CheckIfFieldIsEmpty(Field field)
    {
        if (field != null && field.GetPredicateInstance() == null)
        {
            return true;
        }

        return false;
    }
}
