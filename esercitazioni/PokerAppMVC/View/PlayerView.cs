using PokerAppMVC.Models;
using System;

namespace PokerAppMVC.Views
{
    public class PlayerView
    {
        public void DisplayPlayerInfo(Player player)
        {
            Console.WriteLine($"Player ID: {player.PlayerId}, Name: {player.PlayerName}");
        }
    }
}
