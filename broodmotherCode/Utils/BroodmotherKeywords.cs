using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Shift;
}