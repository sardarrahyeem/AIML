using Microsoft.Extensions.VectorData;

namespace VectorDataAI
{
    internal class CloudService
    {
        [VectorStoreKey]
        public int Key { get; set; }

        [VectorStoreData]
        public string Name { get; set; } = null!;

        [VectorStoreData]
        public string Description { get; set; } = null!;

        [VectorStoreVector(dimensions:384, DistanceFunction = DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float> Vector { get; set; }
    }
}