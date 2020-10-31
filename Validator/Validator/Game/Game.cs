using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Validator.Game
{
    public class Game : ICloneable
    {
        private const string GENERIC_CONST_KEY = "n";

        private readonly Formula _formula;
        private bool _guess;
        private string _sentence;
        private AWorld _world;
        private List<AMove> _history = new List<AMove>();
        private AMove _currentMove = null;
        private Queue<AMove> _nextMoves = new Queue<AMove>();

        private List<string> _genericConsts = new List<string>();
        private int _currentConstIndex = 1;

        public Game(string sentence, AWorld world, bool guess)
        {
            _sentence = sentence;
            _world = world;
            _guess = guess;

            try
            {
                _formula = PL1Parser.Parse(sentence);
            }
            catch (Exception e)
            {
                _formula = null;
            }
        }

        internal string AddGenericConstString()
        {
            string key = GENERIC_CONST_KEY + _currentConstIndex;
            _genericConsts.Add(key);
            _currentConstIndex++;

            return key;
        }

        internal void ReplaceWorldObject(WorldObject obj)
        {
            var list = new List<WorldObject>();

            foreach (var worldObj in _world.WorldParameter.Data)
            {
                if (worldObj.Equals(obj))
                {
                    list.Add(obj);
                }
                else
                {
                    list.Add(worldObj);
                }
            }

            _world.Check(new WorldParameter(list, new List<string>() { _sentence }));
        }

        public List<WorldObject> WorldObjects => _world.WorldParameter.Data;

        public bool Guess => _guess;

        internal void SetGuess(bool guess) => _guess = guess;

        internal void SetNextMove(AMove move)
        {
            _nextMoves.Enqueue(move);
        }

        internal AWorld World => _world;

        public bool IsValid => _formula != null;

        public AMove Play()
        {
            if (_currentMove != null)
            {
                if (_currentMove.CanBePlayed())
                {
                    _currentMove.Play();
                    return NextMove();
                }
                else
                {
                    return _currentMove;
                }
            }
            else
            {
                return Initialize();
            }
        }

        private AMove NextMove()
        {
            return _currentMove = _currentMove.NextMove(this);
        }

        private AMove Initialize()
        {
            return _currentMove = _formula.CreateNextMove(this, new Dictionary<string, string>());
        }

        public object Clone()
        {

            return new Game(_sentence, _world, _guess)
            {
                _history = _history
            };
        }
    }
}
