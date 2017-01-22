using System;
using System.Collections.Generic;
using System.Linq;

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

            if (!isExistStartVertex) throw new ArgumentException("Start vertex is not existed in the graph");
            if (!isExistEndVertex) throw new ArgumentException("End vertex is not existed in the graph");

            _edges.Add(edge);
        }

        public int GetDirectRouteLength(Vertex startVertex, Vertex endVertex)
        {
            if (startVertex == null) throw new ArgumentNullException(nameof(startVertex));
            if (endVertex == null) throw new ArgumentNullException(nameof(endVertex));

            foreach (var edge in _edges)
            {
                if (edge.StartVertexId == startVertex.Id && edge.EndVertexId == endVertex.Id)
                {
                    return edge.TravelTime;
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
        public int GetRouteLength(IEnumerable<Vertex> vertices)
        {
            if (vertices == null || !vertices.Any()) throw new ArgumentException("There are no element in vertices object");
            int travelTime = 0;

            if (vertices.Count() == 1)
            {
                return travelTime;
            }

            Vertex startVertex = vertices.First();
            Vertex endVertex = null;

            for (int i = 1; i < vertices.Count(); i++)
            {
                endVertex = vertices.ElementAt(i);
                travelTime += GetDirectRouteLength(startVertex, endVertex);

                startVertex = endVertex;
            }
            return travelTime;
        }

        public Vertex GetVertexByName(string vertexName)
        {
            return _vertices.FirstOrDefault(vertex => vertex.Name == vertexName);
        }
    }
}
