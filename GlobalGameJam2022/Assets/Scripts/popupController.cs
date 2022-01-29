using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class popupController : MonoBehaviour
{
    public Transform pf_damagePopup;
    public damagePopup Create(Vector3 position, int damageAmount, Color color)
    {
        Transform damagePopupTransform = Instantiate(pf_damagePopup, position, Quaternion.identity);
        damagePopup damagePopup = damagePopupTransform.GetComponent<damagePopup>();
        damagePopup.Setup(damageAmount, color);
        return damagePopup;
    }
   
}
