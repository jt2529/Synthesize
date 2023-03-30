using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable<T>
{
    void Heal(T healAmount);
}
