namespace Fittify.Api.Helpers.Extensions
{

    public static class StructExtensions
    {
        public static bool IsDefault<T>(this T value) where T : struct
        {
            bool isDefault = value.Equals(default(T));

            return isDefault;
        }
    }
}
