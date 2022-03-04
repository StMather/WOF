using System;
using System.Collections.Generic;
using LeapWoF.Interfaces;

//test comment

namespace LeapWoF
{

    /// <summary>
    /// The GameManager class, handles all game logic
    /// </summary>
    public class GameManager
    {

        /// <summary>
        /// The input provider
        /// </summary>
        private IInputProvider inputProvider;

        /// <summary>
        /// The output provider
        /// </summary>
        private IOutputProvider outputProvider;

        private string TemporaryPuzzle;
        public List<string> charGuessList = new List<string>();

        public GameState GameState { get; private set; }

        public GameManager() : this(new ConsoleInputProvider(), new ConsoleOutputProvider())
        {

        }

        public GameManager(IInputProvider inputProvider, IOutputProvider outputProvider)
        {
            if (inputProvider == null)
                throw new ArgumentNullException(nameof(inputProvider));
            if (outputProvider == null)
                throw new ArgumentNullException(nameof(outputProvider));

            this.inputProvider = inputProvider;
            this.outputProvider = outputProvider;

            GameState = GameState.WaitingToStart;
        }

        /// <summary>
        /// Manage game according to game state
        /// </summary>
        public void StartGame()
        {
            InitGame();

            while (true)
            {

                PerformSingleTurn();

                if (GameState == GameState.RoundOver)
                {
                    StartNewRound();
                    continue;
                }

                if (GameState == GameState.GameOver)
                {
                    outputProvider.WriteLine("Game over");
                    break;
                }
            }
        }
        public void StartNewRound()
        {
            TemporaryPuzzle = "Stefan git test";


            // update the game state
            GameState = GameState.RoundStarted;
        }

        public void PerformSingleTurn()
        {
            //outputProvider.Clear();
            DrawPuzzle();
            outputProvider.WriteLine("Type 1 to spin, 2 to solve");
            GameState = GameState.WaitingForUserInput;

            var action = inputProvider.Read();

            switch (action)
            {
                case "1":
                    Spin();
                    break;
                case "2":
                    Solve();
                    break;
            }

        }

        /// <summary>
        /// Add -----'s to clue shown
        /// </summary>
        private string HideClue()
        {
            string clueOut = "";
            foreach (char letter in TemporaryPuzzle)
            {
                if (letter != ' ')
                {
                    clueOut += "-";
                }
                else
                {
                    clueOut += " ";
                }
            }

            return clueOut;
        }

        /// <summary>
        /// Draw the puzzle
        /// </summary>
        private void DrawPuzzle()
        {
            outputProvider.WriteLine("The puzzle is:");
            outputProvider.WriteLine(HideClue());
            //outputProvider.WriteLine(TemporaryPuzzle);
            outputProvider.WriteLine();
        }


        /// <summary>
        /// Spin the wheel and do the appropriate action
        /// </summary>
        public void Spin()
        {
            outputProvider.WriteLine("Spinning the wheel...");
            //TODO - Implement wheel + possible wheel spin outcomes
            GuessLetter();
        }

        public void Solve()
        {
            outputProvider.Write("Please enter your solution:");
            var guess = inputProvider.Read();

            if (guess.ToLower() == TemporaryPuzzle.ToLower())
            {
                outputProvider.Clear();
                outputProvider.Write($"Congratulations! You're a WINNER!!! The word was {TemporaryPuzzle}! \n\n");
            } else
            {
                outputProvider.Clear();
                outputProvider.Write($"That's incorrect... Please try again...\n\n");
            }
        }
        public void GuessLetter()
        {
            outputProvider.Write("Please guess a letter: ");
            var guess = inputProvider.Read();
            charGuessList.Add(guess);
        }

        /// <summary>
        /// Optional logic to accept configuration options
        /// </summary>
        public void InitGame()
        {

            outputProvider.WriteLine("Welcome to Wheel of Fortune!");
            StartNewRound();
        }
    }
}
