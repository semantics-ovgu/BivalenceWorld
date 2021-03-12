using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Boards;
using UnityEngine;

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
            _textureCamera.transform.SetPositionAndRotation(board.CamPos.position, board.CamPos.rotation);
            _currentBoard = board;
        }

        public void RemoveBoard(Board board)
        {
            _boards.Remove(board);

            _currentBoard = _boards.LastOrDefault();
        }

        public override BoardHandler CreateBoardHandler()
        {
            return this;
        }
    }
}
