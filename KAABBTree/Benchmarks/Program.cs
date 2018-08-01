using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;
using KAABBTree;
using static System.Diagnostics.Stopwatch;

namespace Benchmarks
{
    class Program
    {
        private static IList<Triangle2d> GetRandomMesh(int numberOfTriangles, Matrix2d rotationMatrix2D, Vector2d center)
        {
            Vector2d TranslateV(Vector2d vec) => rotationMatrix2D * vec + center;
            Triangle2d TranslateT(Triangle2d tra) =>
                new Triangle2d(
                TranslateV(tra[0]),
                TranslateV(tra[1]),
                TranslateV(tra[2]));


            return Enumerable.Range(0, numberOfTriangles)
                .Select(_ => Triangle2DExtension.GetRandom())
                .Select(TranslateT).ToList();
        }

        private static Random Random = new Random();
        private const int NumberOfTriangles = 10000;
        private static (double, double, double, double) GetRandomMesure(double diff)
        {
            var rotMat = Matrix2DExtension.GetRandomRotationMatrix();
            var mesh1 = GetRandomMesh(NumberOfTriangles, rotMat, Vector2d.Zero);
            var mesh2 = GetRandomMesh(NumberOfTriangles, rotMat, rotMat * Vector2d.AxisX * (1 + diff));

            var watch = StartNew();
            AABBTree t1 = new AABBTree(mesh1), t2 = new AABBTree(mesh2);
            watch.Stop();
            var AABBBuildTime = watch.ElapsedMilliseconds;
            var AABBCollisionAction = AABBTree.TestIntersect(t1, t2).Item2;
            watch = StartNew();
            KAABBTree.KAABBTree kt1 = new KAABBTree.KAABBTree(mesh1), kt2 = new KAABBTree.KAABBTree(mesh2);
            watch.Stop();
            var KAABBBuildTime = watch.ElapsedMilliseconds;
            var KAABBCollisionAction = KAABBTree.KAABBTree.TestIntersect(kt1, kt2).Item2;

            return (AABBBuildTime, AABBCollisionAction,KAABBBuildTime, KAABBCollisionAction);
        }

        private static void MesureAndPrint()
        {
            for (int x = 1; x < 1000; x++)
            {
                var (AABBBuildTime, AABBCollisionTime, KAABBBuildTime, KAABBCollisionTime)
                    = GetRandomMesure(x * 0.5 / 1000);
                Console.WriteLine($"{AABBBuildTime} {AABBCollisionTime} {KAABBBuildTime} {KAABBCollisionTime}");
            }
        }

        public static void Main(string[] args)
        {
            MesureAndPrint();
        }
    }
}
