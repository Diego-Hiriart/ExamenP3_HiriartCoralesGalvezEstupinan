using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroRuleEngine;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.IO;

namespace ExamenP3Rules
{
    class Program
    {
        static void Main(string[] args)
        {
            MRE engine = new MRE();

            Rule p1Wins = Rule.Create("p1X", mreOperator.GreaterThan, 15) & Rule.Create("p1X", mreOperator.LessThan, 30) 
                & Rule.Create("p1Z", mreOperator.GreaterThan, 146) & Rule.Create("p1Z", mreOperator.LessThan, 160.5);
            Func<SaveGameData, bool> p1WinsCompiled = engine.CompileRule<SaveGameData>(p1Wins);

            Rule p2Wins = Rule.Create("p2X", mreOperator.GreaterThan, 15) & Rule.Create("p2X", mreOperator.LessThan, 30)
                & Rule.Create("p2Z", mreOperator.GreaterThan, 146) & Rule.Create("p2Z", mreOperator.LessThan, 160.5);
            Func<SaveGameData, bool> p2WinsCompiled = engine.CompileRule<SaveGameData>(p2Wins);


            List<Rule> rulesList = new List<Rule>()
            {
                p1Wins,
                p2Wins
            };

            SaveGameData player1 = new SaveGameData();
            //SaveGameData player2 = new SaveGameData();

            player1.p1X = 16;
            player1.p1Z = 151;

            Console.WriteLine(p1WinsCompiled(player1));
            //Console.WriteLine(p2WinsCompiled(player2));


            string rulesJson = JsonConvert.SerializeObject(rulesList, Formatting.Indented);//Indented for nicer format and easier reading
            File.WriteAllText(@"..\..\..\Assets\Scripts\PlayerRules.json", rulesJson);

            Console.ReadLine();
        }
    }
}
