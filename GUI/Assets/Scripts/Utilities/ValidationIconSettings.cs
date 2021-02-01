using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Validator.World;

namespace Assets.Scripts.Utilities
{
    [CreateAssetMenu]
    public class ValidationIconSettings : ScriptableObject
    {
        [SerializeField]
        private List<Container> _validationIconList = new List<Container>();

        public Container GetValidationContainer(EValidationResult validation)
        {
            return _validationIconList.Find(c => c.Type == validation);
        }

        [Serializable]
        public class Container
        {
            public EValidationResult Type;
            public Sprite Sprite;
            public LocalizedString TooltipText = default;
        }
    }
}
