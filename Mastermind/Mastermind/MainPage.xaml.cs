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
            if(_currColourSelected == null)
            {
                DisplayAlert("Alert", "Please choose a colour", "OK");
            }
            else if((int)b.GetValue(Grid.RowProperty) != _roundCounter){
                DisplayAlert("Alert", "Place the piece in the correct row", "OK");
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
        }

        private void GenerateSolution()
        {
            int i;
            Random random = new Random();
            List<string> colours = new List<string>(8);

            //populate list
            colours.Add("red");
            colours.Add("green");
            colours.Add("blue");
            colours.Add("yellow");
            colours.Add("brown");
            colours.Add("orange");
            colours.Add("black");
            colours.Add("white");

            // random sort list
            colours = colours.OrderBy(x => random.Next()).ToList();

            for (i = 0; i < 4; i++)
            {
                _solutionColours[i] = colours.ElementAt(i);
            }
        }

        private void Check_Clicked(object sender, EventArgs e)
        {
            BoxView[] feedbackPins = new BoxView[4];
            String[] guessArr = new string[4];
            int i, r = 0, reds = 0, whites = 0;



            //ElemetAt starts at 9 and ends at 48 on both grids so _boxViewFinder starts at 48 and goes down by 4 each time user moves to a new row
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

            
            for (i = 0; i < 4; i++)
            {
                //check for red pins
                if (guessArr[i] == _solutionColours[i])
                {
                    reds++;
                }
                //check for white pins
                else if (guessArr[i] == _solutionColours[1] || guessArr[i] == _solutionColours[2] || guessArr[i] == _solutionColours[3] || guessArr[i] == _solutionColours[0])
                {
                    whites++;
                }
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

            // check for game win or loss
            if (reds == 4)
            {
                GameWon();
            }
            else if (_roundCounter == 0)
            {
                GameLost();
            }

            _boxViewFinder -= 4;
            _roundCounter--;
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            int i;

            //reset hidden boxviews + generate new solution
            for (i = 0; i < 4; i++)
            {
               GrdSolution.Children.ElementAt(i).BackgroundColor = Color.Black;
            }
            GenerateSolution();

            //reset gameplay + feedback boxviews
            for (i = 9; i < 49; i++)
            {
                GrdGuessing.Children.ElementAt(i).BackgroundColor = Color.SandyBrown;
                GrdGuessing.Children.ElementAt(i).StyleId = "blank";
                GrdFeedback.Children.ElementAt(i).BackgroundColor = Color.SandyBrown;
            }

            //reset global variables
            _roundCounter = 9;
            _boxViewFinder = 45;

            _currColourSelected = null;

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
            DisplayAlert("You Win", "Congratulations!", "OK");

            //colour in the 4 "hidden" circles
            revealSolution();
        }

        private void GameLost()
        {  
            DisplayAlert("You Loose", "Better luck next time", "OK");

            revealSolution();
        }

        private void revealSolution()
        {
            int i;

            Color color = Color.Red;

            for (i = 0; i < 4; i++)
            {
                switch (_solutionColours[i])
                {
                    case "red":
                        color = Color.Red;
                        break;
                    case "green":
                        color = Color.Green;
                        break;
                    case "blue":
                        color = Color.Blue;
                        break;
                    case "yellow":
                        color = Color.Yellow;
                        break;
                    case "brown":
                        color = Color.Brown;
                        break;
                    case "orange":
                        color = Color.Orange;
                        break;
                    case "black":
                        color = Color.Black;
                        break;
                    case "white":
                        color = Color.White;
                        break;
                }

                GrdSolution.Children.ElementAt(i).BackgroundColor = color;
            }

        }
    }
}
