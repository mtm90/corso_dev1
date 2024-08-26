Dictionary<string, Dictionary<string, List<int>>> grades = [];

int input;
do
{
    Console.Clear();
    Console.WriteLine("1. Add a new grade");
    Console.WriteLine("2. Add a new subject for a student");
    Console.WriteLine("3. Display all students and their grades");
    Console.WriteLine("4. Display average grades for each subject");
    Console.WriteLine("5. Quit");
    input = Convert.ToInt32(Console.ReadLine());

    switch (input)
    {
        case 1:
        Console.WriteLine("Enter the name of a student");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter the subject");
        string subject = Console.ReadLine()!;
        Console.WriteLine("Enter the grade");
        int grade = Convert.ToInt32(Console.ReadLine());

        if (!grades.ContainsKey(name))
        {
            grades[name] = new Dictionary<string, List<int>>();
        }

        if (!grades[name].ContainsKey(subject))
        {
            grades[name][subject] = new List<int>();
        }
        grades[name][subject].Add(grade);
        Console.WriteLine($"Added grade {grade} for {name} in {subject}.");
        break;
        case 2:
        Console.WriteLine("Enter the name of a student");
        string nameS = Console.ReadLine()!;
        Console.WriteLine("Enter the name of a new subject");
        string subjectS = Console.ReadLine()!;

        // Check if the student exists in the dictionary
        if (!grades.ContainsKey(nameS))
        {
            // If the student doesn't exist, notify the user
            Console.WriteLine($"Student {nameS} not found.");
        }
        else
        {
        // Check if the subject already exists for the student
        if (grades[nameS].ContainsKey(subjectS))
        {
            // If the subject already exists, notify the user
            Console.WriteLine($"Subject {subjectS} already exists for {nameS}.");
        }
        else
        {
            // If the subject doesn't exist, add it with an empty list of grades
            grades[nameS][subjectS] = new List<int>();

            // Prompt the user to enter the first grade for the new subject
            Console.WriteLine($"Enter the first grade for {subjectS}:");
            int gradeS = Convert.ToInt32(Console.ReadLine());

            // Add the first grade to the subject's grade list
            grades[nameS][subjectS].Add(gradeS);

            Console.WriteLine($"Subject {subjectS} added with the grade {gradeS} for {nameS}.");
        }
        }
        break;
        case 3:
        // Iterate over each student in the dictionary
        foreach (KeyValuePair<string, Dictionary<string, List<int>>> student in grades)
        {
            // Print the student's name
            Console.WriteLine($"Student: {student.Key}");

            // Iterate over each subject and grades for the current student
            foreach (KeyValuePair<string, List<int>> sub in student.Value)
            {
                // Print the subject name and the grades as a comma-separated list
                Console.WriteLine($"  Subject: {sub.Key}, Grades: {string.Join(", ", sub.Value)}");
            }
        }
        break;
        case 4:
        foreach (KeyValuePair<string, Dictionary<string, List<int>>> student in grades)
        {
            // Print the student's name
            Console.WriteLine($"Student: {student.Key}");
            
            // Iterate over each subject and its grades for the current student
            foreach (KeyValuePair<string, List<int>> sub in student.Value)
            {
                // Get the list of grades for the subject
                List<int> gradesList = sub.Value;
                
                // Calculate the average grade
                double averageGrade = gradesList.Average();
                
                // Print the subject and its average grade
                Console.WriteLine($"  Subject: {sub.Key}, Average Grade: {averageGrade:F2}");
            }
        }
        break;
        case 5:
        Console.WriteLine("Quitting");
        break;



       




    }
}
while (input != 5);