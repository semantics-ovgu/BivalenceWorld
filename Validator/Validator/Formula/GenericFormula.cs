using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal abstract class GenericFormula<T> : Formula where T : Formula
    {
        private List<T> _arguments = new List<T>();

        public GenericFormula(List<T> arguments, string name, string rawFormula) : base(name, rawFormula)
        {
            _arguments = arguments;
        }

        public List<T> Arguments => _arguments;

        protected IEnumerable<R> GetArgumentsOfType<R>() where R : class
        {
            foreach (var item in Arguments)
                yield return item as R;
        }
    }
}
