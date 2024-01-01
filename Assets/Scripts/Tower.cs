using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rangeCircle;

    [SerializeField] private Material selectedMat;
    [SerializeField] private Material normMat;
    [SerializeField] private Material frostMat;
    [SerializeField] private Material fireMat;
    [SerializeField] private Transform firePoint;


    // Start is called before the first frame update
    [Header("Attributes")]
    [SerializeField] private float range = 2f;
    [SerializeField] private float fireRate = 10f;

    private Transform target;
    private float fireTimer = 0f;

    private enum TowerType
    {
        Normal,
        Frost,
        Fire
    }

    TowerType towerType = TowerType.Normal;


    void Start()
    {
        rangeCircle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }
        else
        {
            if (!CheckTargetInRange())
            {
                target = null;
            }
            else
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= 1 / fireRate)
                {
                    Shoot();
                    fireTimer = 0f;
                }  //todo shoot
            }
        }
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(firePoint.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        //range multiplied by the size of the tower

        if (closestEnemy != null && minDistance <= range)
        {
            target = closestEnemy.transform;
        }
    }

    private void Shoot()
    {
        if (towerType == TowerType.Normal || towerType == TowerType.Fire)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            //set sorting order of bullet to be higher than tower
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(target);
            if (towerType == TowerType.Fire)
            {
                bulletScript.SetFire();
            }
        }
        else if (towerType == TowerType.Frost)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetFrost();
            bullet.GetComponent<Bullet>().SetTarget(target);
        }
        

    }

    private bool selected = false;
    private int segments = 50; // Number of segments to create the circle
    [SerializeField] private float lineWidth = 0.05f; // Width of the circle line
    [SerializeField] private LineRenderer lineRenderer;


    public bool IsSelected()
    {
        return selected;
    }

    public void OnMouseDown()
    {


        selected = !selected;
        Debug.Log("Selected: " + selected);
        if (selected)
        {
            Level.instance.DeselectAllTowers();
            selected = true;
            //set the radius of the circle
            rangeCircle.transform.localScale = new Vector3(range, range, 1f);
            rangeCircle.SetActive(true);
            GetComponent<SpriteRenderer>().material = selectedMat;
        }
        else
        {
            rangeCircle.SetActive(false);
            ResetTowerColor();
        }
        Level.instance.ShowHideUpgradePanel(selected);
    }

    private bool CheckTargetInRange()
    {
        if (target == null)
        {
            return false;
        }
        else
        {
            return Vector2.Distance(transform.position, target.position) <= range;
        }
    }

    public void DeSelect()
    {
        selected = false;
        rangeCircle.SetActive(false);
        ResetTowerColor();
    }

    private void ResetTowerColor()
    {
        if (towerType == TowerType.Normal)
        {
            GetComponent<SpriteRenderer>().material = normMat;
        }
        else if (towerType == TowerType.Frost)
        {
            GetComponent<SpriteRenderer>().material = frostMat;
        }
        else if (towerType == TowerType.Fire)
        {
            GetComponent<SpriteRenderer>().material = fireMat;
        }
    }

    public void SetTowerType(int type)
    {
        if (type == 0)
        {
            towerType = TowerType.Normal;
        }
        else if (type == 1)
        {
            towerType = TowerType.Frost;

        }
        else if (type == 2)
        {
            fireRate = fireRate * 5.0f;
            towerType = TowerType.Fire;
            

        }
        ResetTowerColor();
    }

}

