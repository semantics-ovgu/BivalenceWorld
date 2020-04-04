using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public class Result<T>
    {
        #region Variables & Getter

        private string _errorText = "";
        public string Message => _errorText;

        private bool _isValid = true;
        public bool IsValid => _isValid;

        private T _value = default(T);
        public T Value => _value;

        #endregion


        #region Constructor

        private Result()
        {

        }

        #endregion


        #region Public

        public bool HasValue => _value != null;

        #endregion


        #region Factory Methods

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
