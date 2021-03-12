using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World.UnityGUI
{
    public class GUI_World_BoardView : MonoBehaviour
    {
        [SerializeField]
        private Board _prefab = default;
        private Board _instanceBoard = null;

        private void InstantiateBoard()
        {
            if (_instanceBoard == null)
            {
                _instanceBoard = GameManager.Instance.RegisterBoard(_prefab);
            }
        }

        private void OnEnable()
        {
            if (_instanceBoard == null)
            {
                InstantiateBoard();
            }
            GameManager.Instance.SetCurrentActiveBoard(_instanceBoard);
            GameManager.Instance.GetSelectionManager().ResetSelection();
            GameManager.Instance.GetValidation().StartCalculator();
        }

        private void OnDestroy()
        {
            GameManager.Instance.RemoveBoard(_instanceBoard);
        }
    }
}
