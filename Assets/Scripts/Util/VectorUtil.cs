using UnityEngine;
using System.Collections.Generic;

namespace Util
{
        public class VectorUtil
        {
                public static Vector3 MeanVector(List<Vector3> vectors)
                {
                        var mean = Vector3.zero;
                        foreach (var v in vectors)
                        {
                                mean += v;
                        }

                        return mean / vectors.Count;
                }
        }
}