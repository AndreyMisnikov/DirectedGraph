using System;
using System.Linq;

namespace OrientedGraph.Dijkstra
{
    public class DijkstraAlgoritm
    {
        private DijkstraGraph _dijkstraGraph;

        public double GetShortestJourneyTime(Vertex startVertex, Vertex endVertex)
        {
            ClearAlgoritmAdditionalData();

            var currentKeyPoint = _dijkstraGraph.Ways.Keys.First(startPoint => startPoint.Id == startVertex.Id);
            currentKeyPoint.Weight = 0;

            var endKeyPoint = _dijkstraGraph.Ways.Keys.First(point => point.Id == endVertex.Id);

            while (!IsEndSearchJourneyTime(endKeyPoint))
            {
                foreach (var endPointLink in _dijkstraGraph.Ways[currentKeyPoint].Where(endPoint => !endPoint.IsChecked)
                )
                {
                    var pointKey = _dijkstraGraph.Ways.Keys.First(startPoint => startPoint.Id == endPointLink.Id);
                    double journeyTime = currentKeyPoint.Weight + endPointLink.JourneyTimeToThisPoint;
                    if (pointKey.Weight > journeyTime)
                    {
                        pointKey.Weight = journeyTime;
                    }
                }
                currentKeyPoint.IsChecked = true;
                currentKeyPoint = FindNextStartPoint();
            }

            if (endKeyPoint.Weight == int.MaxValue) throw new ArgumentException($"Journey {startVertex.Name} - {endVertex.Name} is invalid");
            return endKeyPoint.Weight;
        }

        private bool IsEndSearchJourneyTime(DijkstraPoint endPoint = null)
        {
            if (endPoint != null && endPoint.IsChecked) return true;
            foreach (var point in _dijkstraGraph.Ways.Keys)
            {
                if (!point.IsChecked)
                {
                    return false;
                }
            }
            return true;
        }

        private DijkstraPoint FindNextStartPoint()
        {
            DijkstraPoint nextStartPoint = null;
            foreach (var point in _dijkstraGraph.Ways.Keys.Where(point => !point.IsChecked))
            {
                if (nextStartPoint == null)
                {
                    nextStartPoint = point;
                    continue;
                }
                if (nextStartPoint.Weight > point.Weight)
                {
                    nextStartPoint = point;
                }
            }
            return nextStartPoint;
        }

        private void ClearAlgoritmAdditionalData()
        {
            foreach (var dijkstraStartPoint in _dijkstraGraph.Ways.Keys)
            {
                dijkstraStartPoint.Weight = int.MaxValue;
                dijkstraStartPoint.IsChecked = false;
            }
        }

        public DijkstraAlgoritm(Graph graph)
        {
            _dijkstraGraph = new DijkstraGraph(graph);
        }
    }
}
