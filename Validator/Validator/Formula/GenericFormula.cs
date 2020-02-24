﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal abstract class GenericFormula<T> : Formula
    {
        private List<T> _arguments = new List<T>();

        public GenericFormula(List<T> arguments, string name, string rawFormula) : base(name, rawFormula)
        {
            _arguments = arguments;
        }

        public List<T> Arguments => _arguments;
    }
}