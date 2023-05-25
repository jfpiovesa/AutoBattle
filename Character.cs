using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float BaseDamage { get; set; }
        public float DamageMultiplier { get; set; }

        public bool stun = false;
        public bool isDead = false;

        public CharacterClassSpecific CharacterClassSpecific;
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; }
        public Character(CharacterClass characterClass)
        {
            switch (characterClass)
            {

                case CharacterClass.Paladin:
                    CharacterClassSpecific.characterClass = CharacterClass.Paladin;
                    CharacterClassSpecific.hpModifier = 150;
                    CharacterClassSpecific.ClassDamage = 20;

                    CharacterClassSpecific.skills = new CharacterSkills[1];
                    CharacterClassSpecific.skills[0].name = "Ataque forte";
                    CharacterClassSpecific.skills[0].damage = 20;
                    CharacterClassSpecific.skills[0].damageMultiplier = 2;

                    break;
                case CharacterClass.Archer:
                    CharacterClassSpecific.characterClass = CharacterClass.Archer;
                    CharacterClassSpecific.hpModifier = 100;
                    CharacterClassSpecific.ClassDamage = 20;

                    CharacterClassSpecific.skills = new CharacterSkills[1];
                    CharacterClassSpecific.skills[0].name = "Throw rock";
                    CharacterClassSpecific.skills[0].damage = 25;
                    CharacterClassSpecific.skills[0].damageMultiplier = 1;

                    break;
                case CharacterClass.Cleric:
                    CharacterClassSpecific.characterClass = CharacterClass.Cleric;
                    CharacterClassSpecific.hpModifier = 130;
                    CharacterClassSpecific.ClassDamage = 15;

                    CharacterClassSpecific.skills = new CharacterSkills[1];
                    CharacterClassSpecific.skills[0].name = "Teleport";
                    CharacterClassSpecific.skills[0].damage = 0;
                    CharacterClassSpecific.skills[0].damageMultiplier = 0;


                    break;
                case CharacterClass.Warrior:
                    CharacterClassSpecific.characterClass = CharacterClass.Warrior;
                    CharacterClassSpecific.hpModifier = 120;
                    CharacterClassSpecific.ClassDamage = 17;

                    CharacterClassSpecific.skills = new CharacterSkills[1];
                    CharacterClassSpecific.skills[0].name = "Slash stun";
                    CharacterClassSpecific.skills[0].damage = 25;
                    CharacterClassSpecific.skills[0].damageMultiplier = 1;
                    break;
            }

            Health = CharacterClassSpecific.hpModifier;
            BaseDamage = CharacterClassSpecific.ClassDamage;

        }
        public bool TakeDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                return true;
            }
            return false;
        }

        public void Die(Character target)
        {
            //TODO >> maybe kill him?
            Console.WriteLine($"Player {target.Name} is died \n");
        }

        public void WalkTO(Grid battlefield)
        {
            // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
            if (currentBox.xIndex > Target.currentBox.xIndex)
            {
                if (battlefield.grids.Exists(x => x.xIndex == currentBox.xIndex - 1 && x.yIndex == currentBox.yIndex))
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                    currentBox.ocupied = true;

                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked left\n");
                    battlefield.DrawBattlefield();

                    return;
                }
            }
            else if (currentBox.xIndex < Target.currentBox.xIndex)
            {
                if (battlefield.grids.Exists(x => x.xIndex == currentBox.xIndex + 1 && x.yIndex == currentBox.yIndex))
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                    currentBox.ocupied = true;

                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked right\n");
                    battlefield.DrawBattlefield();
                    return;
                }
            }

            if (currentBox.yIndex > Target.currentBox.yIndex)
            {
                if (battlefield.grids.Exists(x => x.xIndex == currentBox.xIndex && x.yIndex == currentBox.yIndex - 1))
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.yLength));
                    currentBox.ocupied = true;

                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked up\n");
                    battlefield.DrawBattlefield();
                    return;
                }

            }
            else if (currentBox.yIndex < Target.currentBox.yIndex)
            {
                if (battlefield.grids.Exists(x => x.xIndex == currentBox.xIndex && x.yIndex == currentBox.yIndex + 1))
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.yLength));
                    currentBox.ocupied = true;

                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked down\n");
                    battlefield.DrawBattlefield();

                    return;
                }
            }
        }

        public void StartTurn(Grid battlefield)
        {
            if (isDead | Target.isDead) return;
            if (!stun)
            {
                if (CharacterClassSpecific.characterClass == CharacterClass.Archer)
                {
                    // insira um numero ate 99
                    if (AtackProbabilityPercentageIfGreater(70))
                    {
                        SkillAtack(Target, battlefield);
                        return;
                    }
                }
                if (CheckCloseTargets(battlefield))
                {
                    Attack(Target, battlefield);
                    return;
                }
                else
                {
                    WalkTO(battlefield);
                }
            }
            else
            {
                Console.WriteLine($"Player {Name} is stun effect has passed \n");
                stun = false;
            }
        }
        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.yLength).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.yLength).ocupied);

            if (left || right || up || down)
            {
                return true;
            }
            return false;
        }

        public void Attack(Character target, Grid battlefield)
        {
            if (!isDead)
            {
                if (AtackProbabilityPercentageIfGreater(70))
                {
                    SkillAtack(target, battlefield);
                }
                else
                {
                    BaseAtack(target);
                }
            }
            else
            {
                Die(target);
            }
        }
        void BaseAtack(Character target)
        {
            int takeDamage = GetRandomInt(5, (int)BaseDamage);
            isDead = target.TakeDamage(takeDamage);
            Console.WriteLine($"Player {Name} is attacking the player {target.Name} and did {takeDamage} damage and has {target.Health} life left \n");
        }
        void SkillAtack(Character target, Grid battlefield)
        {
            int takeDamage = (int)(CharacterClassSpecific.skills[0].damage * CharacterClassSpecific.skills[0].damageMultiplier);
            isDead = target.TakeDamage(takeDamage);
            EffectOfSkillAttacks(CharacterClassSpecific.skills[0].name, target, battlefield);
            Console.WriteLine($"Player {Name} is attacking the player {target.Name} with skill { CharacterClassSpecific.skills[0].name} and did {takeDamage} damage and has {target.Health} life left \n");
        }
        void EffectOfSkillAttacks(string name, Character target , Grid battlefield)
        {
            switch (name)
            {
                case "Slash stun":
                    target.stun = true;
                    break;
                case "Teleport":
                    TeleportSkill(battlefield);
                    break;
            }
        }
        void TeleportSkill(Grid battlefield)
        {
            int random = GetRandomInt(0, battlefield.grids.Count);
            GridBox RandomLocation = (battlefield.grids.ElementAt(random));
            if (!RandomLocation.ocupied)
            {               
                currentBox.ocupied = false;
                battlefield.grids[currentBox.Index] = currentBox;
                currentBox = RandomLocation;
                currentBox.ocupied = true;
                battlefield.grids[currentBox.Index] = currentBox;
                Console.WriteLine($"Player {Name} Teleport \n");
                battlefield.DrawBattlefield();
            }
            else
            {
                TeleportSkill(battlefield);
            }       
        }
        int GetRandomInt(int min, int max)
        {
            var rand = new Random();
            int index = rand.Next(min, max);
            return index;
        }
        bool AtackProbabilityPercentageIfGreater(int valor)
        {
            var rand = new Random();
            int index = rand.Next(0, 99);
            if (index > valor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
