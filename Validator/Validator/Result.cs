using System;
using System.Collections.Generic;
using System.Text;
using Validator.World;

namespace Validator
{
    public class Result<T>
    {
        #region Variables & Getter

        private string _errorText = "";
        public string ErrorMessage => _errorText;

        private bool _isValid = true;
        public bool IsValid => _isValid;

        private T _value = default(T);
        public T Value => _value;

        #endregion


        #region Constructor

        protected Result(T value = default(T), bool isValid = true, string errorText = "")
        {
            _errorText = errorText;
            _isValid = isValid;
            _value = value;
        }

        #endregion


        #region Public

        public bool HasValue => _value != null;

        #endregion


        #region Factory Methods

        public static Result<T> CreateResult(T value, string errorText = "")
        {
            return new Result<T>
            {
                _errorText = errorText,
                _isValid = true,
                _value = value
            };
        }

        public static Result<T> CreateResult(bool isValid, T value, string errorText = "")
        {
            return new Result<T>
            {
                _errorText = errorText,
                _isValid = isValid,
                _value = value
            };
        }

        #endregion
    }
}
