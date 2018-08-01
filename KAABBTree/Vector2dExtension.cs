using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace KAABBTree
{
    public static class Vector2dExtension
    {
        private static Random Random = new Random();
        public static Vector2d GetRandom()
        {
            return new Vector2d(Random.NextDouble(), Random.NextDouble());
        }
    }
}
