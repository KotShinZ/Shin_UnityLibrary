using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Shin_UnityLibrary
{
    public static class EnumBuilderFromClasses
    {
        public static List<string> GetDerivedClasses(string baseClassName)
        {
            List<string> derivedClasses = new List<string>();
            Type baseType = Type.GetType(baseClassName);

            if (baseType != null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsSubclassOf(baseType))
                        {
                            derivedClasses.Add(type.FullName);
                        }
                    }
                }
            }

            return derivedClasses;
        }

        public static void BuildEnumFromClasses(List<string> classNames, string enumName)
        {
            Type underlyingType = typeof(int); // 列挙型の基底型を指定します。ここではint型を使用しています。

            EnumBuilder typeBuilder = CreateTypeBuilder(enumName, underlyingType);
            FieldBuilder[] fieldBuilders = CreateEnumFields(classNames, typeBuilder, underlyingType);
            //CreateEnumConstructor(typeBuilder, fieldBuilders);
            //CreateEnumToStringMethod(typeBuilder, fieldBuilders);
            //CreateEnumParseMethod(typeBuilder, fieldBuilders);
        }

        private static EnumBuilder CreateTypeBuilder(string enumName, Type underlyingType)
        {
            AssemblyName assemblyName = new AssemblyName(Guid.NewGuid().ToString());
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            return moduleBuilder.DefineEnum(enumName, TypeAttributes.Public, underlyingType);
        }

        private static FieldBuilder[] CreateEnumFields(List<string> classNames, EnumBuilder typeBuilder, Type underlyingType)
        {
            FieldBuilder[] fieldBuilders = new FieldBuilder[classNames.Count];

            for (int i = 0; i < classNames.Count; i++)
            {
                string className = classNames[i];
                fieldBuilders[i] = typeBuilder.DefineLiteral(className, i);
            }

            return fieldBuilders;
        }

        private static void CreateEnumConstructor(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
        {
            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(int) });

            ILGenerator ilGenerator = constructorBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Call, typeof(Enum).GetConstructor(new Type[] { typeof(int) }));
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void CreateEnumToStringMethod(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "ToString",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(string),
                new Type[0]);

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, typeof(Enum).GetMethod("ToString"));
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void CreateEnumParseMethod(TypeBuilder typeBuilder, FieldBuilder[] fieldBuilders)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "Parse",
                MethodAttributes.Public | MethodAttributes.Static,
                typeof(object),
                new Type[] { typeof(string) });

            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            LocalBuilder enumValue = ilGenerator.DeclareLocal(typeBuilder);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldloca_S, enumValue);
            ilGenerator.Emit(OpCodes.Call, typeof(Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string) }));
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}