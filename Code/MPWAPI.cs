using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//* API version: 1.3.4
namespace MPW
{
    public interface ISpecialActivation
    {
        void SpecialActivation();
    }
    public interface ISpecialActivationContinuous
    {
        void SpecialActivationContinuous(sbyte state);
    }
}
