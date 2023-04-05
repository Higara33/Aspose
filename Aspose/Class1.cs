namespace Aspose
{
    public class Class1
    {
        static void Main(string[] args)
        {
            string filePath = "test.mpp";
            string wbsCode = "1.2.3";

            byte[] bytesToFind = GetBytesFromWbsCode(wbsCode);
            List<long> positions = FindAllBytesInFile(filePath, bytesToFind);

            if (positions.Count > 0)
            {
                Console.WriteLine($"WBS Code {wbsCode} found at positions:");

                foreach (long position in positions)
                {
                    Console.WriteLine($"- {position:X}");
                }
            }
            else
            {
                Console.WriteLine($"WBS Code {wbsCode} not found in file");
            }
        }

        static byte[] GetBytesFromWbsCode(string wbsCode)
        {
            string[] codeParts = wbsCode.Split('.');
            byte[] bytes = new byte[codeParts.Length];

            for (int i = 0; i < codeParts.Length; i++)
            {
                bytes[i] = Convert.ToByte(codeParts[i]);
            }

            return bytes;
        }

        static List<long> FindAllBytesInFile(string filePath, byte[] bytes)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            long position = 0;
            int matches = 0;
            List<long> positions = new List<long>();

            while (true)
            {
                int nextByte = stream.ReadByte();

                if (nextByte == -1)
                {
                    break;
                }

                if (nextByte == bytes[position])
                {
                    position++;

                    if (position == bytes.Length)
                    {
                        position = 0;
                        matches++;
                        positions.Add(stream.Position - bytes.Length);
                    }
                }
                else
                {
                    position = 0;
                }
            }

            stream.Close();

            return positions;
        }
    }
}