using System;
using System.Collections.Generic;
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

        //Destiny: neighbors of map elements
        private Dictionary<int, List<int>> fieldJunctions;
        private Dictionary<int, List<int>> fieldPaths;
        private Dictionary<int, List<int>> junctionFields;
        private Dictionary<int, List<int>> junctionPaths;
        private Dictionary<int, List<int>> junctionJunctions;
        private Dictionary<int, List<int>> pathPaths;
        private Dictionary<int, List<int>> pathJunctions;

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
        /// <param name="h">height of hex tile triangle</param>
        /// <returns>Array with x and z location of field elements corresponding their indexes</returns>
        private float[,] GenerateFieldsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            const int levelCounts = 5;
            
            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 };
            
            //Destiny: Number of fields above or on the same level as given: 0, 3, 7, 12, 16, 19
            var sf = new int[6]; 
            
            sf[0] = f[0];
            for (var i = 0; i < levelCounts; i++)
                sf[i + 1] = sf[i] + f[i + 1];
            var fieldCount = sf[levelCounts];
            
            //Destiny: Values of x and z for every field
            var fields = new float[fieldCount, 2];
            for (var i = 0; i < levelCounts; i++)
            {
                for (var j = 0; j < f[i + 1]; j++)
                {
                    //Destiny: Value of x
                    fields[sf[i] + j, 0] = 3 * a - i * 3 * a / 2;
                    
                    //Destiny: Value of z for even and odd levels
                    if (i % 2 == 0) fields[sf[i] + j, 1] = 2 * h * (i / 2 % 2 + 1) - 2 * j * h;
                    else fields[sf[i] + j, 1] = 3 * h - 2 * j * h;
                }
            }
            return fields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">height of hex tile triangle</param>
        /// <returns>Array with x and z location of junctions elements corresponding their indexes</returns>
        private float[,] GenerateJunctionsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            const int junctionLevelsCount = 12;
            
            //Destiny: Number of junctions on level given
            int[] j = { 0, 3, 4, 4, 5, 5, 6, 6, 5, 5, 4, 4, 3 }; 
            
            //Destiny: Number of junctions above or on the same level: 0, 3, 7, 11, 16, 21, 27, 33, 38, 43, 47, 51, 54
            var sj = new int[junctionLevelsCount + 1];          
            sj[0] = j[0];
            for (var i = 0; i < junctionLevelsCount; i++)
                sj[i + 1] = sj[i] + j[i + 1];

            //Destiny: Value of x and z for every junction
            var junctions = new float[sj[junctionLevelsCount], 2];
            for (var i = 0; i < junctionLevelsCount; i++)
            {
                for (var k = 0; k < j[i + 1]; k++)
                {
                    //Destiny: Levels: 0, 2, 4
                    if (i % 2 == 0 && i < junctionLevelsCount / 2)
                    {
                        junctions[sj[i] + k, 0] = 4 * a - 3 * a * i / 4;
                        junctions[sj[i] + k, 1] = 2 * h + i * h / 2 - 2 * k * h;
                    }
                    //Destiny: Levels: 7, 9, 11
                    else if (i % 2 == 1 && i > junctionLevelsCount / 2)
                    {
                        junctions[sj[i] + k, 0] = -a - 3 * a * (i - 7) / 4;
                        junctions[sj[i] + k, 1] = 4 * h - (i - 7) * h / 2 - 2 * k * h;
                    }
                    //Destiny: Levels: 1, 3, 5
                    else if (i % 2 == 1 && i < junctionLevelsCount / 2)             
                    {
                        junctions[sj[i] + k, 0] = 7 * a / 2 - 3 * a * (i - 1) / 4;
                        junctions[sj[i] + k, 1] = 3 * h + (i - 1) * h / 2 - 2 * k * h;
                    }
                    //Destiny: Levels: 6, 8, 10
                    else if (i % 2 == 0 && i >= junctionLevelsCount / 2)
                    {
                        junctions[sj[i] + k, 0] = -a / 2 - 3 * a * (i - 6) / 4;
                        junctions[sj[i] + k, 1] = 5 * h - (i - 6) * h / 2 - 2 * k * h;
                    }
                }
            }
            return junctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">height of hex tile triangle</param>
        /// <returns>Array with x and z location of paths elements corresponding their indexes</returns>
        private float[,] GeneratePathsPosition(float h)
        {
            //Destiny: Length of hex tile triangle side
            var a = (float)(2 * h * Math.Sqrt(3)/3);
            
            //Destiny: Angle to rotate
            const float angle = 60f;
            
            const int pathLevelsCount = 11;
            
            //Destiny: Number of paths on level given
            int[] p = { 0, 6, 4, 8, 5, 10, 6, 10, 5, 8, 4, 6 };
            
            //Destiny: Number of paths above or on level given: 0, 6, 10, 18, 23, 33, 39, 49, 54, 62, 66, 72
            var sp = new int[pathLevelsCount + 1];                  
            sp[0] = p[0];
            for (var i = 0; i < pathLevelsCount; i++) sp[i + 1] = sp[i] + p[i + 1];

            //Destiny: Value of x and z for every path
            var paths = new float[sp[pathLevelsCount], 3];          
            for (var i = 0; i < pathLevelsCount; i++)
            {
                for (var j = 0; j < p[i + 1]; j++)
                {
                    paths[sp[i] + j, 0] = 15 * a / 4 - 3 * a * i / 4;
                    
                    //Destiny: Levels: 0, 2, 4
                    if (i % 2 == 0 && i < pathLevelsCount / 2)                
                        paths[sp[i] + j, 1] = 5 * h / 2 + i * h / 2 - j * h;
                    //Destiny: Levels: 7, 9
                    else if (i % 2 == 1 && i > pathLevelsCount / 2)           
                        paths[sp[i] + j, 1] = 4 * h - (i - 7) * h / 2 - 2 * j * h;
                    //Destiny: Levels: 1, 3, 5
                    else if (i % 2 == 1 && i <= pathLevelsCount / 2)         
                        paths[sp[i] + j, 1] = 3 * h + (i - 1) * h / 2 - 2 * j * h;
                    //Destiny: Levels: 6, 8, 10
                    else if (i % 2 == 0 && i > pathLevelsCount / 2)        
                        paths[sp[i] + j, 1] = 9 * h / 2 - (i - 6) * h / 2 - j * h;
                    
                    //Destiny: Levels: 1, 3, 5, 7, 9 (no rotation)
                    if (i % 2 == 1) paths[sp[i] + j, 2] = 0;
                }
                
                //Destiny: Levels: 0, 2, 4, 6, 8, 10 (rotation +60/-60 deg)
                if (i % 2 == 0)
                {
                    //Destiny: Every even path (right)
                    for (var j = sp[i]; j < sp[i + 1]; j += 2)
                    {
                        if(i < pathLevelsCount/2)
                            paths[j, 2] = angle;
                        else paths[j, 2] = -angle;
                    }

                    //Destiny: Every odd path (left)
                    for (var j = sp[i] + 1; j < sp[i + 1]; j += 2)
                    {
                        if(i < pathLevelsCount/2)
                            paths[j, 2] = -angle;
                        else paths[j, 2] = angle;
                    }
                }
            }
            return paths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">number of fields on level given</param>
        /// <param name="sf">number of fields above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of field, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GenerateFieldJunctions(int[] f, int[] sf, int[] sj)
        {
            Dictionary<int, List<int>> fieldJunctions = new Dictionary<int, List<int>>();

            for (int i = 0; i < FieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    fieldJunctions[sf[i] + j] = new List<int>();

                    // Destiny: first neighbour
                    if (i <= FieldLevelsNumber / 2)                     // for levels: 0, 1, 2
                        fieldJunctions[sf[i] + j].Add(sj[2 * i] + j);
                    else                                                // for levels: 3, 4
                        fieldJunctions[sf[i] + j].Add(sj[2 * i] + j + 1);

                    // Destiny: second and third neighbour
                    fieldJunctions[sf[i] + j].Add(sj[2 * i + 1] + j);
                    fieldJunctions[sf[i] + j].Add(sj[2 * i + 1] + j + 1);

                    // Destiny: fourth and fifth neighbour
                    fieldJunctions[sf[i] + j].Add(sj[2 * i + 2] + j);
                    fieldJunctions[sf[i] + j].Add(sj[2 * i + 2] + j + 1);

                    // Destiny: sixth neighbour
                    if (i < FieldLevelsNumber / 2)                      // for levels: 0, 1
                        fieldJunctions[sf[i] + j].Add(sj[2 * i + 3] + j + 1);
                    else                                                // for levels: 2, 3, 4
                        fieldJunctions[sf[i] + j].Add(sj[2 * i + 3] + j);
                }
            }

            return fieldJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">number of fields on level given</param>
        /// <param name="sf">number of fields above or on the same level</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <returns>Key: index of field, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GenerateFieldPaths(int[] f, int[] sf, int[] sp)
        {
            Dictionary<int, List<int>> fieldPaths = new Dictionary<int, List<int>>();

            for (int i = 0; i < FieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    fieldPaths[sf[i] + j] = new List<int>();

                    // Destiny: first and second neighbour (top)
                    if (i <= FieldLevelsNumber / 2)                      // for levels: 0, 1, 2
                    {
                        fieldPaths[sf[i] + j].Add(sp[2 * i] + 2 * j);
                        fieldPaths[sf[i] + j].Add(sp[2 * i] + 2 * j + 1);
                    }
                    else                                                // for levels: 3, 4
                    {
                        fieldPaths[sf[i] + j].Add(sp[2 * i] + 2 * j + 1);
                        fieldPaths[sf[i] + j].Add(sp[2 * i] + 2 * j + 2);
                    }

                    // Destiny: third and fourth neighbour (side)
                    fieldPaths[sf[i] + j].Add(sp[2 * i + 1] + j);
                    fieldPaths[sf[i] + j].Add(sp[2 * i + 1] + j + 1);

                    // Destiny: fifth and sixth neighbour (bottom)
                    if (i < FieldLevelsNumber / 2)                      // for levels: 0, 1
                    {
                        fieldPaths[sf[i] + j].Add(sp[2 * i + 2] + 2 * j + 1);
                        fieldPaths[sf[i] + j].Add(sp[2 * i + 2] + 2 * j + 2);
                    }
                    else                                                // for levels: 2, 3, 4
                    {
                        fieldPaths[sf[i] + j].Add(sp[2 * i + 2] + 2 * j);
                        fieldPaths[sf[i] + j].Add(sp[2 * i + 2] + 2 * j + 1);
                    }
                }
            }

            return fieldPaths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of field type</returns>
        private Dictionary<int, List<int>> GenerateJunctionFields()
        {
            Dictionary<int, List<int>> junctionFields = new Dictionary<int, List<int>>();

            for (int i = 0; i < JunctionsNumber; i++)
                junctionFields[i] = new List<int>();

            //Destiny: for each field
            for (int i = 0; i < FieldsNumber; i++)
            {
                //Destiny: for each neighbor of junction type of given field
                fieldJunctions[i].ForEach(delegate (int junction)
                {
                    //Destiny: add fieldId to list of neighbors of field type to given junction
                    if (!junctionFields[junction].Contains(i))
                        junctionFields[junction].Add(i);
                });
            }

            foreach (KeyValuePair<int, List<int>> kvp in junctionFields)
            {
                Console.Write($"{kvp.Key}:");
                kvp.Value.ForEach(delegate (int neighbour)
                {
                    Console.Write($"\t{neighbour}");
                });
                Console.WriteLine();
            }

            return junctionFields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">number of fields on level given</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GenerateJunctionPaths(int[] f, int[] sp, int[] sj)
        {
            Dictionary<int, List<int>> junctionPaths = new Dictionary<int, List<int>>();

            for (int i = 0; i < JunctionsNumber; i++)
                junctionPaths[i] = new List<int>();

            for (int i = 0; i < FieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    if (i <= FieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                    {
                        // junctions at the top
                        junctionPaths[sj[2 * i] + j].Add(sp[2 * i] + 2 * j);            // left path
                        junctionPaths[sj[2 * i] + j].Add(sp[2 * i] + 2 * j + 1);        // right path

                        // if top path exists - add it
                        if (i != 0)
                            junctionPaths[sj[2 * i] + j].Add(sp[2 * i - 1] + j);        // top path

                        // junctions on the side - left junctions at the top
                        if (j != 0)
                        {
                            junctionPaths[sj[2 * i + 1] + j].Add(sp[2 * i] + 2 * j - 1);    // left path
                            junctionPaths[sj[2 * i + 1] + j].Add(sp[2 * i] + 2 * j);        // right path
                            junctionPaths[sj[2 * i + 1] + j].Add(sp[2 * i + 1] + j);        // bottom path
                        }
                        else
                        {
                            junctionPaths[sj[2 * i + 1] + j].Add(sp[2 * i] + 2 * j);        // right path
                            junctionPaths[sj[2 * i + 1] + j].Add(sp[2 * i + 1] + j);        // bottom path
                        }
                    }

                    if (i >= FieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                    {
                        // junctions at the top
                        junctionPaths[sj[2 * i + 3] + j].Add(sp[2 * i + 2] + 2 * j);            // left path
                        junctionPaths[sj[2 * i + 3] + j].Add(sp[2 * i + 2] + 2 * j + 1);        // right path

                        // if bottom path exists - add it
                        if (i != FieldLevelsNumber - 1)
                            junctionPaths[sj[2 * i + 3] + j].Add(sp[2 * i + 3] + j);            // bottom path

                        // junctions on the side - left junctions at the bottom
                        if (j != 0)
                        {
                            junctionPaths[sj[2 * i + 2] + j].Add(sp[2 * i + 2] + 2 * j - 1);    // left path
                            junctionPaths[sj[2 * i + 2] + j].Add(sp[2 * i + 2] + 2 * j);        // right path
                            junctionPaths[sj[2 * i + 2] + j].Add(sp[2 * i + 1] + j);            // top path
                        }
                        else
                        {
                            junctionPaths[sj[2 * i + 2] + j].Add(sp[2 * i + 1] + j);            // top path
                            junctionPaths[sj[2 * i + 2] + j].Add(sp[2 * i + 2] + 2 * j);        // right path
                        }
                    }
                }

                // junctions on the side - right junctions at the top with two paths
                if (i <= FieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                {
                    junctionPaths[sj[2 * i + 2] - 1].Add(sp[2 * i + 1] - 1);        // left path
                    junctionPaths[sj[2 * i + 2] - 1].Add(sp[2 * i + 2] - 1);        // bottom path
                }

                // junctions on the side - right junctions at the bottom with two paths
                if (i >= FieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                {
                    junctionPaths[sj[2 * i + 3] - 1].Add(sp[2 * i + 2] - 1);        // top path
                    junctionPaths[sj[2 * i + 3] - 1].Add(sp[2 * i + 3] - 1);        // left path
                }
            }

            return junctionPaths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">number of fields on level given</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GenerateJunctionJunctions(int[] f, int[] sp, int[] sj)
        {
            Dictionary<int, List<int>> junctionJunctions = new Dictionary<int, List<int>>();

            for (int i = 0; i < JunctionsNumber; i++)
                junctionJunctions[i] = new List<int>();

            for (int i = 0; i < FieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    if (i <= FieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                    {
                        // junctions at the top
                        junctionJunctions[sj[2 * i] + j].Add(sj[2 * i + 1] + j);            // left path
                        junctionJunctions[sj[2 * i] + j].Add(sj[2 * i + 1] + j + 1);        // right path

                        // if top path exists - add it
                        if (i != 0)
                            junctionJunctions[sj[2 * i] + j].Add(sj[2 * i - 1] + j);        // top path

                        // junctions on the side - left junctions at the top
                        if (j != 0)
                        {
                            junctionJunctions[sj[2 * i + 1] + j].Add(sj[2 * i] + j - 1);    // left path
                            junctionJunctions[sj[2 * i + 1] + j].Add(sj[2 * i] + j);        // right path
                            junctionJunctions[sj[2 * i + 1] + j].Add(sj[2 * i + 2] + j);        // bottom path
                        }
                        else
                        {
                            junctionJunctions[sj[2 * i + 1] + j].Add(sj[2 * i] + j);        // right path
                            junctionJunctions[sj[2 * i + 1] + j].Add(sj[2 * i + 2] + j);        // bottom path
                        }
                    }

                    if (i >= FieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                    {
                        // junctions at the top
                        junctionJunctions[sj[2 * i + 3] + j].Add(sj[2 * i + 2] + j);            // left path
                        junctionJunctions[sj[2 * i + 3] + j].Add(sj[2 * i + 2] + j + 1);        // right path

                        // if bottom path exists - add it
                        if (i != FieldLevelsNumber - 1)
                            junctionJunctions[sj[2 * i + 3] + j].Add(sj[2 * i + 4] + j);        // bottom path

                        // junctions on the side - left junctions at the bottom
                        if (j != 0)
                        {
                            junctionJunctions[sj[2 * i + 2] + j].Add(sj[2 * i + 3] + j - 1);    // left path
                            junctionJunctions[sj[2 * i + 2] + j].Add(sj[2 * i + 3] + j);        // right path
                            junctionJunctions[sj[2 * i + 2] + j].Add(sj[2 * i + 1] + j);            // top path
                        }
                        else
                        {
                            junctionJunctions[sj[2 * i + 2] + j].Add(sj[2 * i + 1] + j);            // top path
                            junctionJunctions[sj[2 * i + 2] + j].Add(sj[2 * i + 3] + j);        // right path
                        }
                    }
                }

                // junctions on the side - right junctions at the top with two junctions
                if (i <= FieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                {
                    junctionJunctions[sj[2 * i + 2] - 1].Add(sj[2 * i + 1] - 1);        // left path
                    junctionJunctions[sj[2 * i + 2] - 1].Add(sj[2 * i + 3] - 1);        // bottom path
                }

                // junctions on the side - right junctions at the bottom with two junctions
                if (i >= FieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                {
                    junctionJunctions[sj[2 * i + 3] - 1].Add(sj[2 * i + 2] - 1);        // top path
                    junctionJunctions[sj[2 * i + 3] - 1].Add(sj[2 * i + 4] - 1);        // left path
                }
            }

            return junctionJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathLevelsNumber">number of path levels</param>
        /// <param name="p">number of paths on level given</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of path, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GeneratePathJunctions(int pathLevelsNumber, int[] p, int[] sp, int[] sj)
        {
            Dictionary<int, List<int>> pathJunctions = new Dictionary<int, List<int>>();

            for (int i = 0; i < PathsNumber; i++)
                pathJunctions[i] = new List<int>();

            for (int i = 0; i < pathLevelsNumber; i++)
            {
                for (int j = 0; j < p[i + 1]; j++)
                {
                    if (i % 2 == 1)                                     // for levels: 1, 3, 5, 7, 9 - without rotation
                    {
                        pathJunctions[sp[i] + j].Add(sj[i] + j);
                        pathJunctions[sp[i] + j].Add(sj[i + 1] + j);
                    }
                    else if (i < pathLevelsNumber / 2)                  // for levels: 0, 2, 4 - with rotation
                    {
                        if (j % 2 == 0)                                 // right turn - even paths on the level
                        {
                            pathJunctions[sp[i] + j].Add(sj[i] + j / 2);
                            pathJunctions[sp[i] + j].Add(sj[i + 1] + j / 2);
                        }
                        else                                            // left turn - odd paths on the level
                        {
                            pathJunctions[sp[i] + j].Add(sj[i] + j / 2);
                            pathJunctions[sp[i] + j].Add(sj[i + 1] + j / 2 + 1);
                        }
                    }
                    else                                                // for levels: 6, 8, 10 - with rotation
                    {
                        if (j % 2 == 1)                                 // right turn - odd paths on the level
                        {
                            pathJunctions[sp[i] + j].Add(sj[i] + j / 2 + 1);
                            pathJunctions[sp[i] + j].Add(sj[i + 1] + j / 2);
                        }
                        else                                            // left turn - even paths on the level
                        {
                            pathJunctions[sp[i] + j].Add(sj[i] + j / 2);
                            pathJunctions[sp[i] + j].Add(sj[i + 1] + j / 2);
                        }
                    }
                }
            }

            return pathJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of path, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GeneratePathPaths()
        {
            Dictionary<int, List<int>> pathPaths = new Dictionary<int, List<int>>();

            //Destiny: for each path
            for (int i = 0; i < PathsNumber; i++)
            {
                pathPaths[i] = new List<int>();

                //Destiny: for each junction adjacent to a given path
                pathJunctions[i].ForEach(delegate (int pathJunction)
                {
                    //Destiny: for each path adjacent to a given junction
                    junctionPaths[pathJunction].ForEach(delegate (int path)
                    {
                        //if this path has not been added so far and it is different from the given path, add it to the list
                        if (!pathPaths[i].Contains(path) && path != i)
                            pathPaths[i].Add(path);
                    });
                });
            }

            return pathPaths;
        }

        /// <summary>
        /// Generating lists of neighbors' indexes for each element
        /// </summary>
        private void GenerateElementNeighbors()
        {
            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 };
            //Destiny: Number of fields above or on the same level: 0, 3, 7, 12, 16, 19
            int[] sf = new int[FieldLevelsNumber + 1];
            sf[0] = f[0];
            for (int i = 0; i < FieldLevelsNumber; i++)
                sf[i + 1] = sf[i] + f[i + 1];

            const int junctionLevelsNumber = 12;
            //Destiny: Number of junctions on level given
            int[] j = { 0, 3, 4, 4, 5, 5, 6, 6, 5, 5, 4, 4, 3 };
            //Destiny: Number of junctions above or on the same level: 0, 3, 7, 11, 16, 21, 27, 33, 38, 43, 47, 51, 54
            int[] sj = new int[junctionLevelsNumber + 1];
            sj[0] = j[0];
            for (int i = 0; i < junctionLevelsNumber; i++)
                sj[i + 1] = sj[i] + j[i + 1];

            const int pathLevelsNumber = 11;
            //Destiny: Number of paths on level given
            int[] p = { 0, 6, 4, 8, 5, 10, 6, 10, 5, 8, 4, 6 };
            //Destiny: Number of paths above or on the same level: 0, 6, 10, 18, 23, 33, 39, 49, 54, 62, 66, 72
            int[] sp = new int[pathLevelsNumber + 1];
            sp[0] = p[0];
            for (int i = 0; i < pathLevelsNumber; i++)
                sp[i + 1] = sp[i] + p[i + 1];

            fieldJunctions = GenerateFieldJunctions(f, sf, sj);
            fieldPaths = GenerateFieldPaths(f, sf, sp);
            junctionFields = GenerateJunctionFields();
            junctionPaths = GenerateJunctionPaths(f, sp, sj);
            junctionJunctions = GenerateJunctionJunctions(f, sp, sj);
            pathJunctions = GeneratePathJunctions(pathLevelsNumber, p, sp, sj);
            pathPaths = GeneratePathPaths();
        }

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
        /// Setting up neighbors for each item based on generated lists
        /// </summary>
        private void SetupElementNeighbors()
        {
            //Destiny: Setting up field's neighbors
            for (var i = 0; i < FieldsNumber; i++)
            {
                BoardManager.Fields[i].SetBuildingsID(fieldJunctions[i]);
                BoardManager.Fields[i].SetPathsID(fieldPaths[i]);
            }

            //Destiny: Setting up junction's neighbors
            for (var i = 0; i < JunctionsNumber; i++)
            {
                BoardManager.Junctions[i].SetBuildingsID(junctionJunctions[i]);
                BoardManager.Junctions[i].SetPathsID(junctionPaths[i]);
            }

            //Destiny: Setting up path's neighbors
            for (var i = 0; i < PathsNumber; i++)
            {
                BoardManager.Paths[i].SetBuildingsID(pathJunctions[i]);
                BoardManager.Paths[i].SetPathsID(pathPaths[i]);
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
            random = new Random();
            
            fieldPositions = GenerateFieldsPosition(h);
            junctionPositions = GenerateJunctionsPosition(h);
            pathPositions = GeneratePathsPosition(h);

            GenerateElementNeighbors();

            fields = new GameObject[FieldsNumber];
            junctions = new GameObject[JunctionsNumber];
            paths = new GameObject[PathsNumber];
            
            SetupMapElements();

            if (!isMenu)
            {
                SetBoardElementInfo();
                SetupElementNeighbors();
                BoardManager.SetupInitialDistribution();
            }
        }

        void Update()
        {
            //Destiny: Handle ownership changes (like listening for new requests)
            HandleOwnerChangeRequests();
        }
    }
}