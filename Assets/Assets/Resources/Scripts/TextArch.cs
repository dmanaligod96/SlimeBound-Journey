using UnityEngine;
using TMPro;

public class TextArchController : MonoBehaviour
{
    [Header("Arch Settings")]
    [SerializeField] private float archRadius = 5f;
    [SerializeField] private float angleSpread = 60f; // Total angle in degrees
    [SerializeField] private bool autoUpdateText = true;
    
    private TMP_Text tmpText;
    private TMP_TextInfo textInfo;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (autoUpdateText)
        {
            ArchText();
        }
    }

    public void ArchText()
    {
        if (tmpText == null) return;

        // Force update the mesh
        tmpText.ForceMeshUpdate();
        
        // Get text info
        textInfo = tmpText.textInfo;
        
        // Calculate center angle and angle per character
        float centerAngle = -angleSpread * 0.5f;
        float anglePerChar = angleSpread / Mathf.Max(1, textInfo.characterCount - 1);

        // Iterate through each character
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            // Get character info
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            
            // Calculate angle for this character
            float angle = centerAngle + (anglePerChar * i);
            float radian = angle * Mathf.Deg2Rad;

            // Calculate new position
            float x = Mathf.Sin(radian) * archRadius;
            float y = Mathf.Cos(radian) * archRadius - archRadius;

            // Get vertex indices
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            // Get vertices
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Calculate center of character
            Vector3 charCenter = (vertices[vertexIndex] + vertices[vertexIndex + 2]) * 0.5f;

            // Rotate and position vertices
            for (int j = 0; j < 4; j++)
            {
                Vector3 vertex = vertices[vertexIndex + j];
                Vector3 offset = vertex - charCenter;
                
                // Rotate offset
                float cos = Mathf.Cos(radian);
                float sin = Mathf.Sin(radian);
                float newX = offset.x * cos - offset.y * sin;
                float newY = offset.x * sin + offset.y * cos;
                
                // Apply new position
                vertices[vertexIndex + j] = new Vector3(
                    charCenter.x + newX + x,
                    charCenter.y + newY + y,
                    vertex.z
                );
            }
        }

        // Update the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    // Public methods to control arch parameters
    public void SetArchRadius(float radius)
    {
        archRadius = radius;
        if (autoUpdateText) ArchText();
    }

    public void SetAngleSpread(float angle)
    {
        angleSpread = angle;
        if (autoUpdateText) ArchText(); 
    }
}