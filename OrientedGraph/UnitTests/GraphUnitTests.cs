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
        public void GetDirectJourneyTime_Should_ReturnCorrectJourneyTime(string startVertexName, string endVertexName, int expectedJourneyTime)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            int journeyTime = DataForTesting.GetDirectJourneyTime(startVertex, endVertex);

            //Assert
            Assert.Equal(expectedJourneyTime, journeyTime);
        }

        [Theory]
        [InlineData("Liverpool", "Casablanca", 9)]
        [InlineData("Liverpool", "Cape Town", 12)]
        public void GetDirectJourneyTime_Should_ReturnIncorrectJourneyTime(string startVertexName, string endVertexName, int expectedJourneyTime)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            int journeyTime = DataForTesting.GetDirectJourneyTime(startVertex, endVertex);

            //Assert
            Assert.NotEqual(expectedJourneyTime, journeyTime);
        }

        [Theory]
        [InlineData("Liverpool", "Cape Town 23")]
        [InlineData("Minsk", "Mogilev")]
        public void GetDirectJourneyTime_Should_ReturnArgumentNullException(string startVertexName, string endVertexName)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            Action getDirectJourneyTime = () => DataForTesting.GetDirectJourneyTime(startVertex, endVertex);

            //Assert
            Assert.Throws<ArgumentNullException>(getDirectJourneyTime);
        }

        [Theory]
        [InlineData("New York", "Casablanca")]
        [InlineData("Casablanca", "Buenos Aires")]
        public void GetDirectJourneyTime_Should_ReturnInvalidJourneyException(string startVertexName, string endVertexName)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            Action getDirectJourneyTime = () => DataForTesting.GetDirectJourneyTime(startVertex, endVertex);

            //Assert
            Assert.Throws<ArgumentException>(getDirectJourneyTime);
        }


        public static IEnumerable<object[]> CorrectRoutesAndJourneyTimeTestData => new[]
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

        [Theory, MemberData(nameof(CorrectRoutesAndJourneyTimeTestData))]
        public void GetJourneyTime_Should_ReturnCorrectJourneyTime(IEnumerable<string> verticesRoute, int expectedJourneyTime)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            //Act
            var journeyTime = DataForTesting.GetJourneyTime(vertices);

            //Assert
            Assert.Equal(expectedJourneyTime, journeyTime);
        }

        public static IEnumerable<object[]> CorrectRoutesAndIncorrectJourneyTimeTestData => new[]
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

        [Theory, MemberData(nameof(CorrectRoutesAndIncorrectJourneyTimeTestData))]
        public void GetJourneyTime_Should_ReturnIncorrectJourneyTime(IEnumerable<string> verticesRoute, int expectedJourneyTime)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            //Act
            var journeyTime = DataForTesting.GetJourneyTime(vertices);

            //Assert
            Assert.NotEqual(expectedJourneyTime, journeyTime);
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
        public void GetJourneyTime_Should_ReturnInvalidJourneyException(IEnumerable<string> verticesRoute)
        {
            //Arrange
            List<Vertex> vertices = new List<Vertex>();
            foreach (var vertex in verticesRoute)
            {
                vertices.Add(DataForTesting.GetVertexByName(vertex));
            }

            //Act
            Action getJourneyTime = () => DataForTesting.GetJourneyTime(vertices);

            //Assert
            Assert.Throws<ArgumentException>(getJourneyTime);
        }


        [Theory]
        [InlineData("Buenos Aires", "Liverpool", 8)]
        [InlineData("Cape Town", "Casablanca", 15)]
        [InlineData("New York", "New York", 0)]
        public void GetShortestJourneyTime_Should_ReturnCorrectJourneyTime(string startVertexName, string endVertexName, int expectedJourneyTime)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            var journeyTime = DataForTesting.GetShortestJourneyTime(startVertex, endVertex);

            //Assert
            Assert.Equal(expectedJourneyTime, journeyTime);
        }

        [Theory]
        [InlineData("Buenos Aires", "Liverpool", 10)]
        [InlineData("Buenos Aires", "Liverpool", 16)]
        [InlineData("Buenos Aires", "Liverpool", 0)]
        [InlineData("New York", "New York", 1)]
        public void GetShortestJourneyTime_Should_ReturnIncorrectJourneyTime(string startVertexName, string endVertexName, int expectedJourneyTime)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            var journeyTime = DataForTesting.GetShortestJourneyTime(startVertex, endVertex);

            //Assert
            Assert.NotEqual(expectedJourneyTime, journeyTime);
        }

        [Theory]
        [InlineData("Cape Town", "Buenos Aires")]
        public void GetShortestJourneyTime_Should_ReturnInvalidJourneyException(string startVertexName, string endVertexName)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            Action getJourneyTime = () => DataForTesting.GetShortestJourneyTime(startVertex, endVertex);

            //Assert
            Assert.Throws<ArgumentException>(getJourneyTime);
        }

        [Theory]
        [InlineData("Liverpool", "Liverpool", 3, "Maximum", 4)]
        [InlineData("Buenos Aires", "Liverpool", 4, "Exactly", 3)]
        [InlineData("New York", "New York", 3, "Maximum", 2)]
        public void GetNumberOfRoutes_Should_ReturnCorrectJourneyRoutes(string startVertexName, string endVertexName, int limitOfStops, string compareOperationForStopsParam, int expectedCountRoutes)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            CompareOperationForStopsParam compareOperation;
            Enum.TryParse(compareOperationForStopsParam, out compareOperation);
            var journeyRoutes = DataForTesting.GetNumberRoutes(startVertex, endVertex, limitOfStops, compareOperation);

            //Assert
            Assert.Equal(expectedCountRoutes, journeyRoutes);
        }

        [Theory]
        [InlineData("Liverpool", "Liverpool", 9, "Maximum", 2)]
        [InlineData("New York", "New York", 4, "Maximum", 2)] //3 expteceted
        [InlineData("New York", "New York", 3, "Exactly", 2)] //1
        [InlineData("Buenos Aires", "New York", 1, "Exactly", 2)] //1
        [InlineData("Buenos Aires", "New York", 1, "Maximum", 3)] //2
        [InlineData("Buenos Aires", "New York", 1, "Exactly", 0)] //1
        [InlineData("Buenos Aires", "New York", 1, "Maximum", 0)] //2
        public void GetNumberOfRoutes_Should_ReturnIncorrectJourneyRoutes(string startVertexName, string endVertexName, int limitOfStops, string compareOperationForStopsParam, int expectedCountRoutes)
        {
            //Arrange
            var startVertex = DataForTesting.GetVertexByName(startVertexName);
            var endVertex = DataForTesting.GetVertexByName(endVertexName);

            //Act
            CompareOperationForStopsParam compareOperation;
            Enum.TryParse(compareOperationForStopsParam, out compareOperation);
            var journeyRoutes = DataForTesting.GetNumberRoutes(startVertex, endVertex, limitOfStops, compareOperation);

            //Assert
            Assert.NotEqual(expectedCountRoutes, journeyRoutes);
        }
    }
}
