using BaseLib.Abstracts;
using BaseLib.Utils;
using broodmother.broodmotherCode.Character;

namespace broodmother.broodmotherCode.Potions;

[Pool(typeof(broodmotherPotionPool))]
public abstract class broodmotherPotion : CustomPotionModel;