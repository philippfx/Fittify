using System;
using System.Collections.Generic;
using System.Linq;

namespace Fittify.Common.Helpers
{
    public static class RangeString
    {
        /// <summary>
        /// Converts a valid concatenated string of ints to List of ints
        /// </summary>
        /// <param name="str">Correctly syntaxed concatenated string of ints, for example 1-10,11,12-20</param>
        /// <returns>List of ints</returns>
        public static List<int> ToCollectionOfId(string str)
        {
            List<int> lstNumber = new List<int>();

            string[] cNumberArray = str.Split(',');

            for (int k = 0; k < cNumberArray.Length; k++)
            {
                string tmpDigit = cNumberArray[k];
                if (tmpDigit.Contains("-"))
                {
                    int start = int.Parse(tmpDigit.Split('-')[0].ToString());
                    int end = int.Parse(tmpDigit.Split('-')[1]);

                    for (int j = start; j <= end; j++)
                    {
                        if (!lstNumber.Contains(j))
                            lstNumber.Add(j);
                    }
                }
                else
                {
                    lstNumber.Add(int.Parse(tmpDigit));
                }
            }

            return lstNumber;
        }

        ///// <summary>
        ///// Checks if range of int Ids is in ascending order
        ///// </summary>
        ///// <param name="str">Valid input string, for example "1-5,10,20-25"</param>
        ///// <returns>Returns true if input string was valid and integers were in ascending order.</returns>
        //public static bool IsOrderAscending(string str)
        //{
        //    if (str == null) return false;
        //    var split = str.Split(new char[] {',', '-' });
        //    int current;
        //    int next;
        //    if (!int.TryParse(split[0], out current)) return false; // Covering the case when input is only a single int
        //    for (int i = 0; i < split.Length-1; i++)
        //    {
        //        if (!int.TryParse(split[i], out current)) return false;
        //        if (!int.TryParse(split[i + 1], out next)) return false;
        //        if (current > next) return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// Converts a List of ints to an abbreviated concatenated string
        /// </summary>
        /// <param name="ints">List of ints</param>
        /// <returns>Concatenated string</returns>
        public static string ToStringOfIds(this List<int> ints)
        {
            if (ints == null) return null;
            ints.Remove(0); // Note: Remove this if you like to include the Value 0
            if (ints.Count < 1) return "";
            ints.Sort();
            var lng = ints.Count;
            if (lng == 1)
                return ints[0].ToString();

            var fromnums = new List<int>();
            var tonums = new List<int>();
            for (var i = 0; i < lng - 1; i++)
            {
                if (i == 0)
                    fromnums.Add(ints[0]);
                if (ints[i + 1] > ints[i] + 1)
                {
                    tonums.Add(ints[i]);
                    fromnums.Add(ints[i + 1]);
                }
            }
            tonums.Add(ints[lng - 1]);


            string[] ranges = Enumerable.Range(0, tonums.Count).Select(
                i => fromnums[i].ToString() +
                     (tonums[i] == fromnums[i] ? "" : "-" + tonums[i].ToString())
            ).ToArray();

            if (ranges.Length == 1)
                return ranges[0];
            else
                return String.Join(",", ranges);
        }
    }
}
