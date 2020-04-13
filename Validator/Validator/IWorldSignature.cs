using Validator.World;

namespace Validator
{
    public interface IWorldSignature
    {
        Signature GetSignature();
        WorldResult<EValidationResult> Check(WorldParameter parameter);
    }
}
