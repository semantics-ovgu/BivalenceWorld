using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.World
{
    public abstract class GUI_Factory_TabsButtonPanel : MonoBehaviour
    {
        public abstract TabsButtonPanel Create();
    }
}
