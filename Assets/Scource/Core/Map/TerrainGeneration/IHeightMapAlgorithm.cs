using System;

namespace Core.Map.TerrainGeneration
{
    public interface IHeightMapAlgorithm
    {
        float[,] GetHeightMap(int resolution);
    }
}

