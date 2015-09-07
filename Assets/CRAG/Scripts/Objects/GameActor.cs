﻿using System;
using UnityEngine;
using CRAG.InputSystem;
using CRAG.AchievementSystem;

namespace CRAG
{
    /// <summary>
    /// Исполнитель команд
    /// </summary>
    public class GameActor : MonoBehaviour
    {
        /// <summary>Камера, привязанная к персонажу</summary>
        public Camera cam;
        /// <summary>Префаб инстанцируемый при щелчке мышью</summary>
        public GameObject click;
        /// <summary>Коллайдер для поиска объектов для выхода на орбиту</summary>
        public SearchCollider searchSphere;
        /// <summary>Сила с которой толкается персонаж импульсом</summary>
        public float forceMagnitude;

        private Transform _transform;
        private Rigidbody _rigidbody;

        void Start()
        {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Применяет к персонажу импульс, увеличивая его скорость
        /// </summary>
        public void Impulse()
        {
            RaycastHit hit;
            Vector3 forceDirection = CalculateForceDirection(out hit);
            Instantiate(click, hit.point, Quaternion.identity); //проще было бы инстанцировать отметку клика внутри функции расчёта силы, но это
            _rigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }

        /// <summary>
        /// Выход на орбиту планеты.
        /// </summary>
        /// <remarks>Скорость вращения персонажа по орбите рассчитывается с учётом угла, под которым был выполнен выход.</remarks>
        public void EnterOrbit()
        {
            Collider detected = searchSphere.GetDetectedCollider();
            if (detected != null)
            {
                if (detected.name == "Comet")
                    AchievementManager.instance.UnlockAchievement(Achievements.RideTheComet);

                if (detected.name == "Pluton")
                    AchievementManager.instance.UnlockAchievement(Achievements.TakeInPluton);

                _transform.parent = detected.transform;
                Vector3 radius = _transform.parent.position - _transform.position;
                Vector3 tangent = new Vector3(-radius.z, 0, radius.x);
                _rigidbody.velocity = Vector3.Project(_rigidbody.velocity, tangent);
            }
        }

        /// <summary>
        /// Сойти с текущей орбиты
        /// </summary>
        public void DescendFromOrbit()
        {
            _transform.parent = null;
        }
        
        /// <summary>
        /// Рассчитывает напрвление силы импульса, исходя
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        private Vector3 CalculateForceDirection(out RaycastHit hit)
        {
            Vector3 forceDirection = Vector3.zero;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Raycasting")))
            {
                forceDirection = _transform.position - hit.point;
                forceDirection.Normalize();
            }

            return forceDirection;
        }
    }
}
