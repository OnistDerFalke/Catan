using System;
using System.Linq;
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

        //Destiny: Version for main menu (with some restrictions)
        [Header("Main Menu Handler")][Space(5)]
        [Tooltip("True if script is used by main menu.")] [SerializeField]
        private bool isMenu;
        
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
        [Tooltip("Yellow city")] [SerializeField]
        private GameObject yellowCity;
        [Tooltip("White city")] [SerializeField]
        private GameObject whiteCity;
        [Tooltip("Red city")] [SerializeField]
        private GameObject redCity;
        [Tooltip("Blue village")] [SerializeField]
        private GameObject blueVillage;
        [Tooltip("Yellow village")] [SerializeField]
        private GameObject yellowVillage;
        [Tooltip("White village")] [SerializeField]
        private GameObject whiteVillage;
        [Tooltip("Red village")] [SerializeField]
        private GameObject redVillage;
        [Tooltip("Neutral junction")] [SerializeField]
        private GameObject neutralJunction;
        
        //Destiny: Paths
        [Header("Paths")][Space(5)]
        [Tooltip("Blue path")] [SerializeField]
        private GameObject bluePath;
        [Tooltip("Yellow path")] [SerializeField]
        private GameObject yellowPath;
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
        
        //Destiny: Generating array of numbers (refers to indexes) over the fields for advanced game mode
        private int[] GenerateNumbersAdvancedMode()
        {
            const int sideCount = 6;
            const int levelsCount = 5;
            
            //Destiny: Number of fields on level given
            int[] l = { 0, 3, 4, 5, 4, 3 }; 
            
            //Destiny: Number of fields above or on the same level as given: 0, 3, 7, 12, 16, 19
            var sl = new int[levelsCount + 1];        
            sl[0] = l[0];
            for (var i = 0; i < levelsCount; i++)
            {
                sl[i + 1] = sl[i] + l[i + 1];
            }

            var fieldCount = 0;
            var rand = new Random();
            var chosenJunction = rand.Next(0, sideCount);

            var snail = new int[sl[levelsCount]];
            var circles = new int[l[1], sideCount*(l[1] - 1)];

            for (var i = 0; i < l[1] - 1; i++)
            {
                var ringFieldsCount = sideCount * (l[1] - 1 - i);
                var count = 0;

                //Destiny: Generating circle round over every ring
                //Destiny: Left sides
                for (var j = 0; j < levelsCount - 2 * i; j++)               
                {
                    circles[i, count] = sl[j + i] + i;
                    count++;
                }

                //Destiny: Down from left to right without sides ones
                for (var j = sl[levelsCount - i - 1] + 1 + i; j < sl[levelsCount - i] - 1 - i; j++)      
                {
                    circles[i, count] = j;
                    count++;
                }

                //Destiny: Right sides
                for (var j = levelsCount - i; j > i; j--)          
                {
                    circles[i, count] = sl[j] - 1 - i;
                    count++;
                }

                //Destiny: Up from right to left without sides ones
                for (var j = sl[1 + i] - 2 - i; j > sl[i] + i; j--)                 
                {
                    circles[i, count] = j;
                    count++;
                }

                //Destiny: Generating id's of fields in order in which numbers will be placed on it
                //Destiny: End of ring round
                for (var j = (l[1] - 1 - i) * chosenJunction; j > 0; j--)         
                {
                    snail[fieldCount] = circles[i, ringFieldsCount - j];
                    fieldCount++;
                }

                //Destiny: Start of ring round
                for (var j = 0; j < ringFieldsCount - (l[1] - 1 - i) * chosenJunction; j++)           
                {
                    snail[fieldCount] = circles[i, j];
                    fieldCount++;
                }
            }
            
            //Destiny: Center of the board, smallest ring
            snail[fieldCount] = sl[levelsCount / 2] + l[levelsCount / 2 + 1] / 2;
            return snail;
        }

        //Destiny: Instantiate and setup basic mode hardcoded fields on map
        private void InstantiateBasicModeFields()
        {
            var fieldsBiomes = new[]
            {
                mountainsField, pastureField, forestField, fieldField, hillsField, pastureField, hillsField,
                fieldField, forestField, desertField, forestField, mountainsField, forestField, mountainsField,
                fieldField, pastureField, hillsField, fieldField, pastureField
            };
            
            var fieldsNumbers = new[]{10, 2, 9, 12, 6, 4, 10, 9, 11, 0, 3, 8, 8, 3 ,4, 5, 5, 6, 11};
            for (var i = 0; i < FieldsNumber; i++)
            {
                //Destiny: Instantiate fields and set the numbers over it
                fields[i] = Instantiate(fieldsBiomes[i]);
                if (!isMenu)
                {
                    fields[i].GetComponent<FieldElement>().SetNumberAndApply(fieldsNumbers[i]);
                }
            }
        }

        //Destiny: Instantiate advanced mode fields on map
        private void InstantiateAdvancedModeFields()
        {
            //Destiny: Amount of fields of each type that can be placed
            var fieldsLeft = new[] {4, 4, 4, 3, 3, 1};
            var fieldsNumbers = new[] {5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 11, 3};
            var indexes = GenerateNumbersAdvancedMode();
            
            for (var i = 0; i < FieldsNumber; i++)
            {
                var check = false;
                while (!check)
                {
                    //Destiny: Instantiate fields and set the numbers over it
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

            var iterFixed = 0;
            for (var i = 0; i < FieldsNumber; i++)
            {
                if (isMenu) continue;
                if (fields[indexes[i]].GetComponent<FieldElement>().GetTypeInfo() == FieldElement.FieldType.Desert)
                {
                    fields[indexes[i]].GetComponent<FieldElement>().SetNumberAndApply(0);
                }
                else
                {
                    fields[indexes[i]].GetComponent<FieldElement>().SetNumberAndApply(fieldsNumbers[iterFixed]);
                    iterFixed++;
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
                if (!isMenu)
                {
                    fields[i].GetComponent<FieldElement>().id = i;
                }
                fields[i].SetActive(true);
            }
            //Destiny: Setting up junctions
            for (var i = 0; i < JunctionsNumber; i++)
            {
                var junctionsPosition = new Vector3(junctionPositions[i, 0], 
                    junctionLocationY, junctionPositions[i, 1]); 
                junctions[i] = Instantiate(neutralJunction);
                junctions[i].transform.position = junctionsPosition;
                if (!isMenu)
                {
                    junctions[i].GetComponent<JunctionElement>().id = i;
                }
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
                if (!isMenu)
                {
                    paths[i].GetComponent<PathElement>().id = i;
                }
                paths[i].SetActive(true);
            }
        }

        /*
           Arguments:
           color: Player.Player.Color -> color of the player that is the new owner
           id: int -> id of the element that ownership is changed
       */
        private void ChangePathOwner(Player.Player.Color color, int id)
        {
            //Destiny: Keeping properties from older object
            var pos = paths[id].transform.position;
            var rot = paths[id].transform.rotation;
            
            //Destiny: Old object must be destroyed before new one is created
            Destroy(paths[id]);
            
            //Destiny: Choosing new object based on color
            //No need to create neutral path because ownership means something is not neutral
            paths[id] = color switch
            {
                Player.Player.Color.White => Instantiate(whitePath),
                Player.Player.Color.Yellow => Instantiate(yellowPath),
                Player.Player.Color.Red => Instantiate(redPath),
                Player.Player.Color.Blue => Instantiate(bluePath),
                _ => paths[id]
            };
            
            paths[id].GetComponent<PathElement>().id = id;
            
            //Destiny: Properties that changes because of change of ownership
            paths[id].GetComponent<PathElement>().canBuild = false;
            
            //Destiny: Transform is the same as older one
            paths[id].transform.position = pos;
            paths[id].transform.rotation = rot;
            
            //Destiny: New instances are hidden on default
            paths[id].SetActive(true);
            
            //Destiny: Update info for external classes
            BoardManager.Paths[id] = paths[id].GetComponent<PathElement>();
        }
        
        /*
          Arguments:
          color: Player.Player.Color -> color of the player that is the new owner
          id: int -> id of the element that ownership is changed
          upgraded: bool -> if true - city, if false - just village
        */
        private void ChangeJunctionOwner(Player.Player.Color color, int id, bool upgraded)
        {
            //Destiny: Keeping properties from older object
            var pos = junctions[id].transform.position;
            var rot = junctions[id].transform.rotation;
            var fieldsDump = junctions[id].GetComponent<JunctionElement>();
            
            //Destiny: Old object must be destroyed before new one is created
            Destroy(junctions[id]);
            
            //Destiny: Choosing new object based on color and if the object was ever upgraded
            //No need to create empty junction because ownership means something was built
            if (upgraded)
            {
                junctions[id] = color switch
                {
                    Player.Player.Color.White => Instantiate(whiteCity),
                    Player.Player.Color.Yellow => Instantiate(yellowCity),
                    Player.Player.Color.Red => Instantiate(redCity),
                    Player.Player.Color.Blue => Instantiate(blueCity),
                    _ => junctions[id]
                };
            }
            else
            {
                junctions[id] = color switch
                {
                    Player.Player.Color.White => Instantiate(whiteVillage),
                    Player.Player.Color.Yellow => Instantiate(yellowVillage),
                    Player.Player.Color.Red => Instantiate(redVillage),
                    Player.Player.Color.Blue => Instantiate(blueVillage),
                    _ => junctions[id]
                };
            }
            
            junctions[id].GetComponent<JunctionElement>().id = id;
            
            //Destiny: Properties that changes because of change of ownership
            junctions[id].GetComponent<JunctionElement>().canBuild = false;
            junctions[id].GetComponent<JunctionElement>().type =
                upgraded ? JunctionElement.JunctionType.City : JunctionElement.JunctionType.Village;
            
            //Destiny: Properties that must be moved from old to new object
            junctions[id].GetComponent<JunctionElement>().pathsID = fieldsDump.pathsID;
            junctions[id].GetComponent<JunctionElement>().portType = fieldsDump.portType;

            //Destiny: Transform is the same as older one
            junctions[id].transform.position = pos;
            junctions[id].transform.rotation = rot;
            
            //Destiny: New instances are hidden on default
            junctions[id].SetActive(true);
            
            //Destiny: Update info for external classes
            BoardManager.Junctions[id] = junctions[id].GetComponent<JunctionElement>();
        }

        //Destiny: If any ownership change request is available - handle it
        private void HandleOwnerChangeRequests()
        {
            if (isMenu) return;
            
            //Destiny: Handle request if there is any on list
            if (BoardManager.OwnerChangeRequest.Count > 0)
            {
                //Destiny: Take first request from list and start action to handle it
                var info = BoardManager.OwnerChangeRequest.ElementAt(0);
                switch (info.Type)
                {
                    case OwnerChangeRequest.ElementType.Junction:
                        ChangeJunctionOwner(info.Color, info.ID, info.Upgraded);
                        break;
                    case OwnerChangeRequest.ElementType.Path:
                        ChangePathOwner(info.Color, info.ID);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                //Destiny: After making an action remove request from list
                BoardManager.OwnerChangeRequest.RemoveAt(0);
            }
        }

        //Destiny: Set info about board elements (for external classes)
        private void SetBoardElementInfo()
        {
            BoardManager.Fields = new FieldElement[FieldsNumber];
            BoardManager.Junctions = new JunctionElement[JunctionsNumber];
            BoardManager.Paths = new PathElement[PathsNumber];
            
            for (var i = 0; i < FieldsNumber; i++) 
                BoardManager.Fields[i] = fields[i].GetComponent<FieldElement>();
            for (var i = 0; i < JunctionsNumber; i++) 
                BoardManager.Junctions[i] = junctions[i].GetComponent<JunctionElement>();
            for (var i = 0; i < FieldsNumber; i++) 
                BoardManager.Paths[i] = paths[i].GetComponent<PathElement>();
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
            SetBoardElementInfo();
        }

        void Update()
        {
            //Destiny: Handle ownership changes (like listening for new requests)
            HandleOwnerChangeRequests();
        }
    }
}