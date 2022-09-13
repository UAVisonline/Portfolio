using System;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    [SerializeField] private GeneratorArea generatorArea;
    [SerializeField] private GameObject arriveArea;
    private bool _change = false;
    private float _beforeHeight = 0f;
    [SerializeField] private Player player;

    private void Update()
    {
        if (!_change && Math.Abs(_beforeHeight - transform.position.y) < 0.1f && _beforeHeight != 0f)
        {
            player.init_position(transform.position);
            generatorArea.add_transform_y(transform.position.y / 6);
            arriveArea.transform.position += new Vector3(0, transform.position.y / 6, 0);
            _change = true;
        }

        _beforeHeight = transform.position.y;
    }
}
