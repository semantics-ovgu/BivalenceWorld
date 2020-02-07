using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class TarskiWorld : IWorldPL1Structure, IWorldSignature
    {
        private PL1Structure _pl1Structure = new PL1Structure();
        private Signature _signature = new Signature();


        public TarskiWorld()
        {
            _signature = CreateTarskiWorld();
        }


        private Signature CreateTarskiWorld()
        {
            List<string> consts = new List<string>()
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "f"
            };

            List<(string, int)> predicates = new List<(string, int)>
            {
                ("Tet", 1),
                ("Cube", 1),
                ("Small", 1),
                ("Big", 1),
                ("Bigger", 2),
                ("FrontOf", 2),
                ("InBetween", 3)
            };

            List<(string, int)> functions = new List<(string, int)>
            {
                ("isFront", 1),
                ("leftFarAway", 1)
            };

            return new Signature(consts, predicates, functions);
        }


        public PL1Structure GetPl1Structure()
        {
            return _pl1Structure;
        }

        public Signature GetSignature()
        {
            return _signature;
        }

        public WorldResult<bool> Check(WorldParameter parameter)
        {
            foreach (var data in parameter.Data)
            {
                //--Consts in PL1--//
                //--Check Signature--//
                foreach (var con in data.Consts)
                {
                    if (_signature.Consts.Contains(con))
                    {
                        _pl1Structure.AddConst(con);
                    }
                }


                //--Pred in PL1--//
                foreach (var pred in data.Predicates)
                {
                    if (_signature.Predicates.Any(s => s.Item1 == pred))
                    {
                        //--REfactorn mit oben--//
                        foreach (var con in data.Consts)
                        {
                            _pl1Structure.AddPredicate(pred, new List<string> { con });
                        }
                    }
                }
            }

            //--Pred in PL1--//
            //--Funct in PL1--//
            //--Sentences überprüfen--//

            return new WorldResult<bool>();
        }
    }
}
