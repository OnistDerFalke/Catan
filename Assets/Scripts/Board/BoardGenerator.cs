using System;

namespace Board
{
    public class BoardGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Array of numbers (refers to indexes) over the fields for advanced game mode</returns>
        public int[] GenerateIndexesOrderAdvancedMode()
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
            var circles = new int[f[1], sideCount * (f[1] - 1)];

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
    }
}
