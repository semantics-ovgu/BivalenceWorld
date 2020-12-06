using System;
using System.Collections.Generic;
using System.Linq;

namespace Validator
{
    public struct WorldObject
    {

        private List<string> _consts;
        private List<string> _predicates;
        private List<object> _tags;


        public WorldObject(List<string> consts, List<string> predicates, List<object> tags)
        {
            _consts = new List<string>();
            _consts.AddRange(consts.Select(s => s));
            _predicates = predicates;
            _tags = tags;
        }

        public WorldObject(WorldObject other)
        {
            _consts = new List<string>();
            _predicates = new List<string>();
            _tags = new List<object>();

            _consts.AddRange(other.Consts);
            _predicates.AddRange(other._predicates);
            _tags.AddRange(other._tags);
        }


        public List<string> Consts
        {
            get => this._consts;
            set => _consts = value;
        }

        public List<string> Predicates
        {
            get => this._predicates;
            set => _predicates = value;
        }

        public List<object> Tags
        {
            get => this._tags;
            set => _tags = value;
        }

        public override bool Equals(object other)
        {
            if (other is WorldObject otherObj)
            {
                return Tags.SequenceEqual(otherObj.Tags);
            }
            return base.Equals(other);
        }
    }
}
