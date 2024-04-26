using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and fire effect", menuName = "Data/Item effect/Ice and fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefabs;
	[SerializeField] private float xVelocity;

	public override void ExecuteEffect(Transform _respawnPosition)
	{
		Player player = PlayerManager.instance.player;

		bool thirdAttack = player.primaryAttack.comboCounter == 2;

		if (thirdAttack)
		{
			GameObject newIceAndFire = Instantiate(iceAndFirePrefabs, _respawnPosition.position, player.transform.rotation);
			newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity* player.facingDir, 0);
			Destroy(newIceAndFire, 5f);
		}

	}
}
