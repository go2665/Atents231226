using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Character
    {
        // private, protected, public

        protected float hp;
        public float HP         // 프로퍼티(property)
        {
            get => hp;          // 읽기는 퍼블릭
            private set         // 쓰기는 프라이빗
            {
                //if( hp != value)  // 변경되었을 때를 알 수 있는 코드
                //{
                    hp = value;
                    Console.WriteLine($"[{name}]의 HP는 {hp}가 되었다.");

                    if (hp <= 0)
                    {
                        Die();
                    }
                    //if( hp < 0 ) 
                    //    hp = 0;
                    //if( hp > maxHp ) 
                    //    hp = maxHp;
                    hp = Math.Clamp(value, 0, maxHp);
                //}

            }
        }

        public bool IsAlive => hp > 0;  // 플레이어가 살아있는지 죽어있는지 확인하는 프로퍼티(살아있으면 true, 죽었으면 false)

        protected float maxHp;
        protected float mp;
        protected float maxMp;
        protected int level;
        protected float exp;
        protected const float maxExp = 100;   // 상수. 절대 변경 불가
        protected float attackPower;
        protected float defencePower;
        protected string name;
        public string Name => name;        // Name이라는 프로퍼티를 읽기 전용으로 만들고 읽으면 name을 리턴한다.

        protected const float skillCost = 10.0f;
        private bool CanSkillUse => mp > skillCost;

        Random random;

        public Character()
        {
            hp = 100.0f;
            maxHp = 100.0f;
            mp = 50.0f;
            maxMp = 50.0f;
            level = 1;
            exp = 0.0f;
            attackPower = 10.0f;
            defencePower = 5.0f;
            name = "무명";

            random = new Random();
        }

        public Character(string _name)
        {
            hp = 100.0f;
            maxHp = 100.0f;
            mp = 50.0f;
            maxMp = 50.0f;
            level = 1;
            exp = 0.0f;
            attackPower = 10.0f;
            defencePower = 5.0f;
            name = _name;

            random = new Random();
        }

        public void Attack(Character target)
        {
            if(CanSkillUse)
            {
                if( random.NextSingle() < 0.3f )
                {
                    Skill(target);
                }
                else
                {
                    Console.WriteLine($"[{name}]가 공격한다.");
                    target.Defence(attackPower);
                }
            }
            else
            {
                Console.WriteLine($"[{name}]가 공격한다.");
                target.Defence(attackPower);
            }

            
        }

        public void Skill(Character target) 
        {
            if(CanSkillUse)
            {
                mp -= skillCost;

                //float damage = OnSkill();
                //target.Defence(damage);
                target.Defence(OnSkill());
            }
        }

        protected virtual float OnSkill()    // OnSkill virtual 함수다. => OnSkill 함수는 상속받은 클래스에서 덮어쓸 수 있다(override가능).
        {
            Console.WriteLine("캐릭터가 스킬을 사용한다.");
            return 10.0f;
        }

        void Defence(float damage)
        {
            Console.WriteLine($"[{name}]이 {damage - defencePower} 만큼의 피해를 입었습니다.");
            HP -= (damage - defencePower);
        }

        void LevelUp()
        {

        }

        void Die()
        {
            Console.WriteLine($"[{name}]이 죽었습니다.");
        }
    }
}
