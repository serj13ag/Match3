using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Services.Mono
{
    public class UpdateMonoService : MonoBehaviour, IUpdateMonoService
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

        public void Remove(IUpdatable updatable)
        {
            _updatableEntities.Remove(updatable);
        }
    }
}