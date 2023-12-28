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
                hp = value;

                //if( hp < 0 ) 
                //    hp = 0;
                //if( hp > maxHp ) 
                //    hp = maxHp;
                hp = Math.Clamp(value, 0, maxHp);
            }
        }

        protected float maxHp;
        protected float mp;
        protected float maxMp;
        protected int level;
        protected float exp;
        protected const float maxExp = 100;   // 상수. 절대 변경 불가
        protected float attackPower;
        protected float defencePower;
        protected string name;
        string Name => name;        // Name이라는 프로퍼티를 읽기 전용으로 만들고 읽으면 name을 리턴한다.

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
        }

        public void Attack(Character target)
        {
            //hp = 100;
            Console.WriteLine($"[{name}]가 공격한다.");
            target.Defence(attackPower);
        }

        public virtual void Skill() // Skill은 virtual 함수다. => Skill 함수는 상속받은 클래스에서 덮어쓸 수 있다(override가능).
        {
            Console.WriteLine("캐릭터가 스킬을 사용한다.");
        }

        void Defence(float damage)
        {
            HP -= (damage - defencePower);
            Console.WriteLine($"[{name}]이 {damage - defencePower} 만큼의 피해를 입었습니다.");
        }

        void LevelUp()
        {

        }

        void Die()
        {

        }
    }
}
