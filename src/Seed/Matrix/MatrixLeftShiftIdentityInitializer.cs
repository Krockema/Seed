using Seed.Parameter.TransitionMatrix;

namespace Seed.Matrix
{       
    /// <summary>
    /// Creates a identity matrix (Einheitsmatrix)
    /// </summary>
    public class MatrixLeftShiftIdentityInitializer : IMatrixInitializer
    {
        int _size;
        public MatrixLeftShiftIdentityInitializer(int size)
        {
            _size = size;
        }

        public double CellValue(int i, int j)
        {
            if (i + 1 == j) return 1;
            if (j == 0 && i == _size - 1) return 1;
            return 0;
        }
    }
}