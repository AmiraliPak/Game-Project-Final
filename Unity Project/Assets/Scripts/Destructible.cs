﻿using UnityEngine;

// object with a life
public abstract class Destructible : MonoBehaviour
{
    [SerializeField] float life;
    public float Score;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            IProjectile projectile = collision.gameObject.GetComponent<IProjectile>();
            if(projectile == null) projectile = collision.gameObject.GetComponentInParent<IProjectile>();
            Debug.Log(this.GetType().ToString() + " got hit: -" + projectile.Damage.ToString());
            AddLife(-projectile.Damage);
            projectile.Deactivate();
        }
    }

    void AddLife(float life)
    {
        this.life += life;
        // emit event
        CheckLife();
    }

    void CheckLife()
    {
        if (life <= 0f)
        {
            Debug.Log(this.GetType().ToString() + " life zero: Object deactivated");
            AnimateDestruction();
            OnDestruction();
            // emit event
            gameObject.SetActive(false);
        }
    }
    public virtual void OnDestruction()
    {
        EventSystemCustom.Instance.OnIncreaseScore.Invoke(Score);
    }

    void AnimateDestruction()
    {

    }
}
