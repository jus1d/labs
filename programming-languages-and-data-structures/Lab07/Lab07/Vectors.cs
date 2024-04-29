using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab07
{

    public class Vectors
    {
        public static IVectorable Sum(IVectorable a, IVectorable b)
        {
            if (a.Length != b.Length)
            {
                throw new Exception("Vectors norms are not equals");
            }

            IVectorable vec = new ArrayVector(a.Length);
            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = a[i] + b[i];
            }

            return vec;
        }

        public static double ScalarMultiply(IVectorable a, IVectorable b)
        {
            if (a.Length != b.Length)
            {
                throw new Exception("Vectors norms are not equal");
            }

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result += a[i] * b[i];
            }

            return result;
        }

        public static IVectorable MultiplyByNumber(IVectorable vector, int number)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] *= number;
            }

            return vector;
        }

        public static double GetNormSt(IVectorable vector)
        {
            return vector.GetNorm();
        }

        public static void WriteVectors(FileStream fs, List<IVectorable> vectors)
        {
            fs.Seek(0, SeekOrigin.End);
            for (int i = 0; i < vectors.Count; i++)
            {
                string line = vectors[i].ToString();
                if (i != vectors.Count - 1) line += '\n';

                byte[] buffer = Encoding.ASCII.GetBytes(line);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        public static List<IVectorable> ReadVectors(FileStream fs)
        {
            List<IVectorable> vectors = new List<IVectorable>();
            string content = "";
            int bytesRead;

            do
            {
                byte[] buffer = new byte[1024];
                bytesRead = fs.Read(buffer, 0, buffer.Length);

                content += Encoding.ASCII.GetString(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            string[] lines = content.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(' ');
                IVectorable vector = new ArrayVector(Convert.ToInt32(data[0]));
                for (int j = 0; j < vector.Length; j++)
                {
                    vector[j] = Convert.ToInt32(data[j + 1]);
                }

                vectors.Add(vector);
            }

            return vectors;
        }

        public static void WriteVector(TextWriter w, IVectorable vec)
        {
            w.WriteLine(vec.ToString());
        }

        public static IVectorable ReadVector(TextReader r)
        {
            string[] coordinates = r.ReadLine().Split(' ');
            int length = int.Parse(coordinates[0]);

            ArrayVector vector = new ArrayVector(length);
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = int.Parse(coordinates[i + 1]);
            }

            return vector;
        }
    }
}