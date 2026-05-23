using broodmother.broodmotherCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Powers;

public class MoltThornsPower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Molt>();
}