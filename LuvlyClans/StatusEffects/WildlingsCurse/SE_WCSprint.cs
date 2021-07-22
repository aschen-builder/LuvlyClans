namespace LuvlyClans.StatusEffects.WildlingsCurse
{
    class SE_WCSprint : StatusEffect
    {
        public float m_bonus = 0f;

        public void Awake()
        {
            m_name = "Sprinting Curse of the Wildlings";
            base.name = "Sprinting Curse of the Wildlings";
            m_tooltip = $"Stamina Use while Sprinting increased by {m_bonus}x";
        }

        public void SetSprintStaminaUse(float bonus)
        {
            m_bonus = bonus;
            m_tooltip = $"Stamina Use while Sprinting decreased by {bonus}x";
        }

        public float GetSprintStaminaUse() { return m_bonus; }
    }
}
