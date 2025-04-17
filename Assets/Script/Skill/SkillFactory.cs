public static class SkillFactory {
    public static Skill Create(SkillConfig skillConfig,PlayerSkillController skillController) {
        switch (skillConfig.SkillType) {
            case SkillType.FireBallSkill: return new FireBallSkill(skillConfig,skillController);
            case SkillType.ExplosionAroundPlayerSkill: return new ExplosionAroundPlayerSkill(skillConfig,skillController);
            case SkillType.SpeedIncreaseSkill: return new PlayerIncreasedSpeed(skillConfig,skillController);
            case SkillType.ManaIncreaseSkill: return new PlayerIncreasedManaRegeneration(skillConfig,skillController);
            case SkillType.FireBallSkillSmall: return new FireBallSkill(skillConfig,skillController);
            case SkillType.HealingSkill : return new HealingSkill(skillConfig,skillController);
        }
        throw new System.Exception("Unknown skill type: " + skillConfig.SkillType);
    }
}