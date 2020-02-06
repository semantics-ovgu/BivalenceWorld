using System;

namespace Validator
{
    public abstract class AWorld
    {
        private Signature _signature = default;


        public AWorld(Signature signature) => _signature = signature;

        /// <summary>
        /// Führt alle drei Steps:
        /// 1: Consts
        /// 2: Predicates
        /// 3: Functions:
        /// UND
        /// Die Validierung mit den Sätzen durch
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool Check(WorldParameter parameter);

        public Signature GetSignature() => _signature;

        public static bool CheckWorld(WorldParameter parameter)
        {
            return false;
        }
    }
}
