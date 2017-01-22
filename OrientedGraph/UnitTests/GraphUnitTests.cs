using System;
using System.Collections.Generic;
using OrientedGraph;
using Xunit;
using Xunit.Extensions;


namespace UnitTests
{
    public class GraphUnitTests
    {
        //TODO:  Get rid of search vertices by name and use objects as parameters. Each test should verify only one function.

        public Graph DataForTesting;
        public GraphUnitTests()
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
            Edge edge7 = new Edge(vertexCasablanca, vertexLiverpool, 3);
            Edge edge8 = new Edge(vertexCasablanca, vertexCapeTown, 6);
            Edge edge9 = new Edge(vertexCapeTown, vertexNY, 8);

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
                edge, edge2, edge3, edge4, edge5, edge6, edge7, edge8, edge9
            };

            DataForTesting = new Graph();
            foreach (var item in vertices)
            {
                DataForTesting.AddVertex(item);
            }
            foreach (var item in edges)
            {
                DataForTesting.AddEdge(item);
            }
        }


        [Theory]
        [InlineData("Liverpool")]
        [InlineData("Buenos Aires")]
        public void GetVertexByName_Should_ReturnExistsVertex(string vertexName)
        {
            //Arrange


            //Act
            var vertex = DataForTesting.GetVertexByName(vertexName);

            //Assert
            Assert.NotNull(vertex);
        }

        [Theory]
        [InlineData("Casablanca 3")]
        [InlineData("")]
        public void GetVertexByName_Should_ReturnNull(string vertexName)
        {
            //Arrange

            //Act
            var vertex = DataForTesting.GetVertexByName(vertexName);

            //Assert
            Assert.Null(vertex);
        }


        [Theory]
        [InlineData("Liverpool", "Casablanca", 3)]
        [InlineData("Liverpool", "Cape Town", 6)]
        public void GetDirectRouteLength_Should_ReturnCorrectRouteLength(string startVertexName, string endVertexName, int expectedRouteLength)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            int routeLength = DataForTesting.GetDirectRouteLength(startVertex, endVertex);

            //Assert
            Assert.Equal(expectedRouteLength, routeLength);
        }

        [Theory]
        [InlineData("Liverpool", "Casablanca", 9)]
        [InlineData("Liverpool", "Cape Town", 12)]
        public void GetDirectRouteLength_Should_ReturnIncorrectRouteLength(string startVertexName, string endVertexName, int expectedRouteLength)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            int routeLength = DataForTesting.GetDirectRouteLength(startVertex, endVertex);

            //Assert
            Assert.NotEqual(expectedRouteLength, routeLength);
        }

        [Theory]
        [InlineData("Liverpool", "Cape Town 23")]
        [InlineData("Minsk", "Mogilev")]
        public void GetDirectRouteLength_Should_ReturnArgumentNullException(string startVertexName, string endVertexName)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            Action getDirectRouteLength = () => DataForTesting.GetDirectRouteLength(startVertex, endVertex);

            //Assert
            Assert.Throws<ArgumentNullException>(getDirectRouteLength);
        }

        [Theory]
        [InlineData("New York", "Casablanca")]
        [InlineData("Casablanca", "Buenos Aires")]
        public void GetDirectRouteLength_Should_ReturnInvalidJourneyException(string startVertexName, string endVertexName)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            Action getDirectRouteLength = () => DataForTesting.GetDirectRouteLength(startVertex, endVertex);

            //Assert
            Assert.Throws<ArgumentException>(getDirectRouteLength);
        }


        public static IEnumerable<object[]> CorrectRoutesAndLengthTestData => new[]
        {
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "New York",
                    "Liverpool"
                }, 10
            },
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "Casablanca",
                    "Liverpool"
                }, 8
            },
            new object[]
            {
                new string[] { "Buenos Aires",
                    "Cape Town",
                    "New York",
                    "Liverpool",
                    "Casablanca"}, 19
            }
        };

        [Theory, MemberData(nameof(CorrectRoutesAndLengthTestData))]
        public void GetRouteLength_Should_ReturnCorrectRouteLength(IEnumerable<string> verticesRoute, int expectedRouteLength)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            //Act
            var routeLength = DataForTesting.GetRouteLength(vertices);

            //Assert
            Assert.Equal(expectedRouteLength, routeLength);
        }

        public static IEnumerable<object[]> CorrectRoutesAndIncorrectLengthTestData => new[]
        {
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "New York",
                    "Liverpool"
                }, 999
            },
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "Casablanca",
                    "Liverpool"
                }, 0
            },
            new object[]
            {
                new string[] { "Buenos Aires",
                    "Cape Town",
                    "New York",
                    "Liverpool",
                    "Casablanca"}, -1
            }
        };

        [Theory, MemberData(nameof(CorrectRoutesAndIncorrectLengthTestData))]
        public void GetRouteLength_Should_ReturnIncorrectRouteLength(IEnumerable<string> verticesRoute, int expectedRouteLength)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            //Act
            var routeLength = DataForTesting.GetRouteLength(vertices);

            //Assert
            Assert.NotEqual(expectedRouteLength, routeLength);
        }


        public static IEnumerable<object[]> WrongRoutesTestData => new[]
        {
            new object[]
            {
                new string[] {"Buenos Aires",
                    "Cape Town",
                    "Casablanca"
                }
            },
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "Liverpool",
                    "Cape Town"
                }
            },
            new object[]
            {
                new string[] {
                    "Buenos Aires",
                    "Liverpool",
                    "Buenos Aires"
                }
            },
        };

        [Theory, MemberData(nameof(WrongRoutesTestData))]
        public void GetRouteLength_Should_ReturnInvalidJourneyException(IEnumerable<string> verticesRoute)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            Action getRouteLength = () => DataForTesting.GetRouteLength(vertices);

            //Assert
            Assert.Throws<ArgumentException>(getRouteLength);
        }


    }
}
