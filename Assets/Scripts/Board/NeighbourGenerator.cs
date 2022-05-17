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
        private readonly int junctionsNumber;
        private readonly int fieldsNumber;
        private readonly int pathsNumber;
        
        public NeighbourGenerator(int fieldLevelsNumber, int junctionsNumber, int fieldsNumber, int pathsNumber)
        {
            this.fieldLevelsNumber = fieldLevelsNumber;
            this.junctionsNumber = junctionsNumber;
            this.fieldsNumber = fieldsNumber;
            this.pathsNumber = pathsNumber;
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
        /// <param name="f">number of fields on level given</param>
        /// <param name="sp">number of paths above or on the same level</param>
        /// <param name="sj">number of junctions above or on the same level</param>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GenerateJunctionPaths(int[] f, int[] sp, int[] sj)
        {
            Dictionary<int, List<int>> junctionPaths = new Dictionary<int, List<int>>();

            for (int i = 0; i < junctionsNumber; i++)
                junctionPaths[i] = new List<int>();

            for (int i = 0; i < fieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    if (i <= fieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
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

                    if (i >= fieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                    {
                        // junctions at the top
                        junctionPaths[sj[2 * i + 3] + j].Add(sp[2 * i + 2] + 2 * j);            // left path
                        junctionPaths[sj[2 * i + 3] + j].Add(sp[2 * i + 2] + 2 * j + 1);        // right path

                        // if bottom path exists - add it
                        if (i != fieldLevelsNumber - 1)
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
                if (i <= fieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                {
                    junctionPaths[sj[2 * i + 2] - 1].Add(sp[2 * i + 1] - 1);        // left path
                    junctionPaths[sj[2 * i + 2] - 1].Add(sp[2 * i + 2] - 1);        // bottom path
                }

                // junctions on the side - right junctions at the bottom with two paths
                if (i >= fieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
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

            for (int i = 0; i < junctionsNumber; i++)
                junctionJunctions[i] = new List<int>();

            for (int i = 0; i < fieldLevelsNumber; i++)
            {
                for (int j = 0; j < f[i + 1]; j++)
                {
                    if (i <= fieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
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

                    if (i >= fieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
                    {
                        // junctions at the top
                        junctionJunctions[sj[2 * i + 3] + j].Add(sj[2 * i + 2] + j);            // left path
                        junctionJunctions[sj[2 * i + 3] + j].Add(sj[2 * i + 2] + j + 1);        // right path

                        // if bottom path exists - add it
                        if (i != fieldLevelsNumber - 1)
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
                if (i <= fieldLevelsNumber / 2)                                 // for levels: 0, 1, 2
                {
                    junctionJunctions[sj[2 * i + 2] - 1].Add(sj[2 * i + 1] - 1);        // left path
                    junctionJunctions[sj[2 * i + 2] - 1].Add(sj[2 * i + 3] - 1);        // bottom path
                }

                // junctions on the side - right junctions at the bottom with two junctions
                if (i >= fieldLevelsNumber / 2)                                 // for levels: 2, 3, 4
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

            for (int i = 0; i < pathsNumber; i++)
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
            junctionPaths = GenerateJunctionPaths(f, sp, sj);
            junctionJunctions = GenerateJunctionJunctions(f, sp, sj);
            pathJunctions = GeneratePathJunctions(pathLevelsNumber, p, sp, sj);
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
                BoardManager.Fields[i].SetBuildingsID(fieldJunctions[i]);
                BoardManager.Fields[i].SetPathsID(fieldPaths[i]);
            }

            //Destiny: Setting up junction's neighbors
            for (var i = 0; i < junctionsNumber; i++)
            {
                BoardManager.Junctions[i].SetBuildingsID(junctionJunctions[i]);
                BoardManager.Junctions[i].SetPathsID(junctionPaths[i]);
            }

            //Destiny: Setting up path's neighbors
            for (var i = 0; i < pathsNumber; i++)
            {
                BoardManager.Paths[i].SetBuildingsID(pathJunctions[i]);
                BoardManager.Paths[i].SetPathsID(pathPaths[i]);
            }
        }
    }
}