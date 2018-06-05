using Services.Models;

namespace Services.MatrixService
{
    public class MatrixService: IMatrixService
    {
        public float[,] CreateEmptyMatrix(DimensionsModel dimensions) {
            return new float[dimensions.Height, dimensions.Width];
        }

        public float[,] CopyMatrix(float[,] matrix) {
            var matrixHeight = matrix.GetLength(0);
            var matrixWidth = matrix.GetLength(1);

            var dimensions = new DimensionsModel(matrixHeight, matrixWidth);
            var result = CreateEmptyMatrix(dimensions);

            for (var i = 0; i < matrixHeight; i++) {
                for (var j = 0; j < matrixWidth; j++) {
                    result[i,j] = matrix[i,j];
                }
            }

            return result;
        }
    }
}