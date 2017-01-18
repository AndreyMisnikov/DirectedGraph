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
            foreach (var edge in _edges)
            {
                if (edge.StartVertexId == startVertex.Id && edge.EndVertexId == endVertex.Id)
                {
                    return edge.TravelTime;
                }
            }
            throw new ArgumentException("Journey is invalid");
        }

    }
}
