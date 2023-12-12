using Graphs;
using Matrixes;

namespace Relations
{
    public class Relation
    {
        public DirectedGraph Graph { get; init; }

        public List<List<int>> Matrix => Graph.AdjacencyMatrix;

        public List<string> Set => Graph.Vertexes;

        public Relation(List<List<int>> matrix,
            List<string> set)
            => Graph = new(matrix, set, true);

        public Relation(List<Edge> pairList, List<string> set)
        {
            Graph =
                new(MatrixOperations.CreateEmptyMatrix(set.Count),
                set, true);

            foreach (var pair in pairList)
            {
                if (!set.Contains(pair.From)
                    || !set.Contains(pair.To))
                    throw new ArgumentException(
                        paramName: nameof(pairList),
                        message: "Invalid pair list");

                Graph.TryAddEdgeWithVertexes(pair);
            }
        }

        public bool IsReflexive()
        {
            for (int i = 0; i < Set.Count; i++)
                if (Matrix[i][i] == 0)
                    return false;

            return true;
        }

        public bool IsIrreflexive()
        {
            for (int i = 0; i < Set.Count; i++)
                if (Matrix[i][i] != 0)
                    return false;

            return true;
        }

        public bool IsSymmetric()
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = i + 1; j < Set.Count; j++)
                    if (Matrix[i][j] == Matrix[j][i])
                        return false;

            return true;
        }

        public bool IsAntisymmetric()
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[i][j] != 0 && i != j
                        && Matrix[j][i] != 0)
                        return false;

            return true;
        }

        public bool IsAsymmetric() => IsAntisymmetric() && IsIrreflexive();

        public bool IsTransitive()
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = 0; j < Set.Count; j++)
                    for (int k = 0; k < Set.Count; k++)
                        if (Matrix[i][j] != 0
                            && Matrix[j][k] != 0
                            && Matrix[i][k] == 0)
                            return false;

            return true;
        }

        public bool IsAntitransitive()
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = 0; j < Set.Count; j++)
                    for (int k = 0; k < Set.Count; k++)
                        if (Matrix[i][j] != 0
                            && Matrix[j][k] != 0
                            && Matrix[i][k] != 0)
                            return false;

            return true;
        }

        public bool IsEquivalenceRelation()
            => IsReflexive() && IsSymmetric() && IsTransitive();

        public bool IsToleranceRelation()
            => IsReflexive() && IsSymmetric() && IsAntitransitive();

        public bool IsOrderRelation() => IsTransitive() && IsAntisymmetric();
    }
}
