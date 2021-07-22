namespace LuvlyClans.StatusEffects.WildlingsCurse
{
    class SE_WCRun : StatusEffect
    {
        public float m_bonus = 0f;

        public void Awake()
        {
            m_name = "Running Curse of the Wildlings";
            base.m_name = "Running Curse of the Wildlings";
            m_tooltip = $"Running Skill increased by {m_bonus}x";
        }

        public void SetRunLevel(float bonus)
        {
            m_bonus = bonus;
            m_tooltip = $"Running Skill increased by {bonus}x";
        }

        public float GetRunLevel() { return m_bonus; }
    }
}
