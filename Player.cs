using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Credit_System_Game
{
    class Player
    {
        public int Score;
        public string Name;

        public Player(string N = "null")
        {
            Name = N;
            Score = 0;
        }
    }
}
