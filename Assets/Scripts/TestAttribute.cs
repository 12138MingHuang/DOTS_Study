using System;
using System.Reflection;
using UnityEngine;

// 特性通常以 Attribute 结尾，使用时可以省略后缀Attribute。

// AttributeUsage 用于限制自定义特性的使用范围和行为。它可以控制：
// 特性可以附加到哪些代码元素上（如类、方法、属性等）。
// 是否允许多次附加同一个特性到同一个代码元素上。
// 特性是否可以被派生类继承。
[AttributeUsage(AttributeTargets.Class)]
public class ClassInfoAttribute : Attribute // 类的特性
{
    public string Description { get; }

    public float Version { get; set; }

    public ClassInfoAttribute(string description, Type type)
    {
        Description = description;
        Activator.CreateInstance(type);
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class MethodInfoAttribute : Attribute // 方法的特性
{
    public string Description { get; }

    public MethodInfoAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Property)] // 属性的特性
public class PropertyInfoAttribute : Attribute
{
    public string Description { get; }

    public PropertyInfoAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Field)] // 字段的特性
public class FieldInfoAttribute : Attribute
{
    public string Description { get; }

    public FieldInfoAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Parameter)] // 参数的特性
public class ParameterInfoAttribute : Attribute
{
    public string Description { get; }

    public ParameterInfoAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.ReturnValue)]
public class ReturnInfoAttribute : Attribute
{
    public string Description { get; }

    public ReturnInfoAttribute(string description)
    {
        Description = description;
    }
}

[ClassInfo("这是一个测试类,特性中创建了所标记类的实例(使用反射创建的)", typeof(TestClass), Version = 1.0f)] // 特性中创建了所标记类的实例(使用反射创建的),最后一个参数是命名参数，就是特性中的属性
public class TestClass
{
    [FieldInfo("这是一个测试字段")]
    private int _sampleField;

    [PropertyInfo("这是一个测试属性")]
    public string SampleProperty { get; set; }

    [MethodInfo("这是一个测试方法")]
    [return: ReturnInfo("这是一个测试方法的返回值")]
    public string SampleMethod([ParameterInfo("这是一个测试方法的参数")] string input)
    {
        return $"{input} 这是一个测试方法";
    }
}

public class TestAttribute : MonoBehaviour
{
    private void Start()
    {
        Type type = typeof(TestClass);
        
        ClassInfoAttribute classInfo = type.GetCustomAttribute<ClassInfoAttribute>();
        if (classInfo != null)
        {
            Debug.Log($"Class Desc: {classInfo.Description} Version: {classInfo.Version}");
        }
        
        FieldInfo fieldInfo = type.GetField("_sampleField");
        if (fieldInfo != null)
        {
            FieldInfoAttribute fieldInfoAttribute = fieldInfo.GetCustomAttribute<FieldInfoAttribute>();
            if (fieldInfoAttribute != null)
            {
                Debug.Log($"Field Desc: {fieldInfoAttribute.Description}");
            }
        }
        
        PropertyInfo propertyInfo = type.GetProperty("SampleProperty");
        if (propertyInfo != null)
        {
            PropertyInfoAttribute propertyInfoAttribute = propertyInfo.GetCustomAttribute<PropertyInfoAttribute>();
            if (propertyInfoAttribute != null)
            {
                Debug.Log($"Property Desc: {propertyInfoAttribute.Description}");
            }
        }
        
        MethodInfo methodInfo = type.GetMethod("SampleMethod");
        if (methodInfo != null)
        {
            MethodInfoAttribute methodInfoAttribute = methodInfo.GetCustomAttribute<MethodInfoAttribute>();
            if (methodInfoAttribute != null)
            {
                Debug.Log($"Method Desc: {methodInfoAttribute.Description}");
            }
            
            ParameterInfoAttribute parameterInfoAttribute = methodInfo.GetParameters()[0].GetCustomAttribute<ParameterInfoAttribute>();
            ParameterInfo[] parameters = methodInfo.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                ParameterInfoAttribute paramAttr = parameter.GetCustomAttribute<ParameterInfoAttribute>();
                if(paramAttr != null)
                {
                    Debug.Log($"Parameter Desc: {paramAttr.Description}");
                }
            }

            if (methodInfo.ReturnParameter != null)
            {
                ReturnInfoAttribute returnInfoAttribute = methodInfo.ReturnParameter.GetCustomAttribute<ReturnInfoAttribute>();
                if (returnInfoAttribute != null)
                {
                    Debug.Log($"Return Desc: {returnInfoAttribute.Description}");
                }
            }
        }
    }
}
