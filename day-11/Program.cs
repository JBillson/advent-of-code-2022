﻿using System.Diagnostics;
namespace day11;

class Program{
    private const string newLine = "\r\n";
    private static string _input = string.Empty;
    private static List<Monkey> _monkeys = new();
    private static int _round;
    static void Main(string[] args){
        _input = File.ReadAllText("input.txt");
        var instructions = _input.Split($"{newLine}{newLine}").ToList();
    
        // parse instructions
        foreach (var set in instructions){
            var lines = set.Split("\r\n");
            var name = lines[0];
            var startingItems = lines[1];
            var operation = lines[2];
            var testCase = lines[3];
            var ifTrue = lines[4];
            var ifFalse = lines[5];

            // create monkey and add to list
            var monkey = new Monkey(name, startingItems, operation, testCase, ifTrue, ifFalse);
            _monkeys.Add(monkey);
        }

        StartGame(_monkeys);
    }

    private static void StartGame(List<Monkey> _monkeys){
        _round = 0;
        while (_round < 20){
            foreach (var monkey in _monkeys)
            {
                foreach (var item in monkey.items.ToList())
                {
                    Inspect(monkey, item);
                }
            }

            // increment round
            _round ++;

            // log monkeys after round
            System.Console.WriteLine($"After Round {_round}:");
            foreach (var monkey in _monkeys)
                System.Console.WriteLine($"Monkey {monkey.name}: {string.Join(',', monkey.items)}");
        }

        // get two most active monkeys
        var monkeys = _monkeys.OrderByDescending(x => x.inspectionCount);
        var activeMonkeys = monkeys.Take(2);

        foreach (var monkey in monkeys)
            System.Console.WriteLine($"Monkey {monkey.name} inspected items {monkey.inspectionCount} times.");

        // calculate monkey business
        var monkeyBusiness = 1;
        foreach (var monkey in activeMonkeys)
            monkeyBusiness *= monkey.inspectionCount;
        
        System.Console.WriteLine($"Monkey Business: {monkeyBusiness}");
    }

    private static void Inspect(Monkey monkey, int itemWorryLevel){
        // increment monkey inspect count
        monkey.inspectionCount ++;

        // parse operation
        var amountString = monkey.operation.Split(" ")[^1];
        var symbol = monkey.operation.Split(" ")[^2];
        var amountInt = 0;
        if (amountString.Contains("old")){
            amountInt = itemWorryLevel;
        }
        else{
            amountInt = int.Parse(amountString);
        }
        
        // inspect item & calculate new itemWorryLevel
        switch (symbol)
        {
            case "*":
                itemWorryLevel *= amountInt;
            break;
            case "+":
                itemWorryLevel += amountInt;
            break;
            case "-":
                itemWorryLevel -= amountInt;
            break;
            case "/":
                itemWorryLevel /= amountInt;
            break;
            default:
                throw new Exception($"Unknown Operator {symbol}");
        }

        // divide worryLevel by 3
        itemWorryLevel /= 3;

        // run test
        var test = monkey.test;
        var divisibleBy = int.Parse(test.testCase.Split(" ")[^1]);
        var monkeyName = string.Empty;
        if (itemWorryLevel % divisibleBy == 0){
            monkeyName = test.ifTrue.Split(" ")[^1]; 
        }
        else{
            monkeyName = test.ifFalse.Split(" ")[^1];
        }

        // pass on item
        _monkeys.First(x => x.name == monkeyName).items.Add(itemWorryLevel);
        monkey.items.RemoveAt(0);
    }
}

class Monkey{
    public string name;
    public List<int> items = new();
    public string operation = string.Empty;
    public Test test;
    public int inspectionCount;
    public Monkey(string name, string startingItems, string operation, string testCase, string ifTrue, string ifFalse){
        // name
        this.name = name.Split(":")[0].Split(" ")[1];

        // items
        var itemList = startingItems.Split(": ")[1].Split(",");
        foreach (var item in itemList)
            items.Add(int.Parse(item));

        // operation
        this.operation = operation.Split(": ")[1];

        // test
        testCase = testCase.Split(": ")[1];
        ifTrue = ifTrue.Split(": ")[1];
        ifFalse = ifFalse.Split(": ")[1];
        test = new Test(testCase, ifTrue, ifFalse);
        System.Console.WriteLine($"Creating Monkey:\nName: {this.name}\nStarting Items: {string.Join(',', this.items)}\nOperation: {this.operation}\nTest: {testCase}\n\tIf true: {ifTrue}\n\tIf false: {ifFalse}");
    }

    // public void Log(this Monkey m){
    //     System.Console.WriteLine($"Creating Monkey:\nName: {m.name}\nStarting Items: {string.Join(',', m.items)}\nOperation: {m.operation}\nTest: {m.test.testCase}\n\tIf true: {m.test.ifTrue}\n\tIf false: {m.test.ifFalse}");
    // }
}

class Test{
    public string testCase = string.Empty;
    public string ifTrue = string.Empty;
    public string ifFalse = string.Empty;

    public Test(string testCase, string ifTrue, string ifFalse){
        this.testCase = testCase;
        this.ifTrue = ifTrue;
        this.ifFalse = ifFalse;
    }
}