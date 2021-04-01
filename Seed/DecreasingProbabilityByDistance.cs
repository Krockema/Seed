using System;
using System.Collections.Generic;
using System.Linq;

namespace Seed
{
    public class DecreasingProbabilityByDistance
    {
        private Dictionary<int, List<Tuple<double, int>>> _distanceProbability = new Dictionary<int, List<Tuple<double, int>>>();

        public DecreasingProbabilityByDistance(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                
            }
        }

        public int GetLevelByRandomValue(int startLevel, double roll)
        {
            return _distanceProbability[startLevel].First(x => x.Item1 < roll).Item2;
        }

        private Dictionary<int, List<Tuple<double, int>>> GetProbabilitiesForConvergentStructure(int depthOfAssembly)
        {
            var pkPerI = new Dictionary<int, List<Tuple<double, int>>>();
            for (var i = 2; i <= depthOfAssembly; i++)
            {
                pkPerI[i] = ProbabilityForConvergentStructureLevel(i);
            }
            return pkPerI;
        }

        private List<Tuple<double, int>> ProbabilityForConvergentStructureLevel(int i)
        {
            var pk = new List<Tuple<double, int>>();
            var preValue = 0.0;
            for (var k = 1; k < i; k++)
            {
                preValue += (2 * k / Convert.ToDouble(i * (i - 1)));
                pk.Add(new Tuple<double, int>(preValue, k));
            }
            
            return pk;
        }

        private List<KeyValuePair<int, double>> GetCumulatedProbabilitiesPk2(int i, int depthOfAssembly)
        {
            var pk = new List<KeyValuePair<int, double>>();
            for (var k = i + 1; k <= depthOfAssembly; k++)
            {
                pk.Add(new KeyValuePair<int, double>(k,
                    2 * (k - i) / Convert.ToDouble((depthOfAssembly - i) * (depthOfAssembly - i + 1))));
            }
            pk.Sort(delegate (KeyValuePair<int, double> o1, KeyValuePair<int, double> o2)
            {
                if (o1.Value > o2.Value) return -1;
                return 1;
            });
            return pk;
        }
    }
}