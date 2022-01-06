using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class MainPage : ContentPage
    {
        const int GAMEPLAY_ROWS = 10;
        const int GAMEPLAY_COLS = 4;
        const int GAMEPLAY_SIZE = 35;
        const int FEEDBACK_ROWS = 20;
        const int FEEDBACK_COLS = 2;
        const int FEEDBACK_SIZE = 20;
        const int HIDDEN_ROWS = 1;
        const int HIDDEN_COLS = 4;
        const int HIDDEN_SIZE = 35;
        const int CHOICE_ROWS = 2;
        const int CHOICE_COLS = 4;
        const int CHOICE_SIZE = 35;

        int _roundCounter = 9;
        int _boxViewFinder = 45;

        String[] _solutionColours = new string[4];

        BoxView _currColourSelected;


        public MainPage()
        {
            InitializeComponent();
            GenerateSolution();
            CreateCircles(Color.SandyBrown, "blank", GAMEPLAY_SIZE, GAMEPLAY_COLS, GAMEPLAY_ROWS, GrdGuessing);
            CreateCircles(Color.SandyBrown, "blank", FEEDBACK_SIZE, FEEDBACK_COLS, FEEDBACK_ROWS, GrdFeedback);
            CreateCircles(Color.Black, "blank", HIDDEN_SIZE, HIDDEN_COLS, HIDDEN_ROWS, GrdSolution);
            CreateCircles(Color.Black, "choice", CHOICE_SIZE, CHOICE_COLS, CHOICE_ROWS, GrdChoices);
            
        }

        private void CreateCircles(Color colour, string myStyleId, int circleSize, int circleCols, int circleRows, Grid g)
        {
            int c, r, counter = 0;

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            // creating the event handler  Tapped="TapGestureRecognizer_Tapped"
            if (String.Equals(myStyleId, "blank"))
            {
                t.Tapped += Gameplay_Tapped;
            }
            else if(String.Equals(myStyleId, "choice"))
            {
                t.Tapped += Choice_Tapped;             
            }
            
            BoxView b;

            for (r = 0; r < circleRows; r++)
            {
                for (c = 0; c < circleCols; c++)
                {
                    if (!String.Equals(myStyleId, "blank"))
                    {
                        switch (counter)
                        {
                            case 0:
                                colour = Color.Red;
                                myStyleId = "red";
                                break;
                            case 1:
                                colour = Color.Green;
                                myStyleId = "green";
                                break;
                            case 2:
                                colour = Color.Blue;
                                myStyleId = "blue";
                                break;
                            case 3:
                                colour = Color.Yellow;
                                myStyleId = "yellow";
                                break;
                            case 4:
                                colour = Color.Brown;
                                myStyleId = "brown";
                                break;
                            case 5:
                                colour = Color.Orange;
                                myStyleId = "orange";
                                break;
                            case 6:
                                colour = Color.Black;
                                myStyleId = "black";
                                break;
                            case 7:
                                colour = Color.White;
                                myStyleId = "white";
                                break;
                        }
                    }
                       

                    b = new BoxView();  // instantiate a new object
                    b.BackgroundColor = colour; //param
                    b.StyleId = myStyleId;  // param
                    b.HorizontalOptions = LayoutOptions.Center;
                    b.VerticalOptions = LayoutOptions.Center;
                    b.HeightRequest = circleSize; //param
                    b.WidthRequest = circleSize;    //param
                    b.CornerRadius = 20;
                    b.SetValue(Grid.RowProperty, r);
                    b.SetValue(Grid.ColumnProperty, c);
                    b.GestureRecognizers.Add(t);
                    g.Children.Add(b);
                    counter++;    
                }
            }
        }

        private void Gameplay_Tapped(object sender, EventArgs e)
        {
            BoxView b = (BoxView)sender;

            // make sure colour has been selected and is being placed in correct row
            if(_currColourSelected == null || (int)b.GetValue(Grid.RowProperty) != _roundCounter)
            {
                LblTesting.Text = "empty";
            }
            else
            {
                b.BackgroundColor = _currColourSelected.BackgroundColor; //param
                b.StyleId = _currColourSelected.StyleId;  // param
                GrdGuessing.Children.Add(b);
            }
        }

        private void Choice_Tapped(object sender, EventArgs e)
        {
            _currColourSelected = (BoxView)sender;
            LblTesting.Text = "full";
        }

        private void GenerateSolution()
        {
            int num, i;
            Random random = new Random();

            for (i = 0; i < 4; i++)
            {
                _solutionColours[i] = "red";
                
                num = random.Next(8);
                switch (num)
                {
                    case 0:
                        _solutionColours[i] = "red";
                        break;
                    case 1:
                        _solutionColours[i] = "green";
                        break;
                    case 2:
                        _solutionColours[i] = "blue";
                        break;
                    case 3:
                        _solutionColours[i] = "yellow";
                        break;
                    case 4:
                        _solutionColours[i] = "brown";
                        break;
                    case 5:
                        _solutionColours[i] = "orange";
                        break;
                    case 6:
                        _solutionColours[i] = "black";
                        break;
                    case 7:
                        _solutionColours[i] = "white";
                        break;
                }
            }
        }

        private void Check_Clicked(object sender, EventArgs e)
        {
            BoxView[] feedbackPins = new BoxView[4];
            String[] guessArr = new string[4];
            int i, r = 0, reds = 0, whites = 0;

            // get the 4 styleIds from current row
            for (i = 0; i < 4; i++)
            {
                guessArr[i] = GrdGuessing.Children.ElementAt(i + _boxViewFinder).StyleId;
            }

            //get the 4 feedback boxviews on current row
            for (i = 0; i < 4; i++)
            {
                feedbackPins[i] = (BoxView)GrdFeedback.Children.ElementAt(i + _boxViewFinder);
            }

            //check for red pins
            for (i = 0; i < 4; i++)
            {
                if (guessArr[i] == _solutionColours[i])
                {
                    reds++;
                }
            }

            //check for white pins
            if (guessArr[0] == _solutionColours[1] || guessArr[0] == _solutionColours[2] || guessArr[0] == _solutionColours[3])
            {
                whites++;
            }
            else if (guessArr[1] == _solutionColours[0] || guessArr[1] == _solutionColours[2] || guessArr[1] == _solutionColours[3])
            {
                whites++;
            }
            else if (guessArr[2] == _solutionColours[1] || guessArr[2] == _solutionColours[0] || guessArr[2] == _solutionColours[3])
            {
                whites++;
            }
            else if (guessArr[3] == _solutionColours[1] || guessArr[3] == _solutionColours[2] || guessArr[3] == _solutionColours[0])
            {
                whites++;
            }

            // reset pins
            for (i = 0; i < 4; i++)
            {
                feedbackPins[r].BackgroundColor = Color.SandyBrown;
            }

            // add pins to board
            for (i = 0; i < reds; i++)
            {
                feedbackPins[r].BackgroundColor = Color.Red;
                r++;
            }
            for (i = 0; i < whites; i++)
            {
                feedbackPins[r].BackgroundColor = Color.White;
                r++;
            }

            // check for game win
            if (reds == 4)
            {
                GameWon();
            }

           
            _boxViewFinder -= 4;
            _roundCounter--;
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {

        }

        private void Save_Clicked(object sender, EventArgs e)
        {

        }

        private void Load_Clicked(object sender, EventArgs e)
        {

        }

        private void GameWon()
        {
            //display pop up "You won"

            //colour in the 4 "hidden" circles
        }
    }
}
