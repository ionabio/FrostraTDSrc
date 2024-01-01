using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject towerPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        if (TowerPick.instance.picked)
        {
            // build in mnouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //check if mouse collides with BG_Path
            mousePosition.z = 1;
            LayerMask layerMask = LayerMask.GetMask("Water");
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                return;
            }
            layerMask = LayerMask.GetMask("Tower");
            hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Debug.Log("TowerPlacer:OnMouseUp hit tower");
                return;
            }
            Instantiate(towerPrefab, mousePosition, Quaternion.identity);
            Debug.Log("TowerPlacer placed at" + mousePosition);
            Level.instance.RemoveCoins(100);
            TowerPick.instance.UnPick();
        }
    }
}
