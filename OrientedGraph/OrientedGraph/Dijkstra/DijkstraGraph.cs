using System.Collections.Generic;
using System.Linq;

namespace OrientedGraph.Dijkstra
{
    public class DijkstraGraph
    {
        private Dictionary<DijkstraPoint, List<DijkstraPoint>> _ways;

        internal Dictionary<DijkstraPoint, List<DijkstraPoint>> Ways => _ways;

        public DijkstraGraph(Graph graph)
        {
            _ways = new Dictionary<DijkstraPoint, List<DijkstraPoint>>();

            foreach (var vertex in graph.Vertices)
            {
                _ways[new DijkstraPoint(vertex)] = new List<DijkstraPoint>();
            }

            foreach (var edge in graph.Edges)
            {
                DijkstraPoint dijkstraVertexFrom = _ways.Keys.First(diykstraPoint => diykstraPoint.Id == edge.StartVertexId);
              
                Vertex vertexTo = graph.Vertices.First(vertice => vertice.Id == edge.EndVertexId);
                DijkstraPoint dijkstraVertexTo = new DijkstraPoint(vertexTo);
                dijkstraVertexTo.JourneyTimeToThisPoint = edge.JourneyTime;

                _ways[dijkstraVertexFrom].Add(dijkstraVertexTo);
            }
        }
    }
}
