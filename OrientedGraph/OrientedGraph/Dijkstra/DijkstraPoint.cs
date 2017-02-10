namespace OrientedGraph.Dijkstra
{
    //I use JourneyTimeToThisPoint property if required fast execution. I could create the many-to-many relationship as for Graph class.
    internal class DijkstraPoint : IVertex
    {
        public DijkstraPoint(IVertex vertex)
        {
            Id = vertex.Id;
            Name = vertex.Name;

            InitializeNewPoint();
        }

        private void InitializeNewPoint()
        {
            IsChecked = false;
            Weight = int.MaxValue;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsChecked { get; set; }
        public double Weight { get; set; }

        public double JourneyTimeToThisPoint { get; set; }
    }
}
