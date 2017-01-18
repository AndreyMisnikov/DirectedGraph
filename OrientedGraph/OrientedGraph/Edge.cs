
namespace OrientedGraph
{
    public class Edge
    {
        public Edge(Vertex startVertex, Vertex endVertex, int travelTime)
        {
            StartVertexId = startVertex.Id;
            EndVertexId = endVertex.Id;
            TravelTime = travelTime;
        }

        public string StartVertexId { get; set; }
        public string EndVertexId { get; set; }
        public int TravelTime { get; set; }
    }
}
