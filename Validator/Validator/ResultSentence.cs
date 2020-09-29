using System;
using System.Collections.Generic;
using System.Text;
using Validator.World;

namespace Validator
{
    public class ResultSentence<T> : Result<T>
    {
        private EValidationResult _validationResult = EValidationResult.True;
        public EValidationResult ValidationResult => _validationResult;

        protected ResultSentence(T value = default(T), bool isValid = true, string errorText = "", EValidationResult validationResult = EValidationResult.True)
        : base(value, isValid, errorText)
        {
            _validationResult = validationResult;
        }


        public new static ResultSentence<T> CreateResult(T value, string errorText = "")
        {
            return new ResultSentence<T>(value: value, errorText: errorText);
        }

        public new static ResultSentence<T> CreateResult(bool isValid, T value, string errorText = "")
        {
            return new ResultSentence<T>(value: value, isValid: isValid, errorText: errorText);
        }

        public static ResultSentence<T> CreateResult(EValidationResult validationResult, bool isValid, T value, string errorText = "")
        {
            return new ResultSentence<T>(value: value, validationResult: validationResult, isValid: isValid, errorText: errorText);
        }
    }
}
