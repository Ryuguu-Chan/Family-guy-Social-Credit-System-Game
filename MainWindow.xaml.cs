using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers; // for the Timer class
using System.Diagnostics; // for the Process class

namespace Social_Credit_System_Game
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        Timer mainTimer;
        int GamePhase = 0;

        int INT1 = 0;
        int INT2 = 0;

        bool F1 = true;

        TextBlock TextBlock03, TextBlock04, TextBlock05;

        Player You; // <3

        Image IMG02;

        List<Question> Quests;

        int CalculatePerfectScore(List<Question> QQ)
        {
            int R = 0;

            foreach(Question Q_Q in QQ)
                foreach (Choice C in Q_Q.AmountOfChoice)
                    if (C.IsItCorrect == true)
                        R += C.GetPoints;

            return R;
        }

        int[] ScoreRank =
        {
            -3000,  // EXECUTED
            -1000,  // OUTCASTED
            -300,   // BAD
            0,      // NOT BAD
            1000,   // GOOD
            3194,   // DEV
            3199,   // CHINESE KING (placeholder that would be changed when the questions have been successfully loaded)

            // for compensation
            int.MaxValue
        };

        Dictionary<int, string> RankingText;

        double[][] AnswerArea =
        {
            // coordinates for the X/V check

            //              x    y
            new double[] { 420, 160 }, // TextBlock01
            new double[] { 720, 160 }, // TextBlock02
            new double[] { 420, 260 }, // TextBlock03
            new double[] { 720, 260 }, // TextBlock04
        };

        // QuestionPics[x][0] = Background Image
        // QuestionPics[x][1] = Character Image
        string[][] QuestionPics =
        {
            //              The Background          The character
            new string[] { "GF/IMG/PEKIN.jpg"   , "GF/IMG/PETER.png"    },
            new string[] { "GF/IMG/FORB.jpg"    , "GF/IMG/QUA.png"      },
            new string[] { "GF/IMG/BG2.png"     , "GF/IMG/BAB.png"      },
            new string[] { "GF/IMG/CHINA.jpg"   , "GF/IMG/WOM.png"      },
            new string[] { "GF/IMG/PEKIN.jpg"   , "GF/IMG/DIS.png"      },
            new string[] { "GF/IMG/FORB.jpg"    , "GF/IMG/MORT.png"     },
            new string[] { "GF/IMG/BG2.png"     , "GF/IMG/MAN.png"      },
            new string[] { "GF/IMG/CHINA.jpg"   , "GF/IMG/PETER.png"    },
            new string[] { "GF/IMG/PEKIN.jpg"   , "GF/IMG/QUA.png"      },
            new string[] { "GF/IMG/FORB.jpg"    , "GF/IMG/BAB.png"      },
            new string[] { "GF/IMG/BG2.png"     , "GF/IMG/WOM.png"      },
            new string[] { "GF/IMG/CHINA.jpg"   , "GF/IMG/DIS.png"      },

            // for compensation sake
            new string[] { "", "" }
        };

        private int ArroundScore(int P)
        {
            try
            {
                if (P <= ScoreRank[6])
                {
                    for (int i = 0; i < (ScoreRank.Length); ++i)
                    {
                        if (P >= ScoreRank[i] && P < ScoreRank[i + 1])
                        {
                            return ScoreRank[i];
                        }
                    }
                }
                else
                {
                    return ScoreRank[6];
                }
            }
            catch (IndexOutOfRangeException)
            {
                return ScoreRank.Length - 2;
            }
            return 0;
        }

        public MainWindow()
        {
            InitializeComponent();

            You = new Player();

            Quests = new List<Question>();

            mainTimer = new Timer(1);
            mainTimer.Interval = 500;
            mainTimer.Elapsed += MainTimer_Elapsed;
            mainTimer.Enabled = true;

            IMG02 = new Image();

            // loading all the questions
            Quests.Add(new Question("How many children do you\nhave in your household?", new Choice[] {
                new Choice("one boy"    , 1, 15),
                new Choice("one girl"   , 2, 5),
                new Choice("none"       , 3, 20, true),
                new Choice("multiple"   , 4, 20)
            }));

            Quests.Add(new Question("Which of the following \npolitical parties are illegal?", new Choice[] {
                new Choice("The CCP",           1, 20),
                new Choice("The CZGP",          2, 25),
                new Choice("The DPC",           3, 18, true),
                new Choice("All of the above",  4, 25)
            }));

            Quests.Add(new Question("Do you have consent to\nhave your town used as a nuclear testing site?", new Choice[] {
                new Choice("Yes", 1, 1000, true ),
                new Choice("No",  2, 1000       )
            }));

            Quests.Add(new Question("Who's our enemy?", new Choice[] {
                new Choice("Serpentza", 1, 500, true),
                new Choice("Xi jiping", 2, 999),
                new Choice("Laowhy86", 3, 500),
                new Choice("Trump", 4, 200)
            }));

            Quests.Add(new Question("Do you like Taiwan?", new Choice[] {
                new Choice("Yes", 1, 999),
                new Choice("No", 2, 999, true)
            }));

            Quests.Add(new Question("Who's our best friend?", new Choice[] {
                new Choice("DPRK", 1, 100, true),
                new Choice("JP", 2, 999),
                new Choice("CCCP", 3, 100),
            }));

            Quests.Add(new Question("How many questions was asked before this one?", new Choice[] {
                new Choice("0", 1, 888),
                new Choice("8", 2, 850),
                new Choice("6", 3, 555, true),
                new Choice("5", 4, 500)
            }));

            Quests.Add(new Question("Who's our supreme leader?", new Choice[] {
                new Choice("The Wock", 1, 500),
                new Choice("John Xina", 2, 200),
                new Choice("Xi Jiping", 3, 800, true),
                new Choice("Wu Mao", 4, 300)
            }));

            Quests.Add(new Question("When was China founded?", new Choice[] {
                new Choice("2070 BCE", 1, 8000),
                new Choice("221 BCE", 2, 5000),
                new Choice("1912", 3, 2300),
                new Choice("1949", 4, 8000, true)
            }));

            Quests.Add(new Question("Deez [...]", new Choice[] {
                new Choice("no u", 1, 3000),
                new Choice("nuts", 2, 5000), // very punishing
                new Choice("comrads", 3, 8000, true),
                new Choice("shut up", 4, 9000)
            }));

            Quests.Add(new Question("Femboy?", new Choice[] {
                new Choice("hell yes", 1, 500),
                new Choice("hell no", 2, 300),
                new Choice("will you?", 3, 900),
                new Choice("Kill'em!", 4, 900, true)
            }));

            Quests.Add(new Question("Did any demonstration happened in China?", new Choice[] {
                new Choice("Yes", 1, 9999999),
                new Choice("No", 2, 8000, true)
            }));

            // making calculations for the perfect score
            // so that value would be updated automatically when the amount of questions is updated as well
            ScoreRank[6] = CalculatePerfectScore(Quests);

            IMG02.Visibility = Visibility.Hidden;
        }

        private void MainTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (GamePhase == 0)
            {

                // making the "PRESS ANY KEY" text blinking
                Dispatcher.BeginInvoke((Action)(() => { TextBlock02.Visibility = (INT1 % 2 == 0) ? Visibility.Visible : Visibility.Hidden; }));

                INT1 = (INT1 + 1) % int.MaxValue;
            }
        }

        private string PrintQuestionTitle(Question Q /* not that Qanon shit mind you :) */)
        {
            return (INT2 + 1) + ". " + Q.Title;
        }

        private void NextQuestion() { ++INT2; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // playing the main theme!
            BGM00.Source = new Uri("GF/BGM/GOPEN.mp3", UriKind.RelativeOrAbsolute);
            BGM00.Play();
        }

        // function that will make the question phase automatised (sort of)
        private void MakeQuestionWindow(int Question, string NewBGfilename, string NewCharfilename)
        {
            Canvas.SetLeft(IMG00, 0);
            Canvas.SetTop(IMG00, 0);
            Canvas.SetLeft(IMG01, 155);
            Canvas.SetTop(TextBlock01, 205);
            Canvas.SetLeft(TextBlock01, 450);
            Canvas.SetTop(TextBlock02, 205);
            Canvas.SetLeft(TextBlock02, 750);
            Canvas.SetTop(TextBlock03, 305);
            Canvas.SetLeft(TextBlock03, 450);
            Canvas.SetLeft(TextBlock00, 0);
            Canvas.SetTop(TextBlock04, 305);
            Canvas.SetLeft(TextBlock04, 750);
            Canvas.SetLeft(TextBlock05, 10);
            Canvas.SetTop(TextBlock05, 10);

            F1                      = true;
            IMG00.Source            = new BitmapImage(new Uri(NewBGfilename, UriKind.RelativeOrAbsolute));
            IMG00.Width             = this.Width;
            IMG00.Height            = this.Height;
            IMG00.Stretch           = Stretch.Fill;
            IMG01.Source            = new BitmapImage(new Uri(NewCharfilename, UriKind.RelativeOrAbsolute));
            TextBlock00.Text        = PrintQuestionTitle(Quests[Question]);
            TextBlock01.Foreground  = Brushes.White;
            TextBlock01.Text        = Quests[Question].AmountOfChoice[0].PrintChoice();
            TextBlock01.FontSize    = 44;
            TextBlock02.Visibility  = Visibility.Visible;
            TextBlock02.Foreground  = TextBlock01.Foreground;
            TextBlock02.Background  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            TextBlock02.FontSize    = TextBlock01.FontSize;
            TextBlock03.Foreground  = TextBlock01.Foreground;
            TextBlock03.FontSize    = TextBlock01.FontSize;
            TextBlock03.Visibility  = Visibility.Visible;
            TextBlock04.Foreground  = TextBlock01.Foreground;
            TextBlock04.FontSize    = TextBlock01.FontSize;
            TextBlock04.Visibility  = Visibility.Visible;
            TextBlock05.Visibility  = Visibility.Visible;
            TextBlock05.Text        = "Score : " + You.Score;
            TextBlock05.Foreground  = Brushes.Yellow;
            TextBlock05.Background  = Brushes.Red;

            try { TextBlock02.Text = Quests[Question].AmountOfChoice[1].PrintChoice(); }
            catch (IndexOutOfRangeException) { TextBlock02.Visibility = Visibility.Hidden; }
            
            try { TextBlock03.Text = Quests[Question].AmountOfChoice[2].PrintChoice(); }
            catch (IndexOutOfRangeException) {TextBlock03.Visibility = Visibility.Hidden; }

            try {TextBlock04.Text = Quests[Question].AmountOfChoice[3].PrintChoice();}
            catch (IndexOutOfRangeException) { TextBlock04.Visibility = Visibility.Hidden; }
        }

        private async void CountScore(int ClickedAnswer, int Q)
        {
            F1 = false;

            Canvas.SetLeft(IMG02, AnswerArea[ClickedAnswer - 1][0]);
            Canvas.SetTop(IMG02, AnswerArea[ClickedAnswer - 1][1]);


            if (Quests[Q].AmountOfChoice[ClickedAnswer-1].IsItCorrect == true)
            {
                // define the sign
                IMG02.Source = new BitmapImage(new Uri("GF/IMG/CHECK.png", UriKind.RelativeOrAbsolute));
                IMG02.Visibility = Visibility.Visible;

                // play the sound
                SFX00.Source = new Uri("GF/SFX/DING.wav", UriKind.RelativeOrAbsolute);
                SFX00.Play();

                await Task.Delay(1500);

                // success!
                SuccessScreen(Quests[Q].AmountOfChoice[ClickedAnswer - 1].GetPoints);
            }
            else
            {
                // due to the different image composition (if it makes sens)
                Canvas.SetTop(IMG02, (AnswerArea[ClickedAnswer - 1][1]) + 32);
                Canvas.SetLeft(IMG02, (AnswerArea[ClickedAnswer - 1][0]) - 5);
               
                IMG02.Source = new BitmapImage(new Uri("GF/IMG/wrong.png", UriKind.RelativeOrAbsolute));
                IMG02.Visibility = Visibility.Visible;

                SFX00.Source = new Uri("GF/SFX/WRONG.wav", UriKind.RelativeOrAbsolute);
                SFX00.Play();

                await Task.Delay(1500);

                WrongScreen(Quests[Q].AmountOfChoice[ClickedAnswer - 1].GetPoints);
            }
        }

        private void WrongScreen(int Score)
        {
            Canvas.SetLeft(IMG00, 275);
            Canvas.SetTop(IMG00, 200);
            Canvas.SetLeft(IMG01, 550);
            Canvas.SetLeft(TextBlock02, 914);
            Canvas.SetTop(TextBlock02, (this.ActualHeight - TextBlock02.ActualHeight) - 10);
            Canvas.SetLeft(TextBlock01, 10);
            Canvas.SetTop(TextBlock01, 470);

            IMG02.Visibility        = Visibility.Hidden;
            TextBlock03.Visibility  = Visibility.Hidden;
            TextBlock04.Visibility  = Visibility.Hidden;
            IMG00.Source            = new BitmapImage(new Uri("GF/IMG/DOWN.png", UriKind.RelativeOrAbsolute));
            IMG00.Stretch           = Stretch.Uniform;
            IMG00.Width             = 222;
            IMG00.Height            = 222;
            this.Background         = new SolidColorBrush(Color.FromRgb(0xff, 0, 0));
            IMG01.Source            = new BitmapImage(new Uri("GF/IMG/PETER_UNHAPPY.png", UriKind.RelativeOrAbsolute));
            TextBlock01.Foreground  = Brushes.White;
            TextBlock00.Text        = "-" + Score + " Social Credit!";
            TextBlock01.Text        = "-" + Score + "社会信用";
            You.Score               -= Score;
            TextBlock02.Text        = "Press any key to go to the next question";
            TextBlock02.Foreground  = Brushes.Yellow;
            TextBlock02.Background  = Brushes.Red;
            TextBlock02.FontSize    = 12;
            SFX00.Source            = new Uri("GF/SFX/NOO.wav", UriKind.RelativeOrAbsolute);
            GamePhase               = 0xff;
            TextBlock05.Text        = "Score : " + You.Score;

            SFX00.Play();
        }

        private void SuccessScreen(int Score)
        {
            Canvas.SetLeft(IMG00, 275);
            Canvas.SetTop(IMG00, 200);
            Canvas.SetLeft(IMG01, 550);
            Canvas.SetLeft(TextBlock02, 914);
            Canvas.SetTop(TextBlock02, (this.ActualHeight - TextBlock02.ActualHeight) - 10);
            Canvas.SetLeft(TextBlock01, 10);
            Canvas.SetTop(TextBlock01, 470);

            IMG02.Visibility        = Visibility.Hidden;
            TextBlock03.Visibility  = Visibility.Hidden;
            TextBlock04.Visibility  = Visibility.Hidden;
            IMG00.Source            = new BitmapImage(new Uri("GF/IMG/up.png", UriKind.RelativeOrAbsolute));
            IMG00.Stretch           = Stretch.Uniform;
            IMG00.Width             = 222;
            IMG00.Height            = 222;
            this.Background         = new SolidColorBrush(Color.FromRgb(0, 0xdf, 0));
            IMG01.Source            = new BitmapImage(new Uri("GF/IMG/PETER_HAPPY.png", UriKind.RelativeOrAbsolute));
            TextBlock01.Foreground  = Brushes.White;
            TextBlock00.Text        = "+" + Score + " Social Credit!";
            TextBlock01.Text        = "+" + Score + "社会信用";
            You.Score               += Score;
            TextBlock02.Text        = "Press any key to go to the next question";
            TextBlock02.Foreground  = Brushes.Yellow;
            TextBlock02.Background  = Brushes.Red;
            TextBlock02.FontSize    = 12;
            SFX00.Source            = new Uri("GF/SFX/YEY.wav", UriKind.RelativeOrAbsolute);
            GamePhase               = 0xff;
            TextBlock05.Text        = "Score : " + You.Score;

            SFX00.Play();
        }

        private async void Ending()
        {
            // just for being sure
            F1 = false;

            int MainScore = 0;

            // clearing the list
            // for memory management
            Quests.Clear();

            RankingText = new Dictionary<int, string>()
            {
                { ScoreRank[0], "You are getting executed!" },
                { ScoreRank[1], "You shoud go to Xinjiang!" },
                { ScoreRank[2], "You are a bad citizen!"    },
                { ScoreRank[3], "You should try again!"     },
                { ScoreRank[4], "You are a good citizen!"   },
                { ScoreRank[5], "Just like my creator!"     },
                { ScoreRank[6], "You did a good job!"       }
            };

            // yandere dev mode -> ON
            if      (You.Score <= ScoreRank[0])                                                        { MainScore = ScoreRank[0]; }
            else if (You.Score > ScoreRank[0] && You.Score < ScoreRank[1])                             { MainScore = ScoreRank[1]; }
            else if (You.Score > ScoreRank[1] && You.Score < ScoreRank[2])                             { MainScore = ScoreRank[2]; }
            else if (You.Score > ScoreRank[2] && You.Score < ScoreRank[3])                             { MainScore = ScoreRank[3]; }
            else if (You.Score > ScoreRank[3] && You.Score < ScoreRank[4])                             { MainScore = ScoreRank[4]; }
            else if (You.Score > ScoreRank[4] && You.Score < ScoreRank[5])                             { MainScore = ScoreRank[5]; }
            else if (You.Score > ScoreRank[5] && You.Score < ScoreRank[6] || You.Score > ScoreRank[6]) { MainScore = ScoreRank[6]; }

            // debug
            // MessageBox.Show(MainScore.ToString());

            Canvas.SetTop(TextBlock01, (this.Height / 2) - (TextBlock01.ActualHeight / 2));
            Canvas.SetLeft(TextBlock01, 0);
            Canvas.SetTop(TextBlock00, (this.ActualHeight));

            // in order to avoid key reading mode
            // ik it means nothing and idc :3
            GamePhase = 1;

            // comparing the score
            // so the user will have the correspond ending
            BGM00.Stop();
            SFX00.Stop();

            // "empty" backgrond!
            this.Background = Brushes.Black;

            // set the score title!
            TextBlock01.Text                = RankingText[MainScore];
            TextBlock01.TextAlignment       = TextAlignment.Center;
            TextBlock01.VerticalAlignment   = VerticalAlignment.Center;
            TextBlock01.Width               = this.Width;
            TextBlock01.Height              = 50;

            // Taiwan number one!
            TextBlock00.Text = "Taiwan number one!";

            // applying different style
            TextBlock00.Foreground = Brushes.White;
            TextBlock00.FontSize = 25;
            TextBlock00.Width = this.Width;
            TextBlock00.Height = 50;
            TextBlock00.VerticalAlignment = VerticalAlignment.Center;

            // remove all the other shit here!
            LAYER00.Children.Remove(TextBlock02);
            LAYER00.Children.Remove(TextBlock03);
            LAYER00.Children.Remove(TextBlock04);
            LAYER00.Children.Remove(TextBlock05);
            LAYER00.Children.Remove(IMG00);
            LAYER00.Children.Remove(IMG01);
            LAYER00.Children.Remove(IMG02);


            // preparing for the animation
            TextBlock01.FontSize    = 1;
            TextBlock01.Visibility  = Visibility.Hidden;

            // suspense
            await Task.Delay(1000);

            #region Loading media
            // loading the BGM and the video
            if (MainScore == ScoreRank[0])
            {
                BGM00.Source = new Uri("GF/BGM/VERYBADENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/TRULYBADENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[1])
            {
                BGM00.Source = new Uri("GF/BGM/BADENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/BADENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[2])
            {
                BGM00.Source = new Uri("GF/BGM/BADENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/BADENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[3])
            {
                BGM00.Source = new Uri("GF/BGM/AVERAGEENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/GOODENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[4])
            {
                BGM00.Source = new Uri("GF/BGM/GOODENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/GOODENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[5])
            {
                BGM00.Source = new Uri("GF/BGM/GOODENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/GOODENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            else if (MainScore == ScoreRank[6])
            {
                BGM00.Source = new Uri("GF/BGM/PERFECTENDING.mp3", UriKind.RelativeOrAbsolute);
                VID00.Source = new Uri("GF/VID/PERFECTENDING.mp4", UriKind.RelativeOrAbsolute);
            }
            #endregion

            // clearing it too
            RankingText.Clear();

            VID00.Visibility = Visibility.Visible;
            VID00.Play();
            GamePhase = 128;
            while (GamePhase == 128) { await Task.Delay(1); }
            while (VID00.Opacity > 0) { VID00.Opacity -= 0.01; await Task.Delay(1); }

            await Task.Delay(2000);

            #region the Text animation
            TextBlock01.Visibility = Visibility.Visible;

            // the zoom-in animation
            while (TextBlock01.FontSize < 35)
            {
                TextBlock01.FontSize += 0.5;
                await Task.Delay(10);
            }
            #endregion

            // play it!
            BGM00.Play();

            #region The credit animation
            await Task.Delay(5000);

            //                                          X                               Y
            double[] TextBlock00ActualPos = { Canvas.GetLeft(TextBlock00), Canvas.GetTop(TextBlock00) };
            double[] TextBlock01ActualPos = { Canvas.GetLeft(TextBlock01), Canvas.GetTop(TextBlock01) };

            for (int x = 0; x <= 320; ++x)
            {
                TextBlock00ActualPos[1] -= 1;
                TextBlock01ActualPos[1] -= 1;

                Canvas.SetTop(TextBlock00, TextBlock00ActualPos[1]);
                Canvas.SetTop(TextBlock01, TextBlock01ActualPos[1]);

                await Task.Delay(50);
            }    
            #endregion

            // THE END :)
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (GamePhase == 0)
            {
                // loading all the extra TextBlocks
                TextBlock03 = new TextBlock();
                TextBlock04 = new TextBlock();
                TextBlock05 = new TextBlock();

                TextBlock00.Width = this.Width;

                IMG00.Stretch = Stretch.Fill;

                IMG02.Width = 100;
                IMG02.Height = 100;

                LAYER00.Children.Add(TextBlock03);
                LAYER00.Children.Add(TextBlock04);
                LAYER00.Children.Add(IMG02);
                LAYER00.Children.Add(TextBlock05);

                LAYER00.Children.Remove(buymecoffeeIMG);

                // making all hoverable
                TextBlock01.MouseEnter  += TB_MouseEnter;
                TextBlock02.MouseEnter  += TB_MouseEnter;
                TextBlock03.MouseEnter  += TB_MouseEnter;
                TextBlock04.MouseEnter  += TB_MouseEnter;

                TextBlock01.MouseLeave  += TB_MouseLeave;
                TextBlock02.MouseLeave  += TB_MouseLeave;
                TextBlock03.MouseLeave  += TB_MouseLeave;
                TextBlock04.MouseLeave  += TB_MouseLeave;

                TextBlock01.MouseDown   += TB_MouseDown;
                TextBlock02.MouseDown   += TB_MouseDown;
                TextBlock03.MouseDown   += TB_MouseDown;
                TextBlock04.MouseDown   += TB_MouseDown;

                TextBlock01.MouseUp     += TB_MouseUp;
                TextBlock02.MouseUp     += TB_MouseUp;
                TextBlock03.MouseUp     += TB_MouseUp;
                TextBlock04.MouseUp     += TB_MouseUp;

                mainTimer.Enabled = false;

                buymecoffeeIMG.MouseDown += BuymecoffeeIMG_MouseDown;

                MakeQuestionWindow(INT2, "GF/IMG/PEKIN.jpg", "GF/IMG/PETER.png");

                // ..
                GamePhase = 1;
            }
            else if (GamePhase == 0xff)
            {
                // stop the SFX
                SFX00.Stop();

                if (INT2 < (Quests.Count - 1))
                {
                    // neeeeext
                    //  - dog girl in Hololive
                    NextQuestion();

                    GamePhase = 1;

                    MakeQuestionWindow(INT2, QuestionPics[INT2][0], QuestionPics[INT2][1]);
                }
                else
                {
                    // ending
                    Ending();
                }
            }
            else if (GamePhase == 128)
            {
                VID00.Stop();
                GamePhase = 1;
            }
        }

        private void BuymecoffeeIMG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // support me
            Process.Start("https://www.buymeacoffee.com/ryuguuchan");
        }

        private void TB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (F1 == true)
            {
                TextBlock X = (TextBlock)sender;
                X.Foreground = Brushes.Red;

                // counting the score
                CountScore(int.Parse(X.Text[0].ToString()), INT2);
            }
        }

        private void TB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (F1 == true)
            {
                TextBlock X = (TextBlock)sender;
                X.Foreground = Brushes.DarkRed;
            }
        }

        private void VID00_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (GamePhase == 128)
            {
                VID00.Stop();
                GamePhase = 1;
            }
        }

        private void BGM00_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (GamePhase != 1)
            {
                // loop
                BGM00.Stop();
                BGM00.Play();
            }
        }

        private void TB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (F1 == true)
            {
                TextBlock X = (TextBlock)sender;
                X.Foreground = Brushes.White;
            }
        }

        private void TB_MouseEnter(object sender, MouseEventArgs e)
        {
            if (F1 == true)
            {
                TextBlock X = (TextBlock)sender;
                X.Foreground = Brushes.Red;
            }
        }
    }
}
