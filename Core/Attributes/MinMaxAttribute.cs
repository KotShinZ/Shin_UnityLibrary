using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct MinMax
{
	public float min;
	public float max;

	public float randomValue { get { return Random.Range(min, max); } }

	public MinMax(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public float Clamp(float value)
	{
		return Mathf.Clamp(value, min, max);
	}

	public bool IsInRange(float value)
    {
		return (value <= max && value >= min);
    }
	
	public static MinMax OneMinMax()
    {
		var m = new MinMax(1, 1);
		return m;
    }
    public static MinMax MinMaxNum(float max  ,float min)
    {
        var m = new MinMax(max, min);
        return m;
    }
}

[System.Serializable]
public struct MinMaxInt
{
	public int min;
	public int max;

    public int randomValue { get { return Random.Range(min, max); } }
	public float randomValueInt { get { return Random.Range(min, max); } }

	public MinMaxInt(int min, int max)
	{
		this.min = min;
		this.max = max;
	}

	public int Clamp(int value)
	{
		return Mathf.Clamp(value, min, max);
	}

	public bool IsInRange(int value)
	{
		return (value <= max && value >= min);
	}

	public static MinMaxInt OneMinMax()
	{
		var m = new MinMaxInt(1, 1);
		return m;
	}
}

public class MinMaxRangeAttribute : PropertyAttribute
{
	public float minLimit;
	public float maxLimit;

	public MinMaxRangeAttribute(float minLimit, float maxLimit)
	{

		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}

public class MinMaxIntRangeAttribute : PropertyAttribute
{

	public int minLimit;
	public int maxLimit;

	public MinMaxIntRangeAttribute(int minLimit, int maxLimit)
	{

		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
{

	const int numWidth = 50;
	const int padding = 5;

	MinMaxRangeAttribute minMaxAttribute { get { return (MinMaxRangeAttribute)attribute; } }

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{

		EditorGUI.BeginProperty(position, label, prop);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		Rect minRect = new Rect(position.x, position.y, numWidth, position.height);
		Rect sliderRect = new Rect(minRect.x + minRect.width + padding, position.y, position.width - numWidth * 2 - padding * 2, position.height);
		Rect maxRect = new Rect(sliderRect.x + sliderRect.width + padding, position.y, numWidth, position.height);

		SerializedProperty minProp = prop.FindPropertyRelative("min");
		SerializedProperty maxProp = prop.FindPropertyRelative("max");

		float min = minProp.floatValue;
		float max = maxProp.floatValue;
		float minLimit = minMaxAttribute.minLimit;
		float maxLimit = minMaxAttribute.maxLimit;

		min = Mathf.Clamp(EditorGUI.FloatField(minRect, min), minLimit, max);
		max = Mathf.Clamp(EditorGUI.FloatField(maxRect, max), min, maxLimit);
		EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, minLimit, maxLimit);

		minProp.floatValue = min;
		maxProp.floatValue = max;

		EditorGUI.EndProperty();
	}
}

[CustomPropertyDrawer(typeof(MinMaxIntRangeAttribute))]
public class MinMaxIntRangeDrawer : PropertyDrawer
{

    const int numWidth = 50;
    const int padding = 5;

    MinMaxIntRangeAttribute minMaxAttribute { get { return (MinMaxIntRangeAttribute)attribute; } }

    private static readonly GUIContent MIN_LABEL = new GUIContent("Min");
    private static readonly GUIContent MAX_LABEL = new GUIContent("Max");

    public override void OnGUI(Rect i_position, SerializedProperty i_property, GUIContent i_label)
    {
        var minProperty = i_property.FindPropertyRelative("min");
        var maxProperty = i_property.FindPropertyRelative("max");

        ApplyValue(minProperty, maxProperty);

        i_label = EditorGUI.BeginProperty(i_position, i_label, i_property);

        // プロパティの名前部分を描画。
        Rect contentPosition = EditorGUI.PrefixLabel(i_position, i_label);

        // MinとMaxの2つのプロパティを表示するので、残りのフィールドを半分こ。
        contentPosition.width /= 2.0f;

        // Rangeを配列でもっている際は、その分インデントが深くなっている。揃えたいので0に。
        EditorGUI.indentLevel = 0;

        // 3文字なら、これぐらいの幅があればいいんじゃないかな。
        EditorGUIUtility.labelWidth = 30f;


        EditorGUI.PropertyField(contentPosition, minProperty, MIN_LABEL);

        contentPosition.x += contentPosition.width;

        EditorGUI.PropertyField(contentPosition, maxProperty, MAX_LABEL);


        EditorGUI.EndProperty();
    }

    protected void ApplyValue(SerializedProperty i_minProperty, SerializedProperty i_maxProperty)
    {
        // 小さい数値を基準にして、大きい数値が小さい数値より小さくならないようにしてみよう。
        if (i_maxProperty.intValue < i_minProperty.intValue)
        {
            i_maxProperty.intValue = i_minProperty.intValue;
        }
        if (i_minProperty.intValue > i_maxProperty.intValue)
        {
            i_minProperty.intValue = i_maxProperty.intValue;
        }
    }

    public override float GetPropertyHeight(SerializedProperty i_property, GUIContent i_label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

}

#endif