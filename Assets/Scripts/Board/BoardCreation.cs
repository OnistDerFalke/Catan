using System;
using DataStorage;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Board
{
    //Destiny: Placement of board element on the map
    public class BoardCreation : MonoBehaviour
    {
        private Random random;
        
        //Destiny: Number of elements of any type
        private const int FieldsNumber = 19;
        private const int JunctionsNumber = 54;
        private const int PathsNumber = 72;
        
        //Destiny: Positions of map elements
        private float[,] fieldPositions;
        private float[,] junctionPositions;
        private float[,] pathPositions;
        
        //Destiny: Elements on map
        private GameObject[] fields;
        private GameObject[] junctions;
        private GameObject[] paths;

        //Destiny: Hex tile dimensions
        [Header("Hex tile dimensions")][Space(5)]
        [Tooltip("Length of hex tile triangle")] [SerializeField]
        private float h;
        
        //Destiny: Height dimensions
        [Header("Height dimensions")][Space(5)]
        [Tooltip("Location of fields on y")] [SerializeField]
        private float fieldLocationY;
        [Tooltip("Location of junctions on y")] [SerializeField]
        private float junctionLocationY;
        [Tooltip("Location of paths on y")] [SerializeField]
        private float pathLocationY;
        
        //Destiny: Fields
        [Header("Fields")][Space(5)]
        [Tooltip("Hills field")] [SerializeField]
        private GameObject hillsField;
        [Tooltip("Forest field")] [SerializeField]
        private GameObject forestField;
        [Tooltip("Mountains field")] [SerializeField]
        private GameObject mountainsField;
        [Tooltip("Field field")] [SerializeField]
        private GameObject fieldField;
        [Tooltip("Pasture field")] [SerializeField]
        private GameObject pastureField;
        [Tooltip("Desert field")] [SerializeField]
        private GameObject desertField;
        
        //Destiny: Junctions
        [Header("Junctions")][Space(5)]
        [Tooltip("Blue city")] [SerializeField]
        private GameObject blueCity;
        [Tooltip("Orange city")] [SerializeField]
        private GameObject orangeCity;
        [Tooltip("White city")] [SerializeField]
        private GameObject whiteCity;
        [Tooltip("Red city")] [SerializeField]
        private GameObject redCity;
        [Tooltip("Blue capitol")] [SerializeField]
        private GameObject blueCapitol;
        [Tooltip("Orange capitol")] [SerializeField]
        private GameObject orangeCapitol;
        [Tooltip("White capitol")] [SerializeField]
        private GameObject whiteCapitol;
        [Tooltip("Red capitol")] [SerializeField]
        private GameObject redCapitol;
        [Tooltip("Neutral junction")] [SerializeField]
        private GameObject neutralJunction;
        
        //Destiny: Paths
        [Header("Paths")][Space(5)]
        [Tooltip("Blue path")] [SerializeField]
        private GameObject bluePath;
        [Tooltip("Orange path")] [SerializeField]
        private GameObject orangePath;
        [Tooltip("White path")] [SerializeField]
        private GameObject whitePath;
        [Tooltip("Red path")] [SerializeField]
        private GameObject redPath;
        [Tooltip("Neutral path")] [SerializeField]
        private GameObject neutralPath;
        
        /*
            Arguments:
            h: float -> height of hex tile triangle
            
            Returns:
            float[,] -> array with x and z location of field elements corresponding their indexes
        */
        private float[,] GenerateFieldsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            const int levelCounts = 5;
            
            //Destiny: Number of fields on level given
            int[] p = { 0, 3, 4, 5, 4, 3 };
            
            //Destiny: Number of fields above or on the same level as given: 0, 3, 7, 12, 16, 19
            var sp = new int[6]; 
            
            sp[0] = p[0];
            for (var i = 0; i < levelCounts; i++)
                sp[i + 1] = sp[i] + p[i + 1];
            var fieldCount = sp[levelCounts];
            
            //Destiny: Values of x and z for every field
            var fields = new float[fieldCount, 2];
            for (var i = 0; i < levelCounts; i++)
            {
                for (var j = 0; j < p[i + 1]; j++)
                {
                    //Destiny: Value of x
                    fields[sp[i] + j, 0] = 3 * a - i * 3 * a / 2;
                    
                    //Destiny: Value of z for even and odd levels
                    if (i % 2 == 0) fields[sp[i] + j, 1] = 2 * h * (i / 2 % 2 + 1) - 2 * j * h;
                    else fields[sp[i] + j, 1] = 3 * h - 2 * j * h;
                }
            }
            return fields;
        }
        
        /*
            Arguments:
            h: float -> height of hex tile triangle
            
            Returns:
            float[,] -> array with x and z location of junctions elements corresponding their indexes
        */
        private float[,] GenerateJunctionsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            const int junctionLevelsCount = 12;
            
            //Destiny: Number of junctions on level given
            int[] s = { 0, 3, 4, 4, 5, 5, 6, 6, 5, 5, 4, 4, 3 }; 
            
            //Destiny: Number of junctions above or on the same level: 0, 3, 7, 11, 16, 21, 27, 33, 38, 43, 47, 51, 54
            var ss = new int[junctionLevelsCount + 1];          
            ss[0] = s[0];
            for (var i = 0; i < junctionLevelsCount; i++)
                ss[i + 1] = ss[i] + s[i + 1];

            //Destiny: Value of x and z for every junction
            var junctions = new float[ss[junctionLevelsCount], 2];
            for (var i = 0; i < junctionLevelsCount; i++)
            {
                for (var j = 0; j < s[i + 1]; j++)
                {
                    //Destiny: Levels: 0, 2, 4
                    if (i % 2 == 0 && i < junctionLevelsCount / 2)
                    {
                        junctions[ss[i] + j, 0] = 4 * a - 3 * a * i / 4;
                        junctions[ss[i] + j, 1] = 2 * h + i * h / 2 - 2 * j * h;
                    }
                    //Destiny: Levels: 7, 9, 11
                    else if (i % 2 == 1 && i > junctionLevelsCount / 2)
                    {
                        junctions[ss[i] + j, 0] = -a - 3 * a * (i - 7) / 4;
                        junctions[ss[i] + j, 1] = 4 * h - (i - 7) * h / 2 - 2 * j * h;
                    }
                    //Destiny: Levels: 1, 3, 5
                    else if (i % 2 == 1 && i < junctionLevelsCount / 2)             
                    {
                        junctions[ss[i] + j, 0] = 7 * a / 2 - 3 * a * (i - 1) / 4;
                        junctions[ss[i] + j, 1] = 3 * h + (i - 1) * h / 2 - 2 * j * h;
                    }
                    //Destiny: Levels: 6, 8, 10
                    else if (i % 2 == 0 && i >= junctionLevelsCount / 2)
                    {
                        junctions[ss[i] + j, 0] = -a / 2 - 3 * a * (i - 6) / 4;
                        junctions[ss[i] + j, 1] = 5 * h - (i - 6) * h / 2 - 2 * j * h;
                    }
                }
            }
            return junctions;
        }
        
        /*
            Arguments:
            h: float -> height of hex tile triangle
            
            Returns:
            float[,] -> array with x and z location of paths elements corresponding their indexes
        */
        private float[,] GeneratePathsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            //Destiny: Angle to rotate
            const float angle = 60f;
            
            const int pathLevelsCount = 11;
            
            //Destiny: Number of paths on level given
            int[] d = { 0, 6, 4, 8, 5, 10, 6, 10, 5, 8, 4, 6 };
            
            //Destiny: Number of paths above or on level given: 0, 6, 10, 18, 23, 33, 39, 49, 54, 62, 66, 72
            var sd = new int[pathLevelsCount + 1];                  
            sd[0] = d[0];
            for (var i = 0; i < pathLevelsCount; i++) sd[i + 1] = sd[i] + d[i + 1];

            //Destiny: Value of x and z for every path
            var paths = new float[sd[pathLevelsCount], 3];          
            for (var i = 0; i < pathLevelsCount; i++)
            {
                for (var j = 0; j < d[i + 1]; j++)
                {
                    paths[sd[i] + j, 0] = 15 * a / 4 - 3 * a * i / 4;
                    
                    //Destiny: Levels: 0, 2, 4
                    if (i % 2 == 0 && i < pathLevelsCount / 2)                
                        paths[sd[i] + j, 1] = 5 * h / 2 + i * h / 2 - j * h;
                    //Destiny: Levels: 7, 9
                    else if (i % 2 == 1 && i > pathLevelsCount / 2)           
                        paths[sd[i] + j, 1] = 4 * h - (i - 7) * h / 2 - 2 * j * h;
                    //Destiny: Levels: 1, 3, 5
                    else if (i % 2 == 1 && i <= pathLevelsCount / 2)         
                        paths[sd[i] + j, 1] = 3 * h + (i - 1) * h / 2 - 2 * j * h;
                    //Destiny: Levels: 6, 8, 10
                    else if (i % 2 == 0 && i > pathLevelsCount / 2)        
                        paths[sd[i] + j, 1] = 9 * h / 2 - (i - 6) * h / 2 - j * h;
                    
                    //Destiny: Levels: 1, 3, 5, 7, 9 (no rotation)
                    if (i % 2 == 1) paths[sd[i] + j, 2] = 0;
                }
                
                //Destiny: Levels: 0, 2, 4, 6, 8, 10 (rotation +60/-60 deg)
                if (i % 2 == 0)
                {
                    //Destiny: Every even path (right)
                    for (var j = sd[i]; j < sd[i + 1]; j += 2)
                    {
                        if(i < pathLevelsCount/2)
                            paths[j, 2] = angle;
                        else paths[j, 2] = -angle;
                    }

                    //Destiny: Every odd path (left)
                    for (var j = sd[i] + 1; j < sd[i + 1]; j += 2)
                    {
                        if(i < pathLevelsCount/2)
                            paths[j, 2] = -angle;
                        else paths[j, 2] = angle;
                    }
                }
            }
            return paths;
        }

        //Destiny: Instantiate basic mode hardcoded fields on map
        private void InstantiateBasicModeFields()
        {
            fields[0] = Instantiate(mountainsField);
            fields[1] = Instantiate(pastureField);
            fields[2] = Instantiate(forestField);
            fields[3] = Instantiate(fieldField);
            fields[4] = Instantiate(hillsField);
            fields[5] = Instantiate(pastureField);
            fields[6] = Instantiate(hillsField);
            fields[7] = Instantiate(fieldField);
            fields[8] = Instantiate(forestField);
            fields[9] = Instantiate(desertField);
            fields[10] = Instantiate(forestField);
            fields[11] = Instantiate(mountainsField);
            fields[12] = Instantiate(forestField);
            fields[13] = Instantiate(mountainsField);
            fields[14] = Instantiate(fieldField);
            fields[15] = Instantiate(pastureField);
            fields[16] = Instantiate(hillsField);
            fields[17] = Instantiate(fieldField);
            fields[18] = Instantiate(pastureField);
            
        }

        //Destiny: Instantiate advanced mode fields on map
        private void InstantiateAdvancedModeFields()
        {
            //Destiny: Amount of fields of each type that can be placed
            var fieldsLeft = new[] {4, 4, 4, 3, 3, 1};
            for (var i = 0; i < 19; i++)
            {
                var check = false;
                while (!check)
                {
                    var randomFieldType = random.Next(0, 100) % 6;
                    if (fieldsLeft[randomFieldType] <= 0) continue;
                    fields[i] = randomFieldType switch
                    {
                        0 => Instantiate(forestField),
                        1 => Instantiate(pastureField),
                        2 => Instantiate(fieldField),
                        3 => Instantiate(hillsField),
                        4 => Instantiate(mountainsField),
                        5 => Instantiate(desertField),
                        _ => fields[i]
                    };
                    fieldsLeft[randomFieldType]--;
                    check = true;
                }
            }
        }
        private void SetupMapElements()
        {
            if (GameManager.Mode == GameManager.CatanMode.Basic)
                InstantiateBasicModeFields();
            else if (GameManager.Mode == GameManager.CatanMode.Advanced)
                InstantiateAdvancedModeFields();
                
            //Destiny: Setting up fields
            for (var i = 0; i < FieldsNumber; i++)
            {
                var fieldPosition = new Vector3(fieldPositions[i, 0], 
                    fieldLocationY, fieldPositions[i, 1]);
                fields[i].transform.position = fieldPosition;
                fields[i].SetActive(true);
            }
            //Destiny: Setting up junctions
            for (var i = 0; i < JunctionsNumber; i++)
            {
                var junctionsPosition = new Vector3(junctionPositions[i, 0], 
                    junctionLocationY, junctionPositions[i, 1]); 
                junctions[i] = Instantiate(neutralJunction);
                junctions[i].transform.position = junctionsPosition;
                junctions[i].SetActive(true);
            }
             
            //Destiny: Setting up paths
            for (var i = 0; i < PathsNumber; i++)
            {
                var pathsPosition = new Vector3(pathPositions[i, 0], 
                    pathLocationY, pathPositions[i, 1]);
                paths[i] = Instantiate(neutralPath);
                paths[i].transform.position = pathsPosition;
                paths[i].transform.rotation = Quaternion.Euler(0, pathPositions[i, 2], 0);
                paths[i].SetActive(true);
            }
        }

        void Start()
        {
            random = new Random();
            
            fieldPositions = GenerateFieldsPosition(h);
            junctionPositions = GenerateJunctionsPosition(h);
            pathPositions = GeneratePathsPosition(h);

            fields = new GameObject[FieldsNumber];
            junctions = new GameObject[JunctionsNumber];
            paths = new GameObject[PathsNumber];
            
            SetupMapElements();
        }
    }
}