using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameAssets : MonoBehaviour
{
    private static gameAssets _i;
    public static gameAssets i {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<gameAssets>("GameAssets"));
            return _i;
        }
    }

    public Transform pf_DamagePopup;
}
