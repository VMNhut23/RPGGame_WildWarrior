using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
	[Header("Clone info")]
    [SerializeField] private GameObject clonePrefabs;
	[SerializeField] private float cloneDuration;
	[Space]
	[SerializeField] private bool canAttack;
    public void CreateClone(Transform _cloneTransform, Vector3 _offset)
	{
		GameObject newClone = Instantiate(clonePrefabs);
		newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform));
	}
}
