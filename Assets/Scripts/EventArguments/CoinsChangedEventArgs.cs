using System;

namespace EventArguments
{
    public class CoinsChangedEventArgs : EventArgs
    {
        public int Coins { get; }

        public CoinsChangedEventArgs(int coins)
        {
            Coins = coins;
        }
    }
}