using BaseLib.Abstracts;
using broodmother.broodmotherCode.Extensions;
using Godot;

namespace broodmother.broodmotherCode.Character;

public class broodmotherPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Broodmother.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}