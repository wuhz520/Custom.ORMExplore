using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace hzwu.Framework.SqlDataValidate
{
    public static class ValidateExtend
    {
        public static bool Validate<T>(this T t)
        {
            Type type = t.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
                {
                    object oValue = prop.GetValue(t);
                    var attributeArray = prop.GetCustomAttributes<AbstractValidateAttribute>();
                    foreach (var attribute in attributeArray)
                    {
                        if (attribute.Validate(oValue))
                        {
                            //继续
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                //if (prop.IsDefined(typeof(LengthAttribute), true))
                //{
                //    object oValue = prop.GetValue(t);
                //    var attribute = prop.GetCustomAttribute<LengthAttribute>();
                //    if (attribute.Validate(oValue))
                //    {
                //        //继续
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //if (prop.IsDefined(typeof(LengthAttribute), true))
                //{
                //    object oValue = prop.GetValue(t);
                //    var attribute = prop.GetCustomAttribute<LengthAttribute>();
                //    if (attribute.Validate(oValue))
                //    {
                //        //继续
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
            }
            return true;
        }
    }
}
