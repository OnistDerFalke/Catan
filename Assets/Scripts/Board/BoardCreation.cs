using System;
using UnityEngine;

namespace Board
{
    public class BoardCreation : MonoBehaviour
    {
        [Tooltip("Wysokość trójkąta równobocznego wchodzącego w skład hexagonu")] [SerializeField]
        private float fieldRadius;

        [Tooltip("Położenie pola w pionie")] [SerializeField]
        private float fieldHeight;

        [Tooltip("Wymiary pojedynczej drogi")] [SerializeField]
        private Vector3 roadDimensions;

        [Tooltip("Wymiary miasta")] [SerializeField]
        private Vector3 cityDimensions;

        [Tooltip("Środkowe pole na planszy")] [SerializeField]
        private GameObject centerField;

        [Tooltip("Wewnętrzny pierścień pól na planszy")] [SerializeField]
        private GameObject[] innerRing = new GameObject[6];

        [Tooltip("Zewnętrzny pierścień pól na planszy")] [SerializeField]
        private GameObject[] outerRing = new GameObject[12];
    
        [Tooltip("Drogi pomiędzy polami na planszy")] [SerializeField]
        private GameObject[] roads = new GameObject[72];
    
        [Tooltip("Miasta na planszy")] [SerializeField]
        private GameObject[] cities = new GameObject[54];

        private Vector3 fieldOffset;

        void Start()
        {
            SetTilesPositions();
            if(roads.Length == 72)
                SetRoadsPositions();
            if(cities.Length == 54) 
                SetCitiesPositions();
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
            outerRing[10].gameObject.transform.position = new Vector3(2 * fieldOffset.x, fieldOffset.y, 0f);
            outerRing[11].gameObject.transform.position = new Vector3(-2 * fieldOffset.x, fieldOffset.y, 0f);
        }

        private void SetRoadsPositions()
        {
            var fieldLength = (float)Math.Sqrt(3) * fieldRadius;
        
            //Ustawienie rotacji
            int clf = 0;
            foreach (var road in roads)
            {
                clf++;
                road.transform.localScale = roadDimensions;
                if (clf <= 24) road.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (clf > 24 && clf <=48) road.transform.rotation = Quaternion.Euler(0, 60, 0);
                if (clf > 48) road.transform.rotation = Quaternion.Euler(0, 120, 0);
            }
        
            //Pozycja dróg o podstawowej rotacji
            roads[0].transform.position = new Vector3(0, 0, fieldRadius);
            roads[1].transform.position = new Vector3(0, 0, 3*fieldRadius);
            roads[2].transform.position = new Vector3(0, 0, 5*fieldRadius);
            roads[3].transform.position = new Vector3(0, 0, -fieldRadius);
            roads[4].transform.position = new Vector3(0, 0, -3*fieldRadius);
            roads[5].transform.position = new Vector3(0, 0, -5*fieldRadius);
        
            roads[6].transform.position = new Vector3(1f*fieldLength, 0, 0);
            roads[7].transform.position = new Vector3(1f*fieldLength, 0, 2*fieldRadius);
            roads[8].transform.position = new Vector3(1f*fieldLength, 0, 4*fieldRadius);
            roads[9].transform.position = new Vector3(1f*fieldLength, 0, -2*fieldRadius);
            roads[10].transform.position = new Vector3(1f*fieldLength, 0, -4*fieldRadius);
        
            roads[11].transform.position = new Vector3(-1f*fieldLength, 0, 0);
            roads[12].transform.position = new Vector3(-1f*fieldLength, 0, 2*fieldRadius);
            roads[13].transform.position = new Vector3(-1f*fieldLength, 0, 4*fieldRadius);
            roads[14].transform.position = new Vector3(-1f*fieldLength, 0, -2*fieldRadius);
            roads[15].transform.position = new Vector3(-1f*fieldLength, 0, -4*fieldRadius);
        
            roads[16].transform.position = new Vector3(2f*fieldLength, 0, fieldRadius);
            roads[17].transform.position = new Vector3(2f*fieldLength, 0, 3*fieldRadius);
            roads[18].transform.position = new Vector3(2f*fieldLength, 0, -fieldRadius);
            roads[19].transform.position = new Vector3(2f*fieldLength, 0, -3*fieldRadius);
        
            roads[20].transform.position = new Vector3(-2f*fieldLength, 0, fieldRadius);
            roads[21].transform.position = new Vector3(-2f*fieldLength, 0, 3*fieldRadius);
            roads[22].transform.position = new Vector3(-2f*fieldLength, 0, -fieldRadius);
            roads[23].transform.position = new Vector3(-2f*fieldLength, 0, -3*fieldRadius);

            //Przesunięcie dróg po rotacji
            clf = 0;
            var firstOffset = new Vector3(-fieldLength/2, 0 ,fieldRadius/2);
            var secondOffset = new Vector3(0, 0 , -fieldRadius);
            foreach (var road in roads)
            {
                clf++;
                if (clf > 24 && clf <= 48)
                    road.transform.position = roads[clf-25].transform.position + firstOffset;
                if (clf > 48) 
                    road.transform.position = roads[clf-25].transform.position + secondOffset;
            }
        
            //Podmiana dróg spoza zakresu
        
            //dla rotacji 60 stopni
            roads[26].transform.position = new Vector3(2.5f*fieldLength, 0, 0.5f*fieldRadius);
            roads[37].transform.position = new Vector3(2.5f*fieldLength, 0, 2.5f*fieldRadius);
            roads[45].transform.position = new Vector3(2.5f*fieldLength, 0, -1.5f*fieldRadius);
            //dla rotacji 120 stopni
            roads[53].transform.position = new Vector3(2.5f*fieldLength, 0, -0.5f*fieldRadius);
            roads[63].transform.position = new Vector3(2.5f*fieldLength, 0, -2.5f*fieldRadius);
            roads[71].transform.position = new Vector3(2.5f*fieldLength, 0, 1.5f*fieldRadius);
        }

