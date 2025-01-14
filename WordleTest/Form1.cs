﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordleTest
{
    public partial class Form1 : Form
    {
        public TextBox[][] letters = new TextBox[5][];
        public string wordToGuess;
        public int guesses = 0;
        public checkGuess check = new checkGuess();
        public bool backspace = false;
        public bool hasWon = false;
        public Form1()
        {   
            InitializeComponent();
            textGrid();
            generateWord randWord = new generateWord();
            wordToGuess = randWord.wordToGuess();
            Console.WriteLine(wordToGuess);
            openRow();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            //call method to update row, change colours depending on which letters correct
            updateColours(check.updateGrid(guesses, letters, wordToGuess));
        }
        public void textGrid()
        {
            //generates 5x6 textbox grid
            for (int i = 0; i < 5; i++)
            {
                letters[i] = new TextBox[6];
                for (int j = 0; j < 6; j++)
                {
                    letters[i][j] = new TextBox();
                    //keep all read only, unlock row depending on which guess
                    letters[i][j].ReadOnly = true;
                    letters[i][j].CharacterCasing = CharacterCasing.Upper;
                    letters[i][j].SuspendLayout();
                    letters[i][j].Location = new System.Drawing.Point(70 + (i * 29), 59 + (j * 29));
                    letters[i][j].MaxLength = 1;
                    letters[i][j].Size = new System.Drawing.Size(28, 28);
                    letters[i][j].TabIndex = 0;
                    this.letters[i][j].TextChanged += new System.EventHandler(this.textChange);
                    this.letters[i][j].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressed);
                    this.Controls.Add(letters[i][j]);
                }
            }
        }

        public void updateColours(int[] colours)
        {
            //updates colours of row, green for correct, yellow for within word but wrong position
            //loop through colours, change color, 0 leave, 1 green, 2 yellow
            //method within update colours returns -1 if invalid guess
            if (colours[0] == -1)
            {
                MessageBox.Show("Invalid guess, must guess a valid 5 letter word");
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    //loop through colours and change colour of grid
                {
                    if (colours[i] == 1)
                    {
                        letters[i][guesses].BackColor = Color.Green;
                    }
                    else if (colours[i] == 2)
                    {
                        letters[i][guesses].BackColor = Color.LightGoldenrodYellow;
                    }
                    //lock row after guess
                    letters[i][guesses].ReadOnly = true;
                }
                //valid guess so add to guess and check status of game
                guesses++;
                //check game status, if won/lost then end, otherwise continue with next row
                gameStatus(colours);
                openRow();
            }
            
        }

        public void gameStatus(int[] col)
        {
            hasWon = true;
            //check if each char matches
            for (int i=0; i<col.Length; i++)
            {
                if (col[i]!=1)
                {
                    hasWon = false;
                }
            }
            if (hasWon)
            {
                MessageBox.Show($"Congratulations! You guessed {wordToGuess.ToUpper()} in {guesses} tries");
                this.Hide();
            }
            else if (!hasWon && guesses == 6)
            {
                MessageBox.Show($"Unlucky! The word was {wordToGuess.ToUpper()}");
                this.Hide(); 
            }
        }

        public void openRow()
        {
            //Makes next row unreadable, if game is still active, if all guesses used end game
            if (guesses != 6 && hasWon == false)
            {
                //opens row for guessing
                for (int i = 0; i < 5; i++)
                {
                    letters[i][guesses].ReadOnly = false;
                }
                //focus on new row
                letters[0][guesses].Focus();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void textChange(object sender, EventArgs e)
        {
            //move to next text box after char entered
            Console.WriteLine("Running");
            for (int i=0; i<4; i++)
            {
                if (letters[i][guesses].Text.Length == 1 && !backspace)
                {
                    letters[i + 1][guesses].Focus();
                }
            }
        }
        private void keyPressed(object sender, KeyPressEventArgs e)
        {
            //guesses word when enter key pressed, moves back textbox with backspace
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                backspace = true;
                for (int i = 4; i >= 1; i--)
                {
                    Console.WriteLine($"letters[i][guesses] is {letters[i][guesses]}");
                    if (letters[i][guesses].Text.Length == 0)
                    {
                        Console.WriteLine($"i is {i}");
                        letters[i - 1][guesses].Focus();
                        backspace = false;
                    }
                }
            }
        }
    }
}
