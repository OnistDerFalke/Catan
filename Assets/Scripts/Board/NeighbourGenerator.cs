using System;
using System.Collections.Generic;

namespace Board
{
    public class NeighbourGenerator
    {
        //Destiny: neighbors of map elements
        private Dictionary<int, List<int>> fieldJunctions;
        private Dictionary<int, List<int>> fieldPaths;
        private Dictionary<int, List<int>> junctionFields;
        private Dictionary<int, List<int>> junctionPaths;
        private Dictionary<int, List<int>> junctionJunctions;
        private Dictionary<int, List<int>> pathPaths;
        private Dictionary<int, List<int>> pathJunctions;
        
        private readonly int fieldLevelsNumber;
        private readonly int junctionLevelsNumber;
        private readonly int junctionsNumber;
        private readonly int fieldsNumber;
        private readonly int pathsNumber;
        
        public NeighbourGenerator(int fieldLevelsNumber, int junctionLevelsNumber)
        {
            this.fieldLevelsNumber = fieldLevelsNumber;
            this.junctionLevelsNumber = junctionLevelsNumber;
            junctionsNumber = BoardManager.JunctionsNumber;
            fieldsNumber = BoardManager.FieldsNumber;
            pathsNumber = BoardManager.PathsNumber;
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

            for (int i = 0; i < fieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    fieldJunctions[sf[i] + j] = new List<int>();

                    // Destiny: first neighbour
                    if (i <= fieldLevelsNumber / 2)                     // for levels: 0, 1, 2
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
                    if (i < fieldLevelsNumber / 2)                      // for levels: 0, 1
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

            for (int i = 0; i < fieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    fieldPaths[sf[i] + j] = new List<int>();

                    // Destiny: first and second neighbour (top)
                    if (i <= fieldLevelsNumber / 2)                      // for levels: 0, 1, 2
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
                    if (i < fieldLevelsNumber / 2)                      // for levels: 0, 1
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

            for (int i = 0; i < junctionsNumber; i++)
                junctionFields[i] = new List<int>();

            //Destiny: for each field
            for (int i = 0; i < fieldsNumber; i++)
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
        /// <param name="j">number of junctions on level given</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GenerateJunctionPaths(int[] j, int[] sp, int[] sj)
        {
            Dictionary<int, List<int>> junctionPaths = new Dictionary<int, List<int>>();

            for (int i = 0; i < junctionsNumber; i++)
                junctionPaths[i] = new List<int>();

            //Destiny: for each level of junctions
            for (int i = 0; i < junctionLevelsNumber; i++)
            {
                //Destiny: for each junction in level
                for (int k = 0; k < j[i + 1]; k++)
                {
                    //Destiny: top path (even levels)
                    if (i != 0 && i % 2 == 0)
                        junctionPaths[sj[i] + k].Add(sp[i - 1] + k);

                    //Destiny: side paths (upper part of the board) 
                    if (i < junctionLevelsNumber / 2)
                    {
                        //Destiny: side paths on the same level (junction levels: 0, 2, 4)
                        if (i % 2 == 0)
                        {
                            //Destiny: left path
                            junctionPaths[sj[i] + k].Add(sp[i] + 2 * k);
                            //Destiny: right path
                            junctionPaths[sj[i] + k].Add(sp[i] + 2 * k + 1);
                        }
                        //Destiny: side paths on the level above (junction levels: 1, 3, 5)
                        else
                        {
                            //Destiny: left path exists
                            if (k != 0)
                                junctionPaths[sj[i] + k].Add(sp[i - 1] + 2 * k - 1);

                            //Destiny: right path exists
                            if (k != j[i + 1] - 1)
                                junctionPaths[sj[i] + k].Add(sp[i - 1] + 2 * k);
                        }
                    }
                    //Destiny: side paths (lower part of the board) 
                    else
                    {
                        //Destiny: side paths on the level above (junction levels: 7, 9, 11)
                        if (i % 2 == 1)
                        {
                            //Destiny: left path
                            junctionPaths[sj[i] + k].Add(sp[i - 1] + 2 * k);
                            //Destiny: right path
                            junctionPaths[sj[i] + k].Add(sp[i - 1] + 2 * k + 1);
                        }
                        //Destiny: side paths on the same level (junction levels: 6, 8, 10)
                        else
                        {
                            //Destiny: left path exists
                            if (k != 0)
                                junctionPaths[sj[i] + k].Add(sp[i] + 2 * k - 1);

                            //Destiny: right path exists
                            if (k != j[i + 1] - 1)
                                junctionPaths[sj[i] + k].Add(sp[i] + 2 * k);
                        }
                    }

                    //Destiny: bottom path (odd levels)
                    if (i != junctionLevelsNumber - 1 && i % 2 == 1)
                        junctionPaths[sj[i] + k].Add(sp[i] + k);
                }
            }

            return junctionPaths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of path, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GeneratePathJunctions()
        {
            Dictionary<int, List<int>> pathJunctions = new Dictionary<int, List<int>>();

            for (int i = 0; i < pathsNumber; i++)
                pathJunctions[i] = new List<int>();

            //Destiny: for each junction
            for (int i = 0; i < junctionsNumber; i++)
            {
                //Destiny: for each path adjacent to the given junction
                junctionPaths[i].ForEach(delegate (int path)
                {
                    //Destiny: if junction not added yet then add it
                    if (!pathJunctions[path].Contains(i))
                        pathJunctions[path].Add(i);
                });
            }

            return pathJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GenerateJunctionJunctions()
        {
            Dictionary<int, List<int>> junctionJunctions = new Dictionary<int, List<int>>();

            //Destiny: for each junction
            for (int i = 0; i < junctionsNumber; i++)
            {
                junctionJunctions[i] = new List<int>();

                //Destiny: for each path adjacent to the given junction
                junctionPaths[i].ForEach(delegate (int junctionPath)
                {
                    //Destiny: for each junction adjacent to the given path
                    pathJunctions[junctionPath].ForEach(delegate (int junction)
                    {
                        //Destiny: if junction not added yet then add it
                        if (!junctionJunctions[i].Contains(junction) && junction != i)
                            junctionJunctions[i].Add(junction);
                    });
                });
            }

            return junctionJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of path, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GeneratePathPaths()
        {
            Dictionary<int, List<int>> pathPaths = new Dictionary<int, List<int>>();

            //Destiny: for each path
            for (int i = 0; i < pathsNumber; i++)
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
        public void GenerateElementNeighbors()
        {
            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 };
            //Destiny: Number of fields above or on the same level: 0, 3, 7, 12, 16, 19
            int[] sf = new int[fieldLevelsNumber + 1];
            sf[0] = f[0];
            for (int i = 0; i < fieldLevelsNumber; i++)
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
            junctionPaths = GenerateJunctionPaths(j, sp, sj);
            pathJunctions = GeneratePathJunctions();
            junctionJunctions = GenerateJunctionJunctions();
            pathPaths = GeneratePathPaths();
        }
        
        /// <summary>
        /// Setting up neighbors for each item based on generated lists
        /// </summary>
        public void SetupElementNeighbors()
        {
            //Destiny: Setting up field's neighbors
            for (var i = 0; i < fieldsNumber; i++)
            {
                BoardManager.Fields[i].SetJunctionsID(fieldJunctions[i]);
                BoardManager.Fields[i].SetPathsID(fieldPaths[i]);
            }

            //Destiny: Setting up junction's neighbors
            for (var i = 0; i < junctionsNumber; i++)
            {
                BoardManager.Junctions[i].SetJunctionsID(junctionJunctions[i]);
                BoardManager.Junctions[i].SetPathsID(junctionPaths[i]);
                BoardManager.Junctions[i].SetFieldsID(junctionFields[i]);
            }

            //Destiny: Setting up path's neighbors
            for (var i = 0; i < pathsNumber; i++)
            {
                BoardManager.Paths[i].SetJunctionsID(pathJunctions[i]);
                BoardManager.Paths[i].SetPathsID(pathPaths[i]);
            }
        }
    }
}