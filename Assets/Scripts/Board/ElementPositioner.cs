using System;
using System.Collections.Generic;

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
            
            const int levelsCount = 5;
            
            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 };
            
            //Destiny: Number of fields above or on the same level as given: 0, 3, 7, 12, 16, 19
            var sf = new int[6]; 
            
            sf[0] = f[0];
            for (var i = 0; i < levelsCount; i++)
                sf[i + 1] = sf[i] + f[i + 1];
            var fieldCount = sf[levelsCount];
            
            //Destiny: Values of x and z for every field
            var fields = new float[fieldCount, 2];
            for (var i = 0; i < levelsCount; i++)
            {
                for (var j = 0; j < f[i + 1]; j++)
                {
                    //Destiny: Value of x
                    fields[sf[i] + j, 0] = (levelsCount / 2) * 3 * a / 2 - i * 3 * a / 2;

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
            const int levelsCount = 5;

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
                    //Destiny: Value of x
                    //Destiny: Levels: 0, 2, 4, 6, 8, 10 (x)
                    if (i % 2 == 0)
                        junctions[sj[i] + k, 0] = levelsCount / 2 * 3 * a / 2 + a - i/2 * 3 * a / 2;
                    //Destiny: Levels: 1, 3, 5, 7, 9, 11 (x)
                    else
                        junctions[sj[i] + k, 0] = levelsCount / 2 * 3 * a / 2 + a / 2 - i/2 * 3 * a / 2;

                    //Destiny: Value of z
                    junctions[sj[i] + k, 1] = (j[i + 1] - 1) * h - 2 * h * k;
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

            //Destiny: Number of fields on level given
            int[] f = { 0, 3, 4, 5, 4, 3 };

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

                    //Destiny: Levels: 0, 2, 4, 6, 8, 10
                    if (i % 2 == 0)
                        paths[sp[i] + j, 1] = p[i + 1] * h / 2 - h / 2 - j * h;
                    //Destiny: Levels: 1, 3, 5, 7, 9
                    else
                        paths[sp[i] + j, 1] = f[i/2 + 1] * h - 2 * j * h;
                    
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
        /// <param name="h">height of hex tile triangle</param>
        /// <param name="p">part of triangle's length</param>
        /// <param name="junctionPositions">junction positions</param>
        /// <returns>Array of port ids and for each port values: x, z and angle</returns>
        public float[,] GeneratePortsPosition(float h, float p, float[,] junctionPositions)
        {
            //Destiny: Length of hex tile triangle side
            float a = (float)(2 * h * Math.Sqrt(3) / 3);

            //Destiny: Angle to rotate
            const float angle = 60f;

            const int portsNumber = 18;

            //Destiny: Deltas
            float dx = p * a / 2;
            float dax = p * a / 4;
            float daz = p * h / 2;

            //Destiny: Value of x, z and angles for every port
            var ports = new float[portsNumber, 3];

            ports[0, 0] = junctionPositions[0, 0] + dax;
            ports[1, 0] = junctionPositions[16, 0] + dax;
            ports[2, 0] = junctionPositions[38, 0] + dax;
            ports[0, 1] = junctionPositions[0, 1] + daz;
            ports[1, 1] = junctionPositions[16, 1] + daz;
            ports[2, 1] = junctionPositions[38, 1] + daz;
            ports[0, 2] = -angle;
            ports[1, 2] = -angle;
            ports[2, 2] = -angle;

            ports[3, 0] = junctionPositions[1, 0] + dax;
            ports[4, 0] = junctionPositions[10, 0] + dax;
            ports[5, 0] = junctionPositions[32, 0] + dax;
            ports[3, 1] = junctionPositions[1, 1] - daz;
            ports[4, 1] = junctionPositions[10, 1] - daz;
            ports[5, 1] = junctionPositions[32, 1] - daz;
            ports[3, 2] = angle;
            ports[4, 2] = angle;
            ports[5, 2] = angle;

            ports[6, 0] = junctionPositions[11, 0] - dax;
            ports[7, 0] = junctionPositions[33, 0] - dax;
            ports[8, 0] = junctionPositions[51, 0] - dax;
            ports[6, 1] = junctionPositions[11, 1] + daz;
            ports[7, 1] = junctionPositions[33, 1] + daz;
            ports[8, 1] = junctionPositions[51, 1] + daz;
            ports[6, 2] = angle;
            ports[7, 2] = angle;
            ports[8, 2] = angle;

            ports[9, 0] = junctionPositions[26, 0] - dax;
            ports[10, 0] = junctionPositions[46, 0] - dax;
            ports[11, 0] = junctionPositions[52, 0] - dax;
            ports[9, 1] = junctionPositions[26, 1] - daz;
            ports[10, 1] = junctionPositions[46, 1] - daz;
            ports[11, 1] = junctionPositions[52, 1] - daz;
            ports[9, 2] = -angle;
            ports[10, 2] = -angle;
            ports[11, 2] = -angle;

            ports[12, 0] = junctionPositions[3, 0] + dx;
            ports[13, 0] = junctionPositions[5, 0] + dx;
            ports[14, 0] = junctionPositions[15, 0] + dx;
            ports[12, 1] = junctionPositions[3, 1];
            ports[13, 1] = junctionPositions[5, 1];
            ports[14, 1] = junctionPositions[15, 1];
            ports[12, 2] = 0;
            ports[13, 2] = 0;
            ports[14, 2] = 0;

            ports[15, 0] = junctionPositions[42, 0] - dx;
            ports[16, 0] = junctionPositions[47, 0] - dx;
            ports[17, 0] = junctionPositions[49, 0] - dx;
            ports[15, 1] = junctionPositions[42, 1];
            ports[16, 1] = junctionPositions[47, 1];
            ports[17, 1] = junctionPositions[49, 1];
            ports[15, 2] = 0;
            ports[16, 2] = 0;
            ports[17, 2] = 0;


            return ports;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h">height of hex tile triangle</param>
        /// <param name="junctionPositions">junction positions</param>
        /// <returns></returns>
        public float[,] GeneratePortInfoPosition(float h, float[,] junctionPositions)
        {
            //Destiny: Length of hex tile triangle side
            float a = (float)(2 * h * Math.Sqrt(3) / 3);

            const int portsNumber = 9;

            //Destiny: Value of x and z for every port
            var portInfo = new float[portsNumber, 2];

            portInfo[0, 0] = junctionPositions[3, 0] + a;
            portInfo[0, 1] = junctionPositions[3, 1];
            portInfo[1, 0] = junctionPositions[5, 0] + a;
            portInfo[1, 1] = junctionPositions[5, 1];
            portInfo[2, 0] = junctionPositions[15, 0] + a;
            portInfo[2, 1] = junctionPositions[15, 1];

            portInfo[3, 0] = junctionPositions[26, 0] - a/2;
            portInfo[3, 1] = junctionPositions[26, 1] - h;

            portInfo[4, 0] = junctionPositions[42, 0] - a;
            portInfo[4, 1] = junctionPositions[42, 1];
            portInfo[5, 0] = junctionPositions[49, 0] - a;
            portInfo[5, 1] = junctionPositions[49, 1];
            portInfo[6, 0] = junctionPositions[47, 0] - a;
            portInfo[6, 1] = junctionPositions[47, 1];

            portInfo[7, 0] = junctionPositions[33, 0] - a/2;
            portInfo[7, 1] = junctionPositions[33, 1] + h;

            portInfo[8, 0] = junctionPositions[11, 0] - a / 2;
            portInfo[8, 1] = junctionPositions[11, 1] + h;


            return portInfo;
        }
    }
}