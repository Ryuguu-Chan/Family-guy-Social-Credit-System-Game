using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Credit_System_Game
{
    class Choice
    {
        public string Text;
        public int ID;

        public string PrintChoice()
        {
            return ID + ". " + Text;
        }

        public bool IsItCorrect { get { return Correct; } }
        public int GetPoints    { get { return Points;  } }

        private bool Correct;
        private int Points;

        public Choice(string T, int Identification, int Score, bool IsItCorrectAnswer = false)
        {
            Text    = T;
            ID      = Identification;
            Correct = IsItCorrectAnswer;
            Points  = Score;
        }
    }
}
