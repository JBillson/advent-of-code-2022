class Program{
    static void Main(string[] args){
        var input = File.ReadAllLines("day-1/input.txt");
        List<int> totals = new List<int>();
        int[] topCalorieCarriers = new int[3];
        var currentCalorieCount = 0;
        foreach (var line in input)
        {
            if (line == "") {
                totals.Add(currentCalorieCount);
                currentCalorieCount = 0;
                continue;
            }

            var isValid = int.TryParse(line, out int number);
            if (!isValid) continue;
            currentCalorieCount += number;
        }

        totals = totals.OrderByDescending(x => x).ToList();
        var top3 = totals.Take(3);
        Console.WriteLine("Top 3 Carriers:");
        foreach (var calories in top3)
            Console.WriteLine(calories);

        var summedTotal = 0;
        foreach (var total in top3)
            summedTotal += total;

        Console.WriteLine($"Summed Total: {summedTotal}");
    }
}