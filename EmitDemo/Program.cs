using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Emit();

            PrintTypes();

            CreateObject();

            Console.ReadLine();
        }

        private static void CreateObject()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Pets.dll");
            var asm = Assembly.LoadFile(path);
            var type = asm.GetType("Kitty");
            var obj = Activator.CreateInstance(type, new object[] {10, "Kitty"});
            Console.WriteLine(obj);
            var idProperty = type.GetProperty("ID");
            var nameProperty = type.GetProperty("Name");

            Console.WriteLine($"id: {idProperty.GetValue(obj)}, name: {nameProperty.GetValue(obj)}");
            //idProperty.SetValue(obj, 15, null);
            nameProperty.SetValue(obj, "cat", null);


            Console.WriteLine(obj);
        }

        private static void PrintTypes()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Pets.dll");
            var asm = Assembly.LoadFile(path);
            var types = asm.GetTypes();

            foreach (var type in types)
            {
                Print(type);
            }
        }

        private static void Print(Type type)
        {
            Console.WriteLine($"namespace: {type.Namespace}, type: {type.Name}");
            
            Console.WriteLine("fields:");
            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Console.WriteLine(fieldInfo.Name);
            }

            Console.WriteLine("properties");
            foreach (var propertyInfo in type.GetProperties())
            {
                Console.WriteLine(propertyInfo.Name);
            }

            Console.WriteLine("------------------------");
        }

        private static void Emit()
        {
            var assemblyName = new AssemblyName("Pets");
            var assemblyBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PetModule", "Pets.dll");
            //定义类型
            var typeBuilder = moduleBuilder.DefineType("Kitty", TypeAttributes.Public);

            //定义属性
            var id = typeBuilder.DefineField("id", typeof(int), FieldAttributes.Private);
            var name = typeBuilder.DefineField("name", typeof(string), FieldAttributes.Private);

            //定义构造函数
            Type[] constructorArgs = {typeof(int), typeof(string)};
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorArgs);
            ILGenerator ilGenerator = constructorBuilder.GetILGenerator();

            // Object构造函数
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

            //定义构造函数
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, objCtor);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, id);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Stfld, name);
            ilGenerator.Emit(OpCodes.Ret);

            //方法
            var getId = typeBuilder.DefineMethod("GetId", MethodAttributes.Public, typeof(int), null);
            var getName = typeBuilder.DefineMethod("GetName", MethodAttributes.Public, typeof(string), null);
            var setId = typeBuilder.DefineMethod("SetId", MethodAttributes.Public, null, new Type[] {typeof(int)});
            var setName = typeBuilder.DefineMethod("SetName", MethodAttributes.Public, null, new Type[] {typeof(string)});

            var ilGetId = getId.GetILGenerator();
            ilGetId.Emit(OpCodes.Ldarg_0);
            ilGetId.Emit(OpCodes.Ldfld, id);
            ilGetId.Emit(OpCodes.Ret);

            var ilSetId = setId.GetILGenerator();
            ilSetId.Emit(OpCodes.Ldarg_0);
            ilSetId.Emit(OpCodes.Ldarg_1);
            ilSetId.Emit(OpCodes.Stfld, id);
            ilSetId.Emit(OpCodes.Nop);
            ilSetId.Emit(OpCodes.Ret);

            var ilGetName = getName.GetILGenerator();
            ilGetName.Emit(OpCodes.Ldarg_0);
            ilGetName.Emit(OpCodes.Ldfld, name);
            ilGetName.Emit(OpCodes.Ret);

            var ilSetName = setName.GetILGenerator();
            ilSetName.Emit(OpCodes.Ldarg_0);
            ilSetName.Emit(OpCodes.Ldarg_1);
            ilSetName.Emit(OpCodes.Stfld, name);
            ilSetName.Emit(OpCodes.Nop);
            ilSetName.Emit(OpCodes.Ret);

            //属性
            var idProperty = typeBuilder.DefineProperty("ID", PropertyAttributes.None, typeof(int), null);
            var nameProperty = typeBuilder.DefineProperty("Name", PropertyAttributes.None, typeof(string), null);

            idProperty.SetGetMethod(getId);
            idProperty.SetSetMethod(setId);

            nameProperty.SetGetMethod(getName);
            nameProperty.SetSetMethod(setName);

            //ToString方法
            var toString = typeBuilder.DefineMethod("ToString", MethodAttributes.Virtual | MethodAttributes.Public,
                typeof(string), null);
            var ilToString = toString.GetILGenerator();
            var local = ilToString.DeclareLocal(typeof(string));
            ilToString.Emit(OpCodes.Ldstr, "ID: [{0}], Name: [{1}]");
            ilToString.Emit(OpCodes.Ldarg_0);
            ilToString.Emit(OpCodes.Ldfld, id);
            ilToString.Emit(OpCodes.Box, typeof(int));
            ilToString.Emit(OpCodes.Ldarg_0);
            ilToString.Emit(OpCodes.Ldfld, name);
            ilToString.Emit(OpCodes.Call,
                typeof(string).GetMethod("Format", new Type[] {typeof(string), typeof(object), typeof(object)}));
            ilToString.Emit(OpCodes.Stloc, local);
            ilToString.Emit(OpCodes.Ldloc, local);
            ilToString.Emit(OpCodes.Ret);

            var classType = typeBuilder.CreateType();
            assemblyBuilder.Save("Pets.dll");
        }
    }
}
