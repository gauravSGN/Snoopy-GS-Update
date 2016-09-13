using System;
using UnityEngine;
using System.Collections.Generic;

sealed public class BitmapFontDefinition : ScriptableObject
{
    [Serializable]
    sealed public class Glyph
    {
        public string sequence;
        public Vector2 position;
        public Vector2 size;
        public Vector2 offset;
        public float advance;
    }

    sealed public class GlyphNode
    {
        public Glyph glyph;
        public Dictionary<char, GlyphNode> next;
    }

    sealed private class FontData
    {
        public List<Glyph> glyphs;
        public float lineHeight;
    }

    [SerializeField]
    private TextAsset glyphData;

    [SerializeField]
    private Texture2D texture;

    private GlyphNode root;

    public Texture2D Texture { get { return texture; } }

    public Glyph GetNextGlyph(string input, int offset)
    {
        root = root ?? BuildTree();

        var node = root;
        for (int index = offset, length = input.Length; index < length; index++)
        {
            if (node.next == null)
            {
                break;
            }

            var letter = input[index];
            if (node.next.ContainsKey(letter))
            {
                node = node.next[letter];
            }
        }

        return node.glyph;
    }

    private GlyphNode BuildTree()
    {
        var fontData = JsonUtility.FromJson<FontData>(glyphData.text);
        var result = new GlyphNode();

        if (fontData.glyphs == null)
        {
            Debug.LogWarning("Glyphs are still null");
            return result;
        }

        foreach (var glyph in fontData.glyphs)
        {
            AddGlyphToTree(result, glyph);
        }

        return result;
    }

    private void AddGlyphToTree(GlyphNode node, Glyph glyph)
    {
        foreach (var letter in glyph.sequence)
        {
            node = GetNextNode(node, letter);
        }

        node.glyph = glyph;
    }

    private GlyphNode GetNextNode(GlyphNode currentNode, char key)
    {
        currentNode.next = currentNode.next ?? new Dictionary<char, GlyphNode>();

        if (!currentNode.next.ContainsKey(key))
        {
            currentNode.next.Add(key, new GlyphNode());
        }

        return currentNode.next[key];
    }
}
