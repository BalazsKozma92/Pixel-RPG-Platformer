using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement
    { 
        get
        {
            if (movement)
            {
                return movement;
            }

            Debug.LogError("No movement core component on " + transform.parent.name);
            return null;
        }

        private set { movement = value; }
    }

    public CollisionSenses CollisionSenses
    { 
        get
        {
            if (collisionSenses)
            {
                return collisionSenses;
            }

            Debug.LogError("No collision senses core component on " + transform.parent.name);
            return null;
        }

        private set { collisionSenses = value; }
    }

    public Combat Combat
    { 
        get
        {
            if (combat)
            {
                return combat;
            }

            Debug.LogError("No combat core component on " + transform.parent.name);
            return null;
        }

        private set { combat = value; }
    }

    Movement movement;
    CollisionSenses collisionSenses;
    Combat combat;

    void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>(); 
        Combat = GetComponentInChildren<Combat>();
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
        Combat.LogicUpdate();
    }
}
