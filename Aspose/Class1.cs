namespace Aspose
{
    public class Class1
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: WbsHexDump.exe <mpp file> <wbs code>");
                return;
            }

            string mppFile = args[0];
            string wbsCode = args[1];

            byte[] block = FindDataBlockByWbsCode(mppFile, wbsCode);

            if (block == null)
            {
                Console.WriteLine($"Data block for WBS code {wbsCode} not found in {mppFile}");
                return;
            }

            Console.WriteLine($"Data block for WBS code {wbsCode}:");

            for (int i = 0; i < block.Length; i++)
            {
                Console.Write($"{block[i]:X2} ");
                if ((i + 1) % 16 == 0) Console.WriteLine();
            }
        }

        static byte[] FindDataBlockByWbsCode(string mppFile, string wbsCode)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(mppFile, FileMode.Open, FileAccess.Read);

                // Look for the WBS code in the file
                byte[] wbsBytes = GetBytesFromWbsCode(wbsCode);
                long position = FindBytesInStream(stream, wbsBytes);

                if (position == -1)
                {
                    return null;
                }

                // Skip the WBS code and the 4-byte block size
                stream.Seek(wbsBytes.Length + 4, SeekOrigin.Current);

                // Read the block size
                byte[] sizeBytes = new byte[4];
                stream.Read(sizeBytes, 0, 4);
                int blockSize = BitConverter.ToInt32(sizeBytes, 0);

                // Read the data block
                byte[] block = new byte[blockSize];
                stream.Read(block, 0, blockSize);

                return block;
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        static byte[] GetBytesFromWbsCode(string wbsCode)
        {
            string[] codeParts = wbsCode.Split('.');
            byte[] bytes = new byte[codeParts.Length];

            for (int i = 0; i < codeParts.Length; i++)
            {
                bytes[i] = Convert.ToByte(codeParts[i], 16);
            }

            return bytes;
        }

        static long FindBytesInStream(Stream stream, byte[] bytes)
        {
            int b;
            long i = 0;

            while ((b = stream.ReadByte()) != -1)
            {
                if (b == bytes[i])
                {
                    i++;
                    if (i == bytes.Length) return stream.Position - i;
                }
                else
                {
                    i = 0;
                }
            }

            return -1;
        }
    }
}