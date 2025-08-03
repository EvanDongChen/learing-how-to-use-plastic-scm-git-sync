using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// This attribute lets you create an instance of this database from the Assets menu.
[CreateAssetMenu(fileName = "AttributeDatabase", menuName = "Game/Attribute Database")]
public class AttributeDatabase : ScriptableObject
{
    [System.Serializable]
    public class AttributeIconMapping
    {
        public string attributeName;
        public Sprite icon; // A direct reference to a Sprite
    }

    public List<AttributeIconMapping> attributeIcons;

    public Sprite GetIconByName(string name)
    {
        AttributeIconMapping mapping = attributeIcons.FirstOrDefault(m => m.attributeName == name);

        if (mapping != null)
        {
            return mapping.icon;
        }

        Debug.LogWarning($"No icon found in AttributeDatabase for name: {name}");
        return null;
    }
}