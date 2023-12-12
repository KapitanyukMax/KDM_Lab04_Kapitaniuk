using System.Collections.Generic;

namespace Graphs
{
    public class IndirectedGraph : Graph
    {
        public IndirectedGraph(List<List<int>> adjacencyMatrix,
            List<string> vertexes, bool isPseudoGraph = false)
            : base(adjacencyMatrix, vertexes, isPseudoGraph)
        { }

        public override bool IsValidAdjacencyMatrix()
        {
            for (int i = 0; i < _adjacencyMatrix.Count; i++)
                for (int j = 0; j < _adjacencyMatrix[i].Count; j++)
                    if (_adjacencyMatrix[i][j] != _adjacencyMatrix[j][i])
                        return false;

            return base.IsValidAdjacencyMatrix();
        }

        public override bool IsValidWeightMatrix()
        {
            if (_weightMatrix == null)
                return true;

            for (int i = 0; i < _weightMatrix.Count; i++)
                for (int j = 0; j < _weightMatrix[i].Count; j++)
                    if (_weightMatrix[i][j] != _weightMatrix[j][i])
                        return false;

            return base.IsValidWeightMatrix();
        }

        protected override void SetEdges()
        {
            for (int i = 0; i < _adjacencyMatrix.Count; i++)
                for (int j = i + 1; j < _adjacencyMatrix[i].Count; j++)
                    if (_adjacencyMatrix[i][j] != 0)
                        Edges.Add(new()
                        { From = Vertexes[i], To = Vertexes[j] });
        }

        public override void TryAddEdgeWithVertexes(Edge edge)
        {
            base.TryAddEdgeWithVertexes(edge);

            _adjacencyMatrix[Vertexes.IndexOf(edge.To)][Vertexes.IndexOf(edge.From)] = 1;
        }
    }
}
