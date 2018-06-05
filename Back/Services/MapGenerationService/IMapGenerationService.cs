using Services.Models;

namespace Services.MapGenerationService
{
    public interface IMapGenerationService
    {
        float[,] RandomGen(DimensionsModel dimensions, int nrPlayers);
    }
}