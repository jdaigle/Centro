using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Centro.OpenEntity.Proxy
{
    public class EntityFieldProxyGenerationHook : IProxyGenerationHook
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo memberInfo)
        {
            return memberInfo.IsSpecialName &&
                   (memberInfo.Name.StartsWith("set_", StringComparison.Ordinal) ||
                    memberInfo.Name.StartsWith("get_", StringComparison.Ordinal));
        }

        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public void MethodsInspected()
        {
        }
    }
}