using System;
using Kandooz.KVR;
using UnityEngine;

public class PlayerHandObject : MonoBehaviour
{
    private Vector3 _oldPosition;
    private Vector3 _curPosition;
    private Vector3 _velocity;
    private const int _noteLayer = 6;
    private ReworkNote _reworkNote;

    //[SerializeField] private HandAnimationController handAnimationController;
    [SerializeField] private float boundVelocity = 0.3f;

    private void Start()
    {
        _oldPosition = transform.position;
        //handAnimationController.StaticPose = true;
        //handAnimationController.Pose = 1;
        //handAnimationController.Pose = 0;
    }

    private Vector3 GetVelocity()
    {
        _curPosition = transform.position;
        var dis = _curPosition - _oldPosition;
        var distance = new Vector3(Math.Sign(dis.x) * (float)Math.Sqrt(dis.x * dis.x),
            Math.Sign(dis.y) * (float)Math.Sqrt(Math.Pow(dis.y, 2)),
            Math.Sign(dis.z) * (float)Math.Sqrt(Math.Pow(dis.z, 2)));
        _oldPosition = _curPosition;
        return distance / Time.deltaTime;
    }

    /*
    private void Update()
    {
        Debug.Log(this.gameObject.name + "   " + this.transform.rotation.eulerAngles);
    }
    */

    private void FixedUpdate() // Update를 고침
    {
        _velocity = GetVelocity();
    }

    private bool NeedJudge()
    {
        if (_reworkNote.get_type() == Note3D_type.normal)
            return _velocity[2] >= 0.1f; // original 0.3f
        else if (_reworkNote.get_type() == Note3D_type.slide)
            return _reworkNote.get_direction_type() switch
            {
                Direction_type.up => _velocity[1] >= boundVelocity,
                Direction_type.down => _velocity[1] <= -boundVelocity,
                Direction_type.left => _velocity[0] <= -boundVelocity,
                Direction_type.right => _velocity[0] >= boundVelocity,
                Direction_type.left_down => _velocity[0] <= -boundVelocity && _velocity[1] <= -boundVelocity,
                Direction_type.right_down => _velocity[0] >= boundVelocity && _velocity[1] <= -boundVelocity,
                Direction_type.left_up => _velocity[0] <= -boundVelocity && _velocity[1] >= boundVelocity,
                Direction_type.right_up => _velocity[0] >= boundVelocity && _velocity[1] >= boundVelocity,
                _ => false
            };

        return false;
    }

    private void JudgeNote(Collider other)
    {
        //if ((other.gameObject.layer & _noteLayer) == 0) return;
        if ((_reworkNote = other.gameObject.GetComponent<ReworkNote>()) == null) return;
        if (!NeedJudge()) return;

        if (_reworkNote.get_type() == Note3D_type.normal)
        {
            if(_velocity[2]>=0.3f)
            {
                _reworkNote.note_interaction_correct(Judgement_type.perfect);
            }
            else
            {
                _reworkNote.note_interaction_correct(Judgement_type.good);
            }
        }
        else
        {
            _reworkNote.note_interaction_correct(Judgement_type.perfect);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        JudgeNote(other);
    }

    private void OnTriggerStay(Collider other) // 굳이 Stay까지 써야하는건가???
    {
        JudgeNote(other);
    }

    private void OnTriggerExit(Collider other)
    {
        JudgeNote(other);
    }
}
