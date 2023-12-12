using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public static class GraphOperations
    {
        public static IndirectedGraph Complement(IndirectedGraph graph)
        {
            List<List<int>> complementAdjacencyMatrix = new();

            for (int i = 0; i < graph.AdjacencyMatrix.Count; i++)
            {
                complementAdjacencyMatrix.Add(new());

                for (int j = 0; j < graph.AdjacencyMatrix[i].Count; j++)
                {
                    if (i == j)
                        complementAdjacencyMatrix[i].Add(0);
                    else
                        complementAdjacencyMatrix[i].Add(1 - graph.AdjacencyMatrix[i][j]);
                }
            }

            return new(complementAdjacencyMatrix, graph.Vertexes);
        }

        public static DirectedGraph Complement(DirectedGraph graph)
        {
            List<List<int>> complementAdjacencyMatrix = new();

            for (int i = 0; i < graph.AdjacencyMatrix.Count; i++)
            {
                complementAdjacencyMatrix.Add(new());

                for (int j = 0; j < graph.AdjacencyMatrix[i].Count; j++)
                {
                    if (i == j)
                        complementAdjacencyMatrix[i].Add(0);
                    else
                        complementAdjacencyMatrix[i].Add(1 - graph.AdjacencyMatrix[i][j]);
                }
            }

            return new(complementAdjacencyMatrix, graph.Vertexes);
        }

        public static IndirectedGraph Intersection(IndirectedGraph graph1, IndirectedGraph graph2)
        {
            List<List<int>> intersectionAdjacencyMatrix = new();

            for (int i = 0; i < Math.Min(graph1.AdjacencyMatrix.Count, graph2.AdjacencyMatrix.Count); i++)
            {
                intersectionAdjacencyMatrix.Add(new());

                for (int j = 0; j < Math.Min(graph1.AdjacencyMatrix[i].Count, graph2.AdjacencyMatrix[i].Count); j++)
                    intersectionAdjacencyMatrix[i].Add(graph1.AdjacencyMatrix[i][j] & graph2.AdjacencyMatrix[i][j]);
            }

            return new(intersectionAdjacencyMatrix,
                graph1.Vertexes.Intersect(graph2.Vertexes).ToList());
        }

        public static DirectedGraph Intersection(DirectedGraph graph1, DirectedGraph graph2)
        {
            List<List<int>> intersectionAdjacencyMatrix = new();

            for (int i = 0; i < Math.Min(graph1.AdjacencyMatrix.Count, graph2.AdjacencyMatrix.Count); i++)
            {
                intersectionAdjacencyMatrix.Add(new());

                for (int j = 0; j < Math.Min(graph1.AdjacencyMatrix.Count, graph2.AdjacencyMatrix.Count); j++)
                    intersectionAdjacencyMatrix[i].Add(graph1.AdjacencyMatrix[i][j] & graph2.AdjacencyMatrix[i][j]);
            }

            return new(intersectionAdjacencyMatrix,
                graph1.Vertexes.Intersect(graph2.Vertexes).ToList());
        }

        public static IndirectedGraph Union(IndirectedGraph graph1, IndirectedGraph graph2)
        {
            List<List<int>> unionAdjacencyMatrix = new();

            var smaller = graph1.AdjacencyMatrix.Count < graph2.AdjacencyMatrix.Count ? graph1 : graph2;
            var larger = graph1.AdjacencyMatrix.Count < graph2.AdjacencyMatrix.Count ? graph2 : graph1;

            for (int i = 0; i < smaller.AdjacencyMatrix.Count; i++)
            {
                unionAdjacencyMatrix.Add(new());

                for (int j = 0; j < smaller.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(graph1.AdjacencyMatrix[i][j] | graph2.AdjacencyMatrix[i][j]);

                for (int j = smaller.AdjacencyMatrix[i].Count; j < larger.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(larger.AdjacencyMatrix[i][j]);
            }

            for (int i = smaller.AdjacencyMatrix.Count; i < larger.AdjacencyMatrix.Count; i++)
            {
                unionAdjacencyMatrix.Add(new());

                for (int j = 0; j < larger.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(larger.AdjacencyMatrix[i][j]);
            }

            return new(unionAdjacencyMatrix, graph1.Vertexes.Union(graph2.Vertexes).ToList());
        }

        public static DirectedGraph Union(DirectedGraph graph1, DirectedGraph graph2)
        {
            List<List<int>> unionAdjacencyMatrix = new();

            var smaller = graph1.AdjacencyMatrix.Count < graph2.AdjacencyMatrix.Count ? graph1 : graph2;
            var larger = graph1.AdjacencyMatrix.Count < graph2.AdjacencyMatrix.Count ? graph2 : graph1;

            for (int i = 0; i < smaller.AdjacencyMatrix.Count; i++)
            {
                unionAdjacencyMatrix.Add(new());

                for (int j = 0; j < smaller.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(graph1.AdjacencyMatrix[i][j] | graph2.AdjacencyMatrix[i][j]);

                for (int j = smaller.AdjacencyMatrix[i].Count; j < larger.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(larger.AdjacencyMatrix[i][j]);
            }

            for (int i = smaller.AdjacencyMatrix.Count; i < larger.AdjacencyMatrix.Count; i++)
            {
                unionAdjacencyMatrix.Add(new());

                for (int j = 0; j < larger.AdjacencyMatrix[i].Count; j++)
                    unionAdjacencyMatrix[i].Add(larger.AdjacencyMatrix[i][j]);
            }

            return new(unionAdjacencyMatrix, graph1.Vertexes.Union(graph2.Vertexes).ToList());
        }

        public static IndirectedGraph Difference(IndirectedGraph graph1, IndirectedGraph graph2)
        {
            IndirectedGraph difference = new(new(), new());

            foreach (var vertex in graph1.Vertexes.Except(graph2.Vertexes))
                difference.TryAddVertex(vertex);

            foreach (var edge1 in graph1.Edges)
            {
                bool isInGraph2 = false;
                foreach (var edge2 in graph2.Edges)
                {
                    if (edge1.From == edge2.From && edge1.To == edge2.To)
                    {
                        isInGraph2 = true;
                        break;
                    }
                }

                if (!isInGraph2)
                    difference.TryAddEdgeWithVertexes(edge1);
            }

            difference.SortVertexes();
            return difference;
        }

        public static DirectedGraph Difference(DirectedGraph graph1, DirectedGraph graph2)
        {
            DirectedGraph difference = new(new(), new());

            foreach (var vertex in graph1.Vertexes.Except(graph2.Vertexes))
                difference.TryAddVertex(vertex);

            foreach (var edge1 in graph1.Edges)
            {
                bool isInGraph2 = false;
                foreach (var edge2 in graph2.Edges)
                {
                    if (edge1.From == edge2.From && edge1.To == edge2.To)
                    {
                        isInGraph2 = true;
                        break;
                    }
                }

                if (!isInGraph2)
                    difference.TryAddEdgeWithVertexes(edge1);
            }

            difference.SortVertexes();
            return difference;
        }

        public static IndirectedGraph SymmetricDifference(IndirectedGraph graph1, IndirectedGraph graph2)
        {
            return Union(Difference(graph1, graph2), Difference(graph2, graph1));
        }

        public static DirectedGraph SymmetricDifference(DirectedGraph graph1, DirectedGraph graph2)
        {
            return Union(Difference(graph1, graph2), Difference(graph2, graph1));
        }
    }
}
