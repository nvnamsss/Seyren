using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Default VFX handler for the <c>"explosion"</c> entity key.
    /// Renders a brief orange sphere at the explosion centre as a placeholder visual.
    /// Registered automatically by <see cref="ExplosionModifier"/> via
    /// <see cref="IGraphAbilityVFXProvider.GetVFX"/>.
    /// </summary>
    public class ExplosionDefaultVFX : IGraphAbilityVFX
    {
        private const string ExplosionKey = "explosion";

        public GameObject CreateVisual(string entityKey, AbilityGraphContext ctx)
        {
            if (entityKey != ExplosionKey) return null;
            return BuildSphere(ctx);
        }

        public bool OnEntitySpawned(string entityKey, object entity, AbilityGraphContext ctx)
        {
            if (entityKey != ExplosionKey) return false;
            GameObject sphere = BuildSphere(ctx);
            Object.Destroy(sphere, 0.5f);
            return true;
        }

        public bool OnEntityCompleted(string entityKey, object entity, AbilityGraphContext ctx)
        {
            // Sphere auto-destroys via the Destroy(delay) call in OnEntitySpawned.
            return entityKey == ExplosionKey;
        }

        private static GameObject BuildSphere(AbilityGraphContext ctx)
        {
            Vector3 center = ctx.location.GetValueOrDefault(ctx.caster.Location);

            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position   = center;
            go.transform.localScale = Vector3.one * 3f;

            var col = go.GetComponent<Collider>();
            if (col != null) Object.Destroy(col);

            var mr = go.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(1f, 0.5f, 0.1f, 0.6f);
                mr.material = mat;
            }

            return go;
        }
    }
}
