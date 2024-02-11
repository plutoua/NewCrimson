using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Activeable))]
public class ActivableDoors : MonoBehaviour, IActivable
{
    private bool _closed = true;
    private bool _opening = false;
    private bool _closeing = false;
    private float _last_rotation = 0f;

    private GameObject _circle;

    private void Start()
    {
        _last_rotation = 0f;
        _closed = true;
        _opening = false;
        _closeing = false;
        _circle = transform.Find("Circle").gameObject;
        GetComponent<Activeable>().SetActiveable(this);
    }

    private void Update()
    {
        if (_opening)
        {
            
            _last_rotation += Time.deltaTime * 100f;
            if (_last_rotation > 90f)
            {
                _last_rotation = 90f;
                _opening = false;
                _closed = false;
                
            }
            _circle.transform.rotation = Quaternion.Euler(0, 0, _last_rotation);

        }

        if (_closeing)
        {
            _last_rotation -= Time.deltaTime * 100f;
            if (_last_rotation < 0f)
            {
                _last_rotation = 0f;
                _closeing = false;
                _closed = true;
                
            }
            _circle.transform.rotation = Quaternion.Euler(0, 0, _last_rotation);

        }

        
    }

    public void Activate()
    {
        if (_closed){
            _closeing = false;
            _opening = true;
            
        }
        else
        {
            _opening = false;
            _closeing = true;
            
        }

        

    }

}
