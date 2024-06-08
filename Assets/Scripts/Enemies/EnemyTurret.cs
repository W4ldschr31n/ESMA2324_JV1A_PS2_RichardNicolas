using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPointRight;
    [SerializeField] private Transform bulletSpawnPointLeft;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TextMeshProUGUI ammoText;
    public float delayBeforeFirstShot;
    public float delayBetweenShots;
    public int maxAmmo;
    private int currentAmmo;
    public bool shootToTheRight;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        TimerManager.onTimerStarted.AddListener(OnTimerStarted);
        UpdateAmmoDisplay();
    }

    private void OnDisable()
    {
        TimerManager.onTimerStarted.RemoveListener(OnTimerStarted);
    }

    private void UpdateAmmoDisplay()
    {
        ammoText.text = $"{currentAmmo}/ {maxAmmo}";
    }

    private void OnTimerStarted()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoDisplay();
        // Make sure we have no overlapping shots
        CancelInvoke();
        // Clear all bullets from both shooting points
        foreach (Bullet bullet in bulletSpawnPointRight.GetComponentsInChildren<Bullet>())
        {
            bullet.SelfDestroy();
        }
        foreach (Bullet bullet in bulletSpawnPointLeft.GetComponentsInChildren<Bullet>())
        {
            bullet.SelfDestroy();
        }
        Invoke(nameof(ShootRepeating), delayBeforeFirstShot);
    }

    private void ShootRepeating()
    {
        if(currentAmmo > 0)
        {
            ShootBullet();
            Invoke(nameof(ShootRepeating), delayBetweenShots);
        }
    }

    private void ShootBullet()
    {
        currentAmmo--;
        UpdateAmmoDisplay();
        animator.SetTrigger("Shoot");
        if (shootToTheRight)
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPointRight);
            // Make the bullet look to the right
            newBullet.transform.right = transform.right;
        }
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPointLeft);
            // Make the bullet look to the left
            newBullet.transform.right = -transform.right;
        }
    }
}
