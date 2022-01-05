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


        public MainPage()
        {
            InitializeComponent();
            CreateCircles(Color.SandyBrown, "blank", GAMEPLAY_SIZE, GAMEPLAY_COLS, GAMEPLAY_ROWS, GrdGuessing);
            CreateCircles(Color.Black, "blank", FEEDBACK_SIZE, FEEDBACK_COLS, FEEDBACK_ROWS, GrdFeedback);
            CreateCircles(Color.Black, "blank", HIDDEN_SIZE, HIDDEN_COLS, HIDDEN_ROWS, GrdSolution);
            CreateCircles(Color.Black, "red", CHOICE_SIZE, CHOICE_COLS, CHOICE_ROWS, GrdChoices);
            
        }

        private void CreateCircles(Color colour, string myStyleId, int circleSize, int circleCols, int circleRows, Grid g)
        {
            int c, r, counter = 0;

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            //t.Tapped += Piece_Tapped;   // creating the event handler  Tapped="TapGestureRecognizer_Tapped"
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
                    b.HeightRequest = circleSize;
                    b.WidthRequest = circleSize;
                    b.CornerRadius = 20;
                    b.SetValue(Grid.RowProperty, r);
                    b.SetValue(Grid.ColumnProperty, c);
                    b.GestureRecognizers.Add(t);
                    g.Children.Add(b);
                    counter++;
                }
            }
        }
    }
}
