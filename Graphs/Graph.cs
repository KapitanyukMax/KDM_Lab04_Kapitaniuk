using System;
using System.Collections.Generic;
using System.Linq;
using Matrixes;

namespace Graphs
{
    public abstract class Graph
    {
        public bool IsPseudoGraph { get; init; }

        public List<string> Vertexes { get; private set; }

        public List<Edge> Edges { get; private set; }

        protected List<List<int>> _adjacencyMatrix;

        public List<List<int>> AdjacencyMatrix => _adjacencyMatrix;

        protected List<List<int>>? _weightMatrix;

        public List<List<int>>? WeightMatrix
        {
            get => _weightMatrix;
            set
            {
                if (value != null)
                {
                    MatrixOperations.CloneMatrix(out _weightMatrix, value);
                    if (!IsValidWeightMatrix())
                        throw new ArgumentException("Invalid weight matrix");
                }
                else
                    _weightMatrix = value;
            }
        }

        public Graph(List<List<int>> adjacencyMatrix, List<string> vertexes, bool isPseudoGraph = false)
        {
            IsPseudoGraph = isPseudoGraph;

            MatrixOperations.CloneMatrix(
                out _adjacencyMatrix, adjacencyMatrix);
            Vertexes = vertexes;
            Edges = new();
            SetEdges();

            if (!IsValidAdjacencyMatrix())
                throw new ArgumentException("Invalid adjacency matrix");
        }

        protected abstract void SetEdges();

        public virtual bool IsValidAdjacencyMatrix()
        {
            if (Vertexes.Count != _adjacencyMatrix.Count)
                return false;

            for (int i = 0; i < _adjacencyMatrix.Count; i++)
            {
                if (_adjacencyMatrix[i].Count != _adjacencyMatrix.Count)
                    return false;

                for (int j = 0; j < _adjacencyMatrix[i].Count; j++)
                {
                    if (_adjacencyMatrix[i][j] != 0 && i == j
                        && !IsPseudoGraph)
                        return false;

                    if (_adjacencyMatrix[i][j] != 0
                        && _adjacencyMatrix[i][j] != 1)
                        return false;
                }
            }

            return true;
        }

        public virtual bool IsValidWeightMatrix()
        {
            if (_weightMatrix == null)
                return true;
            if (Vertexes.Count != _weightMatrix.Count)
                return false;

            for (int i = 0; i < _weightMatrix.Count; i++)
            {
                if (_weightMatrix[i].Count != _weightMatrix.Count)
                    return false;

                for (int j = 0; j < _weightMatrix[i].Count; j++)
                {
                    if (i != j)
                    {
                        if (_adjacencyMatrix[i][j] == 0
                            && _weightMatrix[i][j] != int.MaxValue)
                            return false;
                        if (_adjacencyMatrix[i][j] != 0
                            && _weightMatrix[i][j] == 0)
                            return false;
                    }
                    else if (_weightMatrix[i][j] != 0)
                        return false;
                }
            }

            return true;
        }

        public void Clear()
        {
            Vertexes.Clear();
            Edges.Clear();
            _adjacencyMatrix.Clear();
            _weightMatrix?.Clear();
        }

        private void SwapGraphIndexes(int index1, int index2)
        {
            (Vertexes[index1], Vertexes[index2]) = (Vertexes[index2], Vertexes[index1]);

            for (int i = 0; i < Vertexes.Count; i++)
                (_adjacencyMatrix[index1][i], _adjacencyMatrix[index2][i])
                    = (_adjacencyMatrix[index2][i], _adjacencyMatrix[index1][i]);

            for (int i = 0; i < Vertexes.Count; i++)
                (_adjacencyMatrix[i][index1], _adjacencyMatrix[i][index2])
                    = (_adjacencyMatrix[i][index2], _adjacencyMatrix[i][index1]);
        }

        public void SortVertexes()
        {
            for (int i = 0; i < Vertexes.Count; i++)
                for (int j = 0; j < Vertexes.Count - i - 1; j++)
                    if (string.Compare(Vertexes[j], Vertexes[j + 1]) > 0)
                        SwapGraphIndexes(j, j + 1);
        }

        public void TryAddVertex(string vertex)
        {
            if (!Vertexes.Contains(vertex))
            {
                Vertexes.Add(vertex);

                for (int i = 0; i < _adjacencyMatrix.Count; i++)
                    _adjacencyMatrix[i].Add(0);

                _adjacencyMatrix.Add(new());
                for (int j = 0; j < Vertexes.Count; j++)
                    _adjacencyMatrix[^1].Add(0);
            }
        }

        public virtual void TryAddEdgeWithVertexes(Edge edge)
        {
            TryAddVertex(edge.From);
            TryAddVertex(edge.To);

            int fromIndex = Vertexes.IndexOf(edge.From);
            int toIndex = Vertexes.IndexOf(edge.To);

            if (_adjacencyMatrix[fromIndex][toIndex] == 0)
            {
                Edges.Add(edge);
                _adjacencyMatrix[fromIndex][toIndex] = 1;
            }
        }

