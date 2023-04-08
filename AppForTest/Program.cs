using System.Text;

string mppFile = "D:\\Aspose.Tasks-Test-Task\\Aspose.Tasks-Test-Task\\Wbs11.mpp";

var wbsCode = Input();

byte[] bytes = File.ReadAllBytes(mppFile);

string hexString = BitConverter.ToString(bytes).Replace("-", "");

var wbsBytes = Encoding.Unicode.GetBytes(wbsCode.Substring(0, wbsCode.IndexOf('.')));
var wbsBytesToString = BitConverter.ToString(wbsBytes).Replace("-", "");

string text = hexString;
string pattern = wbsBytesToString;
bool checkFind = false;

int[] zArray = CalculateZArray(pattern + "$" + text);
for (int i = 0; i < zArray.Length; i++)
{
    if (zArray[i] == pattern.Length)
    {
        Console.WriteLine("Pattern found at index " + (i - pattern.Length - 1));
        checkFind = true;
    }
}

if(!checkFind)
{
    Console.WriteLine(wbsBytesToString + " not found");
}


static string Input()
{
    Console.WriteLine("Введите  Prefix:");
    string projectCodePrefix = Console.ReadLine();

    Console.WriteLine("Введите количество масок кода:");
    int numberOfMasks = int.Parse(Console.ReadLine());

    List<string> masks = new List<string>();
    for (int i = 0; i < numberOfMasks; i++)
    {
        Console.WriteLine($"Введите маску кода {i + 1} в формате Sequence Length Separator:");
        string mask = Console.ReadLine();
        masks.Add(mask);
    }

    Console.WriteLine("Введите флаг Generate WBS code for new task (True/False):");
    bool generateWbsCode = bool.Parse(Console.ReadLine());

    Console.WriteLine("Введите флаг Verify uniqueness of new WBS codes (True/False):");
    bool verifyUniqueness = bool.Parse(Console.ReadLine());

    // собираем входной параметр wbsCode из полученных значений
    string wbsCode = $"{projectCodePrefix}.";
    foreach (string mask in masks)
    {
        string[] maskParts = mask.Split(' ');
        string sequence = maskParts[0];
        string length = maskParts[1];
        string separator = maskParts[2];

        if (sequence == "Numbers")
        {
            wbsCode += new string('1', int.Parse(length));
        }
        else if (sequence == "ULetters")
        {
            wbsCode += new string('A', int.Parse(length));
        }
        else if (sequence == "LLetters")
        {
            wbsCode += new string('a', int.Parse(length));
        }
        else if (sequence == "Characters")
        {
            wbsCode += new string('*', int.Parse(length));
        }

        if (separator != "")
        {
            wbsCode += separator;
        }
    }

    wbsCode = wbsCode.TrimEnd('.');

    return wbsCode;
}
static int[] CalculateZArray(string str)
{
    int n = str.Length;
    int[] zArray = new int[n];
    int left = 0;
    int right = 0;

    for (int i = 1; i < n; i++)
    {
        if (i > right)
        {
            left = right = i;
            while (right < n && str[right - left] == str[right])
            {
                right++;
            }
            zArray[i] = right - left;
            right--;
        }
        else
        {
            int k = i - left;
            if (zArray[k] < right - i + 1)
            {
                zArray[i] = zArray[k];
            }
            else
            {
                left = i;
                while (right < n && str[right - left] == str[right])
                {
                    right++;
                }
                zArray[i] = right - left;
                right--;
            }
        }
    }
    return zArray;
}

