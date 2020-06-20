using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.MeshSlice
{
    public static class MeshUtils
    {
        public static Vector3 FindCenter (List<Vector3> pairs)
        {
            Vector3 center = Vector3.zero;
            int count = 0;

            for (int i = 0; i < pairs.Count; i += 2)
            {
                center += pairs[i];
                count++;
            }

            return center / count;
        }

        public static void ReorderList (List<Vector3> pairs)
        {
            int nbFaces = 0;
            int faceStart = 0;
            int i = 0;

            while (i < pairs.Count)
            {
                for (int j = i + 2; j < pairs.Count; j += 2)
                {
                    if (pairs[j] == pairs[i + 1])
                    {
                        SwitchPairs (pairs, i + 2, j);
                        break;
                    }
                }

                if (i + 3 >= pairs.Count)
                {
                    break;
                }
                else if (pairs[i + 3] == pairs[faceStart])
                {
                    nbFaces++;
                    i += 4;
                    faceStart = i;
                }
                else
                {
                    i += 2;
                }
            }
        }

        private static void SwitchPairs (List<Vector3> pairs, int pos1, int pos2)
        {
            if (pos1 == pos2)
            {
                return;
            }

            Vector3 temp1 = pairs[pos1];
            Vector3 temp2 = pairs[pos1 + 1];

            pairs[pos1] = pairs[pos2];
            pairs[pos1 + 1] = pairs[pos2 + 1];

            pairs[pos2] = temp1;
            pairs[pos2 + 1] = temp2;
        }
    }
}