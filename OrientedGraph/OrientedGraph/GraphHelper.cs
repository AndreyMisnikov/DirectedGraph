using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedGraph
{
    public enum CompareOperationForStopsParam
    {
        Maximum,
        Exactly
    } 

    class GraphHelper
    {
        private IEnumerable<IGrouping<string, Edge>> _edges;

        public GraphHelper(Graph graph)
        {
            _edges = graph.Edges.GroupBy(edge => edge.StartVertexId);
        }

        /// <summary>
        /// Gets number available routes. Return back to start point in journey is available.
        /// </summary>
        /// <param name="startVertex">
        /// Start point of journey 
        /// </param>
        /// <param name="endVertex">
        /// End point of journey 
        /// </param>
        /// <param name="limitOfStops">
        /// The stops without start point 
        /// </param>
        /// <param name="compareOperationForStopsParam">
        /// Additional param for comparing journey's way with count of the stops
        /// </param>
        /// <returns>
        /// The <see cref="int"/> number of routes.
        /// </returns>
        public int GetNumberRoutes(Vertex startVertex, Vertex endVertex, int limitOfStops, CompareOperationForStopsParam compareOperationForStopsParam)
        {
            int countRoutes = 0;
            GetNextPoints(startVertex.Id, endVertex.Id, limitOfStops, compareOperationForStopsParam, 0, ref countRoutes);

            return countRoutes;
        }

        private void GetNextPoints(string currentVertexId, string endVertexId, int limitOfStops, CompareOperationForStopsParam compareOperationForStopsParam, int currentStops, ref int countRoutes)
        {
            var nextEdges = _edges.First(item => item.Key == currentVertexId);
            if (currentStops > limitOfStops) return;

            foreach (var edge in nextEdges)
            {
                if (edge.EndVertexId == endVertexId)
                {
                    if (compareOperationForStopsParam == CompareOperationForStopsParam.Maximum)
                    {
                        if (currentStops <= limitOfStops)
                        {
                            countRoutes++;
                        }
                    }
                    else
                    {
                        if (currentStops == limitOfStops)
                        {
                            countRoutes++;
                            break;
                        }
                    }
                }
                if (edge == nextEdges.First()) currentStops++;
                GetNextPoints(edge.EndVertexId, endVertexId, limitOfStops, compareOperationForStopsParam, currentStops, ref countRoutes);
            }
        }

    }
}
