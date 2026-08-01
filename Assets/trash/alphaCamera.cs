using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphaCamera : MonoBehaviour
{
    public Vector2 basePos;

    public void setPosition(Vector2 targetPos)
    {
        basePos = targetPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.localPosition;
        pos.x = basePos.x;
        pos.y = basePos.y + 2;
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.08f);
    }
}
