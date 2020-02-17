using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicate
{
    List<Predicate> GetPredicatesList();
    void AddPredicate(Predicate predicate);
}

