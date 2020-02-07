using System.Collections.Generic;

namespace Validator
{
    public struct WorldObject
    {
        private List<string> _consts;
        private List<string> _predicates;
        private List<object> _tags;

        
        public WorldObject(List<string> consts, List<string> predicates, List<object> tags)
        {
            _consts = consts;
            _predicates = predicates;
            _tags = tags;
        }


        public List<string> Consts => _consts;
        public List<string> Predicates => _predicates;
        public List<object> Tags => _tags;
    }
}
