using System.Collections.Generic;
using OrientedGraph;
using Xunit;


namespace UnitTests
{
    public class UnitTest
    {
        [Fact]
        public void GetDirectRouteTest()
        {
            Vertex vertexNY = new Vertex("New York");
            Vertex vertexLiverpool = new Vertex("Liverpool");
            Vertex vertexCasablanca = new Vertex("Casablanca");
            Vertex vertexBA = new Vertex("Buenos Aires");
            Vertex vertexCapeTown = new Vertex("Cape Town");

            Edge edge = new Edge(vertexBA, vertexNY, 6);
            Edge edge2 = new Edge(vertexBA, vertexCasablanca, 5);
            Edge edge3 = new Edge(vertexBA, vertexCapeTown, 4);
            Edge edge4 = new Edge(vertexNY, vertexLiverpool, 4);
            Edge edge5 = new Edge(vertexLiverpool, vertexCasablanca, 3);
            Edge edge6 = new Edge(vertexLiverpool, vertexCapeTown, 6);
            Edge edge7 = new Edge(vertexCasablanca, vertexCapeTown, 6);
            Edge edge8 = new Edge(vertexCapeTown, vertexNY, 8);

            IEnumerable<Vertex> vertices = new List<Vertex>
            {
                vertexNY,
                vertexLiverpool,
                vertexCasablanca,
                vertexBA,
                vertexCapeTown
            };

            IEnumerable<Edge> edges = new List<Edge>
            {
                edge, edge2, edge3, edge4, edge5, edge6, edge7, edge8
            };

            Graph graph = new Graph();
            foreach (var item in vertices)
            {
                graph.AddVertex(item);
            }
            foreach (var item in edges)
            {
                graph.AddEdge(item);
            }

            Assert.Equal(3, graph.GetDirectRouteLength(vertexLiverpool, vertexCasablanca));
        }
    }
}
