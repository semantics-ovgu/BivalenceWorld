using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public class TarskiWorld : AWorld, IWorldPL1Structure
    {
        private PL1Structure _pl1Structure = new PL1Structure();


        public TarskiWorld() : base(Signature.CreateTarskiWorld())
        {

        }


        public override bool Check(WorldParameter parameter)
        {
            return true;
        }

        public PL1Structure GetPl1Structure()
        {
            return _pl1Structure;
        }

        public static bool CheckWorld(WorldParameter parameter)
        { 
        }
    }
}
