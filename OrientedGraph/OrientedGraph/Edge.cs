
namespace OrientedGraph
{
    public class Edge
    {
        public Edge(Vertex startVertex, Vertex endVertex, int journeyTime)
        {
            StartVertexId = startVertex.Id;
            EndVertexId = endVertex.Id;
            JourneyTime = journeyTime;
        }

        public string StartVertexId { get; set; }
        public string EndVertexId { get; set; }
        public int JourneyTime { get; set; }
    }
}
