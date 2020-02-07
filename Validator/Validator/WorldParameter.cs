using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public struct WorldParameter
    {
        private List<WorldObject> _data;
        private List<string> _sentences;


        public WorldParameter(List<WorldObject> data, List<string> sentences)
        {
            _data = data;
            _sentences = sentences;
        }


        public List<string> Sentences => _sentences;
        public List<WorldObject> Data => _data;
    }
}
