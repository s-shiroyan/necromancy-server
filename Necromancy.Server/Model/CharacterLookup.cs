using System.Collections.Generic;

namespace Necromancy.Server.Model
{
    public class CharacterLookup
    {
        private readonly List<Character> _characters;

        private readonly object _lock = new object();

        public CharacterLookup()
        {
            _characters = new List<Character>();
        }

        /// <summary>
        /// Returns all Characters.
        /// </summary>
        public List<Character> GetAll()
        {
            lock (_lock)
            {
                return new List<Character>(_characters);
            }
        }

        /// <summary>
        /// Adds a Character.
        /// </summary>
        public void Add(Character character)
        {
            if (character == null)
            {
                return;
            }

            lock (_lock)
            {
                _characters.Add(character);
            }
        }

        /// <summary>
        /// Removes the Character from all lists and lookup tables.
        /// </summary>
        public void Remove(Character character)
        {
            lock (_lock)
            {
                _characters.Remove(character);
            }
        }

        /// <summary>
        /// Returns a Character by Character name if it exists.
        /// </summary>
        public Character GetBySoulName(string characterName)
        {
            List<Character> characters = GetAll();
            foreach (Character character in characters)
            {
                if (character.Name == characterName)
                {
                    return character;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Character by CharacterId if it exists.
        /// </summary>
        public Character GetByCharacterId(int characterId)
        {
            List<Character> characters = GetAll();
            foreach (Character character in characters)
            {
                if (character.Id == characterId)
                {
                    return character;
                }
            }

            return null;
        }


    }
}
