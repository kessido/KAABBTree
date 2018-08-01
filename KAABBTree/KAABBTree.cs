using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace KAABBTree
{
    public class KAABBTree
    {
        private static readonly Random Random = new Random();
        private const int NumberOfAlignment = 40;
        private static readonly IList<Matrix2d> Alignment = InitAligment();
        private static IList<Matrix2d> InitAligment()
        {
            var res = new Matrix2d[NumberOfAlignment];
            for (var i = 0; i < NumberOfAlignment; ++i)
                res[i] = Matrix2DExtension.GetRandomRotationMatrix();
            return res;
        }

        private readonly KAABBTree _left, _right;
        private readonly IList<AxisAlignedBox2d> _boxes;
        private readonly bool _isALeef;
        private readonly Triangle2d _mesh;

        public KAABBTree(IList<Triangle2d> mesh)
        {
            _boxes = new AxisAlignedBox2d[NumberOfAlignment];
            for (var i = 0; i < NumberOfAlignment; ++i)
            {
                var i1 = i;
                _boxes[i] = mesh.Select(tra => tra.Rotate(Alignment[i1])).GetBBox();
            }

            var alignmentToSplitBy = Random.Next(NumberOfAlignment);
            mesh = (_boxes[alignmentToSplitBy].Height > _boxes[alignmentToSplitBy].Width ?
                mesh.OrderBy(tra => (Alignment[alignmentToSplitBy] * tra.V0).y)
                : mesh.OrderBy(tra => (Alignment[alignmentToSplitBy] * tra.V0).x)).ToList();
            if (mesh.Count != 1)
            {
                _isALeef = false;
                var midIndex = mesh.Count / 2;
                _left = new KAABBTree(mesh.Take(midIndex).ToList());
                _right = new KAABBTree(mesh.Skip(midIndex).Take(mesh.Count - midIndex).ToList());
            }
            else
            {
                _isALeef = true;
                _mesh = mesh.First();
            }
        }

        public static (bool, int) TestIntersect(KAABBTree o1, KAABBTree o2)
        {
            return TestIntersect(0, o1, o2);
        }

        private static (bool, int) TestIntersect(int index, KAABBTree o1, KAABBTree o2)
        {
            if (!o2._boxes[index].Intersects(o1._boxes[index])) return (false, 1);
            if (o1._isALeef && o2._isALeef)
            {
                return (new IntrTriangle2Triangle2(o1._mesh, o2._mesh).Test(), 2);
            }
            else
            {
                if (!o1._isALeef)
                {
                    index = index + 1;
                    if (index == NumberOfAlignment) index = 0;
                    var (b, r) = TestIntersect(index, o1._left, o2);
                    if (b) return (b, r + 1);
                    var (b1, r1) = TestIntersect(index, o1._right, o2);
                    return (b || b1, r + r1 + 1);
                }
                else
                {
                    return TestIntersect(index, o2, o1);
                }
            }
        }
    }
}
