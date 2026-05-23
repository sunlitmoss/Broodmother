using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace broodmother.broodmotherCode.Keywords;


public class BroodmotherKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Metamorphosis;
}