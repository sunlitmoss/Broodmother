using BaseLib.Abstracts;
using broodmother.broodmotherCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Powers;

public class MoltStrengthPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Molt>();
}