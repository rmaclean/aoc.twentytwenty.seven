using System.Security.AccessControl;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var fileData = await File.ReadAllLinesAsync("data.txt");
var bagFinderRegex = new Regex("((?<empty>no other)|(((?<count>\\d+)\\s)?(?<attribute>\\w+)\\s(?<color>\\w+)))\\s(bags?)");
var bags = new List<Bag>();
foreach (var line in fileData)
{
    var matches = bagFinderRegex.Matches(line).ToArray();
    var firstBag = FindExistingOrCreateNewBag(matches[0]);

    for (var index = 1; index < matches.Length; index++)
    {
        var holdMatch = matches[index];
        if (!holdMatch.Empty())
        {
            var bag = FindExistingOrCreateNewBag(holdMatch);
            firstBag.Holds.Add(bag, holdMatch.Count());
            bag.HeldBy.Add(firstBag);
        }
    }
}

var shinyGoldBag = bags.Single(b => b.Attribute == "shiny" && b.Color == "gold");
Step1();
Step2();

void Step2()
{
    var bagsInside = 0;
    WalkHolding(shinyGoldBag, 1);

    void WalkHolding(Bag bag, int modifier)
    {
        // Console.WriteLine($"Checking {bag} with modifier {modifier}");
        foreach (var held in bag.Holds)
        {
            var total = held.Value * modifier;
            bagsInside += total;

            WalkHolding(held.Key, total);
        }
    }

    Console.WriteLine($"Step 2 answer is {bagsInside}");
}

void Step1()
{
    var allPossibleHoldings = new List<Bag>();
    WalkHeldBy(shinyGoldBag);
    Console.WriteLine($"Step 1 answer is {allPossibleHoldings.Distinct().Count()}");

    void WalkHeldBy(Bag bag)
    {
        if (bag.HeldBy.Count == 0)
        {
            return;
        }

        allPossibleHoldings.AddRange(bag.HeldBy);
        foreach (var holdingBag in bag.HeldBy)
        {
            WalkHeldBy(holdingBag);
        }
    }
}

Bag? GetBag(Match match) => bags
    .SingleOrDefault(b => b.Attribute == match.Attribute() && b.Color == match.Color());

Bag FindExistingOrCreateNewBag(Match match)
{
    var bag = GetBag(match);
    if (bag == null)
    {
        bag = new Bag(match);
        bags.Add(bag);
    }

    return bag;
}
