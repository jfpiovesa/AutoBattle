using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {

        public struct CharacterClassSpecific
        {
            public CharacterClass characterClass;
            public float hpModifier;
            public float ClassDamage;
            public CharacterSkills[] skills;
            public CharacterClassSpecific(CharacterClass Class, float hp, float Damage, CharacterSkills[] skills)
            {
                this.characterClass = Class;
                this.hpModifier = hp;
                this.ClassDamage = Damage;
                this.skills = skills;
            }
        }

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;

            public GridBox(int x, int y, bool ocupied, int index)
            {
                this.xIndex = x;
                this.yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;
            }

        }

        public struct CharacterSkills
        {
            public string name;
            public float damage;
            public float damageMultiplier;
            public CharacterSkills(string name, float damage, float damageMultiplier)
            {
                this.name = name;
                this.damage = damage;
                this.damageMultiplier = damageMultiplier;
            }
        }

        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

    }
}
