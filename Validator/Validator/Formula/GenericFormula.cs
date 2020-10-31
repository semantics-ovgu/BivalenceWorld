using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public abstract class GenericFormula<T> : Formula where T : Formula
    {
        private List<T> _arguments = new List<T>();

        public GenericFormula(List<T> arguments, string name, string formattedFormula) : base(name, formattedFormula)
        {
            _arguments = arguments;
        }

        public List<T> Arguments => _arguments;

        protected IEnumerable<R> GetArgumentsOfType<R>() where R : class
        {
            foreach (var item in Arguments)
                yield return item as R;
        }

        protected virtual string ArgumentsToString(Dictionary<string, string> dictVariables)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var argument in Arguments)
            {
                builder.AppendLine(argument.ReformatFormula(dictVariables));
            }
            return builder.ToString();
        }
    }
}
