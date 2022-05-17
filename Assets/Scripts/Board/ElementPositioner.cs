using System;

namespace Board
{
    public class ElementPositioner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">height of hex tile triangle</param>
        /// <returns>Array with x and z location of field elements corresponding their indexes</returns>
        public float[,] GenerateFieldsPosition(float h)
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
                    fields[sf[i] + j, 0] = (levelCounts / 2) * 3 * a / 2 - i * 3 * a / 2;

                    //Destiny: Value of z for even and odd levels
                    if (i % 2 == 0)
                        fields[sf[i] + j, 1] = 2 * h * (f[i + 1] / 2) - 2 * j * h;
                    else
                        fields[sf[i] + j, 1] = 2 * h * (f[i + 1] / 2) - h - 2 * j * h;
                }
            }
            return fields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">height of hex tile triangle</param>
        /// <returns>Array with x and z location of junctions elements corresponding their indexes</returns>
        public float[,] GenerateJunctionsPosition(float h)
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
        public float[,] GeneratePathsPosition(float h)
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
    }
}