using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RisingUI : MonoBehaviour
{
    public List<Text> texts;
    public List<Image> images;
    public List<TMP_Text> tmp_texts;

    public float speed = 3f;
    public AnimationCurve opacityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.1f, 1), new Keyframe(1, 0));
    public float timer = 0f;
    public float duration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > duration)
        {
            Destroy(this.gameObject);
            return;
        }
        this.transform.position += Vector3.up * Time.deltaTime * speed;

        float opactity = opacityCurve.Evaluate(timer / duration);
        foreach (Text t in texts)
        {
            Color c = t.color;
            c.a = opactity;
            t.color = c;
        }
        foreach (TMP_Text t in tmp_texts)
        {
            Color c = t.color;
            c.a = opactity;
            t.color = c;
        }
        foreach (Image i in images)
        {
            Color c = i.color;
            c.a = opactity;
            i.color = c;
        }

        timer += Time.deltaTime;
    }
}