        private void SetCitiesPositions()
        {
            var fieldLength = centerField.transform.localScale.x / 2;

            //Początkowa pozycja i rozmiar
            foreach (var city in cities)
            {
                city.transform.localScale = cityDimensions;
                city.transform.position = new Vector3(0, 0, 0);
            }
        
            //Miasta parzyste
            cities[0].transform.position = new Vector3(fieldLength, 0, 0);
            cities[1].transform.position = new Vector3(-fieldLength, 0, 0);
            cities[2].transform.position = new Vector3(2 * fieldLength, 0, 0);
            cities[3].transform.position = new Vector3(-2 *fieldLength, 0, 0);
            cities[4].transform.position = new Vector3(4 * fieldLength, 0, 0);
            cities[5].transform.position = new Vector3(-4 * fieldLength, 0, 0);
        
            for (var i = 0; i < 6; i++)
            {
                cities[i + 6].transform.position = cities[i].transform.position 
                                                   + new Vector3(0, 0, 2 * fieldRadius);
                cities[i + 12].transform.position = cities[i].transform.position 
                                                    + new Vector3(0, 0, -2 * fieldRadius);
                if (i < 4)
                {
                    cities[i + 18].transform.position = cities[i].transform.position 
                                                        + new Vector3(0, 0, 4 * fieldRadius);
                    cities[i + 22].transform.position = cities[i].transform.position 
                                                        + new Vector3(0, 0, -4 * fieldRadius);
                }
            }
        
            //Miasta nieparzyste
            cities[26].transform.position = new Vector3(0.5f*fieldLength, 0, fieldRadius);
            cities[27].transform.position = new Vector3(-0.5f*fieldLength, 0, fieldRadius);
            cities[28].transform.position = new Vector3(2.5f*fieldLength, 0, fieldRadius);
            cities[29].transform.position = new Vector3(-2.5f*fieldLength, 0, fieldRadius);
            cities[30].transform.position = new Vector3(3.5f*fieldLength, 0, fieldRadius);
            cities[31].transform.position = new Vector3(-3.5f*fieldLength, 0, fieldRadius);

            for (var i = 26; i < 32; i++)
            {
                cities[i + 6].transform.position = cities[i].transform.position 
                                                   + new Vector3(0, 0, -2 * fieldRadius);
                cities[i + 12].transform.position = cities[i].transform.position 
                                                    + new Vector3(0, 0, 2 * fieldRadius);
                cities[i + 18].transform.position = cities[i].transform.position 
                                                    + new Vector3(0, 0, -4 * fieldRadius);
                if (i < 28)
                {
                    cities[i + 24].transform.position = cities[i].transform.position 
                                                        + new Vector3(0, 0, 4 * fieldRadius);
                    cities[i + 26].transform.position = cities[i].transform.position 
                                                        + new Vector3(0, 0, -6 * fieldRadius);
                }
            }
        }
    }
}