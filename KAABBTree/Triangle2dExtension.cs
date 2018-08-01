using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace KAABBTree
{
    public static class Triangle2DExtension
    {
        public static AxisAlignedBox2d GetBBox(this IEnumerable<Triangle2d> triangle)
        {
            return triangle.Select(tra =>
            {
                var res = AxisAlignedBox2d.Empty;
                res.Contain(tra.V0);
                res.Contain(tra.V1);
                res.Contain(tra.V2);
                return res;
            }).Aggregate((a1, a2) =>
            {
                a1.Contain(a2);
                return a1;
            });
        }
        public static Triangle2d GetRandom()
        {
            return new Triangle2d(
                Vector2dExtension.GetRandom(),
                Vector2dExtension.GetRandom(),
                Vector2dExtension.GetRandom());
        }

       public static Triangle2d Rotate(this Triangle2d tra, Matrix2d rotationMatrix)
       {
            return new Triangle2d(rotationMatrix * tra[0],
                rotationMatrix * tra[1],
                rotationMatrix * tra[2]);
       }

    }
}
