using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Credit_System_Game
{
    class Question
    {
        public string Title;
        public Choice[] AmountOfChoice;

        public Question(string Text, Choice[] choices)
        {
            Title           = Text;
            AmountOfChoice  = choices;
        }
    }
}
