﻿/*
Dictionary<string, string> colori = new Dictionary<string, string>();
colori.Add("rosso", "#FF0000");
colori.Add("verde", "#00FF00");
colori.Add("blu", "#0000FF");


foreach (KeyValuePair<string, string> colore in colori)
{
    Console.WriteLine("Il colore " + colore.Key + " ha il codice " + colore.Value);
}
*/

// o usando var

var dizionario = new Dictionary<string, string> 
{
    {"rosso", "#FF0000"},
    {"verde", "#00FF00"},
    {"blu", "0000FF"}
};

foreach (var elemento in dizionario)
{
    Console.WriteLine($"Chiave: {elemento.Key}, Valore: {elemento.Value}");
}