using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LightSaber : NetworkBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (ViveAvatar.me && ViveAvatar.me.findControllerFromChild(other.gameObject) != null) return;

        // Show spark
        Vector3 collide_point = gameObject.GetComponent<Collider>().ClosestPointOnBounds(other.gameObject.transform.position);
        GameObject spark = Instantiate((GameObject)Resources.Load("spark_prefab"));
        spark.transform.position = collide_point;
        spark.GetComponent<ParticleSystem>().Play();
        Destroy(spark, 0.1f);

        ViveController controller; 
        if (ViveAvatar.me && (controller = ViveAvatar.me.findControllerFromChild(this.gameObject)))
        {
            controller.LongHaptic();
        }
    }
}
