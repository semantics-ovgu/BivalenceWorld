namespace Validator
{
    interface IWorldSignature
    {
        Signature GetSignature();
        WorldResult<bool> Check(WorldParameter parameter);
    }
}
