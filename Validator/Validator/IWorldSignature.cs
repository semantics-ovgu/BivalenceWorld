namespace Validator
{
    public interface IWorldSignature
    {
        Signature GetSignature();
        WorldResult<bool> Check(WorldParameter parameter);
    }
}
