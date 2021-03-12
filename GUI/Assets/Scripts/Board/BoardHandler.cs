using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Boards
{
    public interface BoardHandler
    {
        Board GetCurrentBoard();

        Board CreateNewBoard(Board board);

        void SetCurrentBoard(Board board);

        void RemoveBoard(Board board);
    }
}
