using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    float randomposionx = 0;
    float randomposionz = 0;
    public GameObject magicBall;
    public GameObject mushroompreb;
    public GameObject stonemonsterpreb;
    public GameObject minidragonpreb;
    public GameObject fireBall;
    public GameObject boss_fireBall;
    public GameObject explosion;
    #region 오브젝트풀
    Queue<Explosion> explosionspool = new Queue<Explosion>();
    //GameObject expl;

    Queue<MagicBall> magicBallpool = new Queue<MagicBall>();    
    GameObject tmpobj;

    Queue<FireBall> fireBallpool = new Queue<FireBall>();
    //GameObject fire;

    Queue<Boss_Fireball> boss_fireBallpool = new Queue<Boss_Fireball>();
    //GameObject boss_fire;
        
    Queue<Mushroom> mushroompool = new Queue<Mushroom>();
    //GameObject mush;

    int randompoint = 0;
    public Transform[] stonemonsterspwnpoint;
    Queue<StoneMonster> stoneMonsterpool = new Queue<StoneMonster>();
    //GameObject stone;

    public Transform[] minidragonspwnpoint;
    Queue<MiniDragon> minidragonpool = new Queue<MiniDragon>();
    //GameObject mini;

    public GameObject HPPotion;
    Queue<HPpotion> HpPotionpool = new Queue<HPpotion>();
    //GameObject hp;
    public GameObject MPPotion;
    Queue<MPpotion> MpPotionpool = new Queue<MPpotion>();
    //GameObject mp;
    #endregion

    void Start()
    {
        tmpobj = Instantiate(magicBall, transform);
        magicBallpool.Enqueue(tmpobj.GetComponent<MagicBall>());
        tmpobj.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            tmpobj = Instantiate(fireBall, transform);
            fireBallpool.Enqueue(tmpobj.GetComponent<FireBall>());
            tmpobj.SetActive(false);
        }

        tmpobj = Instantiate(boss_fireBall, transform);
        boss_fireBallpool.Enqueue(tmpobj.GetComponent<Boss_Fireball>());
        tmpobj.SetActive(false);
        for (int i = 0; i < 7; i++) //머쉬룸
        {
            randomposionx = (Random.Range(67f, 132f)) * -1;
            randomposionz = Random.Range(-25f, 25f);
            Vector3 mushspwnpositon = new Vector3(randomposionx, 0, randomposionz);
            tmpobj = Instantiate(mushroompreb, mushspwnpositon, Quaternion.identity, transform);
            mushroompool.Enqueue(tmpobj.GetComponent<Mushroom>());
            tmpobj.SetActive(true);
        }
        StartCoroutine(SpawnMushroom());
        for (int i = 0; i < 10; i++) //스톤몬스터
        {
            randompoint = Random.Range(0, 9);
            tmpobj = Instantiate(stonemonsterpreb, stonemonsterspwnpoint[randompoint].position, Quaternion.identity, transform);
            stoneMonsterpool.Enqueue(tmpobj.GetComponent<StoneMonster>());
            tmpobj.SetActive(true);
        }
        StartCoroutine(SpawnStonMonster());
        for (int i = 0; i < 7; i++) //미니드래곤
        {
            randompoint = Random.Range(0, 9);
            tmpobj = Instantiate(minidragonpreb, minidragonspwnpoint[randompoint].position, Quaternion.identity, transform);
            minidragonpool.Enqueue(tmpobj.GetComponent<MiniDragon>());
            tmpobj.SetActive(true);
        }
        StartCoroutine(SpawnMinidragon());
        for (int i = 0; i < 5; i++)
        {
            tmpobj = Instantiate(HPPotion, transform);
            HpPotionpool.Enqueue(tmpobj.GetComponent<HPpotion>());
            tmpobj.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            tmpobj = Instantiate(MPPotion, transform);
            MpPotionpool.Enqueue(tmpobj.GetComponent<MPpotion>());
            tmpobj.SetActive(false);
        }

        tmpobj = Instantiate(explosion, transform);
        explosionspool.Enqueue(tmpobj.GetComponent<Explosion>());
        tmpobj.SetActive(false);
    }

    public MagicBall GetMagicBall()
    {
        if (magicBallpool.Count > 0)
        {return magicBallpool.Dequeue(); }
        else
        { return Instantiate(magicBall, transform).GetComponent<MagicBall>();  }
    }
    public void RetunMagicBall(MagicBall magicBall)
    {
        magicBallpool.Enqueue(magicBall);
        magicBall.transform.SetParent(this.transform);
        magicBall.gameObject.SetActive(false);
    }
    public Explosion GetExplosion()
    {
        if (explosionspool.Count > 0)
        { return explosionspool.Dequeue(); }
        else
        { return Instantiate(explosion, transform).GetComponent<Explosion>(); }
    }
    public void RetunExplosion(Explosion expl)
    {
        explosionspool.Enqueue(expl);
        expl.transform.SetParent(this.transform);
        expl.gameObject.SetActive(false);
    }


    public void CreateMushroom()
    {
        Mushroom mushroom;
        if (mushroompool.Count > 0)
        {mushroom = mushroompool.Dequeue(); }
        else
        {return;}
        mushroom.gameObject.SetActive(true);
    }
    IEnumerator SpawnMushroom()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); 
            CreateMushroom();
        }
    }

    public void Returnmushroom(Mushroom mushroom)
    {
        mushroompool.Enqueue(mushroom);
        mushroom.transform.SetParent(this.transform);
        mushroom.gameObject.SetActive(false);
    }
    
    public void CreateStonemonster()
    {
        StoneMonster stonemonster;
        if (stoneMonsterpool.Count > 0)
        { stonemonster = stoneMonsterpool.Dequeue(); }
        else
        { return; }
        stonemonster.gameObject.SetActive(true);
    }
    IEnumerator SpawnStonMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            CreateStonemonster();
        }
    }

    public void ReturnStonemonster(StoneMonster stonemonster)
    {
        stoneMonsterpool.Enqueue(stonemonster);
        stonemonster.transform.SetParent(this.transform);
        stonemonster.gameObject.SetActive(false);
    }
    public void CreateMinidragon()
    {
        MiniDragon miniDragon;
        if (minidragonpool.Count > 0)
        { miniDragon = minidragonpool.Dequeue(); }
        else
        {return;}
        miniDragon.gameObject.SetActive(true);
    }
    IEnumerator SpawnMinidragon()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            CreateMinidragon();
        }
    }
    public void ReturnMiniDragon(MiniDragon miniDragon)
    {
        minidragonpool.Enqueue(miniDragon);
        miniDragon.transform.SetParent(this.transform);
        miniDragon.gameObject.SetActive(false);
    }   
    public FireBall GetFireBall()
    {
        if (fireBallpool.Count > 0)
        {return fireBallpool.Dequeue();}
        else
        {return Instantiate(fireBall, transform).GetComponent<FireBall>();}
    }
    public void RetunFireBall(FireBall fireBall)
    {
        fireBallpool.Enqueue(fireBall);
        fireBall.transform.SetParent(this.transform);
        fireBall.gameObject.SetActive(false);
    }
    public Boss_Fireball GetBossFireBall()
    {
        if (boss_fireBallpool.Count > 0)
        { return boss_fireBallpool.Dequeue(); }
        else
        { return Instantiate(boss_fireBall, transform).GetComponent<Boss_Fireball>(); }
    }
    public void RetunBossFireBall(Boss_Fireball fireBall)
    {
        boss_fireBallpool.Enqueue(fireBall);
        fireBall.transform.SetParent(this.transform);
        fireBall.gameObject.SetActive(false);
    }
    public HPpotion GetHPpotion()
    {       
        if (HpPotionpool.Count>0)
        {
            HPpotion Potion = HpPotionpool.Dequeue();
            Potion.gameObject.SetActive(true);
            return Potion;
        }
        else
        {
           HPpotion newpotion = Instantiate(HPPotion, transform).GetComponent<HPpotion>();
           return newpotion;
        }       
    }
    public void ReturnHPpotion(MPpotion potion)
    {
        MpPotionpool.Enqueue(potion);
        potion.transform.SetParent(this.transform);
        potion.gameObject.SetActive(false);
    }
    public MPpotion GetMPpotion()
    {
        if (MpPotionpool.Count > 0)
        {
            MPpotion Potion = MpPotionpool.Dequeue();
            Potion.gameObject.SetActive(true);
            return Potion;
        }
        else
        {
            MPpotion newpotion = Instantiate(HPPotion, transform).GetComponent<MPpotion>();
            return newpotion;
        }
    }
    public void ReturnHPpotion(HPpotion potion)
    {
        HpPotionpool.Enqueue(potion);
        potion.transform.SetParent(this.transform);
        potion.gameObject.SetActive(false);
    }

}
