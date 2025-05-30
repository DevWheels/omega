using UnityEditor;

[CustomEditor(typeof(SkillConfig))]
public class SkillConfigEditor : Editor {
    private SerializedProperty skillViewConfigNameProp;
    private SerializedProperty nameProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty isPassiveProp;
    private SerializedProperty skillTypeProp;
    private SerializedProperty percentageBuffProp;
    private SerializedProperty damageProp;
    private SerializedProperty speedProp;
    private SerializedProperty ManaCostProp;
    private SerializedProperty cooldownProp;
    private SerializedProperty projectileLifetimeProp;
    private SerializedProperty projectileTypeProp;

    private void OnEnable()
    {
        skillViewConfigNameProp = serializedObject.FindProperty("SkillViewConfigName");
        nameProp = serializedObject.FindProperty("Name");
        descriptionProp = serializedObject.FindProperty("Description");
        isPassiveProp = serializedObject.FindProperty("IsPassive");
        skillTypeProp = serializedObject.FindProperty("SkillType");
        percentageBuffProp = serializedObject.FindProperty("PercentageBuff");
        damageProp = serializedObject.FindProperty("Damage");
        ManaCostProp = serializedObject.FindProperty("ManaCost");
        speedProp = serializedObject.FindProperty("ProjectileSpeed");
        cooldownProp = serializedObject.FindProperty("Cooldown");
        projectileLifetimeProp = serializedObject.FindProperty("ProjectileLifetime");
        projectileTypeProp = serializedObject.FindProperty("ProjectileType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(skillViewConfigNameProp);
        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(descriptionProp);
        

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
            EditorGUILayout.PropertyField(ManaCostProp);
            EditorGUILayout.PropertyField(cooldownProp);
            EditorGUILayout.PropertyField(projectileLifetimeProp);
            EditorGUILayout.PropertyField(projectileTypeProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}