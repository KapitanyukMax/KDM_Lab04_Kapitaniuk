using System.Collections.Generic;

namespace Graphs
{
    public class DirectedGraph : Graph
    {
        public DirectedGraph(List<List<int>> adjacencyMatrix,
            List<string> vertexes, bool isPseudoGraph = false)
            : base(adjacencyMatrix, vertexes, isPseudoGraph)
        { }

        public override bool IsValidAdjacencyMatrix()
        {
            return base.IsValidAdjacencyMatrix();
        }

        protected override void SetEdges()
        {
            for (int i = 0; i < _adjacencyMatrix.Count; i++)
                for (int j = 0; j < _adjacencyMatrix[i].Count; j++)
                    if (_adjacencyMatrix[i][j] != 0)
                        Edges.Add(new()
                        { From = Vertexes[i], To = Vertexes[j] });
        }
    }
}
