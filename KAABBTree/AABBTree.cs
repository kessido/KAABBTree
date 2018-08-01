using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using g3;
namespace KAABBTree
{
    public class AABBTree
    {
        private readonly AABBTree _left, _right;
        private readonly AxisAlignedBox2d _box;
        private readonly bool _isALeef;
        private readonly Triangle2d _mesh;

        public AABBTree(IList<Triangle2d> mesh)
        {
            _box = mesh.GetBBox();
            mesh = (_box.Height > _box.Width ?
                mesh.OrderBy(tra => tra.V0.y)
                : mesh.OrderBy(tra => tra.V0.x)).ToList();
            if (mesh.Count != 1)
            {
                _isALeef = false;
                var midIndex = mesh.Count / 2;
                _left = new AABBTree(mesh.Take(midIndex).ToList());
                _right = new AABBTree(mesh.Skip(midIndex).Take(mesh.Count - midIndex).ToList());
            }
            else
            {
                _isALeef = true;
                _mesh = mesh.First();
            }
        }

        public static (bool, int) TestIntersect(AABBTree o1, AABBTree o2)
        {
            if (!o2._box.Intersects(o1._box)) return (false, 1);
            if (o1._isALeef && o2._isALeef)
            {
                return (new IntrTriangle2Triangle2(o1._mesh, o2._mesh).Test(), 2);
            }
            else
            {
                if (!o1._isALeef)
                {
                    var (b, r) = TestIntersect(o1._left, o2);
                    if (b) return (b, r + 1);
                    var (b1, r1) = TestIntersect(o1._right, o2);
                    return (b || b1, r + r1 + 1);
                }
                else
                {
                    return TestIntersect(o2, o1);

                }
            }
        }
    }
}
