using System;
using System.Collections.Generic;

namespace Board
{
    public class NeighbourGenerator
    {
        //Destiny: neighbors of map elements
        private Dictionary<int, List<int>> fieldJunctions;
        private Dictionary<int, List<int>> junctionFields;
        private Dictionary<int, List<int>> junctionPaths;
        private Dictionary<int, List<int>> junctionJunctions;
        private Dictionary<int, List<int>> pathPaths;
        private Dictionary<int, List<int>> pathJunctions;
                
        public NeighbourGenerator()
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of field, Value: list of neighbors' indexes of junction type</returns>
        private Dictionary<int, List<int>> GenerateFieldJunctions()
        {
            Dictionary<int, List<int>> fieldJunctions = new Dictionary<int, List<int>>();

            for (int i = 0; i < BoardManager.FieldLevelsNumber; i++)
            {
                for (int j = 0; j < BoardManager.f[i + 1]; j++)
                {
                    fieldJunctions[BoardManager.sf[i] + j] = new List<int>();

                    // Destiny: first neighbour
                    if (i <= BoardManager.FieldLevelsNumber / 2)                     // for levels: 0, 1, 2
                    {
                        fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i] + j);
                    }
                    else                                                // for levels: 3, 4
                    {
                        fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i] + j + 1);
                    }

                    // Destiny: second and third neighbour
                    fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 1] + j);
                    fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 1] + j + 1);

                    // Destiny: fourth and fifth neighbour
                    fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 2] + j);
                    fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 2] + j + 1);

                    // Destiny: sixth neighbour
                    if (i < BoardManager.FieldLevelsNumber / 2)                      // for levels: 0, 1
                    {
                        fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 3] + j + 1);
                    }
                    else                                                // for levels: 2, 3, 4
                    {
                        fieldJunctions[BoardManager.sf[i] + j].Add(BoardManager.sj[2 * i + 3] + j);
                    }
                }
            }

            return fieldJunctions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of field type</returns>
        private Dictionary<int, List<int>> GenerateJunctionFields()
        {
            Dictionary<int, List<int>> junctionFields = new Dictionary<int, List<int>>();

            for (int i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                junctionFields[i] = new List<int>();
            }

            //Destiny: for each field
            for (int i = 0; i < BoardManager.FieldsNumber; i++)
            {
                //Destiny: for each neighbor of junction type of given field
                fieldJunctions[i].ForEach(delegate (int junction)
                {
                    //Destiny: add fieldId to list of neighbors of field type to given junction
                    if (!junctionFields[junction].Contains(i))
                    {
                        junctionFields[junction].Add(i);
                    }
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
        /// <returns>Key: index of junction, Value: list of neighbors' indexes of path type</returns>
        private Dictionary<int, List<int>> GenerateJunctionPaths()
        {
            Dictionary<int, List<int>> junctionPaths = new Dictionary<int, List<int>>();

            for (int i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                junctionPaths[i] = new List<int>();
            }

            //Destiny: for each level of junctions
            for (int i = 0; i < BoardManager.JunctionLevelsNumber; i++)
            {
                //Destiny: for each junction in level
                for (int k = 0; k < BoardManager.j[i + 1]; k++)
                {
                    //Destiny: top path (even levels)
                    if (i != 0 && i % 2 == 0)
                    {
                        junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i - 1] + k);
                    }

                    //Destiny: side paths (upper part of the board) 
                    if (i < BoardManager.JunctionLevelsNumber / 2)
                    {
                        //Destiny: side paths on the same level (junction levels: 0, 2, 4)
                        if (i % 2 == 0)
                        {
                            //Destiny: left path
                            junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i] + 2 * k);

                            //Destiny: right path
                            junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i] + 2 * k + 1);
                        }
                        //Destiny: side paths on the level above (junction levels: 1, 3, 5)
                        else
                        {
                            //Destiny: left path exists
                            if (k != 0)
                            {
                                junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i - 1] + 2 * k - 1);
                            }

                            //Destiny: right path exists
                            if (k != BoardManager.j[i + 1] - 1)
                            {
                                junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i - 1] + 2 * k);
                            }
                        }
                    }
                    //Destiny: side paths (lower part of the board) 
                    else
                    {
                        //Destiny: side paths on the level above (junction levels: 7, 9, 11)
                        if (i % 2 == 1)
                        {
                            //Destiny: left path
                            junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i - 1] + 2 * k);

                            //Destiny: right path
                            junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i - 1] + 2 * k + 1);
                        }
                        //Destiny: side paths on the same level (junction levels: 6, 8, 10)
                        else
                        {
                            //Destiny: left path exists
                            if (k != 0)
                            {
                                junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i] + 2 * k - 1);
                            }

                            //Destiny: right path exists
                            if (k != BoardManager.j[i + 1] - 1)
                            {
                                junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i] + 2 * k);
                            }
                        }
                    }

                    //Destiny: bottom path (odd levels)
                    if (i != BoardManager.JunctionLevelsNumber - 1 && i % 2 == 1)
                    {
                        junctionPaths[BoardManager.sj[i] + k].Add(BoardManager.sp[i] + k);
                    }
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

            for (int i = 0; i < BoardManager.PathsNumber; i++)
            {
                pathJunctions[i] = new List<int>();
            }

            //Destiny: for each junction
            for (int i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                //Destiny: for each path adjacent to the given junction
                junctionPaths[i].ForEach(delegate (int path)
                {
                    //Destiny: if junction not added yet then add it
                    if (!pathJunctions[path].Contains(i))
                    {
                        pathJunctions[path].Add(i);
                    }
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
            for (int i = 0; i < BoardManager.JunctionsNumber; i++)
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
                        {
                            junctionJunctions[i].Add(junction);
                        }
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
            for (int i = 0; i < BoardManager.PathsNumber; i++)
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
                        {
                            pathPaths[i].Add(path);
                        }
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
            fieldJunctions = GenerateFieldJunctions();
            junctionFields = GenerateJunctionFields();
            junctionPaths = GenerateJunctionPaths();
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
            for (var i = 0; i < BoardManager.FieldsNumber; i++)
            {
                BoardManager.Fields[i].junctionsID = fieldJunctions[i];
            }

            //Destiny: Setting up junction's neighbors
            for (var i = 0; i < BoardManager.JunctionsNumber; i++)
            {
                BoardManager.Junctions[i].junctionsID = junctionJunctions[i];
                BoardManager.Junctions[i].pathsID = junctionPaths[i];
                BoardManager.Junctions[i].fieldsID = junctionFields[i];
            }

            //Destiny: Setting up path's neighbors
            for (var i = 0; i < BoardManager.PathsNumber; i++)
            {
                BoardManager.Paths[i].junctionsID = pathJunctions[i];
                BoardManager.Paths[i].pathsID = pathPaths[i];
            }
        }
    }
}