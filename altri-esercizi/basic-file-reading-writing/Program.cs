string path = "numbers.txt";
string[] lines = File.ReadAllLines(path);
foreach (string line in lines)
{
    Console.WriteLine(line);
}
List<int> numbers = new List<int>();
foreach (string line in lines)
{
    int number = int.Parse(line);
    numbers.Add(number);
}
List<int> squaredNumbers = new List<int>();
foreach (int number in numbers)
{
    squaredNumbers.Add(number * number);
}
List<string> squaredNumbersStrings = new List<string>();
foreach (int squaredNumber in squaredNumbers)
{
    squaredNumbersStrings.Add(squaredNumber.ToString());
}
string outputPath = "squared_numbers.txt";
File.WriteAllLines(outputPath, squaredNumbersStrings);
