using System;
using System.Linq;
using Assets.Scripts.Board.States;
using DataStorage;
using UnityEngine;
using static Assets.Scripts.Board.States.FieldState;
using static Assets.Scripts.Board.States.JunctionState;
using static Board.BoardElement;
using static Board.States.GameState;
using Random = System.Random;

namespace Board
{
    //Destiny: Placement of board element on the map
    public class BoardCreation : MonoBehaviour
    {
        private Random random;

        //Destiny: Part of triangle's length
        private const float p = 0.75f;

        //Destiny: Positions of map elements
        private float[,] fieldPositions;
        private float[,] junctionPositions;
        private float[,] pathPositions;
        private float[,] portPositions;
        private float[,] portInfoPositions;

        //Destiny: Elements on map
        private GameObject[] fields;
        private GameObject[] junctions;
        private GameObject[] paths;
        private GameObject[] ports;

        //Destiny: Version for main menu (with some restrictions)
        [Header("Main Menu Handler")][Space(5)]
        [Tooltip("True if script is used by main menu.")][SerializeField]
        private bool isMenu;
        
        //Destiny: Hex tile dimensions
        [Header("Hex tile dimensions")][Space(5)]
        [Tooltip("Length of hex tile triangle")][SerializeField]
        private float h;
        
        //Destiny: Height dimensions
        [Header("Height dimensions")][Space(5)]
        [Tooltip("Location of fields on y")][SerializeField]
        private float fieldLocationY;
        [Tooltip("Location of junctions on y")][SerializeField]
        private float junctionLocationY;
        [Tooltip("Location of paths on y")][SerializeField]
        private float pathLocationY;
        [Tooltip("Location of ports on y")][SerializeField]
        private float portLocationY;
        
        //Destiny: Fields
        [Header("Fields")][Space(5)]
        [Tooltip("Hills field")][SerializeField]
        private GameObject hillsField;
        [Tooltip("Forest field")][SerializeField]
        private GameObject forestField;
        [Tooltip("Mountains field")][SerializeField]
        private GameObject mountainsField;
        [Tooltip("Field field")][SerializeField]
        private GameObject fieldField;
        [Tooltip("Pasture field")][SerializeField]
        private GameObject pastureField;
        [Tooltip("Desert field")][SerializeField]
        private GameObject desertField;
        
        //Destiny: Junctions
        [Header("Junctions")][Space(5)]
        [Tooltip("Blue city")][SerializeField]
        private GameObject blueCity;
        [Tooltip("Yellow city")][SerializeField]
        private GameObject yellowCity;
        [Tooltip("White city")][SerializeField]
        private GameObject whiteCity;
        [Tooltip("Red city")][SerializeField]
        private GameObject redCity;
        [Tooltip("Blue village")][SerializeField]
        private GameObject blueVillage;
        [Tooltip("Yellow village")][SerializeField]
        private GameObject yellowVillage;
        [Tooltip("White village")][SerializeField]
        private GameObject whiteVillage;
        [Tooltip("Red village")][SerializeField]
        private GameObject redVillage;
        [Tooltip("Neutral junction")][SerializeField]
        private GameObject neutralJunction;
        
        //Destiny: Paths
        [Header("Paths")][Space(5)]
        [Tooltip("Blue path")][SerializeField]
        private GameObject bluePath;
        [Tooltip("Yellow path")][SerializeField]
        private GameObject yellowPath;
        [Tooltip("White path")][SerializeField]
        private GameObject whitePath;
        [Tooltip("Red path")][SerializeField]
        private GameObject redPath;
        [Tooltip("Neutral path")][SerializeField]
        private GameObject neutralPath;
        
