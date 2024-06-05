using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UmmAPI;
using Newtonsoft.Json.Linq;
using MPW;

public class AltFireBehaviour : MonoBehaviour, ISpecialActivation
{
    public UnityEvent OnActivation;
    public void SpecialActivation()
    {
        if (OnActivation != null)
            OnActivation.Invoke();
    }
}