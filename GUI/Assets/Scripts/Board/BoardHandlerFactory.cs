using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Boards;
using UnityEngine;

namespace Assets.Scripts.Boards
{
    public abstract class BoardHandlerFactory : MonoBehaviour
    {
        public abstract BoardHandler CreateBoardHandler();
    }
}