        //Destiny: Ports
        [Header("Ports")][Space(5)]
        [Tooltip("Port Info")][SerializeField]
        private GameObject portInfo;
        [Tooltip("Port")][SerializeField]
        private GameObject port;        
        

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
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
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
        private void InstantiateModeFields()
        {
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
            {
                //Destiny: Instantiate fields and set the numbers over it
                switch (DataManager.FieldStates[i].type)
                {
                    case FieldType.Desert:
                        fields[i] = Instantiate(desertField);
                        break;
                    case FieldType.Forest:
                        fields[i] = Instantiate(forestField);
                        break;
                    case FieldType.Pasture:
                        fields[i] = Instantiate(pastureField);
                        break;
                    case FieldType.Field:
                        fields[i] = Instantiate(fieldField);
                        break;
                    case FieldType.Hills:
                        fields[i] = Instantiate(hillsField);
                        break;
                    case FieldType.Mountains:
                        fields[i] = Instantiate(mountainsField);
                        break;
                }

                if (!isMenu)
                {
                    fields[i].GetComponent<FieldElement>().SetNumberAndApply(DataManager.FieldStates[i].number);
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
            var indexes = new BoardGenerator().GenerateIndexesOrderAdvancedMode();
            
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
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
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
            {
                if (isMenu) continue;
                if (fields[indexes[i]].GetComponent<FieldElement>().type == FieldType.Desert)
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
            if (GameManager.LoadingGame)
            {
                InstantiateModeFields();
            }
            else if (GameManager.State.Mode == CatanMode.Basic)
            {
                InstantiateBasicModeFields();
            }
            else if (GameManager.State.Mode == CatanMode.Advanced)
            {
                InstantiateAdvancedModeFields();
            }
                
            //Destiny: Setting up fields
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
            {
                var fieldPosition = new Vector3(fieldPositions[i, 0],  fieldLocationY, fieldPositions[i, 1]);
                fields[i].transform.position = fieldPosition;
                if (!isMenu)
                {
                    fields[i].GetComponent<FieldElement>().State.id = i;
                }
                fields[i].SetActive(true);
            }

            //Destiny: Setting up junctions
            for (var i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                var junctionsPosition = new Vector3(junctionPositions[i, 0], junctionLocationY, junctionPositions[i, 1]); 
                junctions[i] = Instantiate(neutralJunction);
                junctions[i].transform.position = junctionsPosition;
                if (!isMenu)
                {
                    junctions[i].GetComponent<JunctionElement>().State.id = i;
                }
                junctions[i].SetActive(true);
            }
             
            //Destiny: Setting up paths
            for (var i = 0; i < BoardManager.PathsNumber; i++)
            {
                var pathsPosition = new Vector3(pathPositions[i, 0], pathLocationY, pathPositions[i, 1]);
                paths[i] = Instantiate(neutralPath);
                paths[i].transform.position = pathsPosition;
                paths[i].transform.rotation = Quaternion.Euler(0, pathPositions[i, 2]+90, 0);
                if (!isMenu)
                {
                    paths[i].GetComponent<PathElement>().State.id = i;
                }
                paths[i].SetActive(true);
            }
            
            //Destiny: Setting up ports
            for (var i = 0; i < BoardManager.PortsNumber; i++)
            {
                var portPosition = new Vector3(portPositions[i, 0], portLocationY, portPositions[i, 1]);
                ports[i] = Instantiate(port);
                ports[i].transform.position = portPosition;
                ports[i].transform.rotation = Quaternion.Euler(0, portPositions[i, 2]+90, 0);
                ports[i].SetActive(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">color of the player that is the new owner</param>
        /// <param name="id">id of the element that ownership is changed</param>
        /// <param name="toDelete">if true - the element is about to be removed, if false - is about to be built</param>
        private void ChangePathOwner(Player.Player.Color color, int id, bool toDelete)
        {
            //Destiny: Keeping properties from older object
            var pos = paths[id].transform.position;
            pos.y = pathLocationY;
            var rot = paths[id].transform.rotation;
            var pathsDump = paths[id].GetComponent<PathElement>();
            bool forFree = ((PathState)paths[id].GetComponent<PathElement>().State).forFree;

            //Destiny: Old object must be destroyed before new one is created
            Destroy(paths[id]);
            
            //Destiny: Choosing new object based on color
            //No need to create neutral path because ownership means something is not neutral
            if (!toDelete)
            {
                paths[id] = color switch
                {
                    Player.Player.Color.White => Instantiate(whitePath),
                    Player.Player.Color.Yellow => Instantiate(yellowPath),
                    Player.Player.Color.Red => Instantiate(redPath),
                    Player.Player.Color.Blue => Instantiate(bluePath),
                    _ => paths[id]
                };
            }
            else
            {
                paths[id] = Instantiate(neutralPath);
            }

            //Destiny: New instances are hidden on default
            paths[id].SetActive(true);

            paths[id].GetComponent<PathElement>().State.id = id;
            ((PathState)paths[id].GetComponent<PathElement>().State).forFree = forFree;
            
            //Destiny: Properties that changes because of change of ownership
            ((PathState)paths[id].GetComponent<PathElement>().State).canBuild = toDelete;

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
        /// <param name="toDelete">if true - the element is about to be removed, if false - is about to be built</param>
        private void ChangeJunctionOwner(Player.Player.Color color, int id, bool upgraded, bool toDelete)
        {
            //Destiny: Keeping properties from older object
            var pos = junctions[id].transform.position;
            pos.y = junctionLocationY;
            var rot = junctions[id].transform.rotation;
            var fieldsDump = junctions[id].GetComponent<JunctionElement>();
            
            //Destiny: Old object must be destroyed before new one is created
            Destroy(junctions[id]);
            
            //Destiny: Choosing new object based on color and if the object was ever upgraded
            //No need to create empty junction because ownership means something was built
            if (upgraded && !toDelete)
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
            else if ((!upgraded && !toDelete) || (upgraded && toDelete))
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
            else
            {
                junctions[id] = Instantiate(neutralJunction);
            }

            //Destiny: New instances are hidden on default
            junctions[id].SetActive(true);

            junctions[id].GetComponent<JunctionElement>().State.id = id;

            JunctionType junctionType;
            if (toDelete && upgraded)
                junctionType = JunctionType.Village;
            else if (toDelete && !upgraded)
                junctionType = JunctionType.None;
            else if (!toDelete && upgraded)
                junctionType = JunctionType.City;
            else
                junctionType = JunctionType.Village;

            //Destiny: Properties that changes because of change of ownership
            ((JunctionState)junctions[id].GetComponent<JunctionElement>().State).canBuild = toDelete;
            ((JunctionState)junctions[id].GetComponent<JunctionElement>().State).type = junctionType;

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
            BoardManager.Junctions[id].junctionsID.ForEach(delegate (int junctionId)
            {
                ((JunctionState)BoardManager.Junctions[junctionId].State).canBuild = toDelete;
                ((JunctionState)junctions[junctionId].GetComponent<JunctionElement>().State).canBuild = toDelete;

                if (toDelete)
                {
                    BoardManager.Junctions[junctionId].junctionsID.ForEach(delegate(int adjacentJunctionId) {
                        if (((JunctionState)BoardManager.Junctions[adjacentJunctionId].State).type != JunctionType.None)
                            ((JunctionState)BoardManager.Junctions[junctionId].State).canBuild = false;
                    });
                }
            });
        }

        /// <summary>
        /// If any ownership change request is available - handle it
        /// </summary>
        private void HandleOwnerChangeRequests()
        {
            if (isMenu)
            {
                return;
            }
            
            //Destiny: Handle request if there is any on list
            if (BoardManager.OwnerChangeRequest.Count > 0)
            {
                //Destiny: Take first request from list and start action to handle it
                var info = BoardManager.OwnerChangeRequest.ElementAt(0);
                switch (info.Type)
                {
                    case ElementType.Junction:
                        ChangeJunctionOwner(info.Color, info.Id, info.Upgraded, info.ToDelete);
                        break;
                    case ElementType.Path:
                        ChangePathOwner(info.Color, info.Id, info.ToDelete);
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
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
            {
                BoardManager.Fields[i] = fields[i].GetComponent<FieldElement>();
                if (GameManager.LoadingGame)
                {
                    BoardManager.Fields[i].SetState(DataManager.FieldStates[i]);
                }
                else
                {
                    ((FieldState)BoardManager.Fields[i].State).type = BoardManager.Fields[i].type;
                }
            }

            for (var i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                BoardManager.Junctions[i] = junctions[i].GetComponent<JunctionElement>();
                if (GameManager.LoadingGame)
                {
                    BoardManager.Junctions[i].SetState(DataManager.JunctionStates[i]);
                }
            }

            for (var i = 0; i < BoardManager.PathsNumber; i++)
            {
                BoardManager.Paths[i] = paths[i].GetComponent<PathElement>();
                if (GameManager.LoadingGame)
                {
                    BoardManager.Paths[i].SetState(DataManager.PathStates[i]);
                }
            }

            if (!GameManager.LoadingGame)
            {
                BoardManager.Fields.Where(field => field.type.Equals(FieldType.Desert))
                    .FirstOrDefault().SetThief(true);
            }
        }

        /// <summary>
        /// Sets the port types on junctions
        /// </summary>
        private void SetPortInfo()
        {
            //Destiny: Setting port info in junctions
            int[] normalPorts = {0, 3, 26, 32, 47, 49, 51, 52};
            int[] woolPorts = { 42, 46 };
            int[] woodPorts = { 11, 16 };
            int[] ironPorts = { 10, 15 };
            int[] clayPorts = { 33, 38 };
            int[] wheatPorts = { 1, 5 };

            foreach (var junction in junctions)
            {
                junction.GetComponent<JunctionElement>().portType = JunctionElement.PortType.None;
            }            
            foreach (var t in normalPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Normal;
            }
            foreach (var t in woolPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Wool;
            }
            foreach (var t in woodPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Wood;
            }
            foreach (var t in ironPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Iron;
            }
            foreach (var t in clayPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Clay;
            }
            foreach (var t in wheatPorts)
            {
                junctions[t].GetComponent<JunctionElement>().portType = JunctionElement.PortType.Wheat;
            }

            //Destiny: Instantiating port info
            for (var i = 0; i < portInfoPositions.Length/2; i++)
            {
                var info = Instantiate(portInfo);
                info.transform.position = 
                    new Vector3(portInfoPositions[i, 0], portInfo.transform.position.y, portInfoPositions[i, 1]);
                info.GetComponent<NumberOverPort.NumberOverPort>().SetInfo(i);
                info.SetActive(true);
            }            
        }

        void Start()
        {
            //Destiny: Creating instances of methods providers
            random = new Random();
            BoardManager.Setup();
            var distributor = new ElementDistributor();
            var positioner = new ElementPositioner(h);
            var neighbourGenerator = new NeighbourGenerator();

            //Destiny: Getting positions from the positioner
            fieldPositions = positioner.GenerateFieldsPosition();
            junctionPositions = positioner.GenerateJunctionsPosition();
            pathPositions = positioner.GeneratePathsPosition();
            portPositions = positioner.GeneratePortsPosition(p, junctionPositions);
            portInfoPositions = positioner.GeneratePortInfoPosition(junctionPositions);

            //Destiny: Generating element neighbours
            neighbourGenerator.GenerateElementNeighbors();

            //Destiny: Preparing empty arrays for elements
            fields = new GameObject[BoardManager.FieldsNumber];
            junctions = new GameObject[BoardManager.JunctionsNumber];
            paths = new GameObject[BoardManager.PathsNumber];
            ports = new GameObject[BoardManager.PortsNumber];

            //Destiny: Setting all elements on the board
            SetupMapElements();

            //Destiny: This happens only if script is not running on menu scene
            if (!isMenu)
            {
                //Destiny: Setting port info
                SetPortInfo();

                //Destiny: Setting info about elements for external classes
                SetBoardElementInfo();

                //Destiny: Setting up neighbours of elements
                neighbourGenerator.SetupElementNeighbors();

                //Destiny: Setting up initial elements distribution to players
                distributor.SetupInitialDistribution();
            }

            GameManager.LoadingGame = false;
        }
        
        void Update()
        {
            //Destiny: Handle ownership changes (like listening for new requests)
            HandleOwnerChangeRequests();
        }
    }
}