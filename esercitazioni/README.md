# Esercitazioni

## 01-Tipi di dati

<details>
<summary>Visualizza il codice</summary>

```csharp
// esempio di dichiarazioni di variabili

int numero;     // dichiaro una variabile di tipo intero

string nome;    // dichiaro una variabile di tipo stringa

bool flag;      // dichiaro una variabile di tipo booleano

                // esempio di inizializzazione di variabili

numero = 10; 

nome = "mario";

flag = false;

                // esempio di dichiarazione ed inizializzazione di variabili

int età = 20;

string cognome = "Rossi";

bool isStudent = false;

                // esempio di dichiarazione e inizializzazione di una costante

const double PI = 3.14159;

                // esempio di dichiarazione e inizializzazione di una variabile con var

var altezza = 1.80;     

                // lo svantaggio è che non è possibile dichiarare una variabile var senza inizializzarla

                // REGOLA IMPORTANTE: una variabile deve essere inizializzata prima di essere utilizzata

                // REGOLA IMPORTANTE: i nomi delle variabili devono essere significativi e autoesplicativi

Console.WriteLine($"Ciao {nome} {cognome}! Come va?");



```
</details>

## 02-Operatori

<details>
<summary>Visualizza il codice</summary>

```csharp
// gli operatori sono utilizzati per eseguire operazioni su uno o più operandi


// operatori aritmetici


int a = 10;

int b = 20;

int somma = a + b;

int differenza = a - b;

int prodotto = a * b ;

int divisione = a / b;

int modulo = a % b;


// operatori di confronto 


int c = 30;

int c == a;             // false

int c != a;             // true

int c > a;              // true

int c < a;              // false

int c >= a;             // true

int c <= a;             // false


// operatori logici


bool x = true;

bool y = false;

// and
bool z = x && y;        // false

// or
bool w = x || y;        // true

// not
bool v = !x;            // false


/* operatori di assegnazione 

assegnazione (=)
assegnazione con addizione(+=)
assegnazione con sottrazione(-=)

*/


// operatori di concatenazione


string f = "Hello";

string g = "World";

string h = f + " " + g;  // h = "Hello World"


// esempio di concatenazione con interpolazione


string i = $"{f} {g}";




```
</details>

## 03-tipi di dati e operatori: ES

<details>
<summary>Visualizza il codice</summary>

```csharp
Console.Clear();

int numero;

numero = 10;

Console.WriteLine(numero);

int età = 20;

Console.WriteLine("L'età è: " + età);

Console.WriteLine($"l'età è: {età}");


// stampare due variabili una stringa e una int

string nome = "Mario";

Console.WriteLine($"Il mio nome è {nome} e la mia età è {età}");
```
</details>

## 04-Assignment

<details>
<summary>Visualizza il codice</summary>

```csharp
Console.Clear();
string name;
Console.WriteLine("Please insert your name");
name = Console.ReadLine();

string id = name + "-000001";

Console.WriteLine($"Hi {name}! Your id is {id}");


```
</details>

## 05-Le Condizioni

<details>
<summary>Visualizza il codice</summary>

```csharp
Console.Clear();

int numero = 11;

if (numero == 11)
{
    Console.WriteLine("Il mio numero è 11");
}
else if (numero > 11)
{
    Console.WriteLine("Il mio numero è maggiore di 11");
}
else 
{
    Console.WriteLine("Il mio numero è minore di 11");
}

```
</details>

## 06-Assignment

<details>
<summary>Visualizza il codice</summary>

```csharp
Console.Clear();

Console.WriteLine("Please insert a number from 1 to 7, it will return the corresponding day of the week");

int giorno = Convert.ToInt32(Console.ReadLine());

switch (giorno)
{
    case 1:
        Console.WriteLine("Lunedì");
        break;

    case 2:
        Console.WriteLine("Martedì");
        break;

    case 3:
        Console.WriteLine("Mercoledì");
        break;

    case 4:
        Console.WriteLine("Giovedì");
        break;

    case 5:
        Console.WriteLine("Venerdì");
        break;

    case 6:
        Console.WriteLine("Sabato");
        break;

    case 7:
        Console.WriteLine("Domenica");
        break;

    default:
        Console.WriteLine("Please enter a valid number");
        break;
}
```
</details>

## 07-Calcolatrice

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 08-Cicli

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 09-Indovina numero

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 12-Metodo random

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 13-Markdown

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 14-Indovina pari-dispari

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 15-FizzBuzz

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 16-Array string

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 17-Array int

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 18-Array length

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 19-List string

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 20-List int

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 21-List count

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 22-assignment-strutture dati

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 23-Dizionario dei colori

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 24-Lista della spesa

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 25-presenze

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 26-Registro Voti

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>

## 27-var

<details>
<summary>Visualizza il codice</summary>

```csharp

```
</details>
