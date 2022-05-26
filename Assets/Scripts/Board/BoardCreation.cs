using System;
using System.Linq;
using DataStorage;
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

        //Destiny: Number of levels of any elements
        private const int FieldLevelsNumber = 5;
        private const int JunctionLevelsNumber = 12;

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
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Array of numbers (refers to indexes) over the fields for advanced game mode</returns>
        private int[] GenerateNumbersAdvancedMode()
        {
            const int sideCount = 6;
            const int levelsCount = 5;
            
            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 }; 
            
            //Destiny: Number of fields above or on the same level as given: 0, 3, 7, 12, 16, 19
            var sf = new int[levelsCount + 1];        
            sf[0] = f[0];
            for (var i = 0; i < levelsCount; i++)
            {
                sf[i + 1] = sf[i] + f[i + 1];
            }

            var fieldCount = 0;
            var rand = new Random();
            var chosenJunction = rand.Next(0, sideCount);

            var snail = new int[sf[levelsCount]];
            var circles = new int[f[1], sideCount*(f[1] - 1)];

            for (var i = 0; i < f[1] - 1; i++)
            {
                var ringFieldsCount = sideCount * (f[1] - 1 - i);
                var count = 0;

                //Destiny: Generating circle round over every ring
                //Destiny: Left sides
                for (var j = 0; j < levelsCount - 2 * i; j++)               
                {
                    circles[i, count] = sf[j + i] + i;
                    count++;
                }

                //Destiny: Down from left to right without sides ones
                for (var j = sf[levelsCount - i - 1] + 1 + i; j < sf[levelsCount - i] - 1 - i; j++)      
                {
                    circles[i, count] = j;
                    count++;
                }

                //Destiny: Right sides
                for (var j = levelsCount - i; j > i; j--)          
                {
                    circles[i, count] = sf[j] - 1 - i;
                    count++;
                }

                //Destiny: Up from right to left without sides ones
                for (var j = sf[1 + i] - 2 - i; j > sf[i] + i; j--)                 
                {
                    circles[i, count] = j;
                    count++;
                }

                //Destiny: Generating id's of fields in order in which numbers will be placed on it
                //Destiny: End of ring round
                for (var j = (f[1] - 1 - i) * chosenJunction; j > 0; j--)         
                {
                    snail[fieldCount] = circles[i, ringFieldsCount - j];
                    fieldCount++;
                }

                //Destiny: Start of ring round
                for (var j = 0; j < ringFieldsCount - (f[1] - 1 - i) * chosenJunction; j++)           
                {
                    snail[fieldCount] = circles[i, j];
                    fieldCount++;
                }
            }
            
            //Destiny: Center of the board, smallest ring
            snail[fieldCount] = sf[levelsCount / 2] + f[levelsCount / 2 + 1] / 2;
            return snail;
        }

        /// <summary>
        /// Instantiate and setup basic mode hardcoded fields on map
        /// </summary>
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

        /// <summary>
        /// Instantiate advanced mode fields on map
        /// </summary>
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

        /// <summary>
        /// Distribution of elements on the map based on the generated positions
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">color of the player that is the new owner</param>
        /// <param name="id">id of the element that ownership is changed</param>
        private void ChangePathOwner(Player.Player.Color color, int id)
        {
            //Destiny: Keeping properties from older object
            var pos = paths[id].transform.position;
            var rot = paths[id].transform.rotation;
            var pathsDump = paths[id].GetComponent<PathElement>();

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


            //Destiny: New instances are hidden on default
            paths[id].SetActive(true);

            paths[id].GetComponent<PathElement>().id = id;
            
            //Destiny: Properties that changes because of change of ownership
            paths[id].GetComponent<PathElement>().canBuild = false;

            //Destiny: Properties that must be moved from old to new object
            paths[id].GetComponent<PathElement>().pathsID = pathsDump.pathsID;
            paths[id].GetComponent<PathElement>().junctionsID = pathsDump.junctionsID;

            //Destiny: Transform is the same as older one
            paths[id].transform.position = pos;
            paths[id].transform.rotation = rot;
            
            //Destiny: Update info for external classes
            BoardManager.Paths[id] = paths[id].GetComponent<PathElement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">color of the player that is the new owner</param>
        /// <param name="id">id of the element that ownership is changed</param>
        /// <param name="upgraded">if true - city, if false - just village</param>
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

            //Destiny: New instances are hidden on default
            junctions[id].SetActive(true);

            junctions[id].GetComponent<JunctionElement>().id = id;

            //Destiny: Properties that changes because of change of ownership
            junctions[id].GetComponent<JunctionElement>().canBuild = false;
            junctions[id].GetComponent<JunctionElement>().type =
                upgraded ? JunctionElement.JunctionType.City : JunctionElement.JunctionType.Village;

            //Destiny: Properties that must be moved from old to new object
            junctions[id].GetComponent<JunctionElement>().pathsID = fieldsDump.pathsID;
            junctions[id].GetComponent<JunctionElement>().junctionsID = fieldsDump.junctionsID;
            junctions[id].GetComponent<JunctionElement>().fieldsID = fieldsDump.fieldsID;
            junctions[id].GetComponent<JunctionElement>().portType = fieldsDump.portType;

            //Destiny: Transform is the same as older one
            junctions[id].transform.position = pos;
            junctions[id].transform.rotation = rot;
            
            //Destiny: Update info for external classes
            BoardManager.Junctions[id] = junctions[id].GetComponent<JunctionElement>();

            //Destiny: Block adjacent junctions
            BoardManager.Junctions[id].junctionsID.ForEach(delegate(int junctionId) 
            { 
                BoardManager.Junctions[junctionId].canBuild = false;
                junctions[junctionId].GetComponent<JunctionElement>().canBuild = false;
            });
        }

        /// <summary>
        /// If any ownership change request is available - handle it
        /// </summary>
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

        /// <summary>
        /// Set info about board elements (for external classes)
        /// </summary>
        private void SetBoardElementInfo()
        {
            BoardManager.Fields = new FieldElement[FieldsNumber];
            BoardManager.Junctions = new JunctionElement[JunctionsNumber];
            BoardManager.Paths = new PathElement[PathsNumber];
            
            for (var i = 0; i < FieldsNumber; i++)
                BoardManager.Fields[i] = fields[i].GetComponent<FieldElement>();
            for (var i = 0; i < JunctionsNumber; i++)
                BoardManager.Junctions[i] = junctions[i].GetComponent<JunctionElement>();
            for (var i = 0; i < PathsNumber; i++)
                BoardManager.Paths[i] = paths[i].GetComponent<PathElement>();
        }

        void Start()
        {
            //Destiny: Creating instances of methods providers
            random = new Random();
            var distributor = new ElementDistributor();
            var positioner = new ElementPositioner();
            var neighbourGenerator = new NeighbourGenerator(
                FieldLevelsNumber, JunctionLevelsNumber, JunctionsNumber, FieldsNumber, PathsNumber);
            
            //Destiny: Getting positions from the positioner
            fieldPositions = positioner.GenerateFieldsPosition(h);
            junctionPositions = positioner.GenerateJunctionsPosition(h);
            pathPositions = positioner.GeneratePathsPosition(h);

            //Destiny: Generating element neighbours
            neighbourGenerator.GenerateElementNeighbors();

            //Destiny: Preparing empty arrays for elements
            fields = new GameObject[FieldsNumber];
            junctions = new GameObject[JunctionsNumber];
            paths = new GameObject[PathsNumber];
            
            //Destiny: Setting all elements on the board
            SetupMapElements();

            //Destiny: This happens only if script is not running on menu scene
            if (!isMenu)
            {
                //Destiny: Setting info about elements for external classes
                SetBoardElementInfo();
                
                //Destiny: Setting up neighbours of elements
                neighbourGenerator.SetupElementNeighbors();
                
                //Destiny: Setting up initial elements distribution to players
                distributor.SetupInitialDistribution();
            }
        }
        
        void Update()
        {
            //Destiny: Handle ownership changes (like listening for new requests)
            HandleOwnerChangeRequests();
        }
    }
}