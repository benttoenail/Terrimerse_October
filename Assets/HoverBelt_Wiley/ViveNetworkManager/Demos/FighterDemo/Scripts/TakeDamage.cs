using UnityEngine;
using System.Collections;

public class TakeDamage : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DoesDamage>() == null) return;

        if (ViveAvatar.me && ViveAvatar.me.findControllerFromChild(gameObject))
        {

            if (ViveAvatar.me.findControllerFromChild(other.gameObject) != null) return;
            var combat = ViveAvatar.me.GetComponent<Combat>();
            combat.CmdTakeDamage(10);

            ViveAvatar.me.controllers[ViveAvatar.LEFT].LongHaptic();
            ViveAvatar.me.controllers[ViveAvatar.RIGHT].LongHaptic();


        }

    }
}
