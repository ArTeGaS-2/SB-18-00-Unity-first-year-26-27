using System;
using UnityEngine;

namespace LowPolyConstructorExpansion
{
    public class LPCSampledFx : MonoBehaviour
    {
        public float fps = 24f;
        public int frameCount = 49;
        public Track[] tracks;

        [Serializable]
        public class Track
        {
            public Transform target;
            public Renderer renderer;
            public Light light;
            public Vector3[] positions, scales;
            public Quaternion[] rotations;
            public float[] alphas, intensities;

            [NonSerialized] internal MaterialPropertyBlock block;
            [NonSerialized] internal Material[] materials;
            [NonSerialized] internal Color[] baseColors;
        }

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private void Update()
        {
            if (Application.isPlaying) Evaluate(Time.time);
        }

        public void Evaluate(float seconds)
        {
            if (tracks == null) return;
            int count = Mathf.Max(1, frameCount);
            float rate = fps > 0f && !float.IsInfinity(fps) ? fps : 24f;
            float frame = count > 1 && !float.IsNaN(seconds) && !float.IsInfinity(seconds)
                ? Mathf.Repeat(seconds * rate, count - 1) : 0f;
            int a = Mathf.FloorToInt(frame);
            int b = Mathf.Min(a + 1, count - 1);
            float t = frame - a;

            foreach (Track track in tracks)
            {
                if (track == null) continue;
                if (track.target != null)
                {
                    if (HasValues(track.positions)) track.target.localPosition = Vector3.Lerp(Sample(track.positions, a), Sample(track.positions, b), t);
                    if (HasValues(track.scales)) track.target.localScale = Vector3.Lerp(Sample(track.scales, a), Sample(track.scales, b), t);
                    if (HasValues(track.rotations)) track.target.localRotation = Quaternion.Slerp(Sample(track.rotations, a), Sample(track.rotations, b), t);
                }
                if (track.light != null && HasValues(track.intensities))
                    track.light.intensity = Mathf.Lerp(Sample(track.intensities, a), Sample(track.intensities, b), t);
                if (track.renderer == null || !HasValues(track.alphas)) continue;

                if (track.block == null) track.block = new MaterialPropertyBlock();
                if (track.materials == null || track.baseColors == null)
                {
                    track.materials = track.renderer.sharedMaterials;
                    track.baseColors = new Color[track.materials.Length];
                    for (int i = 0; i < track.materials.Length; i++)
                    {
                        Material material = track.materials[i];
                        track.baseColors[i] = material != null && material.HasProperty(BaseColorId)
                            ? material.GetColor(BaseColorId) : Color.white;
                    }
                }
                float alpha = Mathf.Lerp(Sample(track.alphas, a), Sample(track.alphas, b), t);
                for (int i = 0; i < track.materials.Length; i++)
                {
                    Material material = track.materials[i];
                    if (material == null || !material.HasProperty(BaseColorId)) continue;
                    track.renderer.GetPropertyBlock(track.block, i);
                    Color color = track.baseColors[i];
                    color.a = alpha;
                    track.block.SetColor(BaseColorId, color);
                    track.renderer.SetPropertyBlock(track.block, i);
                }
            }
        }

        private static bool HasValues<T>(T[] values) => values != null && values.Length != 0;
        private static T Sample<T>(T[] values, int index) => values[Mathf.Min(index, values.Length - 1)];
    }
}
