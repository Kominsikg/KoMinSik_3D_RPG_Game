public class AllStruct
{
    public struct Share_Stat //ĳ���� ���� ����
    {
        public float HP; //ü��
        public float MaxHP; // �ִ� ü��
        public float MP; //����
        public float MaxMP; // �ִ� ����
        public int Level; //����
        public int EXP; //����ġ
                       
        
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
    public struct individual_Stat // ĳ���� ���� ����
    {
        public float Att; // ���ݷ�
        public float Def; // ����
        public individual_Stat(float att, float def)
        {
            this.Att = att;
            this.Def = def;
        }
    }

    public struct Enemy_Stat // ���� ����
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
