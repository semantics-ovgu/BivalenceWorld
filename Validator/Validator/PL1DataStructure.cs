namespace Validator
{
    public struct PL1DataStructure
    {
        private ConstDictionary _const;
        private PredicateDictionary _predicates;
        private FunctionDictionary _functions;


        public PL1DataStructure(ConstDictionary @const, PredicateDictionary predicates, FunctionDictionary functions)
        {
            _const = @const;
            _predicates = predicates;
            _functions = functions;
        }


        public ConstDictionary Consts => _const;
        public PredicateDictionary Predicates => _predicates;
        public FunctionDictionary Functions => _functions;

        public void Clear()
        {
            _const.Clear();
            _predicates.Clear();
            _functions.Clear();
        }
    }
}
