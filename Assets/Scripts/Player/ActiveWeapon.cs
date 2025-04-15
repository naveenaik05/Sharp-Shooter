using UnityEngine;
using StarterAssets;
using Cinemachine;
using TMPro;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO startingWeapon;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;

    WeaponSO currentWeaponSO;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    FirstPersonController firstPersonController;
    Weapon currentWeapon;

    float timeScinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;
    int currentAmmo;
    const string SHOOT_STRING = "Shoot";
    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        firstPersonController = GetComponentInParent<FirstPersonController>();
        defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        defaultRotationSpeed = firstPersonController.RotationSpeed;
    }

    void Start()
    {
        SwitchWeapon(startingWeapon);
        AdjustAmmo(currentWeaponSO.MagazineSize);
    }
    void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    public void AdjustAmmo(int amount)
    {
        currentAmmo += amount;

        if (currentAmmo > currentWeaponSO.MagazineSize)
        {
            currentAmmo = currentWeaponSO.MagazineSize;
        }
        ammoText.text = currentAmmo.ToString("D2");
    }

    void HandleShoot()
    {
        timeScinceLastShot += Time.deltaTime;

        if (!starterAssetsInputs.shoot) return;
        if (timeScinceLastShot >= currentWeaponSO.fireRate && currentAmmo >0)
        {
            currentWeapon.Shoot(currentWeaponSO);
            animator.Play(SHOOT_STRING, 0, 0f);
            timeScinceLastShot = 0f;
            AdjustAmmo(-1);
        }
        if (!currentWeaponSO.isAutomatic)
        {
            starterAssetsInputs.ShootInput(false);
        }
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponSO.weaponPrefab, transform).GetComponent<Weapon>();
        currentWeapon = newWeapon;
        this.currentWeaponSO = weaponSO;
        AdjustAmmo(currentWeaponSO.MagazineSize);

    }

    void HandleZoom()
    {
        if (!currentWeaponSO.CanZoom) return;

        if (starterAssetsInputs.zoom)
        {
            zoomVignette.SetActive(true);
            playerFollowCamera.m_Lens.FieldOfView = currentWeaponSO.ZoomAmount;
            weaponCamera.fieldOfView = currentWeaponSO.ZoomAmount;
            firstPersonController.ChangeRotationSpeed(currentWeaponSO.ZoomRotationSpeed);
        }
        else
        {
            zoomVignette.SetActive(false);
            playerFollowCamera.m_Lens.FieldOfView = defaultFOV;
            weaponCamera.fieldOfView = defaultFOV;
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }
}
