using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {

    public GameObject healthPointPrefab;
    public float borderSize;

    private Health health;
    private Health slow;
    private float slowTimeFromHealth;

    private RectTransform slowBar;
    private Transform holder;
    private List<RectTransform> healthPoints;

    private float width;
    private float fullHealthSlowWidth;
    private float fullHealthWidth;

	void Start () {
        PlayerMover p = FindObjectOfType<PlayerMover>();
        health = p.health;
        slow = p.slowTime;
        slowTimeFromHealth = p.slowtimeFromHealth;

        slowBar = transform.Find("Slow").GetComponent<RectTransform>();
        holder = transform.Find("HealthPoints");
        healthPoints = new List<RectTransform>();

        width = GetComponent<RectTransform>().rect.width;
        float slowMaxFromHealth = health.Max() * slowTimeFromHealth;
        fullHealthSlowWidth = slow.Max() / (slowMaxFromHealth + slow.Max()) * width;
        fullHealthWidth = width - fullHealthSlowWidth;
        Update();
	}
	
	void Update () {
        SetSlow();
        SetHealth();
	}

    private void SetSlow()
    {
        float currentHealthWidth = fullHealthWidth - health.Ratio() * fullHealthWidth;
        float currentMaxWidth = fullHealthSlowWidth + currentHealthWidth;
        float currentWidth = currentMaxWidth * slow.Ratio() - borderSize * 2;
        float currentLoc = currentWidth / 2 + borderSize + currentMaxWidth - (currentWidth + 2 * borderSize);

        slowBar.sizeDelta = new Vector2(currentWidth, slowBar.sizeDelta.y);
        slowBar.anchoredPosition = new Vector3(currentLoc, 0);
    }

    private void SetHealth()
    {
        int h = (int) health.Amount();
        if (h == healthPoints.Count)
        {
            return;
        }

        if(h < healthPoints.Count)
        {
            RectTransform point = healthPoints[0];
            healthPoints.Remove(point);
            Destroy(point.gameObject);
        }
        else
        {
            while(h > healthPoints.Count)
            {
                RectTransform trs = Instantiate(healthPointPrefab, holder).GetComponent<RectTransform>();
                trs.sizeDelta = new Vector2(fullHealthWidth / health.Max() - borderSize * 2, trs.sizeDelta.y);
                trs.anchoredPosition = new Vector3(width/2 - (trs.sizeDelta.x + borderSize * 2)/2 - (trs.sizeDelta.x + borderSize * 2) * healthPoints.Count, 0);
                healthPoints.Add(trs);
            }
        }
    }
}
