using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace KAABBTree
{
    public static class Matrix2DExtension
    {
        public static Matrix2d GetRotationMatrix(double theta)
        {
            double cos = Math.Cos(theta), sin = Math.Sin(theta);
            return new Matrix2d(cos, -sin, sin, cos);
        }

        private static readonly Random Random = new Random();
        public static Matrix2d GetRandomRotationMatrix()
        {
            return GetRotationMatrix(Random.NextDouble() * Math.PI * 2);
        }


    }
}
