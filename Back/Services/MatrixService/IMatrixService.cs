using Services.Models;

namespace Services.MatrixService
{
    public interface IMatrixService
    {
        float[,] CreateEmptyMatrix(DimensionsModel dimensions);

        float[,] CopyMatrix(float[,] matrix);
    }
}