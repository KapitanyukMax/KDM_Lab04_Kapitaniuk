using BinarySequences;

namespace BoolFunctions
{
    public class BoolFunction
    {
        public List<string> Arguments { get; private init; }

        public List<int> TrueValues { get; private init; }

        public BoolFunction(List<string> arguments, List<int> trueValues)
        {
            Arguments = arguments;
            TrueValues = trueValues.Order().ToList();

            if (TrueValues.Distinct().Count() != TrueValues.Count)
                throw new ArgumentException(paramName: nameof(trueValues),
                    message: "True values list must be distinct");

            if (TrueValues[0] < 0
                || TrueValues[^1] > Math.Pow(2, Arguments.Count))
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(trueValues),
                    message:
                    "Some of the values are out of the definition set");
        }

        public bool GetValue(List<bool> parameters)
        {
            if (parameters.Count != Arguments.Count)
                throw new ArgumentException(paramName: nameof(parameters),
                    message: "Inappropriate parameters count passed");

            string binary = string.Empty;
            for (int i = 0; i < parameters.Count; i++)
                binary += parameters[i] ? "1" : "0";
            int num = Convert.ToInt32(binary, 2);

            return TrueValues.Contains(num);
        }

        public string GetElementaryConjunction(string binary)
        {
            if (binary.Length != Arguments.Count)
                throw new ArgumentException(paramName: nameof(binary),
                    message: "Binary representation must have exactly 1 bit for each argument");

            List<string> elementaryConjunctionList = new();
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                    elementaryConjunctionList.Add(Arguments[i]);
                else if (binary[i] == '0')
                    elementaryConjunctionList.Add("!" + Arguments[i]);
            }

            return string.Join("/\\", elementaryConjunctionList);
        }

        public string GetPerfectDisjunctiveNormalForm()
        {
            List<string> implicantList = new();

            int trueValuesIndex = 0;
            for (int i = 0; i < Math.Pow(2, Arguments.Count); i++)
            {
                if (i == TrueValues[trueValuesIndex])
                {
                    string binary =
                        BinaryOperations.ToBinary(i, Arguments.Count);
                    implicantList.Add(GetElementaryConjunction(binary));
                    trueValuesIndex++;
                }
            }

            return string.Join(" \\/ ", implicantList);
        }

        private bool McCluskeyIteration(
            ref Dictionary<int, List<string>> classes)
        {
            List<string> replaced = new();
            foreach (var classPair in classes)
            {
                if (!classes.TryGetValue(classPair.Key + 1,
                    out var nextClassList))
                {
                    classes[classPair.Key] = classPair.Value.Select(i => i)
                                                               .ToList();
                    continue;
                }
                
                if (!classes.TryGetValue(classPair.Key,
                    out var currentClassList))
                    currentClassList = new();
                for (int i = 0; i < currentClassList.Count; i++)
                {
                    for (int j = 0; j < nextClassList.Count; j++)
                    {
                        int diffIndex = BinaryOperations.FindDifference(
                            currentClassList[i], nextClassList[j]);

                        if (diffIndex != -1)
                        {
                            string newBin = new(currentClassList[i]
                                .Select((ch, k) => k == diffIndex ? '-' : ch)
                                .ToArray());
                            currentClassList.Add(newBin);

                            if (!replaced.Contains(currentClassList[i]))
                                replaced.Add(currentClassList[i]);
                            if (!replaced.Contains(nextClassList[j]))
                                replaced.Add(nextClassList[j]);
                        }
                    }
                }
            }

            foreach (var classList in classes.Values)
                classList.RemoveAll(replaced.Contains);

            return replaced.Count > 0;
        }

        public string McCluskeyAlgorithm()
        {
            Dictionary<int, List<string>> classes = new();
            foreach (var i in TrueValues)
            {
                string binary = BinaryOperations.ToBinary(i, Arguments.Count);
                int classIndex = binary.Count(ch => ch == '1');

                if (classes.TryGetValue(classIndex, out var classList))
                    classList.Add(binary);
                else
                    classes.Add(classIndex, new() { binary });
            }

            while (McCluskeyIteration(ref classes))
                continue;

            List<string> implicantList = new();
            foreach (var classList in classes.Values)
            {
                foreach (var binary in classList)
                {
                    var implicant = GetElementaryConjunction(binary);
                    if (!implicantList.Contains(implicant))
                        implicantList.Add(implicant);
                }
            }

            return string.Join(" \\/ ", implicantList);
        }
    }
}
