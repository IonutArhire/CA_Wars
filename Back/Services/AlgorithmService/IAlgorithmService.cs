using System.Collections.Generic;
using Services.Models;

namespace Services.AlgorithmService
{
    public interface IAlgorithmService
    {
        GameResultModel RunGame(MatchModel match);
    }
}