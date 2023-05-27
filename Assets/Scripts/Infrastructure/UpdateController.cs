using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Infrastructure
{
    public class UpdateController : MonoBehaviour
    {
        private List<IUpdatable> _updatableEntities;

        public void Init()
        {
            _updatableEntities = new List<IUpdatable>();

            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            foreach (IUpdatable updatableEntity in _updatableEntities)
            {
                updatableEntity.OnUpdate(Time.deltaTime);
            }
        }

        public void Register(IUpdatable updatable)
        {
            _updatableEntities.Add(updatable);
        }
    }
}