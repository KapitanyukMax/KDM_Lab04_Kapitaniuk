using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace BinarySequences
{
    public static class BinaryOperations
    {
        public static string ToBinary(int num, int length)
        {
            string binary = Convert.ToString(num, 2);
            if (binary.Length < length)
                binary = new string('0', length - binary.Length)
                    + binary;
            else if (binary.Length > length)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(num),
                    message: "Number is out of the specified range");

            return binary;
        }
        public static int FindDifference(string binary1, string binary2)
        {
            if (binary1.Length != binary2.Length)
                throw new InvalidOperationException(
                    "Cannot compare binaries of different lengths");

            int differenceIndex = -1;
            for (int i = 0; i < binary1.Length; i++)
            {
                if (binary1[i] == '-' ^ binary2[i] == '-')
                    return -1;

                if (binary1[i] != binary2[i])
                {
                    if (differenceIndex == -1)
                        differenceIndex = i;
                    else
                        return -1;
                }
            }

            return differenceIndex;
        }

        public static string GetHammingCode(string initialSequence)
        {
            if (initialSequence.Any(ch => ch != '0' && ch != '1'))
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(initialSequence),
                    message: "The sequence must be binary");

            StringBuilder hammingCode = new(initialSequence);

            int index = 1;
            while (index <= hammingCode.Length)
            {
                hammingCode = hammingCode.Insert(index - 1, 'p');
                index *= 2;
            }

            index = 1;
            while (index < hammingCode.Length)
            {
                int i = index - 1;
                bool xor = false, isEndReached = false;
                while (!isEndReached)
                {
                    for (int j = i; j < i + index; j++)
                    {
                        if (j >= hammingCode.Length)
                        {
                            isEndReached = true;
                            break;
                        }

                        xor ^= hammingCode[j] == '1';
                    }

                    i += 2 * index;
                }

                hammingCode[index - 1] = xor ? '1' : '0';
                index *= 2;
            }

            return hammingCode.ToString();
        }

        public static string GetInitialSequence(string hammingCode)
        {
            string syndrome = string.Empty;
            int index = 1;
            while (index <= hammingCode.Length)
            {
                int i = index - 1;
                bool xor = false, isEndReached = false;
                syndrome += '0';
                while (!isEndReached)
                {
                    for (int j = i; j < i + index; j++)
                    {
                        if (j >= hammingCode.Length)
                        {
                            isEndReached = true;
                            break;
                        }

                        xor ^= hammingCode[j] == '1';
                    }

                    i += 2 * index;
                }

                syndrome += xor ? '1' : '0';
                index *= 2;
            }

            int errorIndex = Convert.ToInt32(syndrome, 2);
            if (syndrome.Count(ch => ch == '1') == 1
                || errorIndex > hammingCode.Length)
            {
                return "2+ errors. Cannot find initial sequence";
            }

            StringBuilder initialSequence = new();
            if (errorIndex == 0)
                initialSequence.Append("0 errors. Initial sequence: ");
            else
                initialSequence.Append(
                    $"1 error at {errorIndex} bit. Initial sequence: ");

            int nextPow = 1;
            for (int i = 0; i < hammingCode.Length; i++)
                if (i == nextPow)
                    nextPow *= 2;
                else
                    initialSequence.Append(hammingCode[i]);

            if (initialSequence.ToString().Count(ch => ch == '1') != 5)
                return "2+ errors. Cannot find initial sequence";

            if (errorIndex > 0)
                initialSequence[errorIndex - 1] =
                    initialSequence[errorIndex - 1] == '0' ? '1' : '0';

            return hammingCode;
        }
    }
}
