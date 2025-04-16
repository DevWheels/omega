using UnityEditor;

[CustomEditor(typeof(SkillConfig))]
public class SkillConfigEditor : Editor
{
    private SerializedProperty nameProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty isPassiveProp;
    private SerializedProperty skillTypeProp;
    private SerializedProperty percentageBuffProp;
    private SerializedProperty damageProp;
    private SerializedProperty speedProp;
    private SerializedProperty cooldownProp;
    private SerializedProperty projectileLifetimeProp;
    private SerializedProperty projectilePrefabProp;
    private SerializedProperty iconProp;

    private void OnEnable()
    {
        nameProp = serializedObject.FindProperty("Name");
        descriptionProp = serializedObject.FindProperty("Description");
        isPassiveProp = serializedObject.FindProperty("IsPassive");
        skillTypeProp = serializedObject.FindProperty("SkillType");
        percentageBuffProp = serializedObject.FindProperty("PercentageBuff");
        damageProp = serializedObject.FindProperty("Damage");
        speedProp = serializedObject.FindProperty("ProjectileSpeed");
        cooldownProp = serializedObject.FindProperty("Cooldown");
        projectileLifetimeProp = serializedObject.FindProperty("ProjectileLifetime");
        projectilePrefabProp = serializedObject.FindProperty("ProjectilePrefab");
        iconProp = serializedObject.FindProperty("Icon");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(descriptionProp);
        EditorGUILayout.PropertyField(iconProp);
        

        EditorGUILayout.PropertyField(isPassiveProp);
        EditorGUILayout.PropertyField(skillTypeProp);


        if (isPassiveProp.boolValue)
        {
            EditorGUILayout.PropertyField(percentageBuffProp);
        }
        else
        {
            EditorGUILayout.PropertyField(damageProp);
            EditorGUILayout.PropertyField(speedProp);
            EditorGUILayout.PropertyField(cooldownProp);
            EditorGUILayout.PropertyField(projectileLifetimeProp);
            EditorGUILayout.PropertyField(projectilePrefabProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}