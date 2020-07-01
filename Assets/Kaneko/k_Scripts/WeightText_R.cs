using UnityEngine;
using UnityEngine.UI;

public class WeightText_R : MonoBehaviour
{
    private Text targetText;
    public GameObject Kyojyu;
    private float distance_UI;

    float weight;

    void Start()
    {
        this.targetText = this.GetComponent<Text>();

    }

    void Update()
    {
        weight = Kyojyu.GetComponent<KyojyuManager>().R_weight;
        this.targetText.text = "R：" + weight;
    }
}
