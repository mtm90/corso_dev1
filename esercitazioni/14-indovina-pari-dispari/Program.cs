/*
Console.Clear();
Random random = new Random();
int number = random.Next(11);
Console.WriteLine("I chose a number. Is the number even or odd?");
string answer = Console.ReadLine().ToLower();
bool isEven = number % 2 == 0;

if ((isEven && answer == "even") || (!isEven && answer == "odd"))
{
    Console.WriteLine("You won");
}
else 
{
    Console.WriteLine("You lost");
}

Console.WriteLine($"The number was {number}");

*/

Console.Clear();

Random random = new Random();
int computerNumber = random.Next(1,5);

Console.WriteLine("Choose a number between 1 and 5");
int userNumber = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("I also chose a number... Do you think the sum of our numbers is even or odd?");
string answer = Console.ReadLine();

bool sumIsEven = (computerNumber + userNumber) % 2 == 0;

if ((sumIsEven && answer == "even") || (!sumIsEven && answer == "odd"))
{
    Console.WriteLine($"You won! I chose {computerNumber} and the sum is {computerNumber + userNumber} ");
}
else
{
    Console.WriteLine($"You lost! I chose {computerNumber} and the sum is {computerNumber + userNumber}");
}






