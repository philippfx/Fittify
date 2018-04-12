using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModels.Models.Sport;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Analyzer.Library
{
    public class ClassAnalyzer
    {
        public void Execute()
        {
            var unorderedArrays = GetClassNames();
            var listErrors = new List<Tuple<string, string, string>>();
            for (int i = 1; i <= unorderedArrays.Length - 1; i++)
            {
                string suffix = GetSuffix(unorderedArrays[0].Item2, unorderedArrays[i].Item2);
                for (int j = 0; j <= unorderedArrays[0].Item2.Length - 1; j++)
                {
                    string currentDataPropertyName = unorderedArrays[0].Item2[j];

                    if (currentDataPropertyName == "DateTimeStartEnd") { continue; }

                    var currentBrotherProperties = unorderedArrays[i].Item2.Where(p => p.ToLower().Replace(suffix.ToLower(), "") == currentDataPropertyName.ToLower()).ToList();

                    if (currentBrotherProperties.Count == 0)
                    {
                        listErrors.Add(Tuple.Create(unorderedArrays[i].Item1, "No brother found", currentDataPropertyName));
                        continue;
                    }

                    if (currentBrotherProperties.Count > 1)
                    {
                        listErrors.Add(Tuple.Create(unorderedArrays[i].Item1, "Ambiguous name", string.Join(string.Empty, currentBrotherProperties)));
                        continue;
                    }

                    if (currentBrotherProperties.FirstOrDefault() != currentDataPropertyName &&
                        currentBrotherProperties.FirstOrDefault().ToLower() == currentDataPropertyName.ToLower())
                    {
                        listErrors.Add(Tuple.Create(unorderedArrays[i].Item1, "Letter case mismatch", string.Join(string.Empty, currentBrotherProperties.FirstOrDefault())));
                    }
                }
            }
        }

        public Tuple<string, string[]>[] GetClassNames()
        {
            Tuple<string, string[]>[] unorderedArrays = 
            {
                Tuple.Create(typeof(Workout).Namespace, typeof(Workout).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(Workout).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForGet).Namespace, typeof(WorkoutOfmForGet).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutOfmForGet).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForPost).Namespace, typeof(WorkoutOfmForPost).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutOfmForPost).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForPatch).Namespace, typeof(WorkoutOfmForPatch).Assembly.GetTypes().Where(t => String.Equals(t.Namespace,  typeof(WorkoutOfmForPatch).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForGet).Namespace, typeof(WorkoutOfmForGet).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutOfmForGet).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForPost).Namespace, typeof(WorkoutOfmForPost).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutOfmForPost).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutOfmForPatch).Namespace, typeof(WorkoutOfmForPatch).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutOfmForPatch).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray()),
                Tuple.Create(typeof(WorkoutViewModel).Namespace, typeof(WorkoutViewModel).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(WorkoutViewModel).Namespace, StringComparison.Ordinal)).Select(s => s.Name).OrderBy(o => o).ToArray())
            };

            return unorderedArrays;
        } 

        public string GetSuffix(string[] data, string[] brother)
        {
            string suffix = null;
            for (int i = 1; i <= data.Length - 1; i++)
            {
                string currentDataPropertyName = data[i];
                var currentBrotherProperties = brother.Where(p => p.ToLower().Contains(currentDataPropertyName.ToLower())).ToList();
                if (currentBrotherProperties.Count == 1 && suffix == null)
                {
                    var stringIndex = currentBrotherProperties.FirstOrDefault().IndexOf(currentDataPropertyName);
                    suffix = currentBrotherProperties.FirstOrDefault().Substring(stringIndex + currentDataPropertyName.Length, currentBrotherProperties.FirstOrDefault().Length - currentDataPropertyName.Length - stringIndex);
                    break;
                }
            }
            return suffix;
        }
    }
}
