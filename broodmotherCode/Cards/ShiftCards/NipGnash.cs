using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards.ShiftCards;


public class Nip() : ShiftCard<Gnash>(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4m, ValueProp.Move),
        new DynamicVar("GnashAmount", 3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Gnash.IncreaseAmount += DynamicVars["GnashAmount"].IntValue;
        if (ShiftRegistries.CombatPairs.TryGetValue(this, out var gnash))
            gnash.DynamicVars.Damage.BaseValue += DynamicVars["GnashAmount"].IntValue;
        
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["GnashAmount"].UpgradeValueBy(2m);
    }
}

public class Gnash() : ShiftCard<Nip>(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public static int IncreaseAmount = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m + IncreaseAmount, ValueProp.Move)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        new List<CardKeyword> { BroodmotherKeywords.Shift, CardKeyword.Exhaust };
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}