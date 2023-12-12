using Matrixes;

namespace Relations
{
    public static class RelationOperations
    {
        public static Relation GetReciprocal(Relation relation)
            => new(MatrixOperations.Transponse(relation.Matrix),
                relation.Set);

        public static Relation Composition(
            Relation relation1, Relation relation2)
        {
            if (relation1.Set.Count != relation2.Set.Count)
                throw new InvalidOperationException(
                    "Cannot compose relations of different sizes");

            for (int i = 0; i < relation1.Set.Count; i++)
                if (relation1.Set[i] != relation2.Set[i])
                    throw new InvalidOperationException(
                        "Cannot compose relations with different vertexes");

            return new(MatrixOperations.LogicalProduct(
                relation1.Matrix, relation2.Matrix),
                relation1.Set);
        }
    }
}
