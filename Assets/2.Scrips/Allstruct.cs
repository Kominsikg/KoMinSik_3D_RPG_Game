public class AllStruct
{
    public struct Share_Stat //캐릭터 공통 스텟
    {
        public float HP; //체력
        public float MaxHP; // 최대 체력
        public float MP; //마나
        public float MaxMP; // 최대 마나
        public int Level; //레벨
        public int EXP; //경험치
                       
        
        public Share_Stat(int level, float hp, float mp,int exp)
        {   
            this.Level = level;            
            this.HP = hp;
            this.MaxHP = hp;
            this.MP = mp;
            this.MaxMP = mp;
            this.EXP = exp;
           
        }
    }
    public struct individual_Stat // 캐릭터 개별 스텟
    {
        public float Att; // 공격력
        public float Def; // 방어력
        public individual_Stat(float att, float def)
        {
            this.Att = att;
            this.Def = def;
        }
    }

    public struct Enemy_Stat // 몬스터 스텟
    {
        public int Level;
        public float Att; 
        public float Def;
        public float HP;
        public float MaxHP;        

        public Enemy_Stat(int level,int att,int def,float hp)
        {
            this.Level=level;
            this.Att=att;
            this.Def=def;
            this.HP = hp;
            this.MaxHP = hp;
        }
    }
}
