// Simple POC of Voice C2 using Speech Recognition
// Author: Dump-GUY (@vinopaljiri) 

// beginner reference C# Speech Recognition: https://youtu.be/caYyjxujZuU

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Speech.Recognition;


namespace VoiceC2_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create ONLY an in-process speech recognizer for the en-US locale.  
            using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
            {

                // Configure input to the speech recognizer -> use default.  
                recognizer.SetInputToDefaultAudioDevice();
                // Create and load my dict grammar.  
                recognizer.LoadGrammar(Create_Grammer());

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Handler_recognizer_SpeechRecognized);
                // Start asynchronous, continuous speech recognition. - dont stop after first recog completed
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.  
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }

        // Handle the SpeechRecognized event.  
        static void Handler_recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
            process_command(e.Result);
        }

        // build grammer
        static Grammar Create_Grammer()
        {
            String[] programs = new string[] { "powershell", "calc", "notepad" };
            String[] actions = new string[] { "run", "kill" };
            String[] websearch = new string[] { "search" };
            String[] search_strings = new string[] { "porn", "food" };
            String[] command = new string[] { "hack this" };


            GrammarBuilder GB1 = new GrammarBuilder(new Choices(actions));
            GB1.Append(new Choices(programs));

            GrammarBuilder GB2 = new GrammarBuilder(new Choices(websearch));
            GB2.Append(new Choices(search_strings));

            GrammarBuilder GB3 = new GrammarBuilder(new Choices(command));

            Grammar my_grammer = new Grammar(new Choices(GB1, GB2, GB3));

            return my_grammer;
        }

        static void process_command(RecognitionResult Result)
        {
            //run specified program
            if (Result.Words[0].Text.Equals("run"))
            {
                Process.Start(Result.Words[1].Text);
            }
            //kill specified program
            if (Result.Words[0].Text.Equals("kill"))
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    if (process.ProcessName.ToUpper().Contains(Result.Words[1].Text.ToUpper()))
                    {
                        process.Kill();
                    }
                }
            }
            //google search
            if (Result.Words[0].Text.Equals("search"))
            {
                Process.Start("https://www.google.com/search?q=" + Result.Words[1].Text);
            }

            //do command in hack_1 function
            if (Result.Text.Equals("hack this"))
            {
                hack_1(); 
            }
        }

        // just show picture from resources
        static void hack_1()
        {
            Bitmap bitmap1 = Resource1.chicken_hacker;
            Image img1 = bitmap1;

            Form form = new Form();
            form.Text = "Hacked !!!";
            form.WindowState = FormWindowState.Maximized;
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = img1;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            form.Controls.Add(pictureBox);
            Application.Run(form);
        }
    }
}
