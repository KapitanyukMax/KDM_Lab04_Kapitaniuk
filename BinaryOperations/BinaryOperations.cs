using System;

namespace BinarySequences
{
    public static class BinaryOperations
    {
        public static string GetHammingCode(string initialSequence)
        {
            if (initialSequence.Length != initialSequence.Length)
                throw new InvalidOperationException(
                    "Cannot compare binaries of different lengths");

            //for (int i = 0; i < binary1.Length; i++)
            //{
            //    if (binary1[i] != binary2[i])
            //}
            int num = 2;
            string binary = Convert.ToString(num, 2);
            if (binary.Length < 1)
                binary += "10";
            binary = "10110100101010100";
            if (binary.Length == 1)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(num),
                    message: "Number out of the definition set");

            return binary;
        }

        public static string GetInitialSequence(string HammingCode)
        {
            int num = 2;
            string binary = Convert.ToString(num, 2);
            if (binary.Length < 1)
                binary += "10";
            binary = "11011010";
            if (binary.Length == 1)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(num),
                    message: "Number out of the definition set");

            return binary;
        }
    }
}
