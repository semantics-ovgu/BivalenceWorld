using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Boards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Boards
{
    public class MultipleBoardsHandler : BoardHandlerFactory, BoardHandler
    {
        private Board _currentBoard = null;
        private int _currentIndexWorld = 0;

        private List<Board> _boards = new List<Board>();

        [SerializeField]
        private Vector3 _boardsOffset = new Vector3(10, 10);
        [SerializeField]
        private Transform _boardsAnchor = default;
        [SerializeField]
        private Board _boardPrefab = default;
        [SerializeField]
        private Camera _textureCamera = default;
        [SerializeField]
        private Button _2DViewButtonSwitch = default;

        private bool _is2DView = false;

        private void Awake()
        {
            _2DViewButtonSwitch.onClick.AddListener(ButtonSwitchEventListener);
        }

        private void ButtonSwitchEventListener()
        {
            _is2DView = !_is2DView;
            SetCamera();
        }

        public Board GetCurrentBoard()
        {
            return _currentBoard;
        }

        public Board CreateNewBoard(Board board)
        {
            Board newBoard = Instantiate(board, _boardsAnchor, true);
            newBoard.transform.position = _boardsAnchor.position + _currentIndexWorld * _boardsOffset;

            _currentIndexWorld++;

            _boards.Add(newBoard);

            return newBoard;
        }

        public void SetCurrentBoard(Board board)
        {
            _currentBoard = board;
            SetCamera();
        }

        private void SetCamera()
        {
            if (_currentBoard != null)
            {
                if (_is2DView)
                {
                    _textureCamera.transform.SetPositionAndRotation(_currentBoard.Cam2DPos.position, _currentBoard.Cam2DPos.rotation);
                    _textureCamera.orthographic = true;
                }
                else
                {
                    _textureCamera.transform.SetPositionAndRotation(_currentBoard.CamPos.position, _currentBoard.CamPos.rotation);
                    _textureCamera.orthographic = false;
                }
            }
        }

        public void RemoveBoard(Board board)
        {
            _boards.Remove(board);
            if (board != null)
            {
                Destroy(board.gameObject);
            }

            _currentBoard = _boards.LastOrDefault();
        }

        public bool Is2DView()
        {
            return _is2DView;
        }

        public override BoardHandler CreateBoardHandler()
        {
            return this;
        }

        public List<Board> GetAllBoards()
        {
            return _boards;
        }
    }
}
