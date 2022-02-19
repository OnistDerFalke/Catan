using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreation : MonoBehaviour
{
    [Tooltip("Wysokość trójkąta równobocznego składającego się na sześcienne pole.")]
    [SerializeField] private float fieldRadius;
    
    [Tooltip("Wysokość/położenie pola w pionie.")]
    [SerializeField] private float fieldHeight;

    [Tooltip("Środkowe pole na planszy.")]
    [SerializeField] private GameObject centerField;
    
    [Tooltip("Wewnętrzny pierścień pól na planszy.")]
    [SerializeField] private GameObject[] innerRing = new GameObject[6];
    
    [Tooltip("Zewnętrzny pierścień pól na planszy.")]
    [SerializeField] private GameObject[] outerRing = new GameObject[12];

    private Vector3 fieldOffset;
    void Start()
    {
        //Ustawienie pozycji pól na planszy
        SetTilesPositions();
    }

    private void SetTilesPositions()
    {
        //Wyznaczanie przesunięć
        fieldOffset = new Vector3((float) Math.Sqrt(3) * fieldRadius, fieldHeight, fieldRadius);

        //Ustawienie pozycji centralnego pola
        centerField.gameObject.transform.position = new Vector3(0f, fieldOffset.y, 0f);

        //Ustawienie pozycji wewnętrznego pierścienia
        innerRing[0].gameObject.transform.position = new Vector3(0f, fieldOffset.y, 2 * fieldOffset.z);
        innerRing[1].gameObject.transform.position = new Vector3(0f, fieldOffset.y, -2 * fieldOffset.z);
        innerRing[2].gameObject.transform.position = new Vector3(fieldOffset.x, fieldOffset.y, fieldOffset.z);
        innerRing[3].gameObject.transform.position = new Vector3(-fieldOffset.x, fieldOffset.y, fieldOffset.z);
        innerRing[4].gameObject.transform.position = new Vector3(fieldOffset.x, fieldOffset.y, -fieldOffset.z);
        innerRing[5].gameObject.transform.position = new Vector3(-fieldOffset.x, fieldOffset.y, -fieldOffset.z);

        //Ustawienie pozycji zewnętrznego pierścienia
        outerRing[0].gameObject.transform.position = new Vector3(0f, fieldOffset.y, 4 * fieldOffset.z);
        outerRing[1].gameObject.transform.position = new Vector3(0f, fieldOffset.y, -4 * fieldOffset.z);
        outerRing[2].gameObject.transform.position = new Vector3(2 * fieldOffset.x, fieldOffset.y, 2 * fieldOffset.z);
        outerRing[3].gameObject.transform.position = new Vector3(-2 * fieldOffset.x, fieldOffset.y, 2 * fieldOffset.z);
        outerRing[4].gameObject.transform.position = new Vector3(2 * fieldOffset.x, fieldOffset.y, -2 * fieldOffset.z);
        outerRing[5].gameObject.transform.position = new Vector3(-2 * fieldOffset.x, fieldOffset.y, -2 * fieldOffset.z);
        outerRing[6].gameObject.transform.position = new Vector3(fieldOffset.x, fieldOffset.y, 3f * fieldOffset.z);
        outerRing[7].gameObject.transform.position = new Vector3(-fieldOffset.x, fieldOffset.y, 3f * fieldOffset.z);
        outerRing[8].gameObject.transform.position = new Vector3(fieldOffset.x, fieldOffset.y, -3f * fieldOffset.z);
        outerRing[9].gameObject.transform.position = new Vector3(-fieldOffset.x, fieldOffset.y, -3f * fieldOffset.z);
        outerRing[10].gameObject.transform.position = new Vector3(2 * fieldOffset.x , fieldOffset.y, 0f);
        outerRing[11].gameObject.transform.position = new Vector3(-2 * fieldOffset.x , fieldOffset.y, 0f);
    }
}
