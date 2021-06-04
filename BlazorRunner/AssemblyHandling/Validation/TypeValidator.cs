using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    /// <summary>
    /// Contains various helper methods that provide validation of instanced objects and their compatibility for certain features in <see cref="BlazorRunner"/>
    /// </summary>
    public static class TypeValidator
    {

        public static readonly object[] DefaultNonZeroPrimitives = {
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            1,
            0.1f,
            0.1d,
            0.1m
        };

        public const TypeCode PrimitiveTypes = TypeCode.SByte | TypeCode.Byte | TypeCode.Int16 | TypeCode.UInt16 | TypeCode.Int32 | TypeCode.UInt32 | TypeCode.Int64 | TypeCode.UInt64 | TypeCode.Single | TypeCode.Double | TypeCode.Decimal;

        public const BinaryTypeCode ImplicitSbyteConversions = (BinaryTypeCode.Int16 | BinaryTypeCode.Int32 | BinaryTypeCode.Int64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.SByte);

        public const BinaryTypeCode ImplicitByteConversions = (BinaryTypeCode.Int16 | BinaryTypeCode.UInt16 | BinaryTypeCode.Int32 | BinaryTypeCode.UInt32 | BinaryTypeCode.Int64 | BinaryTypeCode.UInt64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.Byte);

        public const BinaryTypeCode ImplicitShortConversions = (BinaryTypeCode.Int32 | BinaryTypeCode.Int64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.Int16);

        public const BinaryTypeCode ImplicitUShortConversions = (BinaryTypeCode.Int32 | BinaryTypeCode.UInt32 | BinaryTypeCode.Int64 | BinaryTypeCode.UInt64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.UInt16);

        public const BinaryTypeCode ImplicitIntConversions = (BinaryTypeCode.Int64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.Int32);

        public const BinaryTypeCode ImplicitUIntConversions = (BinaryTypeCode.Int64 | BinaryTypeCode.UInt64 | BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.UInt32);

        public const BinaryTypeCode ImplicitLongConversions = (BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.Int64);

        public const BinaryTypeCode ImplicitULongConversions = (BinaryTypeCode.Single | BinaryTypeCode.Double | BinaryTypeCode.Decimal | BinaryTypeCode.UInt64);

        public const BinaryTypeCode ImplicitFloatConversions = BinaryTypeCode.Double | BinaryTypeCode.Single;

        public const BinaryTypeCode ImplicitDoubleConversions = BinaryTypeCode.Double;

        public const BinaryTypeCode ImplicitDecimalConversions = BinaryTypeCode.Decimal;

        /// <summary>
        /// Attempts to search the collection defined by <paramref name="ValidatorType"/> and if the object is an instance of any of
        /// those types, the type is returned.
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="EligibleType"></param>
        /// <param name="ValidatorType"></param>
        /// <returns></returns>
        public static bool TryGetEligibleType(object Instance, out Type EligibleType, ValidatorTypes ValidatorType)
        {
            Type instanceType = Instance.GetType();

            TypeCode instanceTypeCode = Type.GetTypeCode(instanceType);

            // left open for future extensibility, compiler should simplify it
            switch (ValidatorType)
            {
                case ValidatorTypes.EligibleSliders:
                    if ((instanceTypeCode & PrimitiveTypes) != TypeCode.Empty)
                    {
                        EligibleType = instanceType;
                        return true;
                    }
                    break;
            }

            EligibleType = null;
            return false;
        }

        public static bool TryGetCompatibility(object Instance, Type DesiredType, out CastingCompatibility Compatibility)
        {
            // default to none
            Compatibility = CastingCompatibility.none;

            if (Instance is null)
            {
                return false;
            }

            if (Instance.GetType() == DesiredType)
            {
                Compatibility = CastingCompatibility.SameType;
                return true;
            }

            if (IsImplicitlyCastable(Instance, DesiredType))
            {
                Compatibility = CastingCompatibility.Implicit;
                return true;
            }

            if (IsExplicitlyCastable(Instance, DesiredType))
            {
                Compatibility = CastingCompatibility.Explicit;
                return true;
            }

            if (IsExplicitlyCastable(StripNumericalSymbols(Instance.ToString()), DesiredType))
            {
                Compatibility = CastingCompatibility.Parsable;
                return true;
            }

            return false;
        }

        public static bool IsExplicitlyCastable(object Instance, Type DesiredType)
        {
            if (DesiredType.GetInterface(nameof(IConvertible)) != null)
            {
                // check to see if the instance is IConvertible
                if (Instance is IConvertible convertibleType)
                {
                    try
                    {
                        Convert.ChangeType(convertibleType, DesiredType);

                        return true;
                    }
                    catch (InvalidCastException) { }
                    catch (FormatException) { }
                    catch (OverflowException) { }
                    catch (ArgumentNullException) { }


                    return false;
                }
            }

            return false;
        }

        public static object Cast(object Instance, Type DesiredType, CastingCompatibility compatibility)
        {
            switch (compatibility)
            {
                case CastingCompatibility.SameType:
                    return Instance;
                case CastingCompatibility.Explicit:
                    return Convert.ChangeType(Instance, DesiredType);
                case CastingCompatibility.Parsable:
                    return Convert.ChangeType(StripNumericalSymbols(Instance.ToString()), DesiredType);
                case CastingCompatibility.Implicit:
                    return Instance;
                default:
                    return Instance;
            }
        }

        public static string StripNumericalSymbols(object Instance)
        {
            string s = Instance.ToString();
            s = s.Replace("f", "");
            s = s.Replace("d", "");
            s = s.Replace("m", "");
            return s;
        }

        public static bool IsImplicitlyCastable(object Instance, Type DesiredType)
        {
            Type InstanceType = Instance.GetType();

            TypeCode instanceTypeCode = Type.GetTypeCode(InstanceType);
            TypeCode desiredTypeCode = Type.GetTypeCode(DesiredType);

            // convert system typecode to binary so we can do bit math to determine implicit casting
            BinaryTypeCode desiredBinaryCode = (BinaryTypeCode)Enum.Parse(typeof(BinaryTypeCode), desiredTypeCode.ToString());

            // im so sorry for this
            switch (instanceTypeCode)
            {
                case TypeCode.SByte:
                    return (desiredBinaryCode & ImplicitSbyteConversions) != BinaryTypeCode.Empty;

                case TypeCode.Byte:
                    return (desiredBinaryCode & ImplicitByteConversions) != BinaryTypeCode.Empty;

                case TypeCode.Int16:
                    return (desiredBinaryCode & ImplicitShortConversions) != BinaryTypeCode.Empty;

                case TypeCode.UInt16:
                    return (desiredBinaryCode & ImplicitUShortConversions) != BinaryTypeCode.Empty;

                case TypeCode.Int32:
                    return (desiredBinaryCode & ImplicitIntConversions) != BinaryTypeCode.Empty;

                case TypeCode.UInt32:
                    return (desiredBinaryCode & ImplicitUIntConversions) != BinaryTypeCode.Empty;

                case TypeCode.Int64:
                    return (desiredBinaryCode & ImplicitLongConversions) != BinaryTypeCode.Empty;

                case TypeCode.UInt64:
                    return (desiredBinaryCode & ImplicitULongConversions) != BinaryTypeCode.Empty;

                case TypeCode.Single:
                    return (desiredBinaryCode & ImplicitFloatConversions) != BinaryTypeCode.Empty;

                case TypeCode.Double:
                    return (desiredBinaryCode & ImplicitDoubleConversions) != BinaryTypeCode.Empty;

                case TypeCode.Decimal:
                    return (desiredBinaryCode & ImplicitDecimalConversions) != BinaryTypeCode.Empty;

                default:
                    return false;
            }
        }
    }
}
