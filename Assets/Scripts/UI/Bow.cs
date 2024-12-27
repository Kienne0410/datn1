using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private AudioClip _attackSoundClip;
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator myAnimator;

    private void Update()
    {
        _weaponSpriteRenderer.sortingOrder = PlayerController.Instance.GetSpriteRenderer().sortingOrder + 1;
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
        SoundManager.Instance.PlaySoundEffect(_attackSoundClip);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
