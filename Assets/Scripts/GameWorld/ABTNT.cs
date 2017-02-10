﻿using UnityEngine;
using System.Collections;

public class ABTNT : ABGameObject {

	public float _explosionArea = 1f;
	public float _explosionPower = 1f;
	public float _explosionDamage = 1f;

	public override void Die(bool withEffect = true)
	{
		//ScoreHud.Instance.SpawnScorePoint(200, transform.position);
		Explode (transform.position, _explosionArea, _explosionPower, _explosionDamage, gameObject);

		base.Die(withEffect);
	}

	public static void Explode(Vector2 position, float explosionArea, float explosionPower, float explosionDamage, GameObject explosive) {

		Collider2D[] colliders = Physics2D.OverlapCircleAll (position, explosionArea);

		foreach (Collider2D coll in colliders) {

			if (coll.attachedRigidbody && coll.gameObject != explosive) {

				float distance = Vector2.Distance ((Vector2)coll.transform.position, position);
				Vector2 direction = ((Vector2)coll.transform.position - position).normalized;

				ABGameObject abGameObj = coll.gameObject.GetComponent<ABGameObject> ();
				if(abGameObj)
					coll.gameObject.GetComponent<ABGameObject> ().DealDamage (explosionDamage/distance);
				
				coll.attachedRigidbody.AddForce (direction * (explosionPower / (distance * 2f)), ForceMode2D.Impulse);

				Color c = Color.red;
				c.r /= distance;
				coll.GetComponent<SpriteRenderer> ().color = c;
			}
		}

	}
}
