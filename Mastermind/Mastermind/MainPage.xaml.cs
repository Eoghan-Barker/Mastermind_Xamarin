//Eoghan Barker - G00397072
using System;
using System.IO;
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
        // Global variables and arrays
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

        string[] _solutionColours = new string[4];

        BoxView _currColourSelected;


        public MainPage()
        {
            InitializeComponent();
            // Generates 4 random colors for the top 4 circles, only displayed to user on game win/loss
            GenerateSolution();
            // Creates and draws circular boxviews for each grid on the board
            CreateCircles(Color.SandyBrown, "blank", GAMEPLAY_SIZE, GAMEPLAY_COLS, GAMEPLAY_ROWS, GrdGuessing);
            CreateCircles(Color.SandyBrown, "blank", FEEDBACK_SIZE, FEEDBACK_COLS, FEEDBACK_ROWS, GrdFeedback);
            CreateCircles(Color.Black, "blank", HIDDEN_SIZE, HIDDEN_COLS, HIDDEN_ROWS, GrdSolution);
            CreateCircles(Color.Black, "choice", CHOICE_SIZE, CHOICE_COLS, CHOICE_ROWS, GrdChoices);


            // For testing - uncomment
            // reveals the 4 solution colors to the user
            //revealSolution();
            
        }

        private void CreateCircles(Color colour, string myStyleId, int circleSize, int circleCols, int circleRows, Grid g)
        {
            int c, r, counter = 0;

            // creating the event handler  Tapped="TapGestureRecognizer_Tapped"
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            
            // Gameplay boxViews use the Gameplay tapped event handler which allows the user to select a circle to place the color in
            // Choice boxViews use the Choice_Tapped event handler which allows the user to select a colour to place into one of the gameplay boxviews
            // Feedback and Hidden boxviews also get the gameplay tapped event handler but the user will not be able to change the colours of these pieces due to
            // the _roundCounter variable restricting the rows that can be modified
            if (String.Equals(myStyleId, "blank"))
            {
                t.Tapped += Gameplay_Tapped;
            }
            else if(String.Equals(myStyleId, "choice"))
            {
                t.Tapped += Choice_Tapped;             
            }
            
            BoxView b;

            // move through each row and column of the specified grid to place the boxviews
            for (r = 0; r < circleRows; r++)
            {
                for (c = 0; c < circleCols; c++)
                {
                    // For choice boxviews, they are the only ones that have a colour applied to them at the beginning
                    // they are the only ones who dont get passed a styleId of "blank"
                    // the counter variable is used to apply 1 colour to each of them
                    // the styleIds will be used to compare the guesses to the solution
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
                       
                    // create a BoxView and add to the grid
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
            // this boxview is the one that the user has tapped on
            BoxView b = (BoxView)sender;

            // make sure colour has been selected and is being placed in correct row
            // otherwise alert the user
            // if it is a valid choice update the color and styleId of the chosen boxView
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
            // update global variable so it can be applied to the gameplay boxView the user chooses later
            _currColourSelected = (BoxView)sender;
        }

        private void GenerateSolution()
        {
            int i;
            Random random = new Random();
            List<string> colours = new List<string>(8);

            //populate list with the availble color names
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

            // add the first 4 list items to the solution
            for (i = 0; i < 4; i++)
            {
                _solutionColours[i] = colours.ElementAt(i);
            }
        }

        private void Check_Clicked(object sender, EventArgs e)
        {
            BoxView[] feedbackPins = new BoxView[4];
            string[] guessArr = new string[4];
            int i, r = 0, reds = 0, whites = 0;



            // ElemetAt starts at 9 and ends at 48 on Feedback and Gameplay grids so _boxViewFinder
            // starts at 45(beginning of last row) and goes down by 4 each time user moves to a new row
            // Get the 4 styleIds from current row
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

            // reset pins for the next round
            for (i = 0; i < 4; i++)
            {
                feedbackPins[r].BackgroundColor = Color.SandyBrown;
                feedbackPins[r].StyleId = "blank";
            }

            // add pins to board and update styleId
            for (i = 0; i < reds; i++)
            {
                feedbackPins[r].BackgroundColor = Color.Red;
                feedbackPins[r].StyleId = "red";
                r++;
            }
            for (i = 0; i < whites; i++)
            {
                feedbackPins[r].BackgroundColor = Color.White;
                feedbackPins[r].StyleId = "white";
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

            
            //_boxViewFinder updates so that the boxView in the first column of the correct row can be stored
            //_roundCounter is used to find the correct row when storing boxviews and also to only allow the user to place colors into the
            // row currently in play
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
                GrdFeedback.Children.ElementAt(i).StyleId = "blank";
            }

            //reset global variables
            _roundCounter = 9;
            _boxViewFinder = 45;
            _currColourSelected = null;

        }

        private void Save_Clicked(object sender, EventArgs e)
        {

            int i;
            // Create file "gameSave" in AppData\Local\Packages\44deb0d4-9f2c-47ab-b124-0bcfa00ca5ba_nqm0mxa73aetg\LocalState
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gameSave.txt");

            //clear file if previous save exist
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            //Solution Colors, Feedback styleIds, Gameplay styleIds, roundCounter and boxViewFinder need to be saved
            // each one is appended to the file with a new line character so the file can be split easily when read back in
            //write solution to file        
            for (i = 0; i < 4; i++)
            {
                File.AppendAllText(fileName, _solutionColours[i]);
                File.AppendAllText(fileName, "\n");
            }

            //write guessing and feedback to file
            for (i = 0; i < 40; i++)
            {
                File.AppendAllText(fileName, GrdGuessing.Children.ElementAt(i + 9).StyleId);
                File.AppendAllText(fileName, "\n");
            }
            for (i = 0; i < 40; i++)
            {
                File.AppendAllText(fileName, GrdFeedback.Children.ElementAt(i + 9).StyleId);
                File.AppendAllText(fileName, "\n");
            }

            //write roundCounter and boxViewFinder to file
            File.AppendAllText(fileName, _roundCounter.ToString());
            File.AppendAllText(fileName, "\n");
            File.AppendAllText(fileName, _boxViewFinder.ToString());
            File.AppendAllText(fileName, "\n");

            if (File.Exists(fileName))
            {
                DisplayAlert("Alert", "Game Saved Successfully", "ok");
            }
            else
            {
                DisplayAlert("Alert", "Error saving game", "ok");
            }
        }

        private void Load_Clicked(object sender, EventArgs e)
        {
            int i;
            string content;
            string[] contentSplit;
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gameSave.txt");
            


            if (File.Exists(fileName))
            {
                // read the file into a variable
                StreamReader sr = new StreamReader(fileName);
                using (sr)
                {
                    content = sr.ReadToEnd();
                }

                // split the content at the new line character and store into an array
                // contentSplit[0-3] = solution colours
                // contentSplit[4-43] = Gameplay colours
                // contentSplit[44-83] = Feedback colours              
                // contentSplit[84] = _roundCounter             
                // contentSplit[85] = _boxViewFinder            
                contentSplit = content.Split('\n');

                // load solution
                for (i = 0; i < 4; i++)
                {
                    _solutionColours[i] = contentSplit[i];
                }

                // load gameplay and feedback styleIds
                for (i = 0; i < 40; i++)
                {
                    GrdGuessing.Children.ElementAt(i + 9).StyleId = contentSplit[i + 4];
                }
                for (i = 0; i < 40; i++)
                {
                    GrdFeedback.Children.ElementAt(i + 9).StyleId = contentSplit[i + 44];
                }

                // load roundCounter and boxviewfinder
                _roundCounter = int.Parse(contentSplit[84]);
                _boxViewFinder = int.Parse(contentSplit[85]);

                //recolour boxviews using the styleIds that have been loaded in
                recolourCircles(40, GrdGuessing);
                recolourCircles(40, GrdFeedback);

                DisplayAlert("Alert", "Game Loaded Successfully", "ok");
            }
            else
            {
                DisplayAlert("Alert", "No Game Save Exists", "ok");
            }

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
            //display pop up "You Loose"
            DisplayAlert("You Loose", "Better luck next time", "OK");

            //colour in the 4 "hidden" circles
            revealSolution();
        }

        private void revealSolution()
        {
            recolourCircles(4, GrdSolution);
        }


        // Colors in the boxViews on the specified grid using their styleIds
        // Used to reveal hidden solution and for loading a saved game
        private void recolourCircles(int numCircles, Grid g)
        {
            int i;
            string[] circleStyleIds = new string[40];
            Color color = Color.SandyBrown;

            // solution grid elements start at 0 but feedback and guessing grid elements start at 9
            // so array needs to be populated differently
            if (g == GrdSolution)
            {
                for(i = 0; i < 4; i++)
                {
                    circleStyleIds[i] = _solutionColours[i];
                }
            }
            else
            {
                for (i = 0; i < 40; i++)
                    circleStyleIds[i] = g.Children.ElementAt(i + 9).StyleId;
            }


            

            for (i = 0; i < numCircles; i++)
            {
                switch (circleStyleIds[i])
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
                    default:
                        color = Color.SandyBrown;
                        break;
                }

                // solution grid elements start at 0 but feedback and guessing grid elements start at 9
                // so ElementAt number will be different
                // solution grid only has 4 boxviews other 2 grids have 40
                if (numCircles < 5)
                {
                    g.Children.ElementAt(i).BackgroundColor = color;
                }
                else
                {
                    g.Children.ElementAt(i + 9).BackgroundColor = color;
                }
                
            }
        }
    }
}