        private int FindClosestVertex(int currentVertex, List<int> route)
        {
            if (_weightMatrix == null)
                throw new InvalidOperationException("Cannot find closest vertex in a graph without a weight matrix");

            int closestVertex = -1;
            int closestDistance = int.MaxValue;

            for (int i = 0; i < _weightMatrix.Count; i++)
            {
                if (!route.Contains(i)
                    && closestDistance > _weightMatrix[currentVertex][i])
                {
                    closestVertex = i;
                    closestDistance = _weightMatrix[currentVertex][closestVertex];
                }
            }

            if (closestVertex == -1)
                throw new InvalidOperationException("All vertexes have already been labelled");

            return closestVertex;
        }

        public WeightedRoute ClosestNeighborAlgorithm(int startVertex)
        {
            if (_weightMatrix == null)
                throw new InvalidOperationException("Cannot apply closest neighbor algorith to a graph without a weight matrix");

            WeightedRoute route = new()
            {
                Route = new() { startVertex },
                Distance = 0
            };

            int currentVertex = startVertex;
            while (route.Route.Count < Vertexes.Count)
            {
                int closestVertex = FindClosestVertex(currentVertex, route.Route);
                route.Route.Add(closestVertex);
                route.Distance += _weightMatrix[currentVertex][closestVertex];
                currentVertex = closestVertex;
            }
            route.Route.Add(startVertex);
            route.Distance += _weightMatrix[currentVertex][startVertex];

            return route;
        }

        public List<List<int>> GetReachabilityMatrix_powers()
        {
            List<List<List<int>>> matrixPowers = new() { _adjacencyMatrix };

            for (int k = 1; k < Vertexes.Count; k++)
                matrixPowers.Add(MatrixOperations.LogicalProduct(matrixPowers[^1], _adjacencyMatrix));

            return MatrixOperations.LogicalSum(matrixPowers);
        }

        public List<List<int>> GetReachabilityMatrix_Warshall()
        {
            MatrixOperations.CloneMatrix(out List<List<int>> reachabilityMatrix, _adjacencyMatrix);
            MatrixOperations.CloneMatrix(out List<List<int>> tempMatrix, reachabilityMatrix);

            for (int k = 0; k < Vertexes.Count; k++)
            {
                for (int i = 0; i < Vertexes.Count; i++)
                {
                    for (int j = 0; j < Vertexes.Count; j++)
                    {
                        tempMatrix[i][j] = reachabilityMatrix[i][j]
                            | (reachabilityMatrix[i][k] & reachabilityMatrix[k][j]);

                        reachabilityMatrix.Clear();
                        MatrixOperations.CloneMatrix(out reachabilityMatrix, tempMatrix);
                    }
                }
            }

            return reachabilityMatrix;
        }

        public List<WeightedRoute> DijkstraAlgorithm(string startVertex)
        {
            if (WeightMatrix == null)
                throw new InvalidOperationException("Cannot apply Dijkstra algorithm to a graph without a weight matrix");

            if (!Vertexes.Contains(startVertex))
                throw new ArgumentOutOfRangeException(nameof(startVertex), "Invalid start vertex");

            List<int> labelledVertexIndices = new() { Vertexes.IndexOf(startVertex) };
            List<WeightedRoute> routes = new();
            for (int i = 0; i < Vertexes.Count; i++)
            {
                if (Vertexes[i] == startVertex)
                    routes.Add(new() { Route = new() { i }, Distance = 0 });
                else
                    routes.Add(new() { Route = new(), Distance = int.MaxValue });
            }

            while (labelledVertexIndices.Count < Vertexes.Count)
            {
                int closestVertex = -1;
                int closestDistance = int.MaxValue;
                for (int i = 0; i < Vertexes.Count; i++)
                {
                    if (!labelledVertexIndices.Contains(i))
                    {
                        long newRoute = routes[labelledVertexIndices[^1]].Distance + (long)WeightMatrix[labelledVertexIndices[^1]][i];
                        if (newRoute < routes[i].Distance)
                            routes[i] = new()
                            {
                                Route = new(routes[labelledVertexIndices[^1]].Route) { i },
                                Distance = routes[labelledVertexIndices[^1]].Distance + WeightMatrix[labelledVertexIndices[^1]][i]
                            };

                        if (closestDistance > routes[i].Distance)
                        {
                            closestDistance = routes[i].Distance;
                            closestVertex = i;
                        }
                    }
                }

                if (closestVertex == -1)
                    throw new InvalidOperationException("Cannot apply Dijkstra algorithm to the matrix, not all routes exist");

                labelledVertexIndices.Add(closestVertex);
            }

            return routes;
        }

        public bool IsConnected(string? currentVertex = null, List<string>? visitedVertexes = null)
        {
            if (Vertexes.Count == 0)
                return true;

            currentVertex ??= Vertexes[0];
            if (!Vertexes.Contains(currentVertex))
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(currentVertex),
                    message: "There is no such vertex in graph"
                );

            visitedVertexes ??= new();
            visitedVertexes.Add(currentVertex);

            foreach (var vertex in visitedVertexes)
                if (!Vertexes.Contains(vertex))
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(visitedVertexes),
                        message: "List contains invalid vertexes"
                    );

            if (visitedVertexes.Count == Vertexes.Count)
                return true;

            foreach (var edge in Edges.Where(e => e.From == currentVertex))
                if (!visitedVertexes.Contains(edge.To)
                    && IsConnected(edge.To, visitedVertexes))
                    return true;

            foreach (var edge in Edges.Where(e => e.To == currentVertex))
                if (!visitedVertexes.Contains(edge.From)
                    && IsConnected(edge.From, visitedVertexes))
                    return true;

            return false;
        }
    }
}
