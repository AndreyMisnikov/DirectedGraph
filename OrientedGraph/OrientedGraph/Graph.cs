using System;
using System.Collections.Generic;
using System.Linq;
using OrientedGraph.Dijkstra;

namespace OrientedGraph
{
    public class Graph
    {
        private List<Vertex> _vertices;
        private List<Edge> _edges;

        public Graph()
        {
            _vertices = new List<Vertex>();
            _edges = new List<Edge>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (_vertices.Any(item => item.Id == vertex.Id)) throw new ArgumentException("The vertex is existed already");
            _vertices.Add(vertex);
        }

        public Vertex GetVertexByName(string vertexName)
        {
            return _vertices.FirstOrDefault(vertex => vertex.Name == vertexName);
        }

        public void AddEdge(Edge edge)
        {
            if (edge.StartVertexId == edge.EndVertexId) throw new ArgumentException("Start and End vertices must be different");

            bool isExistStartVertex = false;
            bool isExistEndVertex = false;
            foreach (var vertex in _vertices)
            {
                if (vertex.Id == edge.StartVertexId)
                {
                    isExistStartVertex = true;
                }
                else if (vertex.Id == edge.EndVertexId)
                {
                    isExistEndVertex = true;
                }
            }

            if (!isExistStartVertex) throw new ArgumentException("Start vertex is not existed in the journey map");
            if (!isExistEndVertex) throw new ArgumentException("End vertex is not existed in the journey map");

            _edges.Add(edge);
        }

        public IEnumerable<Vertex> Vertices => _vertices;

        public IEnumerable<Edge> Edges => _edges;

        public int GetDirectJourneyTime(Vertex startVertex, Vertex endVertex)
        {
            if (startVertex == null) throw new ArgumentNullException(nameof(startVertex));
            if (endVertex == null) throw new ArgumentNullException(nameof(endVertex));

            foreach (var edge in _edges)
            {
                if (edge.StartVertexId == startVertex.Id && edge.EndVertexId == endVertex.Id)
                {
                    return edge.JourneyTime;
                }
            }
            throw new ArgumentException($"Journey {startVertex.Name} - {endVertex.Name} is invalid");
        }

        /// <summary>
        /// Gets route length.
        /// </summary>
        /// <param name="vertices">
        /// The vertices ordered from the starting point to the end point
        /// </param>
        /// <exception cref="ArgumentException">There are no element in vertices object exception".</exception>
        /// <returns>
        /// The<see cref="int"/> length of route.
        /// </returns>
        public int GetJourneyTime(IEnumerable<Vertex> vertices)
        {
            if (vertices == null || !vertices.Any()) throw new ArgumentException("There are no element in vertices object");
            int journeyTime = 0;

            if (vertices.Count() == 1)
            {
                return journeyTime;
            }

            Vertex startVertex = vertices.First();
            Vertex endVertex = null;

            for (int i = 1; i < vertices.Count(); i++)
            {
                endVertex = vertices.ElementAt(i);
                journeyTime += GetDirectJourneyTime(startVertex, endVertex);

                startVertex = endVertex;
            }
            return journeyTime;
        }

        public double GetShortestJourneyTime(Vertex startVertex, Vertex endVertex)
        {
            if (startVertex == null) throw new ArgumentNullException(nameof(startVertex));
            if (endVertex == null) throw new ArgumentNullException(nameof(endVertex));
            if (startVertex == endVertex) return 0;

            DijkstraAlgoritm diyDijkstraAlgoritm = new DijkstraAlgoritm(this);
            return diyDijkstraAlgoritm.GetShortestJourneyTime(startVertex, endVertex);
        }

        /// <summary>
        /// Gets number of routes with return to port
        /// </summary>
        /// <param name="startVertex">
        /// Start point of journey 
        /// </param>
        /// <param name="maxStops">
        /// The maximum of stops without start port 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> number of routes.
        /// </returns>
        public int GetNumberOfRoutesWithReturnToPort(Vertex startVertex, int maxStops)
        {
            throw new NotImplementedException();
        }
    }
}
