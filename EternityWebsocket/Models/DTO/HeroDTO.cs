namespace EternityWebsocket.Models.DTO
{
    class HeroDTO
    {
        public int hp { get; set; }
        public int deathessence { get; set; }
        public int mana { get; set; }
        public int maxMana { get; set; }
        public int maxDeathessence { get; set; }

        public HeroDTO()
        {
            this.hp = 30;
            this.maxDeathessence = 0;
            this.maxMana = 1;  
        }
    }
}