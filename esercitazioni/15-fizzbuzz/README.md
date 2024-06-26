#  <font color="yellow">FizzBuzz </font>

### Questa è una tipica esercitazione che evidenzia l'importanza dell'ordine delle condizioni negli statement

### Questo esercizio chiede di restituire un messaggio diverso a seconda che il numero selezionato dal computer sia divisibile per 3, 5 o 15.

##### Il codice seguente mostra un ciclo for che va da 1 a 100

```c#
for (int i = 1; i < 101; i++)
```

##### Il punto importante riguarda la posizione della condizione secondo cui il numero sia divisibile per 15. Va necessariamente messa come <font color="red">  _prima_ </font> condizione del ciclo if altrimenti non viene considerata.


```c#
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine($"numero: {i} FizzBuzz");
    }
    else if (i % 3 == 0)
    {
        Console.WriteLine($"numero: {i} Fizz");
    }
    else if (i % 5 == 0)
    {
        Console.WriteLine($"numero: {i} Buzz");
    }
    else
    {
        Console.WriteLine($"numero: {i}");
    }
}
```

```html
<div style="color:green">
    Markdown inline css styles
</div>
```