using UnityEngine;
using System.Collections.Generic;

namespace Steer2D
{
    public class SteeringAgent : MonoBehaviour
    {
        public string GroupID;
        public float MaxVelocity = 100;
        public float Mass = 10;
        public float Friction = .05f;
        public bool RotateSprite = true;

        [HideInInspector]
        public Vector2 CurrentVelocity;

        public static Dictionary<string, List<SteeringAgent>> AgentList = new Dictionary<string, List<SteeringAgent>>();

        List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();

        public void RegisterSteeringBehaviour(SteeringBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void DeregisterSteeringBehaviour(SteeringBehaviour behaviour)
        {
            behaviours.Remove(behaviour);
        }

        void Start()
        {
            List<SteeringAgent> SALst= new List<SteeringAgent>();
            if (!AgentList.ContainsKey(GroupID))
            {
                AgentList.Add(GroupID, SALst);
            }
            AgentList[GroupID].Add(this);
        }

        void Update()
        {
            Vector2 acceleration = Vector2.zero;

            foreach (SteeringBehaviour behaviour in behaviours)
            {
                if (behaviour.enabled)
                    acceleration += behaviour.GetVelocity() * behaviour.Weight;
            }

            CurrentVelocity += acceleration / Mass;

            CurrentVelocity -= CurrentVelocity * Friction;

            if (CurrentVelocity.magnitude > MaxVelocity)
                CurrentVelocity = CurrentVelocity.normalized * MaxVelocity;

            transform.position = transform.position + (Vector3)CurrentVelocity * Time.deltaTime;
        
            if (RotateSprite && CurrentVelocity.magnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(CurrentVelocity.y, CurrentVelocity.x) * Mathf.Rad2Deg;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            }
        }

        void OnDestroy()
        {
            AgentList[GroupID].Remove(this);
        }
    }
}