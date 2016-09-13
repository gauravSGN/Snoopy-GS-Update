using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
sealed public class BitmapFontRenderer : MonoBehaviour
{
    private static Color INACTIVE_GIZMO_COLOR = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    private static Color SELECT_GIZMO_COLOR = new Color(0.0f, 0.0f, 1.0f, 0.2f);

    [SerializeField]
    private BitmapFontDefinition font;

    [SerializeField]
    private Material material;

    [Multiline]
    [SerializeField]
    private string text;

    [SerializeField]
    private float textScale = 1.0f;

    private Mesh mesh;

    public void OnValidate()
    {
        if (enabled)
        {
            RebuildMesh();
        }
    }

    public void OnEnable()
    {
        OnValidate();
    }

    public void Update()
    {
        if (!EditorApplication.isPlaying && (mesh != null) && (material != null))
        {
            var matrix = transform.localToWorldMatrix;

            Graphics.DrawMesh(mesh, matrix, material, gameObject.layer);
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

    private void RebuildMesh()
    {
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();
        var length = text.Length;
        var offset = 0;
        float x = 0.0f;
        float y = 0.0f;

        while (offset < length)
        {
            var glyph = font.GetNextGlyph(text, offset);

            if (glyph == null)
            {
                Debug.LogWarning(string.Format("Missing glyph for '{0}' in bitmap font for '{1}'", text.Substring(offset, 1), name));
                return;
            }

            var index = vertices.Count;
            triangles.AddRange(new[] { index + 0, index + 1, index + 2, index + 1, index + 3, index + 2 });

            AddGlyphToMesh(vertices, uvs, glyph, x, y);

            x += glyph.advance;
            offset += glyph.sequence.Length;
        }

        mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            uv = uvs.ToArray(),
            triangles = triangles.ToArray(),
        };

        material.mainTexture = font.Texture;
    }

    private void DrawGizmos(Color color)
    {
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = color;
            var matrix = transform.localToWorldMatrix;

            Gizmos.matrix = matrix;
            Gizmos.DrawCube(mesh.bounds.center, mesh.bounds.size);
        }
    }

    private void AddGlyphToMesh(List<Vector3> vertices, List<Vector2> uvs, BitmapFontDefinition.Glyph glyph, float x, float y)
    {
        var textureWidth = font.Texture.width;
        var textureHeight = font.Texture.height;
        var position = glyph.position;
        var size = glyph.size;

        vertices.Add(new Vector3(x* textScale, y * textScale));
        vertices.Add(new Vector3((x + size.x) * textScale, y * textScale));
        vertices.Add(new Vector3(x * textScale, (y - size.y) * textScale));
        vertices.Add(new Vector3((x + size.x) * textScale, (y - size.y) * textScale));

        uvs.Add(new Vector2(position.x / textureWidth, 1.0f - position.y / textureHeight));
        uvs.Add(new Vector2((position.x + size.x) / textureWidth, 1.0f - position.y / textureHeight));
        uvs.Add(new Vector2(position.x / textureWidth, 1.0f - (position.y + size.y) / textureHeight));
        uvs.Add(new Vector2((position.x + size.x) / textureWidth, 1.0f - (position.y + size.y) / textureHeight));
    }
}
