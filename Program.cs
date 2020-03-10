using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MTG
{
    class Program
    {
        static readonly string textFile = "mtg.txt";
        static void Main(string[] args)
        {
            using (StreamReader file = new StreamReader(textFile))
            {
                List<Tournament> results = new List<Tournament>();
                int counter = 0;
                string ln;
                while ((ln = file.ReadLine()) != "All") {}
                file.ReadLine();
                while ((ln = file.ReadLine()) != null && !ln.Contains("You are a Planeswalker"))
                {
                    string lp = ln;
                    while (!ln.Contains("Event Type:"))
                    {
                        lp = ln;
                        ln = file.ReadLine();
                    }
                    string date = lp.Substring(0, 10);
                    string eventType = ln;
                    string eventMultiplier = file.ReadLine();
                    string players = file.ReadLine();
                    string participationPoints = file.ReadLine();
                    string format = file.ReadLine();
                    string location = file.ReadLine();
                    string place = file.ReadLine();
                    string sanctioningNumber = file.ReadLine();
                    string matches = file.ReadLine();
                    Tournament temp = new Tournament(sanctioningNumber.Split(':')[1].Trim(), date.Trim(), format.Split(':')[1].Trim(), Int32.Parse(place.Split(':')[1].Trim()), Int32.Parse(players.Split(':')[1].Trim()));
                    results.Add(temp);
                    while ((ln = file.ReadLine()) != "Planeswalker Points Earned:" && ln!= "Teammates:")
                    {
                        
                        string record = ln;
                        string opponent = file.ReadLine();
                        while (opponent.Contains('(') && opponent.Contains(')'))
                        {
                            temp.Matches.Add(new Games(record.Split(null)[1], "NO DATA"));
                            record = opponent;
                            opponent = file.ReadLine();
                        }
                        if(!opponent.Contains("Planeswalker Points Earned:"))
                        {
                            temp.Matches.Add(new Games(record.Split(null)[1], opponent));
                        }
                        else { break; }
                    }
                    if (ln == "Teammates:") { file.ReadLine(); }
                    while (!ln.Contains("Correct this event.")) { ln=file.ReadLine(); }
                    counter++;
                    Console.WriteLine("=======================\nProcessed: "+counter);
                }
                Console.WriteLine("\n\n\nFinished Reading.\n======================\n");
                file.Close();

                string writeMe = "Event,Date,Format,Player,Result\n";

                results.ForEach(delegate (Tournament tour)
                {
                    tour.Matches.ForEach(delegate (Games game)
                    {
                        writeMe += tour.SN + "," + tour.Date + "," + tour.Format + "," + game.Opponent + "," + game.Result + "\n";
                    });
                });
                System.IO.File.WriteAllText("results.csv", writeMe);
            }
            Console.ReadKey();
        }

    }

    public class Tournament
    {
        public string SN;
        public string Date;
        public string Format;
        public int Place;
        public List<Games> Matches;
        public int Players;

        public Tournament(string sn, string date, string format, int place, int players)
        {
            SN = sn;
            Date = date;
            Format = format;
            Place = place;
            Matches = new List<Games>();
            Players = players;
        }
    }

    public class Games
    {
        public string Result;
        public string Opponent;
        public Games(string result, string opponent)
        {
            Result = result;
            Opponent = opponent;
        }
    }
}
