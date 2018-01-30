using System;
using System.Collections.Generic;
using System.Linq;

namespace Fittify.Api
{
    public static class RangeString
    {
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

        public static string ToStringOfIds(this List<int> ints)
        {
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
