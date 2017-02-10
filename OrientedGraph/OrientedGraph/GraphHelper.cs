using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedGraph
{
    public enum CompareOperationForLimitParam
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
        /// <param name="compareOperationForLimitParam">
        /// Additional param for comparing journey's way with count of the stops
        /// </param>
        /// <returns>
        /// The <see cref="int"/> number of routes.
        /// </returns>
        public int GetNumberRoutes(Vertex startVertex, Vertex endVertex, int limitOfStops, CompareOperationForLimitParam compareOperationForLimitParam)
        {
            int countRoutes = 0;
            GetNextPoints(startVertex.Id, endVertex.Id, limitOfStops, compareOperationForLimitParam, 0, ref countRoutes);

            return countRoutes;
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
        /// <param name="limitJourneyTime">
        /// The limit of journey time
        /// </param>
        /// <param name="compareOperationForLimitParam">
        /// Additional param for comparing journey's way with count of the stops
        /// </param>
        /// <returns>
        /// The <see cref="int"/> number of routes.
        /// </returns>
        public int GetNumberRoutes(Vertex startVertex, Vertex endVertex, double limitJourneyTime, CompareOperationForLimitParam compareOperationForLimitParam)
        {
            int countRoutes = 0;
            GetNextPoints(startVertex.Id, endVertex.Id, limitJourneyTime, compareOperationForLimitParam, 0, ref countRoutes);

            return countRoutes;
        }

        private void GetNextPoints(string currentVertexId, string endVertexId, int limitOfStops, CompareOperationForLimitParam compareOperationForLimitParam, int currentStops, ref int countRoutes)
        {
            var nextEdges = _edges.First(item => item.Key == currentVertexId);
            if (currentStops > limitOfStops) return;

            foreach (var edge in nextEdges)
            {
                if (edge.EndVertexId == endVertexId)
                {
                    if (compareOperationForLimitParam == CompareOperationForLimitParam.Maximum && currentStops <= limitOfStops)
                    {
                        countRoutes++;
                    }
                    else if (currentStops == limitOfStops)
                    {
                        countRoutes++;
                        break;
                    }
                }
                GetNextPoints(edge.EndVertexId, endVertexId, limitOfStops, compareOperationForLimitParam, currentStops + 1, ref countRoutes);
            }
        }

        private void GetNextPoints(string currentVertexId, string endVertexId, double limitJourneyTime, CompareOperationForLimitParam compareOperationForLimitParam, double currentJourneyTime, ref int countRoutes)
        {
            var nextEdges = _edges.First(item => item.Key == currentVertexId);
            if (currentJourneyTime > limitJourneyTime) return;


            foreach (var edge in nextEdges)
            {
                double newJourneyTime = currentJourneyTime + edge.JourneyTime;
                if (edge.EndVertexId == endVertexId)
                {
                    if (compareOperationForLimitParam == CompareOperationForLimitParam.Maximum && newJourneyTime <= limitJourneyTime)
                    {
                            countRoutes++;
                    }
                    else if (newJourneyTime == limitJourneyTime)
                    {
                        countRoutes++;
                        break;
                    }
                }

                GetNextPoints(edge.EndVertexId, endVertexId, limitJourneyTime, compareOperationForLimitParam, newJourneyTime, ref countRoutes);
            }
        }

    }
}
