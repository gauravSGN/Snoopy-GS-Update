using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
sealed public class BitmapFontRenderer : MonoBehaviour
{
    [SerializeField]
    private BitmapFontDefinition font;

    [Multiline]
    [SerializeField]
    private string text;

    [SerializeField]
    private float textScale = 1.0f;

    [SerializeField]
    private Color color = Color.white;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public string Text
    {
        get { return text; }
        set
        {
            text = value;
            RebuildMesh();
        }
    }

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
        var mesh = meshFilter.sharedMesh;
        mesh.colors = mesh.vertices.Select(v => color).ToArray();
    }

    private void RebuildMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        var lines = text.Split(new[] { '\n' });
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();
        var y = lines.Length * font.LineHeight / 2.0f;

        foreach (var line in lines)
        {
            RebuildLine(line, vertices, uvs, triangles, y);
            y -= font.LineHeight;
        }

        meshFilter.sharedMesh = new Mesh
        {
            vertices = vertices.ToArray(),
            uv = uvs.ToArray(),
            triangles = triangles.ToArray(),
            colors = vertices.Select(v => Color.white).ToArray(),
        };

        meshRenderer.sharedMaterial = font.Material;
        meshRenderer.sharedMaterial.mainTexture = font.Texture;
    }

    private void RebuildLine(string line, List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, float y)
    {
        var glyphs = new List<BitmapFontDefinition.Glyph>();
        var length = line.Length;
        var offset = 0;
        var width = 0.0f;

        while (offset < length)
        {
            var glyph = font.GetNextGlyph(line, offset);

            if (glyph == null)
            {
                Debug.LogWarning(string.Format("Missing glyph for '{0}' in bitmap font for '{1}'", line.Substring(offset, 1), name));
                return;
            }

            glyphs.Add(glyph);
            offset += glyph.sequence.Length;
            width += glyph.advance;
        }

        float x = width / -2.0f;

        foreach (var glyph in glyphs)
        {
            AddGlyphToMesh(vertices, uvs, triangles, glyph, x, y);

            x += glyph.advance;
            offset += glyph.sequence.Length;
        }
    }

    private void AddGlyphToMesh(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles,
                                BitmapFontDefinition.Glyph glyph, float x, float y)
    {
        var textureWidth = font.Texture.width;
        var textureHeight = font.Texture.height;
        var position = glyph.position;
        var offset = glyph.offset;
        var size = glyph.size;

        var index = vertices.Count;
        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index + 3);
        triangles.Add(index + 2);

        vertices.Add(new Vector3((x + offset.x) * textScale, (y - offset.y) * textScale));
        vertices.Add(new Vector3((x + offset.x + size.x) * textScale, (y - offset.y) * textScale));
        vertices.Add(new Vector3(x * textScale, (y - offset.y - size.y) * textScale));
        vertices.Add(new Vector3((x + size.x) * textScale, (y - offset.y - size.y) * textScale));

        uvs.Add(new Vector2(position.x / textureWidth, 1.0f - position.y / textureHeight));
        uvs.Add(new Vector2((position.x + size.x) / textureWidth, 1.0f - position.y / textureHeight));
        uvs.Add(new Vector2(position.x / textureWidth, 1.0f - (position.y + size.y) / textureHeight));
        uvs.Add(new Vector2((position.x + size.x) / textureWidth, 1.0f - (position.y + size.y) / textureHeight));
    }
}
