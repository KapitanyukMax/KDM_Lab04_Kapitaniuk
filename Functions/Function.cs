using Graphs;
using Relations;

namespace Functions
{
    public class Function : Relation
    {
        public Dictionary<string, string> ValuesDictionary
        { get; private init; } = new();

        public Function(List<List<int>> matrix,
            List<string> set) : base(matrix, set)
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[i][j] != 0)
                        if (ValuesDictionary.ContainsKey(Set[i]))
                            throw new ArgumentException(
                                paramName: nameof(Matrix),
                                message: "Function must have exactly one value for each argument");
                        else
                            ValuesDictionary[Set[i]] = Set[j];
        }

        public Function(List<Edge> pairList, List<string> set)
            : base(pairList, set)
        {
            for (int i = 0; i < Set.Count; i++)
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[i][j] != 0)
                        if (ValuesDictionary.ContainsKey(Set[i]))
                            throw new ArgumentException(
                                paramName: nameof(Matrix),
                                message: "Function must have exactly one value for each argument");
                        else
                            ValuesDictionary[Set[i]] = Set[j];
        }

        public bool IsInjective()
        {
            for (int i = 0; i < Set.Count; i++)
            {
                int valuesCount = 0;
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[j][i] != 0)
                        valuesCount++;

                if (valuesCount > 1)
                    return false;
            }

            return true;
        }

        public bool IsSurjective()
        {
            for (int i = 0; i < Set.Count; i++)
            {
                int valuesCount = 0;
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[j][i] != 0)
                        valuesCount++;

                if (valuesCount == 0)
                    return false;
            }

            return true;
        }

        public bool IsBijective() => IsInjective() && IsSurjective();

        public Function MakeInjective()
        {
            if (IsInjective())
                return this;

            List<string> unusedValues = Set.Select(i => i).ToList();
            List<string> repeatitiveArguments = new();
            foreach (var argument in Set)
            {
                if (unusedValues.Contains(ValuesDictionary[argument]))
                    unusedValues.Remove(ValuesDictionary[argument]);
                else if (!repeatitiveArguments.Contains(argument))
                    repeatitiveArguments.Add(argument);
            }

            List<Edge> injectivePairList = new();
            foreach (var argument in Set)
            {
                if (!repeatitiveArguments.Contains(argument))
                    injectivePairList.Add(
                        new(argument, ValuesDictionary[argument]));
                else
                {
                    injectivePairList.Add(
                        new(argument, unusedValues[0]));
                    unusedValues.RemoveAt(0);
                }
            }

            return new(injectivePairList, Set);
        }

        public List<string> GetValuesSet()
        {
            List<string> valuesSet = new();
            for (int i = 0; i < Set.Count; i++)
            {
                int valuesCount = 0;
                for (int j = 0; j < Set.Count; j++)
                    if (Matrix[j][i] != 0)
                        valuesCount++;

                if (valuesCount > 0)
                    valuesSet.Add(Set[i]);
            }

            return valuesSet;
        }
    }
}
