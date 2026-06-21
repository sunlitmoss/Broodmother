using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace broodmother.broodmotherCode.Utils;

public class BroodmotherKeywords
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Shift;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sticky;
}