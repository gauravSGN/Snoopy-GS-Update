using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

/*
 * Provides an instantiator for 2D prefabs that renders them in scene view without creating an instance.  This allows
 * the prefab to be freely modified and all PrefabInstance2D usages will be updated appropriately.
 *
 * For reference, see:
 * http://framebunker.com/blog/poor-mans-nested-prefabs/
 * http://gamedev.stackexchange.com/questions/75016/how-can-i-manually-draw-a-sprite-in-unity
 */
[ExecuteInEditMode]
public class PrefabInstance2D : MonoBehaviour
{
    private static Color INACTIVE_GIZMO_COLOR = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    private static Color SELECT_GIZMO_COLOR = new Color(0.0f, 0.0f, 1.0f, 0.2f);

    [SerializeField]
    private GameObject prefab;

#if UNITY_EDITOR
    private struct Visual
    {
        public Mesh mesh;
        public Matrix4x4 matrix;
        public List<Material> materials;
        public MaterialPropertyBlock propertyBlock;
    }

    [NonSerialized]
    private readonly List<Visual> visuals = new List<Visual>();

    [PostProcessScene(-2)]
    public static void OnPostprocessScene()
    {
        foreach (var instance in UnityEngine.Object.FindObjectsOfType<PrefabInstance2D>())
        {
            BakeInstance(instance);
        }
    }

    public void OnValidate()
    {
        visuals.Clear();

        if (enabled)
        {
            Rebuild(prefab, Matrix4x4.identity);
        }
    }

    public void OnEnable()
    {
        OnValidate();
    }

    public void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            var matrix = transform.localToWorldMatrix;

            foreach (var visual in visuals)
            {
                var finalMatrix = matrix * visual.matrix;

                for (int index = 0, count = visual.materials.Count; index < count; index++)
                {
                    Graphics.DrawMesh(
                        visual.mesh,
                        finalMatrix,
                        visual.materials[index],
                        gameObject.layer,
                        null,
                        index,
                        visual.propertyBlock
                    );
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        DrawGizmos(INACTIVE_GIZMO_COLOR);
    }

    public void OnDrawGizmosSelected()
    {
        DrawGizmos(SELECT_GIZMO_COLOR);
    }

    private static void BakeInstance(PrefabInstance2D instance)
    {
        if (instance.enabled && (instance.prefab != null))
        {
            instance.enabled = false;

            var gameObject = PrefabUtility.InstantiatePrefab(instance.prefab) as GameObject;
            gameObject.transform.SetParent(instance.transform, false);

            instance.prefab = null;

            foreach (var child in gameObject.GetComponentsInChildren<PrefabInstance2D>())
            {
                BakeInstance(child);
            }
        }
    }

    private void Rebuild(GameObject source, Matrix4x4 transformation)
    {
        if (source != null)
        {
            var baseMatrix = transformation * Matrix4x4.TRS(
                -source.transform.position,
                Quaternion.identity,
                Vector3.one
            );

            foreach (SpriteRenderer renderer in source.GetComponentsInChildren(typeof(SpriteRenderer), true))
            {
                if (renderer.sprite != null)
                {
                    var propertyBlock = new MaterialPropertyBlock();
                    renderer.GetPropertyBlock(propertyBlock);

                    visuals.Add(new Visual
                    {
                        mesh = CreateMeshForSprite(renderer.sprite),
                        matrix = baseMatrix * renderer.transform.localToWorldMatrix,
                        materials = renderer.sharedMaterials.ToList(),
                        propertyBlock = propertyBlock,
                    });
                }
            }

            foreach (PrefabInstance2D instance in source.GetComponentsInChildren(typeof(PrefabInstance2D), true))
            {
                if (instance.enabled && instance.gameObject.activeSelf)
                {
                    Rebuild(instance.prefab, baseMatrix * instance.transform.localToWorldMatrix);
                }
            }
        }
    }

    private void DrawGizmos(Color color)
    {
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = color;
            var matrix = transform.localToWorldMatrix;

            foreach (var visual in visuals)
            {
                Gizmos.matrix = matrix * visual.matrix;
                Gizmos.DrawCube(visual.mesh.bounds.center, visual.mesh.bounds.size);
            }
        }
    }

    private Mesh CreateMeshForSprite(Sprite sprite)
    {
        var mesh = new Mesh();
        var bounds = sprite.bounds;
        var uScale = 1.0f / sprite.texture.width;
        var vScale = 1.0f / sprite.texture.height;

        mesh.vertices = new[]
        {
            new Vector3(bounds.min.x, bounds.min.y),
            new Vector3(bounds.min.x, bounds.max.y),
            new Vector3(bounds.max.x, bounds.max.y),
            new Vector3(bounds.max.x, bounds.min.y),
        };

        mesh.triangles = new[] { 0, 1, 2, 2, 3, 0 };

        mesh.uv = new[]
        {
            new Vector2(uScale * sprite.rect.x, vScale * sprite.rect.y),
            new Vector2(uScale * sprite.rect.x, vScale * sprite.rect.yMax),
            new Vector2(uScale * sprite.rect.xMax, vScale * sprite.rect.yMax),
            new Vector2(uScale * sprite.rect.xMax, vScale * sprite.rect.y),
        };

        mesh.colors = mesh.vertices.Select(v => Color.white).ToArray();

        return mesh;
    }
#endif
}
