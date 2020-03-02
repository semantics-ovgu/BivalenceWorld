using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public struct WorldResult<T>
    {
        private Result<List<Result<T>>> _result;

        public WorldResult(Result<List<Result<T>>> result)
        {
            _result = result;
        }


        public Result<List<Result<T>>> Result => _result;
    }
}
