using System;
using Core.Bioms;
using Core.Interactivity.AI.AIStates;
using Core.Interactivity.Movement;
using Gameplay;

namespace Core.Interactivity.AI.Brains
{
    public class KillerBrains:ArtificialIntelligence
    {
        private Player _player;
        private Health _playerHealth;

        #region ArtificialIntelligence

        protected override void InitStates()
        {
            _availiableStates.Add(EAIState.Alert, new AIStateAlert(this));
            _availiableStates.Add(EAIState.Attack, new AIStateAttack(this));
            BaseState = EAIState.Alert;
        }

        public void Punch()
        {
            if (_player == null)
            {
                _player = Game.Instance.CurrentSession.Player;
                _playerHealth = _player.GetComponent<Health>();
            }
            if (_playerHealth != null)
            {
                _playerHealth.CurrentHealthAmount -= 15;
            }
        }

        #endregion
    }
}

