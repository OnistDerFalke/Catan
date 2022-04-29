using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardElement : MonoBehaviour
{
    void OnMouseDown()
    {
        GameManager.Instance.selectedElement = this;
    }
}
