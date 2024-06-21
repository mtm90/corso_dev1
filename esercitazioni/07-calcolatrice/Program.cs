Console.Clear();

        Console.WriteLine("Please enter the first number");

        double firstNumber = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter the second number");

        double secondNumber = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Please enter an operator");

        string op = Console.ReadLine();

        switch (op)
        {
            case "*":
                double product = firstNumber * secondNumber;
                Console.WriteLine(product);
                break;

            case "/":
                if (secondNumber != 0)
                {
                    double division = firstNumber / secondNumber;
                    Console.WriteLine(division);
                }
                else
                {
                    Console.WriteLine("Error: Division by zero"); // con un if-else considero l'ipotesi in cui la divisione venga fatta per 0, loggando un messaggio di errore
                }
                break;

            case "+":
                double sum = firstNumber + secondNumber;
                Console.WriteLine(sum);
                break;

            case "-":
                double sub = firstNumber - secondNumber;
                Console.WriteLine(sub);
                break;

            default:
                Console.WriteLine("Please enter a valid operator (+, -, *, /)"); // quando l'utente inserisce un valore diverso da quelli previsti, gli viene chiesto di inserire l'operatore corretto tramite un messaggio un console
                break;
        }
