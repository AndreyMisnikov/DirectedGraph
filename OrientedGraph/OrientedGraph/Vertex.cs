using System;

namespace OrientedGraph
{
    public class Vertex
    {
        public Vertex(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
